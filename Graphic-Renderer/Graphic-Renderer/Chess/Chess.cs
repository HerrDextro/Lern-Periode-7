using Graphic_Renderer.SmartPainterFiles;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Graphic_Renderer.Chess
{
    public class Chess
    {
        private GameState? currentGame;
        private Guid yourID;
        private string _gameId;
        private SPainter painter;
        private SReader reader;

        private int _pushTimeSlot;

        private ChessLogic _chessLogic;

        record SecretData
        {
            public string API_key { get; set; }
            public string API_token { get; set; }
            public string list_id { get; set; }
        }

        SecretData secrets;



        public Chess(SPainter painter, SReader reader)
        {
            yourID = Guid.NewGuid();

            string jsonString = File.ReadAllText(@"..\..\..\Chess\chess_secret_config.json");
            secrets = JsonSerializer.Deserialize<SecretData>(jsonString);

            this.reader = reader;
            this.painter = painter;
        }


        // Asynchronous state getter
        private readonly object _getterLock = new object();
        private GameState _gameState;
        private Task? _gameStateGetterTask;
        private CancellationTokenSource? _cts;

        private Task? _gameStatePusherTask;


        private void GetGameStateParralel(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                string url = $"https://api.trello.com/1/cards/{_gameId}?key={secrets.API_key}&token={secrets.API_token}&fields=desc";


                var response = APIRequestHelper.GetCard(url).GetAwaiter().GetResult();

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    continue;
                }

                string stringified = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                using JsonDocument doc = JsonDocument.Parse(stringified);
                string gameStateJson = doc.RootElement.GetProperty("desc").GetString();

                GameState gameState = JsonSerializer.Deserialize<GameState>(gameStateJson);

                lock (_getterLock)
                {
                    _gameState = gameState;
                }

            }
        }

        public void PushGameStateParralel(GameState state)
        {
            int second = DateTime.Now.Second;
            while (!(second % 4 == _pushTimeSlot))
            {
                // Busy waiting
            }
            string gameStateJson = JsonSerializer.Serialize(state);
            string url = $"https://api.trello.com/1/cards/{_gameId}?key={secrets.API_key}&token={secrets.API_token}&desc={gameStateJson}";
        }

        public void StartGame()
        {
            //ChessLogic.ParseFen(GameState.BoardObj);

            painter.clear();

            painter.clear();
            painter.clear();
            painter.loadImage(15, 1, @"..\..\..\Chess\Assets\title.json");
            painter.updateFrame();


            
            
            
            int multiplayerForm = DisplayMenu();
            if (multiplayerForm == 1)
            {
                _gameId = CreateGame();
                AwaitGameStart(_gameId);

                _pushTimeSlot = 0;

            }
            else
            {
                bool validIDGiven = false;
                bool firstTry = true;

                while (!validIDGiven)
                {
                    string gameID = GetGameIdFromUser(firstTry);
                    firstTry = false;
                    bool sucess = LogIntoGame(gameID);
                    if (sucess)
                    {
                        validIDGiven = true;
                        _gameId = gameID;
                    }

                    

                }
                _pushTimeSlot = 2;
            }

            // Start game state getter
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            _gameStateGetterTask = Task.Run(() => GetGameStateParralel(token), token);


            while (true)
            {
                GameState currentStatePoint;
                lock (_getterLock)
                {
                    currentStatePoint = _gameState;
                }

                if (currentStatePoint != null)
                {
                    bool isYourTurn = currentStatePoint.CurrentPlayerID == yourID.ToString();
                }

                // Imitate Neo stuff
                painter.clear();
                if(currentStatePoint == null)
                {
                    continue;
                }

                _chessLogic.RenderFrame(currentStatePoint);
                painter.updateFrame();
                GameState? newStatePoint = null;

                // Push new GameState to board
                if(newStatePoint != null && (_gameStatePusherTask != null && !_gameStatePusherTask.IsCompleted))
                {
                    _gameStatePusherTask = Task.Run(() => PushGameStateParralel(newStatePoint));
                }
                


            }
        }

        private int DisplayMenu()
        {
            // dark green #4E7837
            // light green #69923e
            // cream #ECDC9F
            // dark gray 4b4847
            // dark green / cream for chess board


            bool validInputGiven = false;
            while (!validInputGiven)
            {
                int[] mouse = reader.getMousePos();
                
                if (InBounds(5, mouse[0], 19) && InBounds(10, mouse[1], 14))
                {
                    painter.fillRectangle("#69923e", 5, 10, 14, 4);
                    if (reader.IsLeftMouseButtonDown())
                    {
                        return 2;
                    }
                }
                else
                {
                    painter.fillRectangle("#4E7837", 5,10, 14, 4);
                }

                if (InBounds(25, mouse[0], 50) && InBounds(10, mouse[1], 14))
                {
                    painter.fillRectangle("#69923e", 30, 10, 25, 4);

                    if (reader.IsLeftMouseButtonDown())
                    {
                        return 1;
                    }
                }
                else
                {
                    painter.fillRectangle("#4E7837", 30, 10, 25, 4);
                }


                painter.loadImage(5, 10, @"..\..\..\Chess\Assets\join_game.json");
                painter.loadImage(30, 10, @"..\..\..\Chess\Assets\create_game.json");

                painter.updateFrame();
            }
            




            return 2;

        }

        private string GetGameIdFromUser(bool isFirstTry)
        {
            painter.clear();

            painter.loadImage(15, 1, @"..\..\..\Chess\Assets\title.json");
            painter.writeText("Paste Game Code", 3,10);

            reader.StartKeyCapture();

            if (!isFirstTry)
            {
                painter.writeText("Invalid Code, Try again", 22,10, "#5a1010");
            }

            bool done = false;
            while (!done)
            {
                painter.writeText(reader.ReadKeyCapture() + " ", 3, 11);

                if (reader.KeyDown(SReader.enter)) done = true;

                painter.updateFrame();
            }

            return reader.EndCapture();





        }

        private void AwaitGameStart(string code)
        {
            bool playerJoined = false;

            painter.clear();
            painter.loadImage(15, 1, @"..\..\..\Chess\Assets\title.json");
            painter.writeText("Awaiting second player...", 3, 10, "#69923e");

            painter.writeText("Your Code: " + code, 3, 11, "#69923e");
            painter.updateFrame();

            while (!playerJoined)
            {
                string url = $"https://api.trello.com/1/cards/{_gameId}?key={secrets.API_key}&token={secrets.API_token}&fields=desc";


                var response = APIRequestHelper.GetCard(url).GetAwaiter().GetResult();

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    continue;
                }

                string stringified = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                using JsonDocument doc = JsonDocument.Parse(stringified);
                string gameStateJson = doc.RootElement.GetProperty("desc").GetString();

                GameState gameState = JsonSerializer.Deserialize<GameState>(gameStateJson);

                if(gameState.BlackUID != null)
                {
                    playerJoined = true;
                }

            }

            painter.clear();
            painter.updateFrame();
        }

        private string CreateGame()
        {
            DateTime dueDateTime = DateTime.UtcNow;
            dueDateTime = dueDateTime.AddHours(3);

            // Convert to ISO 8601 string in UTC format with 'Z'
            string trelloDue = dueDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", System.Globalization.CultureInfo.InvariantCulture);


            //_chessLogic = new ChessLogic(painter, reader, null);
            GameState emptyGame = new GameState(yourID);
            _chessLogic = new ChessLogic(painter, reader, emptyGame);
            _chessLogic.ParseFen();

           


            Request createCard = new Request()
            {
                idList = secrets.list_id,
                key = secrets.API_key,
                token = secrets.API_token,

                name = Guid.NewGuid().ToString(),
                desc = JsonSerializer.Serialize(emptyGame),
                due = trelloDue
            };

            var result = APIRequestHelper.PostCard(createCard, "https://api.trello.com/1/cards").GetAwaiter().GetResult();

            result.EnsureSuccessStatusCode();

            string response = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            using JsonDocument doc = JsonDocument.Parse(response);
            string id = doc.RootElement.GetProperty("id").GetString();

            return id;

        }

        private bool LogIntoGame(string gameId)
        {
            GameState emptyGame = new GameState(yourID);
            _chessLogic = new ChessLogic(painter, reader, emptyGame);
            _chessLogic.ParseFen();



            string url = $"https://api.trello.com/1/cards/{gameId}?key={secrets.API_key}&token={secrets.API_token}&fields=desc";
            var response = APIRequestHelper.GetCard(url).GetAwaiter().GetResult();

            if(response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return false;
            }



            string stringified = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            using JsonDocument doc = JsonDocument.Parse(stringified);
            string gameStateJson = doc.RootElement.GetProperty("desc").GetString();

            GameState gameState = JsonSerializer.Deserialize<GameState>(gameStateJson);
            gameState.JoinPlayer(yourID);

            string gameStateJsonNew = JsonSerializer.Serialize(gameState);

            string putUrl = $"https://api.trello.com/1/cards/{gameId}?key={secrets.API_key}&token={secrets.API_token}&desc={gameStateJsonNew}";
            var getGesponse = APIRequestHelper.UpdateCard(putUrl).GetAwaiter().GetResult();
            getGesponse.EnsureSuccessStatusCode();

            return true;

        }

        private bool InBounds(int min, int value, int max)
        {
            return (min <= value && value <= max);
        }


        
    }
    /*public class Chess
    {
        public static void StartGame()
        {
            //Guid guid = Guid.NewGuid();

            //create GUID's for all


        }
    }*/

    public record Request
    {
        public required string idList { get; set; }
        public required string key {  get; set; }
        public required string token { get; set; }
        
        public string? name { get; set; }
        public string? desc { get; set; }
        public string? due { get; set;}
    }
}

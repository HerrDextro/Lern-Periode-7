using Graphic_Renderer.SmartPainterFiles;
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

        public void StartGame()
        {
            /*
             User Interface
             */
            painter.clear();
            painter.loadImage(1, 1, @"C:\Users\alex\source\repos\HerrDextro\Lern-Periode-7\Graphic-Renderer\Graphic-Renderer\Chess\Assets\title.json");
            painter.updateFrame();

            Thread.Sleep(10000);

            
            
            
            int multiplayerForm = DisplayMenu();
            if (multiplayerForm == 1)
            {
                _gameId = CreateGame();
            }
            else
            {
                bool validIDGiven = false;
                
                while (!validIDGiven)
                {
                    string gameID = GetGameIdFromUser();
                    bool sucess = LogIntoGame(gameID);
                    if (sucess)
                    {
                        validIDGiven = true;
                        _gameId = gameID;
                    }
                }
            }


            RenderChess(currentGame);
        }

        private int DisplayMenu()
        {
            return 2;
        }

        private string GetGameIdFromUser()
        {
            return "68c3deac8808395fc1b4114c";
        }

        private string CreateGame()
        {
            GameState emptyGame = new GameState(yourID);


            Request createCard = new Request()
            {
                idList = secrets.list_id,
                key = secrets.API_key,
                token = secrets.API_token,

                name = Guid.NewGuid().ToString(),
                desc = JsonSerializer.Serialize(emptyGame)
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




        public GameState? RenderChess(GameState gameState)
        {
            if (gameState.BoardState == currentGame.BoardState)
            {
                return null; // No changes detected
            }
            else
            {
                currentGame = gameState;
                return currentGame;
            }
                
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

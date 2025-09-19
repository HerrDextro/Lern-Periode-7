using Chess;
using Graphic_Renderer.SmartPainterFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphic_Renderer.Neo
{
    class GameManager
    {
        public void StartChessGame(SPainter painter, SReader reader)
        {
            ChessGame chessGame = new ChessGame(painter, reader);
            

        }
    }
    class ChessGame
    {
        private readonly SPainter painter;
        private readonly SReader reader;
        private string _testCode;
        public CurrentPlayerEnum CurrentPlayer { get; set; } = CurrentPlayerEnum.white; //white starts //invert after every incoming different game obj
        //public Piece[,] Board { get; set; } = new Piece[8, 8]; //together with initboard method

        public ChessBoard GeraBoard { get; private set; } = new ChessBoard(); //gera

        public ChessGame(SPainter painter, SReader reader)
        {
            this.painter = painter;
            this.reader = reader;
        }

        public enum CurrentPlayerEnum
        {
            white,
            black
        }

        public void StartChessGame()
        {
            
            painter.clear();
            //?? goes here (multiplayer stuff)
            painter.writeText("Starting Chess Game...", 15, 30);

            while (true)
            {
                RenderChess(this.GeraBoard);
            }
            
        }

        public ChessGame RenderFrame(ChessGame previousGameObj) //only accepts GERA //Call this every frame
        {
            Console.WriteLine("GetGameObj called"); //works but cant test so well
            //Console.WriteLine(GeraBoard.ToPgn());

            if (this.Equals(previousGameObj))
            {
                
                return null;
            }
            else
            {
                GeraBoard = previousGameObj.GeraBoard;
                CurrentPlayer = CurrentPlayer == CurrentPlayerEnum.white ? CurrentPlayerEnum.black : CurrentPlayerEnum.white; //invert current player color
                
                MakeMove(); //make the move from input boardstate and also from client here

                RenderChess(this.GeraBoard);
                return this;
            }
        }

        //client exclusive logic here
        //make move, check validity, update board state, etc
        public void MakeMove() //take start and end pos from cursor tracking method (yet to omplement) //make stateless 
        {
            if (!GeraBoard.IsEndGame) //if game isnt done
            {
                //check if piece selected/belongs to current player
                //check if destination seletced/valid
                //check if confirmation given
                //update board state
                string boardState = GeraBoard.ToFen();

            }
        }

        public string[] ParseFen(string fen)
        {
            fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"; //starting pos placeholder


            return fen.Split(' ');
        } //not done

        public string ConvertMoveInput((int x, int y) start, (int x, int y) end)
        {
            return "e2e4"; //placeholder
        } //not done


        public override bool Equals(object obj) //only checks boardstate, change this when chat object comes in too
        {
            var thisBoard = this.GeraBoard.ToFen(); //updates FEN string
            var otherBoard = ((ChessGame)obj).GeraBoard.ToFen();

            return thisBoard == otherBoard;
        } //compares this object to the incoming one from alex call

        
        public void RenderChess(ChessBoard GeraBoard)
        {
            // dark green #4E7837
            // light green #69923e
            // cream #ECDC9F
            // dark gray 4b4847
            // dark green / cream for chess board
            painter.updateFrame();
            painter.clear();
            int startPosX = 18;
            int startPosY = 0;
            int squareSize = 3; 
            
            var Board = GeraBoard.ToFen();
            painter.writeText("Current Board State (FEN): " + Board, 15, 30);

            painter.fillRectangle("#ECDC9F", startPosX, startPosY, squareSize * 8, squareSize * 8); //background

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    painter.fillRectangle("#4E7837", startPosX + squareSize, startPosY, squareSize, squareSize); //light squares //start 21, 0 
                    startPosX += squareSize * 2; //moves square X
                    
                }

                startPosX = 18 - squareSize; //resets X 
                if (i % 2 == 1) //for odd rows start 1 square right
                {
                    startPosX += squareSize;
                }
                startPosY += squareSize;
            }
        }
    }
    class Piece
    {
        public string Type { get; set; }
        public string Color { get; set; }
        public Piece(string type, string color)
        {
            Type = type;
            Color = color;
        }
    }
}

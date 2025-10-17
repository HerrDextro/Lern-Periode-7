using Chess;
using Graphic_Renderer.SmartPainterFiles;
using System;
using System.Collections.Generic;
using System.Data;
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
        public BoardObj BoardObj { get; private set; }

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
            BoardObj = new BoardObj();
            
            ParseFen(BoardObj);
            
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

        public static BoardObj ParseFen(BoardObj boardObj)
        {
            char[,] boardState = new char[8, 8];
            //string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"; //starting pos placeholder
            string fen = "r1bqkb1r/pppppppp/n4n2/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"; //for TESTING
            //string fen = "r1bk3r/p2pBpNp/n4n2/1p1NP2P/6P1/3P4/P1P1K3/q5b1 test"; //For TESTING
            string fenBoard = fen.Split(' ')[0];
            //string meta = fen.Split(" ")[1];

            int colIndex = 0;
            int rowIndex = 0;
            foreach (char c in fen)
            {
                if (c != '/' && Char.IsLetter(c))
                {
                    boardState[rowIndex, colIndex] = c;
                    if (colIndex < 7) { colIndex++; } else { colIndex = 0; if (rowIndex < 7) rowIndex++; }

                }
                else if (Char.IsDigit(c))
                {
                    int spaces = (int)char.GetNumericValue(c);

                    for (int i = 0; i < spaces; i++)
                    {

                        boardState[rowIndex, colIndex] = '-';
                        if (colIndex < 7) { colIndex++; } else { colIndex = 0; if (rowIndex < 7) rowIndex++; }

                    }
                }
            }
            //BoardState done now meta infos
            boardObj.boardState = boardState;
            boardObj.currentPlayerTurn = Convert.ToChar(fen.Split(" ")[1]);
            boardObj.castleMove = fen.Split(" ")[2];
            boardObj.enPassantMove = fen.Split(" ")[3];
            boardObj.halfMove = Convert.ToInt16(fen.Split(" ")[4]);
            boardObj.fullMove = Convert.ToInt16(fen.Split(" ")[5]);

            return boardObj;
        }

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
            // cream #eeeed2
            // dark gray 4b4847
            // dark green / cream for chess board
            painter.updateFrame();
            painter.clear();
            int startPosX = 18;
            int startPosY = 0;
            int squareSize = 3;

            var Board = GeraBoard.ToFen();
            painter.writeText("Current Board State (FEN): " + Board, 15, 30);

            //now render the board
            painter.fillRectangle("#eeeed2", startPosX, startPosY, squareSize * 8, squareSize * 8); //background

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

            //resetting vars
            startPosX = 18 * 2;
            startPosY = 0;
            //now render the pieces
            string color;
            if (BoardObj.currentPlayerTurn == 'w') { color = "#ffffff"; } else { color = "#000000"; }

            char[,] boardState = BoardObj.boardState;
            int counter = 0;
            int row = 0;
            int col = 0;
            //start at board start
            col = col + startPosX;
            row = row + startPosY;
            foreach (char c in boardState)
            {
                painter.writeText(c.ToString(), col, row, "#FF6961"); //watch out here row col reversed
                counter++;
                if (col < (7 * (squareSize * 2)) + startPosX) 
                { 
                    col = col + (squareSize * 2); 
                } 
                else 
                { 
                    col = 0 + startPosX;
                    if (row < (7 * squareSize) + startPosY)
                    {
                        row = row + squareSize;
                    }
                }
            }
        }
    }
    record BoardObj //for parsing FEN
    {
        public char[,] boardState;
        public char currentPlayerTurn { get; set; }
        public string castleMove { get; set; }
        public string enPassantMove { get; set; }
        public int halfMove { get; set; }
        public int fullMove { get; set; }

    }
}

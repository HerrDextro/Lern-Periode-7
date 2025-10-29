using Chess;
using Graphic_Renderer.Chess;
using Graphic_Renderer.SmartPainterFiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphic_Renderer.Chess
{
    /*class GameManager
    {
        public void StartChessGame(SPainter painter, SReader reader)
        {
            ChessGame chessGame = new ChessGame(painter, reader);
            

        }
    }*/
    public class ChessLogic
    {
        private readonly SPainter painter;
        private readonly SReader reader;
        private string _testCode;

        //stateless of the MakeMove()
        public int SelectedOriginR = -1;
        public int SelectedOriginC = -1;
        public bool originSelectionMade = false;
        public int SelectedDestinationR;
        public int SelectedDestinationC;
        public bool destinationSelectionMade = false;
        public bool moveChosen = false;
        //for debugging
        public string DebugMsg = "";
        public ChessBoard GeraBoard { get; private set; } = new ChessBoard(); //gera
        //public GameState BoardObj { get; private set; }
        public GameState BoardObj { get; set; }

        public ChessLogic(SPainter painter, SReader reader, GameState emptyGame)
        {
            this.painter = painter;
            this.reader = reader;
            BoardObj = emptyGame;
        }

        public GameState RenderFrame(GameState previousBoardObj, bool isYourTurn) //Call this every frame
        {
            //Console.WriteLine("GetGameObj called"); //works but cant test so well
            //Console.WriteLine(GeraBoard.ToPgn());
            BoardObj = previousBoardObj;
            if (isYourTurn)
            {
                MakeMove(); //make the move from input boardstate and also from client here
            }
            
            RenderChess(isYourTurn); //change from gera board to boardstate

            if (BoardObj.Equals(previousBoardObj))//seems to mess up here but idk how what where when doesnt make sense (literal biden makes more sense in a speech)
            {
                return null;
            }
            else
            {

                return BoardObj;
            }
        }

        //client exclusive logic here
        //make move, check validity, update board state, etc

        bool lastMouseDown = false; // put this as a class-level field, not inside the function

        public void MakeMove()
        {
            if (GeraBoard.IsEndGame) return;

            int startPosX = 18;
            int startPosY = 0;
            int squareSize = 3;

            int[] mousePos = reader.getMousePos();
            int col = (mousePos[0] - startPosX) / squareSize;
            int row = (mousePos[1] - startPosY) / squareSize;

            // detect actual click transition
            bool currentMouseDown = reader.IsLeftMouseButtonDown();
            bool justClicked = currentMouseDown && !lastMouseDown;
            lastMouseDown = currentMouseDown;

            // quick display debug
            painter.writeText($"Mouse: {mousePos[0]}, {mousePos[1]}", 0, 3);
            painter.writeText($"Col/Row: {col}, {row}", 0, 4);

            // Board bounds
            bool withinBoard =
                mousePos[0] >= startPosX &&
                mousePos[0] < startPosX + squareSize * 8 &&
                mousePos[1] >= startPosY &&
                mousePos[1] < startPosY + squareSize * 8;

            if (justClicked && withinBoard)
            {
                if (!originSelectionMade)
                {
                    SelectedOriginC = col;
                    SelectedOriginR = row;
                    originSelectionMade = true;
                    DebugMsg = "Origin selected";
                }
                else if (!destinationSelectionMade && (col != SelectedOriginC || row != SelectedOriginR))
                {
                    SelectedDestinationC = col;
                    SelectedDestinationR = row;
                    destinationSelectionMade = true;
                    DebugMsg = "Destination selected";
                }
            }

            // confirmation button click
            bool inConfirmButton =
                mousePos[0] >= 22 && mousePos[0] <= 38 &&
                mousePos[1] >= 25 && mousePos[1] <= 28;

            if (justClicked && originSelectionMade && destinationSelectionMade && inConfirmButton)
            {
                painter.fillRectangle("#FF6961", 22, 25, 16, 3);
                painter.writeText("Confirmed", 54, 26, "white");

                ConvertMoveInput(
                    (SelectedOriginC, SelectedOriginR),
                    (SelectedDestinationC, SelectedDestinationR)
                );

                originSelectionMade = false;
                destinationSelectionMade = false;

                DebugMsg = "Move confirmed";
            }
        }//revamped

        public void ConvertMoveInput((int x, int y) start, (int x, int y) end)
        {
            char[] xCoord = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };
            char[] yCoord = { '8', '7', '6', '5', '4', '3', '2', '1' };
            char x = '0';
            char y = '0';
            string origin;
            string destination;

            //origin coord
            for (int i = 0; i <= start.x; i++)
            {
                x = xCoord[i];
            }
            for (int i = 0; i <= start.y; i++)
            {
                y = yCoord[i];
            }
            origin = $"{x}{y}";
            x = '0';
            y = '0';

            //destination coord
            for (int i = 0; i <= end.x; i++)
            {
                x = xCoord[i];
            }
            for (int i = 0; i <= end.y; i++)
            {
                y = yCoord[i];
            }
            destination = $"{x}{y}";



            bool moveCheck = GeraBoard.Move(new Move(origin, destination));
            if (moveCheck == false) { this.DebugMsg = "Illegal move"; }

            string check = GeraBoard.ToFen();
            ParseFen();

            //Console.WriteLine("ConvertMoveInput called");

        } //not done

        public GameState ParseFen()
        {
            char[][] boardState = new char[8][];

            for (int i = 0; i < boardState.Length; i++)
            {
                boardState[i] = new char[8];
            }

            //string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"; //starting pos placeholder
            //string fen = "r1bqkb1r/pppppppp/n4n2/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"; //for TESTING
            //string fen = "r1bk3r/p2pBpNp/n4n2/1p1NP2P/6P1/3P4/P1P1K3/q5b1 test"; //For TESTING
            string fen = GeraBoard.ToFen(); //get FEN from boardObj
            string fenBoard = fen.Split(' ')[0];
            //string meta = fen.Split(" ")[1];

            int colIndex = 0;
            int rowIndex = 0;
            foreach (char c in fenBoard)
            {
                if (c != '/' && Char.IsLetter(c))
                {
                    boardState[rowIndex][colIndex] = c;
                    if (colIndex < 7) { colIndex++; } else { colIndex = 0; if (rowIndex < 7) rowIndex++; }

                }
                else if (Char.IsDigit(c))
                {
                    int spaces = (int)char.GetNumericValue(c);

                    for (int i = 0; i < spaces; i++)
                    {

                        boardState[rowIndex][colIndex] = '-';
                        if (colIndex < 7) { colIndex++; } else { colIndex = 0; if (rowIndex < 7) rowIndex++; }

                    }
                }
            }
            //BoardState done now meta infos
            BoardObj.boardState = boardState;
            BoardObj.currentPlayerTurn = Convert.ToChar(fen.Split(" ")[1]);
            BoardObj.castleMove = fen.Split(" ")[2];
            BoardObj.enPassantMove = fen.Split(" ")[3];
            BoardObj.halfMove = Convert.ToInt16(fen.Split(" ")[4]);
            BoardObj.fullMove = Convert.ToInt16(fen.Split(" ")[5]);

            return BoardObj;
        }

        public bool CheckChange(object obj) //only checks boardstate, change this when chat object comes in too
        {
            var thisBoard = this.GeraBoard.ToFen(); //updates FEN string
            var otherBoard = ((ChessLogic)obj).GeraBoard.ToFen();

            return thisBoard == otherBoard;
        } //compares this object to the incoming one from alex call


        public void RenderChess(bool isYourTurn)
        {
            //Board with pieces rendering
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

            //var Board = GeraBoard.ToFen();
            //painter.writeText("Current Board State (FEN): " + Board, 30, 30);

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
            
            //if (BoardObj.currentPlayerTurn == 'w') { color = "#ffffff"; } else { color = "#000000"; }
            //if (isYourTurn) { color = "#ff0000"; } else

            char[][] boardState = BoardObj.boardState;
            int counter = 0;
            int row = 0;
            int col = 0;
            //start at board start
            col = col + startPosX;
            row = row + startPosY;
            foreach (var x in boardState)
            {
                foreach (var y in x)
                {
                    if (Char.IsUpper(y)) { color = "#ffffff"; } else if (y == '-') { color = "#ff0000"; } else { color = "#000000"; }
                    painter.writeText(y.ToString(), col, row, color); //watch out here row col reversed
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

            
            
            //confirm button and chat rendering

            //confirmation
            painter.fillRectangle("white", 22, 25, 16, 3); //confirm move
            painter.writeText("Confirm Move", 54, 26, "red");

            //chat
            painter.fillRectangle("white", 45, 19, 10, 5); //chat

            //rendering infos and 
            painter.writeText("Current Turn: " + BoardObj.currentPlayerTurn, 0, 0);
            painter.writeText("Your color: " + (isYourTurn ? "White" : "Black"), 0, 10);
            painter.writeText("Num Halfmoves: " + BoardObj.halfMove, 0, 1);
            painter.writeText("Num Fullmoves: " + BoardObj.fullMove, 0, 2);
            painter.writeText(this.DebugMsg, 0, 5);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphic_Renderer.Chess
{
    public class GameState
    {
        public string RoomID { get;  set; }
        public string WhiteUID { get;  set; }
        public string BlackUID { get;  set; }
        public string CurrentPlayerID { get;  set; }
        public List<ChessPiece> BoardState { get;  set; }

        public GameState(string roomID, string whiteUID, string blackUID)
        {
            RoomID = roomID;
            WhiteUID = whiteUID;
            BlackUID = blackUID;
            CurrentPlayerID = whiteUID; // white moves first
            BoardState = InitializeBoard();

            

        }

        public GameState(Guid player1)
        {
            WhiteUID = player1.ToString();
            
            
            BoardState = InitializeBoard();
        }

        public GameState() // For Deserialisation
        {

        }

        public void JoinPlayer(Guid player2)
        {
            BlackUID = player2.ToString();
        }

        private List<ChessPiece> InitializeBoard()
        {
            var pieces = new List<ChessPiece>();

            // Pawns
            for (int i = 0; i < 8; i++)
            {
                pieces.Add(new ChessPiece { OwnerID = WhiteUID, Type = PieceType.Pawn, Position = (i, 1) });
                pieces.Add(new ChessPiece { OwnerID = BlackUID, Type = PieceType.Pawn, Position = (i, 6) });
            }

            // Rooks
            pieces.Add(new ChessPiece { OwnerID = WhiteUID, Type = PieceType.Rook, Position = (0, 0) });
            pieces.Add(new ChessPiece { OwnerID = WhiteUID, Type = PieceType.Rook, Position = (7, 0) });
            pieces.Add(new ChessPiece { OwnerID = BlackUID, Type = PieceType.Rook, Position = (0, 7) });
            pieces.Add(new ChessPiece { OwnerID = BlackUID, Type = PieceType.Rook, Position = (7, 7) });

            // Knights
            pieces.Add(new ChessPiece { OwnerID = WhiteUID, Type = PieceType.Knight, Position = (1, 0) });
            pieces.Add(new ChessPiece { OwnerID = WhiteUID, Type = PieceType.Knight, Position = (6, 0) });
            pieces.Add(new ChessPiece { OwnerID = BlackUID, Type = PieceType.Knight, Position = (1, 7) });
            pieces.Add(new ChessPiece { OwnerID = BlackUID, Type = PieceType.Knight, Position = (6, 7) });

            // Bishops
            pieces.Add(new ChessPiece { OwnerID = WhiteUID, Type = PieceType.Bishop, Position = (2, 0) });
            pieces.Add(new ChessPiece { OwnerID = WhiteUID, Type = PieceType.Bishop, Position = (5, 0) });
            pieces.Add(new ChessPiece { OwnerID = BlackUID, Type = PieceType.Bishop, Position = (2, 7) });
            pieces.Add(new ChessPiece { OwnerID = BlackUID, Type = PieceType.Bishop, Position = (5, 7) });

            // Queens
            pieces.Add(new ChessPiece { OwnerID = WhiteUID, Type = PieceType.Queen, Position = (3, 0) });
            pieces.Add(new ChessPiece { OwnerID = BlackUID, Type = PieceType.Queen, Position = (3, 7) });

            // Kings
            pieces.Add(new ChessPiece { OwnerID = WhiteUID, Type = PieceType.King, Position = (4, 0) });
            pieces.Add(new ChessPiece { OwnerID = BlackUID, Type = PieceType.King, Position = (4, 7) });

            return pieces;
        }

        public void NextTurn()
        {
            CurrentPlayerID = (CurrentPlayerID == WhiteUID) ? BlackUID : WhiteUID;
        }

        public bool MovePiece((int X, int Y) from, (int X, int Y) to)
        {
            var piece = BoardState.FirstOrDefault(p => p.Position == from && p.OwnerID == CurrentPlayerID);
            if (piece == null)
                return false; // no piece to move or wrong player

            // capture if piece exists at target
            var target = BoardState.FirstOrDefault(p => p.Position == to);
            if (target != null && target.OwnerID != CurrentPlayerID)
                BoardState.Remove(target);

            piece.Position = to;
            NextTurn();
            return true;
        }
    }
    /*internal record Gamestate
    {
        public string blackUID;
        public string whiteUID;
        public string roomID;
        public string currentUID;
        public string chatUID;
        public int[][] boardState;

    }*/
}

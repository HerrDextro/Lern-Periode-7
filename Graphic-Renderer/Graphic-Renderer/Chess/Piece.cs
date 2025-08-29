using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Graphic_Renderer.Chess
{
    public enum PieceType
    {
        Pawn, Rook, Knight, Bishop, Queen, King
    }

    public class ChessPiece
    {
        public string OwnerID { get; set; } // WhiteUID or BlackUID
        public PieceType Type { get; set; }
        public (int X, int Y) Position { get; set; }

        public override string ToString()
        {
            return $"{OwnerID} {Type} at ({Position.X},{Position.Y})";
        }
    }
    /*internal class Piece
    {
        public string type;
        public string texturePath;
        public string pieceUID; //looka t template first
    }*/
}

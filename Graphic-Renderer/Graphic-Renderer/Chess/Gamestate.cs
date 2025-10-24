using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphic_Renderer.Chess
{
    public class GameState //for parsing FEN
    {
        public char[,] boardState;
        public char currentPlayerTurn { get; set; }
        public string castleMove { get; set; }
        public string enPassantMove { get; set; }
        public int halfMove { get; set; }
        public int fullMove { get; set; }

        //added
        public string CurrentPlayerID { get; set; }
        public string BlackUID { get; set; }
        public string WhiteUID { get; set; }

        public GameState(Guid player1)
        {
            WhiteUID = player1.ToString();
        }

        public GameState()
        {

        }

        public void JoinPlayer(Guid player)
        {
            BlackUID = player.ToString();
        }

        public void NextTurn()
        {
            CurrentPlayerID = (CurrentPlayerID == WhiteUID) ? BlackUID : WhiteUID;
        }

    }
}

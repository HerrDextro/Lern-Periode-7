using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphic_Renderer.Chess
{
    public class GameState : IEquatable<GameState> //for parsing FEN
    {
        public char[][] boardState;
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
            CurrentPlayerID = WhiteUID;
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

        /*public static bool operator ==(GameState obj1, GameState obj2)
        {
            return obj1.boardState.SequenceEqual(obj2.boardState);
        }
        public static bool operator !=(GameState obj1, GameState obj2)
        {
            return !obj1.boardState.SequenceEqual(obj2.boardState);
        }*/

        /*public bool Equals(GameState obj1, GameState obj2)
        {
            return obj1 == obj2;
        }*/
        //public override bool Equals(object obj) => Equals(obj as GameState); //actually what is this even for? its for: "Overrides Object.Equals(object obj) to call the type-specific Equals(T other) method." Thanks intellisense. You learn something new every day
        public bool Equals(GameState? other)
        {
            return boardState.SequenceEqual(other?.boardState);
        }
    }
}

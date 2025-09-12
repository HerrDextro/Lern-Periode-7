using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphic_Renderer.Chess
{
    public static class Chess
    {
        private static GameState? currentGame;

        public static void StartGame(string roomID, string whiteUID, string blackUID)
        {
            currentGame = new GameState(roomID, whiteUID, blackUID);
            RenderChess(currentGame);
        }

        public static GameState? RenderChess(GameState gameState)
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
}

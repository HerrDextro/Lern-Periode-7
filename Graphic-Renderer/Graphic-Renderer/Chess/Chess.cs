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
        }

        public static GameState? UpdateFrame()
        {
            return currentGame;
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

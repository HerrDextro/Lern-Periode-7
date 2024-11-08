using System.Reflection;
using System.Text;

namespace Graphic_Renderer
{
    public class Pong
    {
        public void StartGame(SPainter painter)
        {
            //Default for a. PC (Fullscreen): 144,44
            //Default for a. PC (Small): 60,30

            //variables for loops arrays and whatnot (change names)
            bool alive = true;
            string paddlePath = "..\\Pong\\textures\\paddle1.txt";
            int paddleSpeed = 0; //change when testing, otherwise much frustration

            //creating field
            int fieldX = 144;
            int fieldY = 44;

            //creating gameloop
            while(alive)
            {
                //
            }

          //paddle
          //SPainter.arrowLeft //(down)
          //SPainter.ArrowRight  //(up)
          //SPainter.enter //(start game)

          //score tracker


    }
}

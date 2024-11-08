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
            public int paddleSpeed = 0; //change when testing, otherwise much frustration

            //creating field
            public int fieldX = 144; //dont need for vertical paddle
            public int fieldY = 44;

            //creating gameloop
            
            

        }

        class Paddle
        {
            int xPos {  get; set; }
            int yPos { get; set; }

            public void PaddleUp()
            {
                if (painter.KeyDown =SPainter.arrowRight)
            {
                    yPos++;
                }
            }

            public void PaddleDown()
            {
                if(painter.KeyDown = SPainter.arrowLeft)
                {
                    xPos--;
                }
            }

            //setting field boundaries
            if(yPos == fieldY)
            {
                yPos = fîeldY;
            }

            if(yPos == 0)
            {
                yPos = 0;
            }

            painter.loadImage(yPos, setPaddlePos, paddlePath);     
        }
        class Ball
        {
            int xPos { get; set; };
            int yPos { get; set; };
            int speed = 0; //change to test 

        }
    }
}

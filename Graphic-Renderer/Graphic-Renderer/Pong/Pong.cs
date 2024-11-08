/*using System.Reflection;
using System.Text;

namespace Graphic_Renderer
{
    public class PongGame
    {
        public int fieldX = 144; // Field declarations moved outside methods
        public int fieldY = 44;

        public void StartGame(SPainter painter)
        {
            // Variables for loops, arrays, and whatnot
            bool alive = true;
            string paddlePath = "..\\Pong\\textures\\paddle1.txt";
            string ballPath = "..\\Pong\\textures\\ball.txt";
            int paddleSpeed = 0; // Change when testing, otherwise much frustration

            // Logic for the game start can be added here
        }

        public class Paddle
        {
            public int xPos { get; set; }
            public int yPos { get; set; }

            public void PaddleUp(SPainter painter)
            {
                if (painter.KeyDown == SPainter.arrowRight)
                {
                    yPos++;
                }
            }

            public void PaddleDown(SPainter painter)
            {
                if (painter.KeyDown == SPainter.arrowLeft)
                {
                    yPos--;
                }
            }

            public void SetFieldBoundaries(int fieldY)
            {
                if (yPos >= fieldY)
                {
                    yPos = fieldY;
                }

                if (yPos <= 0)
                {
                    yPos = 0;
                }
            }

            public void RenderPaddle(SPainter painter, string paddlePath)
            {
                painter.loadImage(yPos, xPos, paddlePath);
            }
        }

        public class Ball
        {
            public int xPos { get; set; }
            public int yPos { get; set; }
            public int speed = 0; // Change to test

            public void SetFieldBoundaries(int fieldX, int fieldY)
            {
                if (xPos >= fieldX)
                {
                    xPos = fieldX;
                }

                if (xPos <= 0)
                {
                    xPos = 0;
                }

                if (yPos >= fieldY)
                {
                    yPos = fieldY;
                }

                if (yPos <= 0)
                {
                    yPos = 0;
                }
            }

            public void RenderBall(SPainter painter, string ballPath)
            {
                painter.loadImage(yPos, xPos, ballPath);
            }
        }
    }
}*/

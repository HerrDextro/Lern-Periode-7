using System.ComponentModel.Design;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Graphic_Renderer
{
    public class Pong
    {

        public bool collision = false;
        public bool running = true;
        public bool alive = true;

        SPainter painter;

        public void StartGame(SPainter painterInp)
        {
            painter = painterInp;
            painter.clear();
            painter.fillRectangle("black", 0, 0, 60, 30);
            

            string redPaddlePath = @"..\..\Pong\textures\redPaddle.txt";

            Pong pong = new Pong();
            //Paddle paddleLeft = new Paddle(painter); //add .render for each added paddle
            Paddle paddleRight = new Paddle(painter,redPaddlePath, 58, 15, SReader.arrowUp, SReader.arrowDown);
            Ball ball = new Ball(painter);

            while (running)
            {
                painter.updateFrame();
                painter.clear();
 
                int paddleSpeed = 0; // Change when testing, otherwise much frustration
                Thread.Sleep(35); //fps adjuster, standard is 35 ms
                
                paddleRight.render();
                ball.render(paddleRight);
            }
        }
    }
}

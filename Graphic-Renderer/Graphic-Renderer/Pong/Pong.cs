using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Graphic_Renderer
{
    public class Pong
    {
        SPainter painter;

        public void StartGame(SPainter painterInp)
        {
            painter = painterInp;
            painter.clear();
            painter.fillRectangle("black", 0, 0, 60, 30);
            int fieldX = 144;
            int fieldY = 44;

            Paddle paddle = new Paddle(painter);
            Ball ball = new Ball(painter);

            bool running = true;

            while (running)
            {
                painter.updateFrame();
                painter.clear();
                bool alive = true;
                //string paddlePath = "..\\Pong\\textures\\paddle1.txt";
                //string ballPath = "..\\Pong\\textures\\ball.txt";
                int paddleSpeed = 0; // Change when testing, otherwise much frustration
                Thread.Sleep(25); //fps adjuster, standard is 25 ms

                paddle.render();
                ball.render(); 

                
            }
        }
    }
}

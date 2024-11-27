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
            int fieldX = 144;
            int fieldY = 44;

            Pong pong = new Pong();
            Paddle paddle = new Paddle(painter);
            Ball ball = new Ball(painter);

            while (running)
            {
                painter.updateFrame();
                painter.clear();
 
                int paddleSpeed = 0; // Change when testing, otherwise much frustration
                Thread.Sleep(35); //fps adjuster, standard is 35 ms
                
                paddle.render();
                ball.render(paddle);
            }
        }
    }
}

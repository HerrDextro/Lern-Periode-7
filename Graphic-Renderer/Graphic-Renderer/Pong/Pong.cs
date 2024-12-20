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
        SReader reader;

        public void StartGame(SPainter painterInp, SReader reader)
        {
            string redPaddlePath = @"..\..\..\Pong\textures\redPaddle.txt";
            painter = painterInp;
            painter.clear();
            painter.fillRectangle("black", 0, 0, 60, 30);
            Pong pong = new Pong();
            //Paddle paddleLeft = new Paddle(painter); //add .render for each added paddle
            Paddle paddleRight = new Paddle(reader, painter,redPaddlePath, 58, 15, SReader.arrowUp, SReader.arrowDown);
            Ball ball = new Ball(painter);

            while (running)
            {
                painter.updateFrame();
                painter.clear();
                Thread.Sleep(35); //fps adjuster, standard is 35 ms
                paddleRight.render();
                ball.render(paddleRight);

                if (reader.KeyDown(SReader.escape)) { running = false;  }

            }

            /*if (ball.alive = false)
            {
                if (reader.KeyDown(SReader.escape))
                {

                }
            }*/

            /*while (!(reader.KeyDown(SReader.escape)))
            {
                painter.updateFrame();
            }

            painter.clear();
            Thread.Sleep(100);*/

        }
    }
}

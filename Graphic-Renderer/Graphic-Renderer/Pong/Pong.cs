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
                //string paddlePath = "..\\Pong\\textures\\paddle1.txt";
                //string ballPath = "..\\Pong\\textures\\ball.txt";
                int paddleSpeed = 0; // Change when testing, otherwise much frustration
                Thread.Sleep(25); //fps adjuster, standard is 25 ms
                
                paddle.render();
                ball.render(paddle);

                /*//ball and paddle collision checker
                if (ball.xpos >= 58 && ball.ypos < paddle.ypos && ball.ypos > paddle.ypos - 4) //leave on 59, as ball is 1 pixel
                {
                    collision = true;
                    //Console.WriteLine("collision true");
                }
                if (pong.collision == false & ball.xpos >= 59)
                {

                    ball.xspeed = 0;
                    ball.yspeed = 0;
                    //painter.clear();
                    painter.writeText("Game Over", 30, 15);
                    Console.WriteLine("GameOver");
                    running = false;

                }*/


            }
            

        }
    }
}

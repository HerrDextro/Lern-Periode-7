using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.Marshalling;
using System.Text;

namespace Graphic_Renderer
{
    public class Ball
    {
        SPainter painter;
        Pong pong = new Pong();

        public int xspeed = 1;
        public int yspeed = 1;

        string defaultBallTexture = @"C:\Users\Neo\Source\Repos\HerrDextro\Lern-Periode-7\Graphic-Renderer\Graphic-Renderer\Pong\textures\ball.txt";
        string ballTexture;

       
        /*only smart constructor here*/
        public Ball(SPainter painterInp, string? ballTexturePath = null)
        {
            painter = painterInp;
            ballTexture = ballTexturePath ?? defaultBallTexture;

        }

        
        

        public int xpos = 30; //middle fo filed is x 30 or x 29 or smt
        public int ypos = 15; //middle of field is y15 //public for collision checker  
        //PLS DONOUT RESET IN RENDER YOU DAMABSS
        

        public void render(Paddle paddle)
        {
            painter.loadImage(xpos, ypos, ballTexture);

            //ballmovement stuff plus constaint
            //int xposB = 30; //center of field
            //int yposB = 22; //center of field (I hope)

            xpos += xspeed;
            ypos += yspeed;

            if (xpos <= 2)
            {
                xspeed *= -1;
            }
            if (ypos <= 0)
            {
                yspeed *= -1;
            }
            if (paddle.ypos <= ypos && ypos <= paddle.ypos + 4 && xpos >= 57) //
            {
                xspeed *= -1;
            }
            if (xpos >= 59 && ypos !<= paddle.ypos && ypos !> paddle.ypos - 4)
            {
                xspeed = 0; 
                yspeed = 0;
                painter.fillRectangle("darkred", 0, 0, 60, 30);
                painter.writeText("Game Over", 30, 15);
            }
            if (ypos >= 29)
            {
                yspeed *= -1;
            }

        }
    }
}

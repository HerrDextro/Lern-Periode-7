using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using System.Text;

namespace Graphic_Renderer
{
    public class Ball
    {
        public Ball(SPainter painterInp)
        {
            painter = painterInp;
        }

        string ballTexture = @"C:\Users\Neo\Source\Repos\HerrDextro\Lern-Periode-7\Graphic-Renderer\Graphic-Renderer\Pong\textures\ball.txt";

        SPainter painter;

        int xpos = 30; //middle fo filed is x 30 or x 29 or smt
        int ypos = 15; //middle of field is y15
        //PLS DONOUT RESET IN RENDER YOU DAMABSS
        int xspeed = 1;
        int yspeed = 1;

        public void render()
        {
            painter.loadImage(xpos, ypos, ballTexture);

            //ballmovement stuff plus constaint
            //int xposB = 30; //center of field
            //int yposB = 22; //center of field (I hope)

            xpos += xspeed;
            ypos += yspeed;

            if (xpos <= 0)
            {
                yspeed *= -1;
            }
            if (ypos <= 0)
            {
                yspeed *= -1;
            }
            if (xpos >= 58)
            {
                xspeed *= -1;
            }
            if (ypos >= 29)
            {
                yspeed *= -1;
            }

        }

        

        
    }
}

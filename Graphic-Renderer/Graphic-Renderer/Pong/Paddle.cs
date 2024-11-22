using System.Runtime.CompilerServices;

namespace Graphic_Renderer
{
    public class Paddle
    {
        public Paddle(SPainter painterInp)
        {
            painter = painterInp;
        }
        SPainter painter;

        string paddle1Texture = @"C:\Users\Neo\Source\Repos\HerrDextro\Lern-Periode-7\Graphic-Renderer\Graphic-Renderer\Pong\textures\paddle1.txt";
        int xpos = 59; //horizontal position of paddle, the higher the further right, make no more than 59
        int ypos;

        public void render()
        {

            painter.loadImage(xpos, ypos, paddle1Texture);

            if (painter.KeyDown(SPainter.arrowUp))
            {
                ypos--;
                
            }
            if (painter.KeyDown(SPainter.arrowDown))
            {
                ypos++;
                
            }


            //field movement constraints
            if (ypos <= 0)
            {
                ypos = 0;
            }
            if (ypos >= 26) //26 bc paddle is 4 long i think
            {
                ypos = 26;
            }

        }
    }
}

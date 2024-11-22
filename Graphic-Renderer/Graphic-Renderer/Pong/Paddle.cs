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

        string texture = @"C:\Users\Neo\Source\Repos\HerrDextro\Lern-Periode-7\Graphic-Renderer\Graphic-Renderer\Pong\textures\paddle1.txt";

        int ypos = 22;

        SPainter painter;

        public Paddle(SPainter painterInp)
        {
            painter = painterInp;
        }

        public void render()
        {

            if (painter.KeyDown(SPainter.arrowLeft))
            {
                ypos++;
                
            }
            if (painter.KeyDown(SPainter.arrowRight))
            {
                ypos--;
                
            }
            

            

            if (ypos <= 0) //field boundaries
            {
                ypos = 0;
            }
            if (ypos >= 55)
            {
                ypos = 55;
            }

            painter.loadImage(30, ypos, texture);

        }
    }
}

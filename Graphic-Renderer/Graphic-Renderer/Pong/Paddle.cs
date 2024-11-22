using System.Runtime.CompilerServices;

namespace Graphic_Renderer
{
    public class Paddle
    {
        string texture = @"..\..\..\Pong\textures\paddle.txt";

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

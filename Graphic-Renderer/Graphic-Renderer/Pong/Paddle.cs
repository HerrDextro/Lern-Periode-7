using System.Runtime.CompilerServices;

namespace Graphic_Renderer
{
    public class Paddle
    {
        SPainter painter;
        string paddleTexture = @"C:\Users\Neo\Source\Repos\HerrDextro\Lern-Periode-7\Graphic-Renderer\Graphic-Renderer\Pong\textures\paddle1.txt";
        int xpos; //horizontal position of paddle, the higher the further right, make no more than 59
        public int ypos; //public for collision access
        int upKey;
        int downKey;

        /*default constructor*/
        public Paddle(SPainter painterInp) 
        {
            painter = painterInp;
            xpos = 58;
            ypos = 15;
            upKey = SPainter.arrowUp;
            downKey = SPainter.arrowDown;
        }
        

        /*SMART constructor*/
        public Paddle(SPainter painterInp, string texturePath, int initialX, int initialY, int upKey, int downKey)
        {
            painter = painterInp;
            paddleTexture = texturePath;
            xpos = initialX;
            ypos = initialY;

        }

        

        public void render()
        {
            painter.loadImage(xpos, ypos, paddleTexture);

            if (painter.KeyDown(upKey))
            {
                ypos--;
                
            }
            if (painter.KeyDown(downKey))
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

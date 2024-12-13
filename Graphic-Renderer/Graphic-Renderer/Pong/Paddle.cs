using System.Runtime.CompilerServices;

namespace Graphic_Renderer
{
    public class Paddle
    {
        SPainter painter;
        SReader reader;
        string paddleTexture = @"..\..\Graphic-Renderer\Pong\textures\paddle1.txt";
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
            upKey = SReader.arrowUp;
            downKey = SReader.arrowDown;
        }
        

        /*SMART constructor*/
        public Paddle(SPainter painterInp, string texturePath, int initialX, int initialY, int upKey, int downKey)
        {
            painter = painterInp;
            paddleTexture = texturePath;
            xpos = initialX;
            ypos = initialY;
            upKey = upKey;
            downKey = upKey;
        }

        

        public void render()
        {
            painter.loadImage(xpos, ypos, paddleTexture);

            if (reader.KeyDown(upKey))
            {
                ypos--;
                
            }
            if (reader.KeyDown(downKey))
            {
                ypos++;
                
            }

            //field movement constraints
            //field sizes as declared in Pong.cs, with neccesary modifiers

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

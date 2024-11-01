using System;
using System.Runtime.InteropServices;
using System.Threading;
using Graphic_Renderer;

namespace Graphic_Renderer
{
     internal class Program
    {
        static void Main(string[] args)
        {
            SPainter painter = new SPainter(60, 30,"Gray"); //Default for a. PC (Fullscreen): 144,44
                                                              //Default for a. PC (Small):
            painter.renderFrame();

            int xpos = 6;
            int ypos = 6;

            int xvel = 1;
            int yvel = 1;

            int playerX = 0;

            while (true)
            {
                //c++;
                painter.fillRectangle("White",0, 0, 30, 30);
                
                painter.fillRectangle("Black", xpos, ypos, 2, 2);
                painter.fillRectangle("Black", playerX, 1, 5, 2);

                xpos += xvel;
                ypos += yvel;

                if (painter.KeyDown(SPainter.arrowLeft))
                {
                    playerX += 2;
                }

                if (painter.KeyDown(SPainter.arrowRight))
                {
                    playerX -= 2;
                }



                if (ypos == 3 && playerX <= xpos && xpos <= playerX + 10)
                {
                    yvel *= -1;
                }


                if (ypos >= 27 || ypos <= 0 )
                {
                    yvel *= -1;
                }
                if (xpos >= 56 || xpos <= 0)
                {
                    xvel *= -1;
                }

                painter.updateFrame();
                painter.clear();

                Thread.Sleep(50);
            }





        }
    }
}

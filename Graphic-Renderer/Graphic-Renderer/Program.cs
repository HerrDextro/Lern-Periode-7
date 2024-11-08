using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Threading;
using Graphic_Renderer;

namespace Graphic_Renderer
{
     public class Program
    {
        static void Main(string[] args)
        {
            string[] gamelist =
            {
                "Space Invaders",
                "Pong" //neow
            };
            int cursorheight = 0;
            int cursorcool = 0;


            SPainter painter = new SPainter(60, 30,"Black"); //Default for a. PC (Fullscreen): 144,44
                                                             //Default for a. PC (Small): 60,30

            //Startup Sequence (Animation)





            //Main Loop

            for (int i = 0; i < gamelist.Length; i++)
            {
                painter.writeText(gamelist[i], 3, i + 1);

            }

            painter.renderFrame();

            while (true)
            {
                painter.updateFrame();
                painter.clear();

                painter.fillRectangle("darkgray",1,cursorheight,10,1);
                for (int i = 0; i < gamelist.Length; i++)
                {
                    painter.writeText(gamelist[i] ,3 , i+1);

                }

                if (cursorcool > 0)
                {
                    cursorcool--;
                }

                if (painter.KeyDown(SPainter.arrowDown) && cursorcool == 0)
                {
                    cursorheight += 1;
                    cursorcool = 2;
                }
                if (painter.KeyDown(SPainter.arrowUp) && cursorcool == 0)
                {
                    cursorheight -= 1;
                    cursorcool = 2;
                }

                if (painter.KeyDown(SPainter.enter))
                {
                    switch (cursorheight)
                    {
                        case 1:
                            SpaceInvader spaceInvaders = new SpaceInvader();
                            spaceInvaders.StartGame(painter);
                            painter.updateFrame();
                            break;
                        case 2:
                            Pong pong = new Pong();
                            pong.StartGame(painter);  
                            painter.updateFrame();
                            break;

                    }
                }




                Thread.Sleep(50);

            }





        }
    }
}

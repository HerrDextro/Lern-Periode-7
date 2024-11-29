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

            SPainter painter = new SPainter(60, 30, "Black"); //Default for a. PC (Fullscreen): 144,44
                                                              //Default for a. PC (Small): 60,30
            //painter.renderFrame(); //moved

            //Startup Sequence (Animation)
            //StartUp.StartUpAnim(painter);
            
            //painter.renderFrame(); //moved it here and the weird problem dissapreared. I think its bc startup anim cant render without it




            //Main Loop, only activates once the bool returned from StartUpAnim(completed) is true

            
            
                string[] gamelist =
                   {
                "Space Invaders",
                "StartUpAnim V1", //neow
                "Dev Paint",
                "Pong"
                 };
                int cursorheight = 0;
                int cursorcool = 0;

                for (int i = 0; i < gamelist.Length; i++)
                {
                    painter.writeText(gamelist[i], 3, i + 1);//deactivate when startupanim

                }

                painter.renderFrame();

                while (true)
                {
                    painter.updateFrame();
                    painter.clear(); //deactivate this when startupanim

                    painter.fillRectangle("darkgray", 1, cursorheight, 10, 1);
                    for (int i = 0; i < gamelist.Length; i++)
                    {
                        painter.writeText(gamelist[i], 3, i + 1);

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
                                StartUp startUp= new StartUp();
                                startUp.StartGame(painter);
                                painter.updateFrame();
                                break;
                            case 3:
                                DevPaint Defpaint = new DevPaint();
                                Defpaint.StartGame(painter);
                                painter.updateFrame();
                                break;
                            case 4:
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

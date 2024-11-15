using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Graphic_Renderer
{
    internal class DevPaint
    {

        int CurrentColor = 0;


        string[] ColorsArr =
        {
            "white",
            "black",
            "darkGray",
            "gray",
            "red",
            "darkred",
            "darkblue",
            "blue",
            "darkgreen",
            "green",
            "darkMagenta",
            "magenta",
            "darkCyan",
            "cyan",
            "DarkYellow",
            "yellow",
        };


        int cursorX = 0;
        int cursorY = 0;
        int counter = 0;

        int rectStartX;
        int rectStartY;



        string[,] pixels = new string[60,30];

        bool keypress = true;
        bool keypress2 = true;

        public DevPaint()
        {
            pixels = populateList(pixels,"black");
        }

        public void StartGame(SPainter painter)
        {
            painter.clear();
            painter.fillRectangle("black",0, 0, 60, 30);

            bool running = true;

            while (running)
            {
                counter++;
                
                painter.updateFrame();
                painter.clear();


                for (int i = 0; i < 60; i++)
                {
                    for (int j = 0; j < 30; j++)
                    {
                        painter.changePixel(pixels[i,j],i,j);
                    }
                }







                painter.fillRectangle(ColorsArr[CurrentColor], 56, 1, 3, 3);

                int colneg = CurrentColor - 1;
                int colnex = CurrentColor + 1;


                if (colneg < 0)
                {
                    colneg = 15;
                }
                if (colneg > 15)
                {
                    colneg = 0;
                }

                if (colnex < 0)
                {
                    colnex = 15;
                }
                if (colnex > 15)
                {
                    colnex = 0;
                }

                painter.changePixel(ColorsArr[colneg], 55, 2);
                painter.changePixel(ColorsArr[colnex], 59, 2);



                if (counter > 6)
                {
                    if (!(CurrentColor == 1))
                    {
                        painter.changePixel(ColorsArr[CurrentColor], cursorX, cursorY);
                    }
                    else
                    {
                        painter.changePixel("white", cursorX, cursorY);
                    }
                }

                if (counter == 15)
                {
                    counter = 0;
                }

                if (painter.KeyDown(SPainter.shift) && keypress)
                {
                    keypress = false;
                    rectStartX = cursorX;
                    rectStartY = cursorY;
                }
                else if (!(painter.KeyDown(SPainter.shift)) && !(keypress))
                {
                    int rectEndX = Math.Abs(rectStartX - (cursorX));
                    int rectEndY = Math.Abs(rectStartY - (cursorY));

                    for (int i = 0; i < rectEndX; i++)
                    {
                        for (int j = 0; j < rectEndY; j++)
                        {
                            pixels[rectStartX + i, rectStartY + j] = ColorsArr[CurrentColor];
                        }
                    }
                    keypress = true;
                }

                if (!keypress)
                {

                    int rectEndX = Math.Abs(rectStartX - cursorX);
                    int rectEndY = Math.Abs(rectStartY - cursorY);

                    painter.fillRectangle(ColorsArr[CurrentColor], rectStartX,rectStartY,rectEndX,rectEndY);
                }

                if (painter.KeyDown(SPainter.C) && keypress2)
                {
                    keypress2 = false;
                    rectStartX = cursorX;
                    rectStartY = cursorY;
                }
                else if (!(painter.KeyDown(SPainter.C)) && !(keypress2))
                {
                    int rectEndX = Math.Abs(rectStartX - (cursorX));
                    int rectEndY = Math.Abs(rectStartY - (cursorY));

                    painter.saveImage(rectStartX, rectStartY, rectEndX, rectEndY, @"..\..\..\DevPaint\Textures\texture.txt");

                    keypress2 = true;
                }

                if (!keypress2)
                {

                    int rectEndX = Math.Abs(rectStartX - cursorX);
                    int rectEndY = Math.Abs(rectStartY - cursorY);

                    painter.fillRectangle(ColorsArr[CurrentColor], rectStartX, rectStartY, rectEndX, rectEndY);
                }




                if (painter.KeyDown(SPainter.escape))
                {
                    running = false;
                }
                if (painter.KeyDown(SPainter.arrowLeft) && painter.KeyDown(SPainter.alt))
                {
                    CurrentColor++;
                    Thread.Sleep(100);
                }
                if (painter.KeyDown(SPainter.arrowRight) && painter.KeyDown(SPainter.alt))
                {
                    CurrentColor--;
                    Thread.Sleep(100);
                }

                if (painter.KeyDown(SPainter.space))
                {
                    pixels[cursorX, cursorY] = ColorsArr[CurrentColor];
                }


                if (!painter.KeyDown(SPainter.alt))
                {

                    if (painter.KeyDown(SPainter.arrowDown) && cursorY <= 28)
                    {
                        cursorY++;
                        Thread.Sleep(25);
                    }
                    if (painter.KeyDown(SPainter.arrowUp) && cursorY >= 1)
                    {
                        cursorY--;
                        Thread.Sleep(25);
                    }
                    if (painter.KeyDown(SPainter.arrowLeft) && cursorX <= 50)
                    {
                        cursorX++;
                        Thread.Sleep(25);
                    }
                    if (painter.KeyDown(SPainter.arrowRight) && cursorX >= 1)
                    {
                        cursorX--;
                        Thread.Sleep(25);
                    }
                }


                if (CurrentColor < 0)
                {
                    CurrentColor = 15;
                }
                if (CurrentColor > 15)
                {
                    CurrentColor = 0;
                }

                Thread.Sleep(50);

            }






        }
        private string[,] populateList(string[,] list, string input)
        {
            for (int x = 0; x < list.GetLength(0); x++)
            {
                for (int y = 0; y < list.GetLength(1); y++)
                {
                    list[x, y] = input;
                }
            }
            return list;
        }
    }

}

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
            SPainter painter = new SPainter(60, 30,"Black"); //Default for a. PC (Fullscreen): 144,44
                                                              //Default for a. PC (Small):
            painter.renderFrame();

            painter.fillRectangle("red", 0, 0, 20, 20);
            painter.fillRectangle("white", 4, 4, 12, 12);

            painter.updateFrame();





            painter.saveImage(0, 0, 20, 20, @"..\..\test.txt");

            painter.clear();

            Thread.Sleep(5000);

            painter.loadImage(5, 5, @"..\..\test.txt");

            painter.updateFrame();

            Thread.Sleep(100000);

        }
    }
}

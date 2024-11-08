using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Threading;
using Graphic_Renderer;

namespace Graphic_Renderer
{
    public class Pong
    {
        public void StartGame()
        {
            SPainter painter = new SPainter(60, 30, "black");
            Console.Clear();
            painter.renderFrame();

            painter.fillRectangle("red", 2, 2, 10, 10);

            painter.updateFrame();

            Thread.Sleep(5000);

            //how to make pong brainstorm
            //define field sizes
            //const in fieldX
            //const int fieldY

            //define paddle

        }
    }
}


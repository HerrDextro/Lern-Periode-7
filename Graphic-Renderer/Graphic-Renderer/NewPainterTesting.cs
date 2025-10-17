using Graphic_Renderer.SmartPainterFiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphic_Renderer
{
    internal class NewPainterTesting
    {
        NewPainter painter;
        
        
        public NewPainterTesting(SPainter painter, SReader reader)
        {
            this.painter = new NewPainter(painter);
        }



        public void Start()
        {
            double x = 60;
            double y = 30;

            double xvel = 0.678658675;
            double yvel = 1.0;

            painter.FillRectangle(0, 0, 119, 60, "#ffffff");
            painter.UpdateFrame();
            //Thread.Sleep(100000000);


            Point[] points = { 
                new Point { X = 59, Y = 0 }, 
                new Point { X = 10, Y = 20 }, 
                new Point { X = 35, Y = 45 }, 
                new Point { X = 83, Y = 45 }, 
                new Point { X = 108, Y = 20 } };

            Point[] points2 = {
                new Point { X = 59, Y = 5 },
                new Point { X = 10, Y = 25 },
                new Point { X = 35, Y = 50 },
                new Point { X = 83, Y = 50 },
                new Point { X = 108, Y = 25 } };

            Point[] points3 = {
                new Point { X = 59, Y = 10 },
                new Point { X = 10, Y = 30 },
                new Point { X = 35, Y = 55 },
                new Point { X = 83, Y = 55 },
                new Point { X = 108, Y = 30 } };

            painter.AntiAliasing = true;
            painter.AntiAliasingSamples = 3;
            painter.FillPolygon(points, "#ff0000");
            painter.UpdateFrame();
            Thread.Sleep(1000);
            painter.FillPolygon(points2, "#00ff0098");
            painter.UpdateFrame();
            Thread.Sleep(1000);
            painter.FillPolygon(points3, "#0000ff98");
            painter.UpdateFrame();
            Thread.Sleep(100);



            double t = 0;
            while (true)
            {
                x += xvel;
                y += yvel;

                if (x >= 116)
                {
                    xvel *= -1;
                }
                if (x <= 2)
                {
                    xvel *= -1;
                }
                if (y >= 56)
                {
                    yvel *= -1;
                }
                if ( y <= 2)
                {
                    yvel *= -1;
                }


                
                painter.FillRectangle(0, 0, 118, 58, "#3f00000f");

                t += 0.1;
                int r = (int)(Math.Sin(t) * 127 + 128), g = (int)(Math.Sin(t + 2 * Math.PI / 3) * 127 + 128), b = (int)(Math.Sin(t + 4 * Math.PI / 3) * 127 + 128);

                //painter.FillRectangle(0, 0, 118, 58, NewPainter.Util.RGB(r, g, b, 1));

                painter.ChangePixel((int)x, (int)y, NewPainter.Util.RGB(r,g,b));


                painter.UpdateFrameAsync();
                Thread.Sleep(25);
            }


            
        }
    }
}

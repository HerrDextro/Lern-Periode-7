
using Graphic_Renderer.SmartPainterFiles;
using Graphic_Renderer.SmartPainterFiles.DataObjects;
using Microsoft.VisualBasic;
using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;


using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Graphic_Renderer
{
    internal class NewPainterTesting
    {
        NewPainter painter;
        NewReader reader;
        
        public NewPainterTesting(SPainter painter, SReader reader)
        {
            this.painter = new NewPainter(painter);
            this.reader = new NewReader(reader);
        }

        public void Test1()
        {
            double x = 60;
            double y = 30;

            double xvel = 0.678658675;
            double yvel = 1.0;

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
                if (y <= 2)
                {
                    yvel *= -1;
                }



                painter.FillRectangle(0, 0, 118, 58, "#3f00000f");

                t += 0.1;
                int r = (int)(Math.Sin(t) * 127 + 128), g = (int)(Math.Sin(t + 2 * Math.PI / 3) * 127 + 128), b = (int)(Math.Sin(t + 4 * Math.PI / 3) * 127 + 128);

                //painter.FillRectangle(0, 0, 118, 58, NewPainter.Util.RGB(r, g, b, 1));

                painter.ChangePixel((int)x, (int)y, NewPainter.Util.RGB(r, g, b));


                painter.UpdateFrameAsync();
                Thread.Sleep(25);
            }
        }

        public void Test2()
        {
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
        }

        public void Test3()
        {
            painter.AntiAliasing = true;
            int brightnessGoal = new Random().Next(130,255);
            int brightness = 20;
            int brightnessLast = 21;
            int brightnessLastLast = 22;

            while (true)
            {
                if (brightness < brightnessGoal)
                {
                    brightness++;
                }
                if (brightness > brightnessGoal)
                {
                    brightness--;
                }
                if (brightness == brightnessGoal)
                {
                    brightnessGoal = new Random().Next(200, 230);
                }
                
                
                
                painter.FillRectangle(0, 0, 120, 60, "#000001");

                string c1 = NewPainter.Util.RGB(219, 128, 31, brightness-20);
                string c2 = NewPainter.Util.RGB(241, 250, 185, brightness-20);
                string c3 = NewPainter.Util.RGB(194, 43, 6, Math.Max(0,brightness-170));
                painter.FillCircle(60, 30, (double)(brightnessLast / 14), c1);
                painter.FillCircle(60, 30, (double)(brightness / 31), c2);
                painter.FillCircle(60, 30, (double)(brightnessLastLast / 9), c3);
                painter.UpdateFrame();

                brightnessLastLast = brightnessLast;
                brightnessLast = brightness;

                Thread.Sleep(10);
            }
        }

        public void Test4()
        {
            painter.FillRectangle(0, 0, 120, 60, "#010000");
            painter.UpdateFrame();

            int ctr = 0;
            DateTime last = DateTime.Now;

            while (true)
            {
                painter.FillRectangle(0, 0, 120, 120, "#ffffff");

                //Put filepath of folder of images
                painter.LoadImage(-1, 0, $@"C:\Users\alex\Downloads\maxwellCatGif\Maxwell_Cat-{ctr}.png", 0.4);

                painter.WriteText(0, 0, "FPS: " + Convert.ToString(painter.FPS), "#000000");

                int diff = (int)(DateTime.Now - last).Milliseconds;
                painter.WriteText(0, 2, "TPS: " + Convert.ToString((int)(1000 / diff)), "#000000");
                last = DateTime.Now;

                ctr++;
                if (ctr >= 75)
                {
                    ctr = 0;
                }

                painter.UpdateFrameAsync();
                Thread.Sleep(50);
            }
            Thread.Sleep(1000000);
        }

        public async void Test5()
        {
            painter.FillRectangle(0, 0, 120, 60, "#ffffff", true);
            painter.UpdateFrame();

            bool running = true;

            while (running)
            {
                //painter.FillRectangle(0, 0, 120, 60, "#ffffff");

                reader.GetKey(reader.C, () =>
                {
                    painter.FillRectangle(0, 0, 120, 60, "#ffffff", true);
                });

                reader.GetKey(reader.Escape, () =>
                {
                    running = false;
                });

                var pos = reader.GetMousePosition();

                reader.GetKey(reader.MouseRight, () =>
                {
                    painter.ChangePixel(pos.X, pos.Y, "#0000ff");
                });
                
                painter.UpdateFrame();
            }
            painter.FillCircle(10, 10, 8, "#ff0000");
            painter.UpdateFrame();
        }

        public void Start()
        {
            Action[] tests =
            {
                Test1,
                Test2,
                Test3,
                Test4,
                Test5,
            };

            tests[4]();









            
        }
    }
}








using Graphic_Renderer.SmartPainterFiles;
using System.Text.RegularExpressions;

namespace HyperPaint
{
    public class HyperPaintApp
    {
        SPainter painter;
        SReader reader;

        public HyperPaintApp(SPainter painter, SReader reader)
        {
            this.painter = painter;
            this.reader = reader;
        }

        string[] colors = { "#0a0a0a", "#e8e8eb", "#fcf805", "#fc1e05", "#05fc26" };
        int selected = 0;

        enum Tool
        {
            Writer,
            Pen,
            Eraser,
            Undef2
        }
        Tool currentTool = Tool.Pen;

        public void Start()
        {
            bool running = true;
            painter.clear();
            painter.fillRectangle("none", 0, 0, 60, 30);

            while (running)
            {
                painter.updateFrame();
                if (reader.KeyDown(SReader.escape))
                {
                    running = false;
                }

                painter.fillRectangle("#e0c29b", 0, 0, 60, 5);
                painter.writeText("<== Home", 4, 1, "#545250");

                var mouse = reader.getMousePos();
                if (1 < mouse[0] && mouse[0] < 6 && 1 < mouse[1] && mouse[1] < 3)
                {
                    painter.writeText("<== Home", 4, 1, "#3d48e3");
                    if (reader.IsLeftMouseButtonDown())
                    {
                        running = false;
                    }
                }

                bool requestColorSelection = false;

                for (int i = 0; i<colors.Length; i++)
                {
                    if (i == selected)
                    {
                        painter.fillRectangle("#b5b5b5", 56 - (i * 3), 0, 4, 4);

                        if (56 - ((i) * 3) < mouse[0] && mouse[0] < 56 - ((i) * 3) + 4 && 0 <= mouse[1] && mouse[1] < 2)
                        {
                            painter.writeText(colors[i], (56 - (i * 3)) * 2, 0, "#3d48e3");

                            if (reader.IsLeftMouseButtonDown() ||reader.IsRightMouseButtonDown())
                            {
                                requestColorSelection = true;
                            }



                        }
                        else
                        {
                            painter.writeText(colors[i], (56 - (i * 3)) * 2, 0, "#545250");
                        }


                    }
                    painter.fillRectangle(colors[i], 57 - (i * 3), 1, 2, 2);
                    if (56 - (i * 3) < mouse[0] && mouse[0] < 56 - (i * 3) + 3 && 1 < mouse[1] && mouse[1] < 4 && reader.IsLeftMouseButtonDown() )
                    {
                        selected = i;
                    }
                }



                painter.fillRectangle("#8f8f8f", 29, 1, 15, 3);

                painter.fillRectangle("#2e2e2e", 32, 1, 1, 3);
                painter.fillRectangle("#2e2e2e", 36, 1, 1, 3);
                painter.fillRectangle("#2e2e2e", 40, 1, 1, 3);

                for (Tool tool = Tool.Writer; tool <= Tool.Undef2; tool++)
                {
                    int toolIndex = (int)tool;
                    
                    if (tool == currentTool)
                    {
                        painter.fillRectangle("#b5b5b5", 29+((toolIndex)*4), 1, 3, 3);
                    }

                    if (29+(toolIndex*4) < mouse[0] && 32+(toolIndex*4) > mouse[0] && 1 <= mouse[1] && mouse[1] <= 3 && (reader.IsLeftMouseButtonDown() || reader.IsRightMouseButtonDown()))
                    {
                        currentTool = tool;
                    }
                }

                painter.writeText("      ||   // ||  /  /||::::::", 29*2, 1, "#000000");
                painter.writeText(" ABC_ ||  //  || /__/ ||:none:", 29*2, 2, "#000000");
                painter.writeText("      || |/   ||/__/  ||::::::", 29*2, 3, "#000000");

                if (mouse[1] >= 5)
                {
                    // Main Canvas Action :D
                    if (currentTool == Tool.Pen)
                    {
                        if (reader.IsRightMouseButtonDown())
                        {
                            painter.changePixel(colors[selected], mouse[0], mouse[1], true);
                        }
                    }

                    if (currentTool == Tool.Writer)
                    {
                        if (reader.IsRightMouseButtonDown())
                        {
                            Thread.Sleep(100);
                            reader.StartKeyCapture();
                            while (!reader.KeyDown(SReader.enter) && !reader.IsRightMouseButtonDown() && !reader.IsLeftMouseButtonDown())
                            {
                                painter.writeText(reader.ReadKeyCapture() + " ", mouse[0] * 2, mouse[1], colors[selected]);
                                painter.updateFrame();
                            }
                            reader.EndCapture();
                        }
                        else
                        {

                        }
                    }

                    if (currentTool == Tool.Eraser)
                    {
                        if (reader.IsRightMouseButtonDown())
                        {
                            painter.changePixel("none", mouse[0], mouse[1], true);
                        }
                    }

                }

                if(reader.KeyDown(SReader.control) || reader.KeyDown(SReader.S))
                {
                    int[][] bounds = painter.GetBounds(0, 5, 60*2, 25);

                    painter.saveImage((int)(bounds[0][0]/2), bounds[0][1], (int)(bounds[1][0]/2)- (int)(bounds[0][0] / 2)+1, bounds[1][1] - bounds[0][1]+1, "..\\..\\..\\Hyperpaint\\Images\\image.json");

                    painter.clear();
                    painter.loadImage(0, 0, "..\\..\\..\\Hyperpaint\\Images\\image.json");
                    painter.updateFrame();
                    Thread.Sleep(3000);

                }



                if (requestColorSelection)
                {
                    reader.StartKeyCapture();
                    bool validStringGiven = false;
                    while (!validStringGiven)
                    {
                        painter.writeText("       ", (56 - (selected * 3)) * 2, 0, "#3d48e3");
                        painter.writeText(reader.ReadKeyCapture(), (56 - (selected * 3)) * 2, 0, "#3d48e3");
                        painter.updateFrame();
                        if (reader.KeyDown(SReader.enter))
                        {
                            if (IsValidHexColor(reader.ReadKeyCapture()))
                            {
                                validStringGiven = true;
                                colors[selected] = reader.EndCapture();
                            }
                            else
                            {
                                validStringGiven = true;
                                reader.EndCapture();
                            }
                        }
                    }
                }


            }
        }






        public static bool IsValidHexColor(string color)
        {
            if (string.IsNullOrEmpty(color))
                return false;

            // Matches #RGB, #RRGGBB (case insensitive)
            var regex = new Regex("^#([0-9a-fA-F]{3}|[0-9a-fA-F]{6})$");
            return regex.IsMatch(color);
        }
    }


}

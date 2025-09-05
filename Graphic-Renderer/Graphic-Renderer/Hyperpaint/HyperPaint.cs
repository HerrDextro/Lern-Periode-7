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

        public void Start()
        {
            bool running = true;
            painter.clear();
            painter.fillRectangle("#ffffff", 0, 0, 60, 30);

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

                        if (56 - ((i+1) * 3) < mouse[0] && mouse[0] < 56 - ((i+1) * 3) + 4 && 0 <= mouse[1] && mouse[1] < 2)
                        {
                            painter.writeText(colors[i], (56 - (i * 3)) * 2, 0, "#3d48e3");

                            if (reader.IsLeftMouseButtonDown())
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
                    if (55 - (i * 3) < mouse[0] && mouse[0] < 55 - (i * 3) + 3 && 1 < mouse[1] && mouse[1] < 4 && reader.IsLeftMouseButtonDown() )
                    {
                        selected = i;
                    }
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

using Graphic_Renderer.SmartPainterFiles.DataObjects;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Graphic_Renderer.SmartPainterFiles
{
    public class SPainter
    {
        // Position arrays
        string[,] pixel;
        string[,] pixelLast;
        string[,] charType;
        string[,] charTypeLast;
        string[,] letterColor;
        string[,] letterColorLast;

        // Setting default colors
        string defaultTextColor;
        string defaultBGColor;

        // Setting console size
        public int consoleWidth;
        public int consoleHeight;



        [DllImport("kernel32.dll")]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        public SPainter(int width, int height, string color)
        {
            width *= 2; // Adapting coords for Console

            defaultBGColor = color;

            consoleHeight = height;
            consoleWidth = width / 2;

            defaultTextColor = "White";

            Console.CursorVisible = false;


            pixel = new string[width, height];
            pixel = populateList(pixel, color); // Initialising pixel array

            pixelLast = pixel.Clone() as string[,]; // Initialising pixelLast array

            charType = new string[width, height]; // Initialising charType array
            charType = populateList(charType, "█");

            letterColor = new string[width, height];
            letterColor = populateList(letterColor, defaultTextColor);

            letterColorLast = letterColor.Clone() as string[,];


            // for opening console

            EnableAnsi();

        }

        public int[] GetSize()
        {
            return new int[] { consoleWidth, consoleHeight };
        }

        public static void EnableAnsi()
        {
            const int STD_OUTPUT_HANDLE = -11;
            const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

            IntPtr handle = GetStdHandle(STD_OUTPUT_HANDLE);
            GetConsoleMode(handle, out uint mode);
            SetConsoleMode(handle, mode | ENABLE_VIRTUAL_TERMINAL_PROCESSING);
        }

        public void updateFrame()
        {
            for (int i = 0; i < pixel.GetLength(1); i++)
            {
                for (int j = 0; j < pixel.GetLength(0); j++)
                {
                    if (pixel[j, i] != pixelLast[j, i] || charType[j, i] != charTypeLast[j, i] || letterColor[j, i] != letterColorLast[j, i])
                    {
                        Console.SetCursorPosition(j, i);



                        if (charType[j, i] == "█")
                        {
                            setColor(getColor(j,i,pixel[j, i]));
                        }
                        else
                        {
                            setBGColor(getColor(j, i, pixel[j, i]));
                            setColor(getColor(j, i, letterColor[j, i]));
                        }


                        Console.Write(charType[j, i]);
                    }
                }
            }
            pixelLast = pixel.Clone() as string[,];
            charTypeLast = charType.Clone() as string[,];
            letterColorLast = letterColor.Clone() as string[,];
        }

        public void updateText() // Update all texts (not done by updateFrame)
        {
            for (int i = 0; i < pixel.GetLength(1); i++)
            {
                for (int j = 0; j < pixel.GetLength(0); j++)
                {
                    if (charType[j, i] != "█")
                    {
                        Console.SetCursorPosition(j, i);

                        setColor(letterColor[j, i]);
                        setBGColor(pixel[j, i]);

                        Console.Write(charType[j, i]);
                    }
                }
            }
        }
        public void clear() // Clear backlog
        {
            pixel = populateList(pixel, defaultBGColor);
            charType = populateList(charType, "█");
            letterColor = populateList(letterColor, defaultTextColor);
        }

        public void renderFrame() // Initialisation of the Console
        {
            for (int i = 0; i < pixel.GetLength(1) - 1; i++)
            {
                for (int j = 0; j < pixel.GetLength(0); j++)
                {
                    if (charType[j, i] == "█")
                    {
                        setColor(pixel[j, i]);
                    }
                    else
                    {
                        setBGColor(pixel[j, i]);
                        setColor(letterColor[j, i]);
                    }

                    Console.Write(charType[j, i]);
                }
                Console.WriteLine();
            }
            pixelLast = pixel.Clone() as string[,];
        }
        public void writeText(string text, int xpos, int ypos, string color = "defaultTextColor")
        {
            for (int i = 0; i < text.Length; i++)
            {
                try
                {
                    charType[xpos + i, ypos] = Convert.ToString(text[i]);
                    if (color != "defaultTextColor")
                    {
                        letterColor[xpos + i, ypos] = color;
                    }
                }
                catch (IndexOutOfRangeException) { }
            }
        }
        public void changeTextColor(string color)
        {
            defaultTextColor = color;
        }

        public void changePixel(string color, int xpos, int ypos, bool overrideText = false)
        {
            xpos *= 2; //Adapting coords for Console
            try
            {
                pixel[xpos, ypos] = color;
                pixel[xpos + 1, ypos] = color;
                if (overrideText)
                {
                    charType[xpos, ypos] = "█";
                    charType[xpos + 1, ypos] = "█";
                }
            }
            catch (IndexOutOfRangeException) { }
        }
        public void fillRectangle(string color, int xstart, int ystart, int xsize, int ysize)
        {
            xsize *= 2; //Adapting coords for Console
            xstart *= 2;

            for (int i = 0; i < xsize; i++)
            {
                for (int j = 0; j < ysize; j++)
                {

                    try
                    {
                        pixel[i + xstart, j + ystart] = color;
                        charType[i + xstart, j + ystart] = "█";
                    }
                    catch (IndexOutOfRangeException) { }
                }
            }
        }
        public void saveImage(int xstart, int ystart, int xsize, int ysize, string filepath)
        {
            xsize *= 2;
            xstart *= 2; //Adapting coords for Console


            Image.Pixel[][] pixels = new Image.Pixel[ysize][];
            for (int y = 0; y < ysize; y++)
            {
                pixels[y] = new Image.Pixel[xsize];
                for (int x = 0; x < xsize; x++)
                {
                    string color = pixel[xstart + x, ystart + y];
                    string letter = charType[xstart + x, ystart + y];
                    string letterCol = letterColor[xstart + x, ystart + y];

                    pixels[y][x] = new Image.Pixel { color = color, letter = letter, letterColor = letterCol };
                }
            }
            Image image = new Image() { pixels = pixels };

            string json = JsonConvert.SerializeObject(image, Formatting.Indented);

            File.WriteAllText(filepath, json);

        }
        public void loadImage(int xpos, int ypos, string filepath)
        {
            string extension = Path.GetExtension(filepath).ToLower();
            if(extension == ".txt")
            {
                string[] textInpRaw = File.ReadAllLines(filepath);
                xpos *= 2; // Adapting coords for Console

                for (int i = 0; i < textInpRaw.Length; i++)
                {
                    string[] line = textInpRaw[i].Split(";");

                    for (int j = 0; j < line.Length - 1; j++)
                    {
                        try
                        {
                            if (xpos + j >= 0 && ypos + i >= 0 && xpos + j <= consoleWidth * 2 - 1 && ypos + i <= consoleHeight * 2 - 1)
                            {
                                if (line[j] != "none")
                                {
                                    pixel[xpos + j, ypos + i] = line[j];
                                }
                            }
                        }
                        catch (IndexOutOfRangeException) { }
                    }
                }
            }
            else if (extension == ".json")
            {
                string jsonString = File.ReadAllText(filepath);

                xpos *= 2;

                // Throw if invalid JSON
                if (jsonString == null) { throw new InvalidDataException("Targeted file is empty"); }

                Image? imageJson = JsonConvert.DeserializeObject<Image>(jsonString);

                if (imageJson == null) { throw new InvalidDataException("Targeted file is empty"); }

                for (int i = 0; i < imageJson.pixels.GetLength(0); i++) // YES IT DOES COMPLAIN BUT NO IT CANNOT BE NULL
                {
                    for (int j = 0; j < imageJson.pixels[i].GetLength(0); j++)
                    {
                        try
                        {
                            if (xpos + j >= 0 && ypos + i >= 0 && xpos + j <= consoleWidth * 2 - 1 && ypos + i <= consoleHeight * 2 - 1)
                            {
                                var imgPixel = imageJson.pixels[i][j];

                                if(imgPixel.color != "none")
                                {
                                   pixel[xpos + j, ypos + i] = imgPixel.color;
                                }
                                charType[xpos + j, ypos + i] = imgPixel.letter.ToString();



                                letterColor[xpos + j, ypos + i] = imgPixel.letterColor;

                            }
                        }
                        catch (IndexOutOfRangeException) { }
                    }
                }

            }
        }

        public int[][] GetBounds(int minX, int minY, int maxX, int maxY) // ONLY USE WHEN NECESSARY!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        {
            int rows = maxX - minX;
            int cols = maxY - minY;
            
            
            int minRow = rows, maxRow = -1;
            int minCol = cols, maxCol = -1;

            for (int r = minX; r < maxX; r++)
            {
                for (int c = minY; c < maxY; c++)
                {
                    if (pixel[r, c] != "none" || charType[r,c] != "█")
                    {
                        if (r < minRow) minRow = r;
                        if (r > maxRow) maxRow = r;
                        if (c < minCol) minCol = c;
                        if (c > maxCol) maxCol = c;
                    }
                }
            }


            int[][] returnPackage = new int[2][];


            if (maxRow >= 0 && maxCol >= 0)
            {
                returnPackage[0] = new int[] { minRow, minCol };
                returnPackage[1] = new int[] { maxRow, maxCol };
            }
            else
            {
                returnPackage[0] = new[] { 0, 0 };
                returnPackage[1] = new[] { 0, 0 };
            }


            return returnPackage;

        }

        private string getColor(int x, int y, string color)
        {
            if (color == "none")
            {
                if (((int)(x / 2) + y) % 2 == 0)
                {
                    return "#ededed";
                }
                else
                {
                    return "#d6d6d6";
                }
            }
            else
            {
                return color;
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

        private string toSingleString(string[] input)
        {
            string result = string.Empty;

            for (int x = 0; x < input.Length; x++)
            {
                result += input[x] + "\n";
            }
            return result;
        }


        private string currentBG = null; // tracks current ANSI background

        private void setColor(string color)
        {
            // Try named ConsoleColor first
            if (TrySetConsoleColor(color, true)) return;

            // Try hex color
            currentBG = null;
            if (TrySetHexColor(color, true)) return;

            // Fallback
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void setBGColor(string color)
        {
            // Try named ConsoleColor first
            if (TrySetConsoleColor(color, false))
            {
                currentBG = null; // clear tracking when using ConsoleColor
                return;
            }

            // Try hex color
            if (TrySetHexColor(color, false))
            {
                currentBG = color; // store hex background for setColor
                return;
            }

            // Fallback
            Console.BackgroundColor = ConsoleColor.Black;
            currentBG = null;
        }

        // --- Helpers ---

        private bool TrySetConsoleColor(string color, bool foreground)
        {
            if (Enum.TryParse(typeof(ConsoleColor), color, true, out object result))
            {
                if (foreground) Console.ForegroundColor = (ConsoleColor)result;
                else Console.BackgroundColor = (ConsoleColor)result;

                return true;
            }
            return false;
        }

        // --- Helpers ---
        private bool TrySetHexColor(string color, bool foreground)
        {
            var match = Regex.Match(color, @"^#([0-9a-fA-F]{6})$*");
            if (!match.Success) 
                return false;

            

            int r = Convert.ToInt32(match.Groups[1].Value.Substring(0, 2), 16);
            int g = Convert.ToInt32(match.Groups[1].Value.Substring(2, 2), 16);
            int b = Convert.ToInt32(match.Groups[1].Value.Substring(4, 2), 16);

            if (foreground)
            {
                int br = 0, bg = 0, bb = 0;

                // Always read current background for ANSI combination
                if (!string.IsNullOrEmpty(currentBG))
                {
                    var bgMatch = Regex.Match(currentBG, @"^#([0-9a-fA-F]{6})$");
                    if (bgMatch.Success)
                    {
                        br = Convert.ToInt32(bgMatch.Groups[1].Value.Substring(0, 2), 16);
                        bg = Convert.ToInt32(bgMatch.Groups[1].Value.Substring(2, 2), 16);
                        bb = Convert.ToInt32(bgMatch.Groups[1].Value.Substring(4, 2), 16);
                    }
                }

                if (br != 0 || bg != 0 || bb != 0)
                    Console.Write($"\u001b[38;2;{r};{g};{b}m\u001b[48;2;{br};{bg};{bb}m");
                else
                    Console.Write($"\u001b[38;2;{r};{g};{b}m");
            }
            else
            {
                Console.Write($"\u001b[48;2;{r};{g};{b}m");
                currentBG = color; // track hex background
            }

            return true;
        }

    }
}

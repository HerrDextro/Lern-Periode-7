using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Drawing;

namespace Graphic_Renderer
{
    public class SPainter
    {
        string[,] pixel;
        string[,] pixelLast;
        string[,] charType;
        string defaultTextColor;
        string defaultBGColor;

        public int consoleWidth;
        public int consoleHeight;

        // Constructor
        public SPainter(int width, int height,string color)
        {
            width *= 2;

            defaultBGColor = color;

            consoleHeight = height;
            consoleWidth = width/2;

            
            pixel = new string[width,height];
            pixel = populateList(pixel, color);

            pixelLast = pixel.Clone() as string[,];

            charType = new string[width,height];
            charType = populateList(charType,"█");

            defaultTextColor = "White";

            Console.CursorVisible = false;
        }

        public void updateFrame()
        {
            for (int i = 0; i < pixel.GetLength(1); i++)
            {
                for (int j = 0; j < pixel.GetLength(0); j++)
                {
                    if (pixel[j, i] != pixelLast[j, i])
                    {
                        Console.SetCursorPosition(j, i);
                        if (charType[j, i] == "█")
                        {
                            setColor(pixel[j, i]);
                        }
                        else
                        {
                            setColor(defaultTextColor);
                            setBGColor(pixel[j, i]);
                        }
                        Console.Write(charType[j, i]);
                    }
                    //pixelLast[j, i] = pixel[j, i];
                }
            }
            pixelLast = pixel.Clone() as string[,];
        }
        public void updateText()
        {
            for (int i = 0; i < pixel.GetLength(1); i++)
            {
                for (int j = 0; j < pixel.GetLength(0); j++)
                {
                    if (charType[j,i] != "█")
                    {
                        Console.SetCursorPosition(j, i);

                        setColor(defaultTextColor);
                        setBGColor(pixel[j, i]);
                        
                        Console.Write(charType[j, i]);
                    }
                }
            }
        }
        public void clear()
        {
            pixel = populateList(pixel, defaultBGColor);
            charType = populateList(charType, "█");
        }

        public void renderFrame()
        {
            
            for (int i = 0; i < pixel.GetLength(1)-1; i++)
            {
                for (int j = 0; j < pixel.GetLength(0); j++)
                {
                    if (charType[j,i] == "█")
                    {
                        setColor(pixel[j, i]);
                    }
                    else
                    {
                        setColor(defaultTextColor);
                        setBGColor(pixel[j, i]);
                    }

                    Console.Write(charType[j,i]);
                }
                Console.WriteLine();
            }
            pixelLast = pixel.Clone() as string[,];
        }
        public void writeText(string text, int xpos, int ypos)
        {
            for (int i = 0; i < text.Length; i++)
            {
                charType[xpos + i, ypos] = Convert.ToString(text[i]);
            }
        }
        public void changeTextColor(string color)
        {
            defaultTextColor = color;
        }

        public void changePixel(string color, int xpos, int ypos)
        {
            xpos *= 2;
            try
            {
                pixel[xpos, ypos] = color;
                pixel[xpos + 1, ypos] = color;
            }
            catch (IndexOutOfRangeException) { }
        }
        public void fillRectangle(string color,int xstart, int ystart, int xsize, int ysize)
        {
            xsize *= 2;
            xstart *= 2;

            for (int i = 0; i < xsize; i++)
            {
                for (int j = 0;j < ysize; j++)
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

        public void saveImage(int xstart, int ystart, int xsize, int ysize,string filepath)
        {
            xsize *= 2;
            xstart *= 2;

            string[] save = new string[ysize];

            for (int i = 0; i < xsize; i++)
            {
                for (int j = 0; j < ysize; j++)
                {
                    try
                    {
                        save[j] += (pixel[xstart + i, ystart + j]+";");
                    }
                    catch { }
                }
            }
            File.WriteAllText(filepath, toSingleString(save));
        }
        public void loadImage(int xpos, int ypos, string filepath)
        {
            string[] textInpRaw = File.ReadAllLines(filepath);
            xpos *= 2;

            for (int i = 0;i < textInpRaw.Length; i++)
            {
                string[] line = textInpRaw[i].Split(";");

                for (int j = 0;j < line.Length-1; j++)
                {
                    try
                    {
                        if (pixel[xpos + j, ypos + i] != "none")
                        {
                            pixel[xpos + j, ypos + i] = line[j];
                        }
                    }
                    catch (IndexOutOfRangeException) { }
                }
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

            for (int x = 0;x < input.Length; x++)
            {
                result += (input[x] + "\n");
            }
            return result;
        }

        private void setColor(string color)
        {
            try
            {
                Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color, true);
            }
            catch (ArgumentException)
            {
                Console.ForegroundColor = ConsoleColor.White; // Default color if the string is invalid
            }
        }
        private void setBGColor(string color)
        {
            try
            {
                Console.BackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color, true);
            }
            catch (ArgumentException)
            {
                Console.BackgroundColor = ConsoleColor.White; // Default color if the string is invalid
            }
        }
    }
}
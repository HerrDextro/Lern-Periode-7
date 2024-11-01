using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Graphic_Renderer
{
    class SPainter
    {
        string[,] pixel;
        string[,] pixelLast;
        string[,] charType;
        string defaultTextColor;
        string defaultBGColor;

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        // Letters
        public const int A = 0x41;
        public const int B = 0x42;
        public const int C = 0x43;
        public const int D = 0x44;
        public const int E = 0x45;
        public const int F = 0x46;
        public const int G = 0x47;
        public const int H = 0x48;
        public const int I = 0x49;
        public const int J = 0x4A;
        public const int K = 0x4B;
        public const int L = 0x4C;
        public const int M = 0x4D;
        public const int N = 0x4E;
        public const int O = 0x4F;
        public const int P = 0x50;
        public const int Q = 0x51;
        public const int R = 0x52;
        public const int S = 0x53;
        public const int T = 0x54;
        public const int U = 0x55;
        public const int V = 0x56;
        public const int W = 0x57;
        public const int X = 0x58;
        public const int Y = 0x59;
        public const int Z = 0x5A;

        // Numbers
        public const int n0 = 0x30;
        public const int n1 = 0x31;
        public const int n2 = 0x32;
        public const int n3 = 0x33;
        public const int n4 = 0x34;
        public const int n5 = 0x35;
        public const int n6 = 0x36;
        public const int n7 = 0x37;
        public const int n8 = 0x38;
        public const int n9 = 0x39;

        // Special Characters
        public const int space = 0x20;
        public const int enter = 0x0D;
        public const int escape = 0x1B;
        public const int arrowLeft = 0x27;
        public const int arrowUp = 0x26;
        public const int arrowRight = 0x25;
        public const int arrowDown = 0x28;
        public const int shift = 0x10;
        public const int control = 0x12;
        public const int alt = 0x12;
        public const int tab = 0x09;
        public const int backspace = 0x08;
        public const int capslock = 0x14;
        public const int delete = 0x2E;


        // Constructor
        public SPainter(int width, int height,string color)
        {
            defaultBGColor = color;
            
            pixel = new string[width,height];
            pixel = populateList(pixel, color);

            pixelLast = pixel.Clone() as string[,];

            charType = new string[width,height];
            charType = populateList(charType,"█");

            defaultTextColor = "White";

            
        }
        public void updateFrame()
        {
            for (int i = 0; i < pixel.GetLength(1); i++)
            {
                for (int j = 0; j < pixel.GetLength(0); j++)
                {
                    if (pixel[j,i] != pixelLast[j,i])
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
                    pixelLast[j, i] = pixel[j, i];
                }
            }
            pixelLast = pixel.Clone() as string[,];
        }

        public void clear()
        {
            pixel = populateList(pixel, defaultBGColor);
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
        public void writeText(string text,string color, int xpos, int ypos)
        {
            for (int i = 0; i < text.Length; i++)
            {
                charType[xpos + i, ypos] = Convert.ToString(text[i]);
                //pixel[xpos + i,ypos] = color;
            }
        }
        public void changeTextColor(string color)
        {
            defaultTextColor = color;
        }

        public void changePixel(string color, int xpos, int ypos)
        {
            pixel[xpos, ypos] = color;
        }
        public void fillRectangle(string color,int xstart, int ystart, int xsize, int ysize)
        {
            xsize *= 2;
            for (int i = 0; i < xsize; i++)
            {
                for (int j = 0;j < ysize; j++)
                {
                    try
                    {
                        pixel[i + xstart, j + ystart] = color;
                        charType[i + xstart, j + ystart] = "█";
                    }
                    catch { }

                }
            }
        }

        public void saveImage(int xstart, int ystart, int xsize, int ysize,string filepath)
        {
            xsize *= 2;

            //FileStream fs = File.Create(filepath);

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
                        pixel[xpos + j, ypos + i] = line[j];
                    }
                    catch { }
                }
            }


        }


        public bool KeyDown(int keyCode)
        {
            return (GetAsyncKeyState(keyCode) & 0x8000) != 0;
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
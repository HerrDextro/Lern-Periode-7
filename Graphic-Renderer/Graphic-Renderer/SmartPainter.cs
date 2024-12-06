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
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);


        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);




        [StructLayout(LayoutKind.Sequential)]
        public struct POINT { public int X, Y; }

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindowByCaption(IntPtr zeroOnly, string lpWindowName);

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }


        private const int VK_LBUTTON = 0x01; // Virtual key code for the left mouse button
        private const int VK_RBUTTON = 0x02; // Virtual key code for the right mouse button

        [StructLayout(LayoutKind.Sequential)]
        struct Coord
        {
            public short X;
            public short Y;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        struct CONSOLE_FONT_INFO_EX
        {
            public uint cbSize;
            public uint nFont;
            public Coord dwFontSize;
            public int FontFamily;
            public int FontWeight;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string FaceName;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool bMaximumWindow, ref CONSOLE_FONT_INFO_EX lpConsoleCurrentFontEx);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        const int STD_OUTPUT_HANDLE = -11;

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

            Console.Title = "GraphicsEngine";
        }

        public int[] getMousePos() // Relative to Window in SPainter Pixels
        {
            // Get Mouse Position
            
            POINT currentPos;
            GetCursorPos(out currentPos);

            int cursorRawX = currentPos.X;
            int cursorRawY = currentPos.Y;

            // Get Console Window

            IntPtr handle = FindWindowByCaption(IntPtr.Zero, "GraphicsEngine");
            Rect rect = new Rect();

            int windowX;
            int windowY;

            if (GetWindowRect(handle, ref rect))
            {
                windowX = rect.Left;
                windowY = rect.Top;
            }
            else
            {
                windowX = 0;
                windowY = 0;
            }

            // Get Line Size
            var consoleHandle = GetStdHandle(STD_OUTPUT_HANDLE);
            var fontInfo = new CONSOLE_FONT_INFO_EX { cbSize = (uint)Marshal.SizeOf<CONSOLE_FONT_INFO_EX>() };

            double fontY;
            double fontX;

            if (GetCurrentConsoleFontEx(consoleHandle, false, ref fontInfo))
            {
                fontY = fontInfo.dwFontSize.Y;
                fontX = fontInfo.dwFontSize.Y / 2;
            }
            else
            {
                fontY = 1;
                fontX = 1;
            }

            // Substract Window - Cursor
            double cursorX = cursorRawX - windowX;
            double cursorY = cursorRawY - windowY;

            // Divide for accurate Coordinates

            int cursorXLine = Convert.ToInt32(cursorX / fontX);
            int cursorYLine = Convert.ToInt32(cursorY / fontY);

            int cursoradaptX = Convert.ToInt32(Math.Floor((cursorXLine / 2.35) - 1));
            int cursoradaptY = Convert.ToInt32(Math.Ceiling(cursorYLine * 0.85) - 3);

            return [cursoradaptX,cursoradaptY];
        }

        public bool IsLeftMouseButtonDown()
        {
            return (GetAsyncKeyState(VK_LBUTTON) & 0x8000) != 0; // Check high-order bit
        }

        public bool IsRightMouseButtonDown()
        {
            return (GetAsyncKeyState(VK_RBUTTON) & 0x8000) != 0; // Check high-order bit
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
using System;

namespace Graphic_Renderer
{
    class SPainter
    {
        string[,] pixel;

        // Constructor
        public SPainter(int width, int height)
        {
            pixel = new string[width,height];
            pixel = populateList(pixel, "Black");
        }

        public void renderFrame()
        {
            for (int i = 0; i < pixel.GetLength(1); i++)
            {
                for (int j = 0; j < pixel.GetLength(0); j++)
                {
                    setColor(pixel[j, i]);
                    Console.Write("█");
                }
                Console.WriteLine();
            }
        }
        public void changePixel(string color, int xpos, int ypos)
        {
            pixel[xpos, ypos] = color;
        }
        public void fillRectangle(string color,int xstart, int ystart, int xsize, int ysize)
        {
            for (int i = 0; i < xsize; i++)
            {
                for (int j = 0;j < ysize; j++)
                {
                    pixel[i+xstart,j+ystart] = color;
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
    }
}
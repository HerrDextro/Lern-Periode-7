using System;

namespace Graphic_Renderer
{
    class SPainter
    {
        string[,] pixel;

        // Constructor
        public SPainter()
        {
            pixel = new string[120, 100];
            pixel = populateList(pixel, "000000");
        }

        public void renderFrame()
        {
            // This will print the 2D array in a grid format
            for (int i = 0; i < pixel.GetLength(0); i++)
            {
                for (int j = 0; j < pixel.GetLength(1); j++)
                {
                    Console.Write(pixel[i, j] + " ");
                }
                Console.WriteLine();
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
    }
}
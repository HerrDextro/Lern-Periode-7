using System.Reflection;
using System.Text;
using System.Media;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System;
using System.Drawing;
using System.Globalization;
using Graphic_Renderer.SmartPainterFiles;

namespace Graphic_Renderer
{
    public class ShapeBuilder
    {
        // Storing Painter
        SPainter painter;

        // Storing all Shapes
        bool[][] shape1 =
        {
            new bool[] {true,true},
            new bool[] {true,true}
        };

        bool[][] shape2 =
        {
            new bool[] {true,true,true,true }
        };

        bool[][] shape3 =
        {
            new bool[] {false,true,true},
            new bool[] {true,true,false}
        };

        bool[][] shape4 =
        {
            new bool[] {true,true,false},
            new bool[] {false,true,true}
        };

        bool[][] shape5 =
        {
            new bool[] {true,true,true},
            new bool[] {false,false,true}
        };

        bool[][] shape6 =
        {
            new bool[] {true,true,true},
            new bool[] {true,false,false}
        };

        bool[][] shape7 =
        {
            new bool[] {true,true,true},
            new bool[] {false,true,false}
        };

        public string color;
        string[] ColorsArr =
{
            "red",
            "darkred",
            "darkgreen",
            "green",
            "darkMagenta",
            "magenta",
            "DarkYellow",
            "blue",
            "darkBlue"
        };


        public bool[,] getShape()
        {
            Random rand = new Random();
            color = ColorsArr[rand.Next(ColorsArr.Length)];

            bool[][][] shapes = { shape1, shape2, shape3, shape4, shape5, shape6, shape7 };

            return To2D(shapes[rand.Next(shapes.Length)]);
        }


        private static T[,] To2D<T>(T[][] source)
        {
            try
            {
                int FirstDim = source.Length;
                int SecondDim = source.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular

                var result = new T[FirstDim, SecondDim];
                for (int i = 0; i < FirstDim; ++i)
                    for (int j = 0; j < SecondDim; ++j)
                        result[i, j] = source[i][j];

                return result;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("The given jagged array is not rectangular.");
            }
        }

    }
}
using System.Reflection;
using System.Text;
using System.Media;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography.X509Certificates;

namespace Graphic_Renderer
{
    public class Shape
    {
        // Storing Painter
        SPainter painter;

        // Storing Color & position
        public string color = "black";

        int xpos = 1;
        public int ypos = 1;

        int keyCool = 0;
        

        // Storing texture for shape
        public bool[,] texture;

        ShapeBuilder factory;

        public Shape(SPainter painterInput)
        {
            painter = painterInput;
            factory = new ShapeBuilder();
            texture = factory.getShape();
            color = factory.color;
        }

        public void writeShape()
        {
            for (int x = 0; x < texture.GetLength(0); x++)
            {
                for (int y = 0; y < texture.GetLength(1); y++)
                {
                    if (texture[x,y])
                    {
                        painter.changePixel(color, x+xpos, y+ypos);
                    }
                }
            }
        }

        public List<int[]> lockShape()
        {
            List<int[]> coords = new List<int[]>();
            for (int x = 0; x < texture.GetLength(0); x++)
            {
                for (int y = 0; y < texture.GetLength(1); y++)
                {
                    if (texture[x, y])
                    {
                        coords.Add(new int[] { x+xpos-1, y+ypos-1 });
                    }
                }
            }
            return coords;
        }
        
        public bool isColliding(Block[,] board)
        {
            if (ypos + texture.GetLength(1) >= 29)
            {
                return true;
            }
            for (int x = 0; x < texture.GetLength(0); x++)
            {
                for (int y = 0; y < texture.GetLength(1); y++)
                {
                    if (texture[x, y])
                    {
                        if (board[x + xpos - 1,y + ypos].occupied)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        
        
        public void updatePosition()
        {
            ypos++;
        }

        public void checkKeyboardInputs()
        {
            if (keyCool > 0)
            {
                keyCool--;
            }
            
            
            if (painter.KeyDown(SPainter.arrowUp) && keyCool == 0)
            {
                texture = RotateLeft(texture);
                keyCool = 15;
            }
            if (painter.KeyDown(SPainter.arrowDown) && keyCool == 0)
            {
                texture = RotateRight(texture);
                keyCool = 15;
            }

            if (painter.KeyDown(SPainter.arrowLeft) && keyCool == 0)
            {
                xpos++;
                keyCool = 15;
            }
            if (painter.KeyDown(SPainter.arrowRight) && keyCool == 0)
            {
                xpos--;
                keyCool = 15;
            }

            if (xpos < 1)
            {
                xpos = 1;
            }


            if (xpos > (15-texture.GetLength(0)))
            {
                xpos = 15- texture.GetLength(0);
            }
        }

        private static bool[,] RotateRight(bool[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            bool[,] rotated = new bool[cols, rows];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    rotated[col, rows - 1 - row] = matrix[row, col];
                }
            }

            return rotated;
        }

        private static bool[,] RotateLeft(bool[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            bool[,] rotated = new bool[cols, rows];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    rotated[cols - 1 - col, row] = matrix[row, col];
                }
            }

            return rotated;
        }


    }
}
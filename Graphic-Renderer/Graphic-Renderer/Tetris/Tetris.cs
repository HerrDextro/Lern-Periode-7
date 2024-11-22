using System.Reflection;
using System.Text;
using System.Media;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography.X509Certificates;

namespace Graphic_Renderer
{
    public class Tetris
    {
        // Storing Painter
        SPainter painter;

        // Storing Gamegrid
        public Block[,] board = new Block[14, 28]; // Size of gamegrid

        // Utility Variables
        bool running = true;


        Shape shape;

        public Tetris(SPainter painterInput)
        {
            painter = painterInput;
            FillBlocks();

            shape = new Shape(painter);

        }

        public void StartGame()
        {
            int time = 0;
            while (running)
            {
                // Basic Painter updates
                painter.updateFrame();
                painter.clear();
                Thread.Sleep(10);

                time++;


                // Rendering Board
                painter.fillRectangle("darkgray", 0, 0, 16, 30);

                for (int x = 0; x < board.GetLength(0); x++)
                {
                    for (int y = 0; y < board.GetLength(1); y++)
                    {
                        painter.changePixel(board[x, y].color, x + 1, y + 1);
                    }
                }

                // Rendering Dropping Shape

                shape.checkKeyboardInputs();
                shape.writeShape();

                if (time % 10 == 0) // 100
                {
                    shape.updatePosition();
                }

                if (shape.isColliding(board))
                {
                    List<int[]> list = new List<int[]>(shape.lockShape());

                    for (int i = 0; i < list.Count; i++)
                    {
                        board[list[i][0], list[i][1]].LockBlock(shape.color);
                    }

                    shape = new Shape(painter);

                    if (shape.isColliding(board) && shape.ypos < 2)
                    {
                        running = false;
                    }


                }

                for (int i = 0; i < board.GetLength(1); i++)
                {
                    if (isRowDone(i))
                    {
                        for (int j = i; j >= 0; j--)
                        {
                            moveLine(j);
                        }
                    }
                }
            }


            painter.clear();
            painter.loadImage(5, 5, @"..\..\..\Tetris\Textures\endscreen.txt");

            painter.writeText("Press esc to continue", 15, 20);
            //painter.writeText("Music: youtube.com/@TenM", 15, 21);

            while (!(painter.KeyDown(SPainter.escape)))
            {
                painter.updateFrame();
            }

            painter.clear();
            Thread.Sleep(100);
        }
    





    

        private void moveLine(int line)
        {
            for (int i = 0;i < board.GetLength(0); i++)
            {
                try
                {
                    board[i, line] = board[i, line - 1];
                }
                catch (IndexOutOfRangeException)
                {
                    board[i, line] = new Block(painter);
                }
            }
        }




        private bool isRowDone(int row)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                if (!board[i, row].occupied)
                {
                    return false;
                }
            }
            return true;
        }
        private void FillBlocks()
        {
            for (int r = 0; r < board.GetLength(0); r++)
            {
                for (int c = 0; c < board.GetLength(1); c++)
                {
                    board[r, c] = new Block(painter);
                }
            }
        }
    }
}
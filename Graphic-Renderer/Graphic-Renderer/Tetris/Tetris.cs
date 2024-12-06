using System.Reflection;
using System.Text;
using System.Media;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography.X509Certificates;
using System.Numerics;

namespace Graphic_Renderer
{
    public class Tetris
    {
        // Storing Painter
        SPainter painter;
        SReader reader;

        // Storing Gamegrid
        public Block[,] board = new Block[14, 28]; // Size of gamegrid

        // Utility Variables
        bool running = true;

        int score = 0;
        int level = 0;

        bool boosting = false;

        Shape shape;

        public Tetris(SPainter painterInput, SReader readerInp)
        {
            painter = painterInput;
            reader = readerInp;

            FillBlocks();

            shape = new Shape(painter,reader);

        }

        private void playMusic()
        {
            string audioPath = @"..\..\..\Tetris\audio\tetris.wav";
            SoundPlayer player = new SoundPlayer(audioPath);

            Thread musicThread = new Thread(() =>
            {
                player.PlayLooping();
                while (musicRunning)
                {
                    Thread.Sleep(100); // Small delay to prevent busy-waiting
                }
                player.Stop(); 
            });

            musicThread.IsBackground = true;
            musicThread.Start();
        }
        volatile bool musicRunning = true;

        public void StartGame()
        {
            int time = 0;

            Thread musicThread = new Thread(playMusic);
            musicThread.Start();

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

                painter.fillRectangle("black", 16, 2, 20, 3);
                painter.writeText("Your Level: " + Convert.ToString(level), 33, 2);
                painter.writeText("Your Score: " + Convert.ToString(score), 33, 3);
                painter.writeText("Next Level: " + Convert.ToString(Math.Pow((level + 1), 2)*100), 33, 4);

                if (score/100 >= Math.Pow((level+1),2))
                {
                    painter.updateText();
                    level++;
                }


                // Rendering Dropping Shape

                shape.checkKeyboardInputs();
                shape.writeShape();

                if ((reader.KeyDown(SReader.space) || reader.IsLeftMouseButtonDown())&& !boosting)
                {
                    boosting = true;
                }

                if (time % (25-level) == 0 || boosting) // 100
                {
                    shape.updatePosition();
                }

                if (shape.isColliding(board))
                {
                    
                    List<int[]> list = new List<int[]>(shape.lockShape());


                    painter.updateText();

                    for (int i = 0; i < list.Count; i++)
                    {
                        board[list[i][0], list[i][1]].LockBlock(shape.color);
                        boosting = false;
                    }

                    shape = new Shape(painter,reader);

                    if (shape.isColliding(board) && shape.ypos < 2)
                    {
                        running = false;
                    }


                }

                for (int i = 0; i < board.GetLength(1); i++)
                {
                    if (isRowDone(i))
                    {
                        painter.fillRectangle("white", 1, i+1, 14, 1);
                        painter.updateFrame();

                        score += 100;
                        for (int j = i; j >= 0; j--)
                        {
                            moveLine(j);
                        }
                    }
                }
            }


            painter.clear();
            painter.loadImage(5, 5, @"..\..\..\Tetris\Textures\endscreen.txt");

            painter.writeText("Press esc to continue", 15, 19);
            painter.writeText("Music: Sma$her - Tetris Phonk", 15, 20);

            while (!(reader.KeyDown(SReader.escape)))
            {
                painter.updateFrame();
            }

            musicRunning = false;

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
                    board[i, line] = new Block(painter,i%2==0);
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
                    board[r, c] = new Block(painter,r%2==0);
                }
            }
        }
    }
}
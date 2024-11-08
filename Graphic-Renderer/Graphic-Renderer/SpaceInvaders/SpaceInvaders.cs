using System.Reflection;
using System.Text;

namespace Graphic_Renderer
{
    public class SpaceInvader
    {
        public void StartGame(SPainter painter)
        {
            painter.clear();
            painter.fillRectangle("Cyan",0,0,5,4);

            int[,] arr =
            {
                {0, 1, 0, 1, 0},
                {0, 1, 1, 1, 0},
                {1, 1, 1, 1, 1},
                {1, 0, 0, 0, 1}
            };

            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    if (arr[i, j]  == 0)
                    {
                        painter.changePixel("black", j,i);
                    }
                    
                }
            }

            painter.saveImage(0, 0, 5, 4, "C:\\Users\\alex\\Source\\Repos\\HerrDextro\\Lern-Periode-7\\Graphic-Renderer\\Graphic-Renderer\\SpaceInvaders\\textures\\enemy01.txt");


            painter.updateFrame();
            Thread.Sleep(10000);
        }
    }
}

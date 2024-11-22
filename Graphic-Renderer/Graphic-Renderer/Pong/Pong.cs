using System.Reflection;
using System.Text;

namespace Graphic_Renderer
{
    public class Pong
    {
        SPainter painter;

        public void StartGame(SPainter painterInp)
        {
            painter = painterInp;

            painter.clear();
            painter.fillRectangle("black", 0, 0, 60, 30);

            bool runnning = true;

            while (runnning)
            {

            }

        }
    }
}

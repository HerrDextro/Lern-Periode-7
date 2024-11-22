using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using System.Text;

namespace Graphic_Renderer
{
    public class Ball
    {
        public Ball(SPainter painterInp)
        {
            painter = painterInp;
        }

        SPainter painter;

        int xpos;
        int ypos;

        public void render()
        {
            painter.changePixel("darkCyan", xpos, ypos);
            ypos--;
        }

        public bool stillExists()
        {
            return ypos >= 0;
        }
    }
}

using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using System.Text;

namespace Graphic_Renderer
{
    public class Bullet
    {
        SPainter painter;

        int xpos;
        int ypos;
        
        public Bullet(int xinp, SPainter paintInp)
        {
            xpos = xinp;
            ypos = 60;

            painter = paintInp;

        }
        
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

using System.ComponentModel.Design;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Graphic_Renderer
{
    public class StartUp
    {
        SPainter painter;

        public static void Run(SPainter painter)
        {
            painter.writeText("Hello World!", 30, 15);
            painter.clear();
            
            painter.fillRectangle("darkred", 0, 0, 60, 30);
            Thread.Sleep(1000);
        }
    }
}
using System.ComponentModel.Design;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Graphic_Renderer
{
    public class StartUp
    {
        SPainter painter;

        public static void Run()
        {
            painter.writeText("Hello World!", 30, 15);
            //Console.WriteLine("Hello World!");
            Thread.Sleep(1000);
        }
    }
}
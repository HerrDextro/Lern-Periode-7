using System.ComponentModel.Design;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Graphic_Renderer
{
    public class StartUp
    {
        SPainter painter;
           

        public void StartGame(SPainter painterInp)
        {
            //first red edge turning into red snow static
            //then red snow static turns into solid block
            //out of block there spawns a swiss flag (in middle of screen)
            //swiss flag moves left, revealing smart painter text
            //swiss flag is left and smartpainter text right

            painter = painterInp;
            painter.clear();
            painter.fillRectangle("black", 0, 0, 60, 30);

            bool running = true;

            while(running)
            {
                //INSERT STARTUP ANIMATIONS HERE
                //painter.fillRectangle("darkred", 0, 0, 60, 30);
                //painter.fillRectangle("black", 2, 2, 56, 25);

                //Thread.Sleep(2000); //dictates how long startup anim will be
            }

            
            
            






           
          

        }
    }
}
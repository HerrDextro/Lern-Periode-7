using System.ComponentModel.Design;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Graphic_Renderer
{
    public class StartUp
    {
        SPainter painter;
        StartUp startUp = new StartUp();    

        public static void StartUpAnim(SPainter painter)
        {
            bool startUpAnimComplete = false; //set this to true only when start up animation is completed
            

            //INSERT STARTUP ANIMATIONS HERE
            painter.fillRectangle("darkred", 0, 0, 60, 30);
            painter.updateFrame();
            
            Thread.Sleep(2000); //dictates how long startup anim will be 
          
      





        }
    }
}
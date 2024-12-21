using System.ComponentModel.Design;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Graphic_Renderer
{
    public class StartUp
    {
        SPainter painter;
        Helper helper = new Helper();
        

        string smartPainterText = @"..\..\..\StartUpAnim\textures\SPtext.txt";
        string swissFlag = @"..\..\..\StartUpAnim\textures\SwissFlagV5.txt";

        public void StartUpAnim(SPainter painterInp)
        {
            painter = painterInp;
            painter.clear();
            painter.fillRectangle("black", 0, 0, 60, 30);

            int sfXpos = 23;  //x23 y8 is middle for 13x13 swiss flag
            int sfYpos = 8; 
            int sptXpos = 23; //final position for smart painter text
            int sptYpos = 8;
            bool running = true;

            while(running)
            {
                painter.updateFrame();
                painter.clear();
               

                painter.loadImage(sfXpos, sfYpos, swissFlag);
                if (sfXpos >= 10)
                {
                    sfXpos--;
                }
                if (sfXpos == 9)
                {
                    painter.loadImage(sptXpos, sptYpos, smartPainterText);
                }

                //helper.SmartGrower(painter); //grows single center pixel to swiss flag
                

                


                Thread.Sleep(50); //animation speed (default: 50ms)
            }

           












        }
    }
}
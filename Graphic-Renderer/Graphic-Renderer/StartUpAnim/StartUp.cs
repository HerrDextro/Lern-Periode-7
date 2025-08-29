using System.ComponentModel.Design;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Graphic_Renderer.SmartPainterFiles;

namespace Graphic_Renderer
{
    public class StartUp
    {
        SPainter painter;
        SReader reader;
        Helper helper = new Helper();
        

        string smartPainterText = @"..\..\..\StartUpAnim\textures\SPtext.txt";
        string swissFlag = @"..\..\..\StartUpAnim\textures\SwissFlagV5.txt";

        public void StartUpAnim(SPainter painterInp, SReader readerInp)
        {
            painter = painterInp;
            reader = readerInp;

            painter.clear();
            painter.fillRectangle("black", 0, 0, 60, 30);

            int sfXpos = 23;  //x23 y8 is middle for 13x13 swiss flag
            int sfYpos = 8; 
            int sptXpos = 23; //final position for smart painter text
            int sptYpos = 8;
            bool running = true;
            bool animationDone = false;

            while(running)
            {
                painter.updateFrame();
                painter.clear();


                painter.loadImage(sfXpos, sfYpos, swissFlag);
                if (sfXpos >= 8)
                {
                    sfXpos--;
                }
                if (sfXpos == 7)
                {
                    painter.clear();
                    painter.loadImage(sptXpos, sptYpos, smartPainterText);
                    painter.loadImage(7, sfYpos, swissFlag);
                    running = false;
                    painter.updateFrame();
                    Thread.Sleep(1000);
                }

                
                
                if (reader.KeyDown(SReader.escape))
                {
                    running = false;
                }
                



                Thread.Sleep(50); //animation speed (default: 50ms)
            }

           












        }
    }
}
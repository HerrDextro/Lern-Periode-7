using System.ComponentModel.Design;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Graphic_Renderer.SmartPainterFiles;

namespace Graphic_Renderer
{
    public class Helper
	{
		SPainter painter;


		//change to relatives after testing
		string SmartpainterText = @"..\..\..\StartUpAnim\textures\SPtext.txt";
		string swissFlag = @"C:\Users\Neo\Source\Repos\HerrDextro\Lern-Periode-7\Graphic-Renderer\Graphic-Renderer\StartUpAnim\textures\SwissFlagV5.txt";

		public static void DotDot(SPainter painterInp) //for the dot to dot (damit es punkten kann) (unbenutzt)
		{
			SPainter painter;
			painter = painterInp;
			painter.fillRectangle("black", 0, 0, 60, 30);
			
		}

		public void FlaggusInflatus(SPainter painterInp) //Logic broken do not call
		{
            painter = painterInp;
            int xpos = 30;
            int ypos = 15;
            int xsize = 1;
            int ysize = 1;

            painter.fillRectangle("blue", xpos, ypos, xsize, ysize);

			while (xpos >= 23 && ypos >= 8)
			{
                if (xpos >= 23)
                {
                    
                    xpos -= 3;
                    xsize += 2;
                    painter.fillRectangle("blue", xpos, ypos, xsize, ysize);
                    painter.updateFrame();

                }
                /*if (ypos >= 8)
                {
                    
                    ypos += 3;
                    ysize += 2;
                    painter.fillRectangle("blue", xpos, ypos, xsize, ysize);
                    painter.updateFrame();
                }*/
            }
			
			
        }
    }
}
using System.Net.NetworkInformation;
using Graphic_Renderer.SmartPainterFiles;

namespace Graphic_Renderer
{
    public class GCI
    {
        SPainter painter;
        SReader reader;
        Map map;

        string rigTXT = @"C:\Users\alex\Source\Repos\HerrDextro\Lern-Periode-7\Graphic-Renderer\Graphic-Renderer\GrassCutterIdle\Textures\rig.txt";

        public GCI(SPainter painter, SReader reader)
        {
            this.painter = painter;
            this.reader = reader;

            map = new Map(painter,reader);


        }
        public void startGame()
        {
            map.renderNorm();
            painter.loadImage(0,0,rigTXT);
            painter.updateFrame();




            Thread.Sleep(100000);
        }
    }
}
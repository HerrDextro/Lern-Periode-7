using Graphic_Renderer.SmartPainterFiles;

namespace Graphic_Renderer
{
    public class Map
    {
        SPainter painter;
        SReader reader;
        
        public Map(SPainter painter, SReader reader)
        {
            this.painter = painter;
            this.reader = reader;
        }
        public void renderNorm()
        {
            painter.clear();
            painter.fillRectangle("blue", 0, 0, 60, 30);
            painter.updateFrame();

            Thread.Sleep(1000);

        }
    }
}
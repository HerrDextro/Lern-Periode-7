using Graphic_Renderer.SmartPainterFiles;

namespace Graphic_Renderer
{
    public class GPlayer
    {
        SPainter painter;
        SReader reader;

        int xpos = 0;
        int ypos = 0;
        
        public GPlayer(SPainter painter, SReader reader)
        {
            this.painter = painter;
            this.reader = reader;
        }
        public void startGame()
        {
            painter.fillRectangle("darkgreen",0,0,10,10);
            painter.updateText();
            painter.updateFrame();
            Thread.Sleep(100000);
        }
    }
}
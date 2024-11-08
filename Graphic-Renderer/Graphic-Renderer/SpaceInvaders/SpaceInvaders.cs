using System.Reflection;
using System.Text;

namespace Graphic_Renderer
{
    public class SpaceInvader
    {
        public void StartGame(SPainter painter)
        {
            painter.clear();
            painter.fillRectangle("red",0,0,10,10);
            painter.updateFrame();
            Thread.Sleep(10000);
        }
    }
}

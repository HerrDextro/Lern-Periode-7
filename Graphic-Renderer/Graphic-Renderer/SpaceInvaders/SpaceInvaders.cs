using System.Reflection;
using System.Text;

namespace Graphic_Renderer
{
    public class SpaceInvader
    {

        SPainter painter;
        public void StartGame(SPainter painterInp)
        {
            
            
            painter = painterInp;

            Player player = new Player(painter);

            painter.clear();
            painter.fillRectangle("black", 0, 0, 60, 30);

            bool running = true;
            
            while (running)
            {
                painter.updateFrame();
                painter.clear();
                Thread.Sleep(25);

                player.render();

            }




        }
    }
}

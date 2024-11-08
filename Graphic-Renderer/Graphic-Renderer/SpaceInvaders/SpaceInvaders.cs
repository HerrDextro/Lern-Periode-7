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

            bool running = true;
            
            while (running)
            {
                painter.updateFrame();
                painter.clear();

                player.render();

            }




        }
    }
}

using System.Reflection;
using System.Text;

namespace Graphic_Renderer
{
    public class SpaceInvader
    {

        SPainter painter;
        List<Enemy> enemys = new List<Enemy>();


        public void StartGame(SPainter painterInp)
        {
            
            
            painter = painterInp;

            Player player = new Player(painter);

            painter.clear();
            painter.fillRectangle("black", 0, 0, 60, 30);

            bool running = true;
            int counter = 90;


            while (running)
            {
                counter ++;
                
                painter.updateFrame();
                painter.clear();
                Thread.Sleep(25);

                player.render();

                if (counter == 160)
                {
                    enemys.Add(new Enemy(painter,false));
                    counter = 0;
                }

                for (int i = 0; i < enemys.Count; i++)
                {
                    enemys[i].render((counter%20==0),player);
                    if (!(enemys[i].stillExists()))
                    {
                        enemys.RemoveAt(i);
                        break;
                    }
                    if (enemys[i].stillRunning())
                    {
                        running = false;
                    }
                }

            }




        }
    }
}

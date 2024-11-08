using System.Runtime.CompilerServices;

namespace Graphic_Renderer
{
    public class Player
    {
        string texture = @"..\..\..\SpaceInvaders\textures\player.txt";

        int xpos = 1;
        int cool = 0;

        List<Bullet> bullets = new List<Bullet>();

        SPainter painter;

        public Player(SPainter painterInp)
        {
            painter = painterInp;
        }

        public void render()
        {
            painter.loadImage(xpos, 25, texture);

            if (cool > 0)
            {
                cool--;
            }

            if (painter.KeyDown(SPainter.arrowLeft) && cool == 0)
            {
                xpos++;
                cool = 2;
            }
            if (painter.KeyDown(SPainter.arrowRight) && cool == 0)
            {
                xpos--;
                cool = 2;
            }
            if (painter.KeyDown(SPainter.enter))
            {
                bullets.Add(new Bullet(xpos + 2, painter));
            }

            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].render();
                if (!(bullets[i].stillExists()))
                {
                    bullets.RemoveAt(i);
                    break;
                }
            }

        }
    }
}

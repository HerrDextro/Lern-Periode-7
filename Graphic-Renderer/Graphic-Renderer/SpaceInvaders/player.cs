using System.Runtime.CompilerServices;

namespace Graphic_Renderer
{
    public class Player
    {
        string texture = @"..\..\..\SpaceInvaders\textures\player.txt";

        int xpos = 1;
        int cool = 0;

        int bulletCool = 0;

        List<Bullet> bullets = new List<Bullet>();

        SPainter painter;
        SReader reader;

        public Player(SPainter painterInp, SReader readerInp)
        {
            painter = painterInp;
            reader = readerInp;
        }

        public List<Bullet> getBullets()
        {
            return bullets;
        }

        public void destroyBullet(int index)
        {
            bullets.RemoveAt(index);
        }

        public void render()
        {
            if (cool > 0)
            {
                cool--;
            }

            if (bulletCool > 0)
            {
                bulletCool--;
            }

            if (reader.KeyDown(SReader.arrowLeft) && cool == 0)
            {
                xpos++;
                cool = 2;
            }
            if (reader.KeyDown(SReader.arrowRight) && cool == 0)
            {
                xpos--;
                cool = 2;
            }
            if (reader.KeyDown(SReader.arrowUp) && bulletCool == 0)
            {
                bullets.Add(new Bullet(xpos + 2, painter));
                bulletCool = 7;
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

            if (xpos <= 0)
            {
                xpos = 0;
            }
            if (xpos >= 55)
            {
                xpos = 55;
            }

            painter.loadImage(xpos, 25, texture);

        }
    }
}

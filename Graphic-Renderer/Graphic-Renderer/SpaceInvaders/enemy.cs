using System.Reflection;
using System.Text;

namespace Graphic_Renderer
{
    public class Enemy
    {
        int xpos = 30;
        int ypos;

        int xvel = 1;
        int yvel = 0;

        int defXvel = 1;

        int healthPoints;

        int blinkCD = 0;

        string texture;
        string textureHIT;
        string renderTexture;

        SPainter painter;

        public Enemy(SPainter painterInp, bool isBoss)
        {
            ypos = 1;
            
            painter = painterInp;

            Random random1 = new Random();

            if (!isBoss)
            {
                switch (random1.Next(3))
                {
                    case 0:
                        texture = @"..\..\..\SpaceInvaders\textures\enemy01.txt";
                        textureHIT = @"..\..\..\SpaceInvaders\textures\enemy01HIT.txt";
                        healthPoints = 2;
                        break;
                    case 1:
                        texture = @"..\..\..\SpaceInvaders\textures\enemy02.txt";
                        textureHIT = @"..\..\..\SpaceInvaders\textures\enemy02HIT.txt";
                        healthPoints = 2;
                        break;
                    case 2:
                        texture = @"..\..\..\SpaceInvaders\textures\enemy03.txt";
                        textureHIT = @"..\..\..\SpaceInvaders\textures\enemy03HIT.txt";
                        healthPoints = 4;
                        break;
                }
            }
            else
            {
                texture = @"..\..\..\SpaceInvaders\textures\enemy04.txt";
                textureHIT = @"..\..\..\SpaceInvaders\textures\enemy04HIT.txt";
                healthPoints = 10;
            }

            renderTexture = texture;

            Random random2 = new Random();
            if (random1.Next(2) == 0)
            {
                xvel = -1;
                defXvel = -1;
            }


        }

        public void render(bool move,Player player)
        {
            renderGraphics();
            renderCollisions(player);

            if (move)
            {
                renderPosition();
            }



        }
        public bool stillRunning()
        {
            return ypos > 20;
        }


        public bool stillExists()
        {
            return healthPoints > 0;
        }

        private void renderGraphics()
        {
            painter.loadImage(xpos, ypos,renderTexture);
            if (blinkCD > 0)
            {
                blinkCD--;
            }
            if (blinkCD == 0)
            {
                renderTexture = texture;
            }
            
        }

        private void renderCollisions(Player player)
        {
            List<Bullet> bullets = player.getBullets();
            
            for (int i = 0; i < bullets.Count; i++)
            {
                int bulletX = bullets[i].getXpos();
                int bulletY = bullets[i].getYpos();

                if ((xpos <= bulletX && bulletX <= (xpos + 5)) && (ypos <= bulletY && bulletY <= ypos + 4))
                {
                    healthPoints--;
                    player.destroyBullet(i);
                    renderTexture = textureHIT;
                    blinkCD = 4;
                }
            }
        }


        private void renderPosition()
        {
            xpos += xvel;
            ypos += yvel;

            if (xpos >= 54)
            {
                xvel = 0;
                yvel = 1;
            }

            if (xpos <= 1)
            {
                xvel = 0;
                yvel = 1;
            }

            if ((ypos-6)%12 == 0 && defXvel == 1)
            {
                ypos++;
                yvel = 0;
                xvel = defXvel*-1;
            }
            else if (ypos % 12 == 0 && defXvel == -1)
            {
                ypos++;
                yvel = 0;
                xvel = defXvel * -1;
            }
        }


    }
}
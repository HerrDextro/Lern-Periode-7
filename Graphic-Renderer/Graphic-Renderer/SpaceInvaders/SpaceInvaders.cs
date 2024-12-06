using System.Reflection;
using System.Text;
using System.Media;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Graphic_Renderer
{
    
    
    public class SpaceInvader
    {

        SPainter painter;
        List<Enemy> enemys = new List<Enemy>();

        int difficulty = 0;


        private void playMusic()
        {
            string audioPath = @"..\..\..\SpaceInvaders\audio\guardian.wav";
            SoundPlayer player = new SoundPlayer(audioPath);

            Thread musicThread = new Thread(() =>
            {
                player.PlayLooping();
                while (musicRunning)
                {
                    Thread.Sleep(100); // Small delay to prevent busy-waiting
                }
                player.Stop();
            });

            musicThread.IsBackground = true;
            musicThread.Start();
        }
        volatile bool musicRunning = true;

        public void StartGame(SPainter painterInp, SReader reader)
        {
            
            painter = painterInp;

            Player player = new Player(painter,reader);

            painter.clear();
            painter.fillRectangle("black", 0, 0, 60, 30);
            bool running = true;
            int EnemyMoving = 0;

            Thread musicThread = new Thread(playMusic);
            musicThread.Start();

            while (running)
            {
                EnemyMoving ++;
                
                painter.updateFrame();
                painter.clear();


                Thread.Sleep(25);

                player.render();


                /*
                if (counter == 160)
                {
                    enemys.Add(new Enemy(painter,false));
                    counter = 0;
                }
                */

                if (enemys.Count() == 0)
                {
                    SummonWave();
                }

                for (int i = 0; i < enemys.Count; i++)
                {
                    enemys[i].render((EnemyMoving%(20) == 0),player);

                    if (EnemyMoving >= 20)
                    {
                        EnemyMoving = 0;
                    }

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
            painter.clear();

            painter.writeText("Press esc to continue", 15, 20);
            painter.writeText("Music: youtube.com/@TenM", 15, 21);

            painter.loadImage(4, 4, @"..\..\..\..\Graphic-Renderer\SpaceInvaders\textures\deathScreen.txt");

            while (!(reader.KeyDown(SReader.escape)))
            {
                painter.updateFrame();
            }
            musicRunning = false;

            painter.clear();
            Thread.Sleep(100);
        }

        private void SummonWave()
        {
            difficulty++;

            double levelPercent = (difficulty*100)/30;

            int difficultySpawn = Math.Clamp(difficulty, 1, 12);
            Random rnd = new Random();

            int enemysNumber = rnd.Next(1, difficultySpawn + 1); 

            int spawnRange = 55;

            if (enemysNumber == 1)
            {
                enemys.Add(new Enemy(painter, 1, spawnRange / 2)); // Spawn in the center (X=30).
            }
            else
            {
 
                int spacing = spawnRange / (enemysNumber + 1); 

                for (int i = 0; i < enemysNumber; i++)
                {
                    int level;
                    int random = rnd.Next(1, 100);

                    if ((random >= levelPercent))
                    {
                        level = 1;
                    }
                    else
                    {
                        level = 2;
                    }

                    if (random <= 5)
                    {
                        level = 3;
                    }

                    int x = spacing * (i + 1);

                    enemys.Add(new Enemy(painter, level, x));
                }
            }

            

        }
    }
}

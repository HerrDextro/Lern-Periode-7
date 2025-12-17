using Graphic_Renderer.Chess;
using Graphic_Renderer.SmartPainterFiles;
using HyperPaint;
using System;
using System.Data;
using System.Media;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;


namespace Graphic_Renderer
{
    public class Program
     {
        static void Main(string[] args)
        {
            string[] gamelist =
            {
                "StartUpAnim",
                "Parkour", //neow
                "Dev Paint",
                "Tetris",
                "Pong",
                "Space Invaders",
                "The Rig Idle",
                "Hyperpaint",
                "Chess",
                "Neo",
                "NewPainter"
            };
            int cursorheight = 0;
            int cursorcool = 0;


            SPainter painter = new SPainter(60, 30,"Black"); //Default for a. PC (Fullscreen): 144,44
                                                             //Default for a. PC (Small): 60,30
            SReader reader = new SReader();

            painter.renderFrame();

            //Startup Sequence (Animation)
            //StartUp.StartUpAnim(painter);
            StartUp startUp = new StartUp();
            startUp.StartUpAnim(painter, reader);
            painter.updateFrame();


            //Main Loop
            


            Program program = new Program();
            //program.PlayMusic();

            program.ShowText(painter, gamelist);

            while (true)
            {
                painter.updateFrame();
                painter.clear();

                painter.fillRectangle("#b0918f", 1,cursorheight,10,1);
                program.ShowText(painter, gamelist);


                if (cursorcool > 0)
                {
                     cursorcool--;
                }

                if (reader.KeyDown(SReader.arrowDown) && cursorcool == 0)
                {
                    cursorheight += 1;
                    cursorcool = 2;
                }
                if (reader.KeyDown(SReader.arrowUp) && cursorcool == 0)
                {
                    cursorheight -= 1;
                    cursorcool = 2;
                }



                if (reader.KeyDown(SReader.enter) && cursorheight >= 1 && cursorheight <= gamelist.Length)
                {
                    
                    
                    switch (cursorheight)
                    {
                        case 1:
                            //StartUp startUp = new StartUp();
                            //startUp.StartUpAnim(painter,reader);
                            //painter.updateFrame();
                            break;
                        case 2:
                            Parkour parkour = new Parkour(painter,reader);
                            parkour.StartGame();
                            painter.updateFrame();
                            break;
                        case 3:
                            DevPaint devPaint = new DevPaint();
                            devPaint.StartGame(painter,reader);
                            painter.updateFrame();
                            break;
                        case 4:
                            Tetris tetris = new Tetris(painter, reader);
                            tetris.StartGame();
                            painter.updateFrame();
                            break;
                        case 5:
                            Pong pong = new Pong();
                            pong.StartGame(painter, reader);
                            painter.updateFrame();
                            break;
                        case 6:
                            SpaceInvader spaceInvaders = new SpaceInvader();
                            spaceInvaders.StartGame(painter, reader);
                            painter.updateFrame();
                            break;
                        case 7:
                            GCI gci = new GCI(painter,reader);
                            gci.startGame();
                            painter.updateFrame();
                            break;
                        case 8:
                            HyperPaintApp paint = new(painter, reader);
                            paint.Start();
                            painter.updateFrame();
                            break;
                        case 9:
                            Chess.Chess chess = new(painter, reader);
                            chess.StartGame();
                            painter.updateFrame();
                            break;
                        case 10:
                            /*ChessGame chessGame = new ChessGame(painter, reader);
                            chessGame.StartChessGame();
                            painter.updateFrame();*/
                            break;
                        case 11:
                            NewPainterTesting newTest = new(painter, reader);
                            newTest.Start();
                            painter.updateFrame();
                            break;
                    }
                    program.PlayMusic();

                    program.ShowText(painter, gamelist);



                    
                }
                Thread.Sleep(50);
            }
        }

        private void ShowText(SPainter painter, string[] gamelist)
        {
            for (int i = 0; i < gamelist.Length; i++)
            {
                painter.writeText(gamelist[i], 3, i + 1, "#ffffff");
            }
            painter.writeText("Music: Jono - Please Hold", 3, 29);
        }

        private void PlayMusic()
        {
            string audioPath = @"..\..\..\Audio\mainscreen.wav";
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
    }

}

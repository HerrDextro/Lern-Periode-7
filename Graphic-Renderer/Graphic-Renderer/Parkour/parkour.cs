using Graphic_Renderer;

public class Parkour
{
    SPainter painter;
    SReader reader;

    double globalspeed = 0.2;
    double globalgrav = 0.08;
    

    public Parkour(SPainter painterInp, SReader readerInp)
    {
        painter = painterInp;
        reader = readerInp;
    }
    public void StartGame()
    {
        painter.clear();
        painter.fillRectangle("black", 0, 0, 15, 30);
        painter.updateText();


        Platform platform1 = new Platform(painter, 6, 20);
        Platform platform2 = new Platform(painter, 50, 20);
        Player player = new Player(painter, reader, platform1,platform2);

        while (true)
        {
            painter.updateFrame();
            painter.clear();
            
            player.RenderGraphics();
            player.RenderPhysics(globalspeed,globalgrav);

            platform1.RenderPhysics(globalspeed);
            platform1.RenderGraphics();

            platform2.RenderPhysics(globalspeed);
            platform2.RenderGraphics();

            platform1.Checkpos();
            platform2.Checkpos();

            Thread.Sleep(25);

            if (player.isFlying())
            {
                globalspeed += 0.3;
                globalgrav = 0.02;
            }
            else
            {
                globalspeed = 0.5;
                globalgrav = 0.08;
            }
        }
        

        



    }





}
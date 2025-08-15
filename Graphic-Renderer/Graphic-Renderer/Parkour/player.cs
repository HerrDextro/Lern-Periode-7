using Graphic_Renderer.SmartPainterFiles;
using System.Net.Mail;
using System.Runtime.InteropServices.Marshalling;

public class Player
{
    SPainter painter;
    SReader reader;
    Platform platform1;
    Platform platform2;

    double ypos = 15;
    double yvel = 0;
    int boostcount = 0;
    int hovercount = 0;
    bool forceground = false;
    double score = 0;


    int fuel = 13;
    int durability = 13;

    List<Bullet> bullets = new List<Bullet>();

    string textureJet = @"..\..\..\..\Graphic-Renderer\Parkour\Textures\player.txt";
    string textureFly = @"..\..\..\..\Graphic-Renderer\Parkour\Textures\flyer.txt";

    string texture;


    public Player(SPainter painterInp, SReader readerInp, Platform platform1,Platform platform2)
    {
        painter = painterInp;
        reader = readerInp;
        this.platform1 = platform1;
        this.platform2 = platform2;

        texture = textureJet;
    }

    public void RenderGraphics()
    {
        painter.loadImage(15, Convert.ToInt32(ypos), texture);
    }

    public void RenderPhysics(double globalspeed,double globalgrav)
    {
        double alt = 25;
        forceground = false;
        yvel += globalgrav;
        
        if (ypos >= 25 || colliding(platform1))
        {
            forceground = true;
            alt = platform1.ypos;
        }
        if (ypos >= 25 || colliding(platform2))
        {
            forceground = true;
            alt = platform2.ypos;
        }



        if (forceground)
        {
            ypos = alt -4;
            yvel = 0;

            fuel = 13;
            durability = 13;
            //ypos = 30;
        }

        if (ypos <= 1)
        {
            ypos = 1;
            yvel *= -0.2;
        }

        if ((reader.IsLeftMouseButtonDown() || reader.KeyDown(SReader.arrowUp) && fuel > 0))
        {
            yvel-= 0.24;
            boostcount++;

            if (boostcount >= 3)
            {
                bullets.Add(new Bullet(painter, 15, ypos));
                boostcount = 0;
                fuel--;
            }
        }

        if ((reader.IsRightMouseButtonDown() || reader.KeyDown(SReader.arrowDown)) && durability > 0)
        {
            texture = textureFly;
            hovercount++;
            if (hovercount >= 3)
            {
                durability--;
                hovercount = 0;
            }
        }
        else
        {
            texture = textureJet;
        }

        for (int i = 0; i < bullets.Count; i++)
        {
            bullets[i].RenderGraphics();
            bullets[i].RenderPhysics(globalspeed);
            if (bullets[i].ShouldntExists())
            {
                bullets.Remove(bullets[i]);
                break;
            }
        }

        painter.fillRectangle("red", 0, 0, fuel, 1);
        painter.fillRectangle("darkmagenta", 47, 0, durability, 1);

        ypos += yvel;

        score += (globalspeed);
        painter.writeText($"Score: {Convert.ToString(Convert.ToInt32(score))}m", 32, 0);
        painter.updateText();
    }
    public bool colliding(Platform platform)
    {
        return (platform.xpos < 20 && platform.xpos > -3) && ypos+4 >= platform.ypos; 
    }

    public bool isFlying()
    {
        return texture == textureFly;
    }
}
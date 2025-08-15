using Graphic_Renderer.SmartPainterFiles;

public class Platform
{
    SPainter painter;

    public double ypos;
    public double xpos;

    string texture = @"..\..\..\..\Graphic-Renderer\Parkour\Textures\platform.txt";


    public Platform(SPainter painterInp, int xposInp, double yposInp)
    {
        painter = painterInp;
        xpos = xposInp;
        ypos = yposInp;
    }

    public void RenderGraphics()
    {
        painter.loadImage(Convert.ToInt32(xpos), Convert.ToInt32(ypos), texture);
    }

    public void RenderPhysics(double globalspeed)
    {
        xpos -= globalspeed; //Later Globalspeed
    }

    public void Checkpos()
    {
        if( ypos >= 30 || xpos <= -18)
        {
            xpos = 60;
            Random rand = new Random();
            ypos = rand.Next(15,20);
        }
    }

    





}
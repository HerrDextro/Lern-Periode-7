using Graphic_Renderer.SmartPainterFiles;

public class Bullet
{
    SPainter painter;
    SReader reader;

    double ypos;
    double xpos;



    public Bullet(SPainter painterInp, int xposInp, double yposInp)
    {
        painter = painterInp;
        xpos = xposInp;
        ypos = yposInp+3;
    }

    public void RenderGraphics()
    {
        painter.changePixel("red", Convert.ToInt32(xpos), Convert.ToInt32(ypos));
    }

    public void RenderPhysics(double globalspeed)
    {
        xpos -= globalspeed; //Later Globalspeed
        ypos += 1;
    }

    public bool ShouldntExists()
    {
        return ypos >= 30 || xpos <= 0;
    }

    





}
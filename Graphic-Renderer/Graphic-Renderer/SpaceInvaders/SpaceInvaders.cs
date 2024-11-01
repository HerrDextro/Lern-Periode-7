namespace Graphic_Renderer
{
public class SpaceInvaders
    {
        public SpaceInvaders()
        {

        }
        public void runGame()
        {
            SPainter painter = new SPainter(60, 30, "black");
            Console.Clear();
            painter.renderFrame();

            painter.fillRectangle("red", 2, 2, 10, 10);

            painter.updateFrame();

            Thread.Sleep(5000);


        }
    }

}
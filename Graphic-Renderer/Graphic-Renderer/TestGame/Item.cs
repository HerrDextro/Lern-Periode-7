/*using System.Reflection;
using System.Text;

namespace Graphic_Renderer
{
    public class Item
    {
        public int xpos { get; private set; }
        public int ypos { get; private set; }
        SPainter painter;

        public Item(SPainter painterInp)
        {
            painter = painterInp;
            Random random = new Random();
            xpos = random.Next(0, 60); // Assuming a random position within 60 units
            ypos = 0; // Starting at the top of the screen
        }

        public void render()
        {
            painter.changePixel("green", xpos, ypos);
            ypos++; // Moving downwards
        }
    }
}*/

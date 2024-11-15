namespace Graphic_Renderer
{
    public class ItemCollectorPlayer
    {
        SPainter painter;
        int xpos;
        int ypos;
        int width;
        int height;

        public ItemCollectorPlayer(SPainter painterInp)
        {
            painter = painterInp;
            xpos = 30;
            ypos = 28;
            width = 5;  // Example width
            height = 5; // Example height
        }

        public void render()
        {
            painter.changePixel("blue", xpos, ypos);
        }

        public bool checkCollision(int itemXpos, int itemYpos, int itemWidth, int itemHeight)
        {
            // Check if bounding boxes overlap
            bool collisionX = xpos < itemXpos + itemWidth && xpos + width > itemXpos;
            bool collisionY = ypos < itemYpos + itemHeight && ypos + height > itemYpos;
            return collisionX && collisionY;
        }
    }

    public class Item
    {
        public int xpos { get; private set; }
        public int ypos { get; private set; }
        public int width { get; private set; }
        public int height { get; private set; }
        SPainter painter;

        public Item(SPainter painterInp)
        {
            painter = painterInp;
            Random random = new Random();
            xpos = random.Next(0, 60);
            ypos = 0;
            width = 3;  // Example width
            height = 3; // Example height
        }

        public void render()
        {
            painter.changePixel("green", xpos, ypos);
            ypos++;
        }
    }

}

/*
namespace Graphic_Renderer
{
    public class ItemCollector
    {
        SPainter painter;
        List<Item> items = new List<Item>();
        
        public void StartGame(SPainter painterInp)
        {
            painter = painterInp;

            ItemCollectorPlayer player = new ItemCollectorPlayer(painter);

            painter.clear();
            painter.fillRectangle("black", 0, 0, 60, 30);

            bool running = true;
            int counter = 0;

            while (running)
            {
                counter++;
                painter.updateFrame();
                painter.clear();
                Thread.Sleep(25);

                player.render();

                if (counter % 100 == 0)
                {
                    items.Add(new Item(painter)); // Should now work correctly
                }

                for (int i = 0; i < items.Count; i++)
                {
                    items[i].render();
                    if (player.checkCollision(items[i].xpos, items[i].ypos)) // Accessing public properties
                    {
                        items.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }
}
*/
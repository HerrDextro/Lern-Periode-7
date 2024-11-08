using System.Runtime.CompilerServices;

namespace Graphic_Renderer
{
    public class Player
    {
        string texture = @"..\..\Graphic-Renderer\SpaceInvaders\textures\player.txt";

        int xpos = 0;

        SPainter painter;

        public Player(SPainter painterInp)
        {
            painter = painterInp;
        }

        public void render()
        {
            painter.loadImage(xpos, 10, texture);
        }
    }
}

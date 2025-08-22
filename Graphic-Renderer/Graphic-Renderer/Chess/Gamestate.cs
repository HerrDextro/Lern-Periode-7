using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphic_Renderer.Chess
{
    internal record Gamestate
    {
        public string blackUID;
        public string whiteUID;
        public string roomID;
        public string currentUID;
        public string chatUID;
        public int[][] boardState;

    }
}

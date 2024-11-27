using System.Reflection;
using System.Text;
using System.Media;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography.X509Certificates;

namespace Graphic_Renderer
{
    public class Block
    {
        // Storing Painter
        SPainter painter;

        // Storing Color & occupation
        public string color = "black";

        public bool occupied = false;

        string texturePath;

        public Block(SPainter painterInput,bool colorInput)
        {
            painter = painterInput; 

            if (colorInput)
            {
                color = "black";
            }
            else
            {
                color = "black";
            }


        }

        public void LockBlock(string colorInp)
        {
            color = colorInp;
            occupied = true;
        }




    }
}
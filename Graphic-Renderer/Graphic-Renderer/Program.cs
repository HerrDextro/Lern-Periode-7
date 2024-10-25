namespace Graphic_Renderer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello, World from Alexander!");

            SPainter painter = new SPainter(120, 29); //Default for a. PC (Fullscreen): 144,44
                                                      //Default for a. PC (Small):
            painter.fillRectangle("Blue", 10, 10, 10, 5);

            painter.renderFrame();


            Console.ReadLine();





        }
    }
}

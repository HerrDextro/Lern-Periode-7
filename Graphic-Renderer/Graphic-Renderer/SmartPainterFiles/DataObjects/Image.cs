using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Graphic_Renderer.SmartPainterFiles.DataObjects
{
    public record Image
    {
        public Pixel[][]? pixels {  get; set; }


        public record Pixel
        {
            public required string color { get; set; }
            public required string letter { get; set; }
            public required string letterColor { get; set; }
        }
    }
}

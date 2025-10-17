using System;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TextCopy;
using static Graphic_Renderer.SmartPainterFiles.DataObjects.Image;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Graphic_Renderer.SmartPainterFiles
{
    internal class NewPainter
    {
        // C++ code imports
        [DllImport("GraphicRendererNewCPP.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void ProcessArray([In]  PhysicalPixel[] pixel, int x, int y);

        [DllImport("GraphicRendererNewCPP.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void InstanceRuntimePixelArray(int x, int y);

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        private struct PhysicalPixel
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
            public string backgroundColor;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
            public string foregroundColor;

            public uint chartype;
        }

        // Constructors and fields
        private int _consoleWidth;
        private int _consoleHeight;

        private PhysicalPixel[,] _physicalPixels;

        public bool AntiAliasing = false;
        public int AntiAliasingSamples = 3;
        public NewPainter(SPainter oldPainter)
        {
            var dimensions = oldPainter.GetSize();

            _consoleWidth = dimensions[0] * 2;
            _consoleHeight = dimensions[1];


            _physicalPixels = new PhysicalPixel[_consoleWidth,_consoleHeight];
            _physicalPixels = Populate2DArrayWithPixels(_physicalPixels);

            InstanceRuntimePixelArray(_consoleWidth, _consoleHeight);

        }

        public NewPainter(int consoleWidth, int consoleHeight)
        {
            _consoleWidth = consoleWidth * 2;
            _consoleHeight = consoleHeight;

            _physicalPixels = new PhysicalPixel[_consoleWidth, _consoleHeight];
            _physicalPixels = Populate2DArrayWithPixels(_physicalPixels);

            InstanceRuntimePixelArray(_consoleWidth, _consoleHeight);
        }




        /// <summary>
        /// Updates the console to the current buffer status
        /// </summary>
        public void UpdateFrame()
        {
            var singleDimensionalPixels = Flatten(_physicalPixels);

            ProcessArray(singleDimensionalPixels, _consoleWidth, _consoleHeight);
        }

        /// <summary>
        /// Updates the console to the current buffer status, does not wait for completion and is thus better for performance and allows for seperation of fps and tps
        /// </summary>
        private Task? _updateTask;
        private readonly object _lock = new();

        public void UpdateFrameAsync()
        {
            lock (_lock)
            {
                if( _updateTask == null  || _updateTask.IsCompleted)
                {
                    _updateTask = Task.Run(UpdateFrameAsyncBacker);
                }
                else
                {
                    return;
                }
            }
        }

        private async Task UpdateFrameAsyncBacker()
        {
            var singleDimensionalPixels = Flatten(_physicalPixels);

            ProcessArray(singleDimensionalPixels, _consoleWidth, _consoleHeight);
        }

        /// <summary>
        /// Changes a pixel on the screen buffer
        /// </summary>
        /// <param name="x">X coordinate of changed pixel</param>
        /// <param name="y">Y coordinate of changed pixel</param>
        /// <param name="color">color of the new pixel</param>
        /// <param name="overrideText">if the pixel should override the currently present symbol with a square</param>
        /// <param name="checkNecessary">Check for color validity, only set to false if you really know what you are doing</param>
        public void ChangePixel(int x, int y, string color, bool overrideText = false, bool checkNecessary = true)
        {
            // Calculate pixel change necessary
            decimal decimalY = y / 2;
            int actualY = (int)Math.Floor(decimalY);
            bool isForeground = !(y % 2 == 0);

            // Calculate true color
            if (checkNecessary)
            {
                ValidateRGB(color);
            }

            bool colorHasTransparencyValue = HasTransparencyValue(color);
            


            if (isForeground)
            {
                if (colorHasTransparencyValue)
                {
                    _physicalPixels[x,actualY].foregroundColor = MergeColors(_physicalPixels[x,actualY].foregroundColor, color);
                }
                else
                {
                    _physicalPixels[x, actualY].foregroundColor = color;
                }
            }
            else
            {
                if (colorHasTransparencyValue)
                {
                    _physicalPixels[x, actualY].backgroundColor = MergeColors(_physicalPixels[x,actualY].backgroundColor, color);
                }
                else
                {
                    _physicalPixels[x, actualY].backgroundColor = color;
                }
            }

            if (overrideText)
            {
                _physicalPixels[x, actualY].chartype = (uint)char.ConvertToUtf32(" ", 0);
            }
        }



        public (string, char) GetPixel(int x, int y)
        {
            decimal decimalY = y / 2;
            int actualY = (int)Math.Floor(decimalY);
            bool isForeground = !(y % 2 == 0);

            if (isForeground)
            {
                return (_physicalPixels[x, actualY].foregroundColor, (char)_physicalPixels[x, actualY].chartype);
            }
            else
            {
                return (_physicalPixels[x, actualY].foregroundColor, (char)_physicalPixels[x, actualY].chartype);
            }
        }

        /// <summary>
        /// Fills a rectangular surface in the buffer with pixels of a certaian color
        /// </summary>
        /// <param name="xStart">Starting point for the rectangle on the X axis</param>
        /// <param name="yStart">Statring point for the rectangle on the Y axis</param>
        /// <param name="xEnd">Endpoint of the rectangle on the X axis</param>
        /// <param name="yEnd">Endpoint of the rectangle on the Y axis</param>
        /// <param name="color">Color of the rectangle as a hex string</param>
        /// <param name="overrideText">If the rectangle should overwrite text that's currently there</param>
        public void FillRectangle(int xStart, int yStart, int xEnd, int yEnd, string color, bool overrideText = false)
        {
            ValidateRGB(color);
            for (int x  = xStart; x < xEnd; x++)
            {
                for (int y = yStart; y < yEnd; y++)
                {
                    ChangePixel(x, y, color, overrideText, false);
                }
            }
        }

        public void FillCircle(int xCenter, int yCenter, int radius, string color, bool overrideText = false)
        {
            ValidateRGB(color);
        }

        /// <summary>
        /// Saves an image into a .spt file at the specified position
        /// </summary>
        /// <param name="xStart">Starting point for the rectangle on the X axis</param>
        /// <param name="yStart">Starting point for the rectangle on the Y axis</param>
        /// <param name="xEnd">Endpoint of the rectangle on the X axis</param>
        /// <param name="yEnd">Endpoint of the rectangle on the Y axis</param>
        /// <param name="path">Filepath to which the file will be saved</param>
        /// <param name="greenscreenColor">This color should be read as transparent, nullable</param>
        public void SaveImage(int xStart, int yStart, int xEnd, int yEnd, string path, string greenscreenColor = null)
        {
            string header = "{version:1}";
            string body = "";

            for (int y = xStart; y < xEnd; y++)
            {
                body += "[";
                for (int x = yStart; x < yEnd; x++)
                {
                    if (greenscreenColor == null)
                    {
                        body += $"<{_physicalPixels[x, y].foregroundColor}|{_physicalPixels[x, y].backgroundColor}|{_physicalPixels[x,y].chartype}>";
                    }
                    else
                    {
                        string foregroundColor = _physicalPixels[x, y].foregroundColor == greenscreenColor ? "#00000000" : _physicalPixels[x, y].foregroundColor;
                        string backgroundColor = _physicalPixels[x, y].backgroundColor == greenscreenColor ? "#00000000" : _physicalPixels[x, y].backgroundColor;
                        body += $"<{foregroundColor}|{backgroundColor}|{_physicalPixels[x, y].chartype}>";
                    }
                }
                body += "]";
            }

            string spt = header + body;
            File.WriteAllText(path += ".spt", spt);
        }

        /// <summary>
        /// Load a .spt image at a specified location from a filepath
        /// </summary>
        /// <param name="x">X Position for the file to be loaded in</param>
        /// <param name="y">Y Position for the file to be loaded in</param>
        /// <param name="path">Path where the .spt file is located</param>
        public void LoadImage(int x, int y, string path)
        {
            string spt = File.ReadAllText(path);

            var match = Regex.Match(spt, @"\{([^}]*)\}\s*((?:\[[^\]]*\]\s*)+)", RegexOptions.Singleline | RegexOptions.Multiline);
            string header = match.Groups[1].Value;
            string body = match.Groups[2].Value;

            var lines = Regex.Matches(body, @"\[[^\]]*\]", RegexOptions.Singleline | RegexOptions.Multiline);

            for (int yIndividual = 0; yIndividual < lines.Count; yIndividual++)
            {
                Match line = lines[yIndividual];

                string lineValue = line.Value;
                var pixels = Regex.Matches(lineValue, @"<(.*?)\|(.*?)\|(.*?)>", RegexOptions.Singleline | RegexOptions.Multiline);

                

                for (int xIndividual = 0; xIndividual < pixels.Count; xIndividual++)
                {
                    Match pixel = pixels[xIndividual];
                    PhysicalPixel physicalPixel = new PhysicalPixel()
                    {
                        foregroundColor = pixel.Groups[1].Value,
                        backgroundColor = pixel.Groups[2].Value,
                        chartype = (uint)char.ConvertToUtf32(pixel.Groups[3].Value, 0)
                    };

                    _physicalPixels[x+xIndividual, y+yIndividual] = physicalPixel;
                }
            }
        }

        /// <summary>
        /// Fill a polygon using a Collection of points. TODO: Fix transparent anti aliasing
        /// </summary>
        /// <param name="polygon">A collection of points</param>
        /// <param name="color">The color of the shape</param>
        public void FillPolygon(IEnumerable<Point> polygon, string color, bool overrideText = false)
        {
            ValidateRGB(color);
            
            // Get the bounding box to limit the pixels we check
            int minX = int.MaxValue, minY = int.MaxValue;
            int maxX = int.MinValue, maxY = int.MinValue;

            foreach (var p in polygon)
            {
                minX = Math.Min(minX, p.X);
                minY = Math.Min(minY, p.Y);
                maxX = Math.Max(maxX, p.X);
                maxY = Math.Max(maxY, p.Y);
            }

            // Clamp bounding box to the grid boundaries
            minX = Math.Max(0, minX);
            minY = Math.Max(0, minY);
            maxX = Math.Min(_consoleWidth - 1, maxX);
            maxY = Math.Min(_consoleHeight*2 - 1, maxY);

            Point[] aliasingPolygon = new Point[polygon.Count()];
            if (AntiAliasing)
            {
                int idx = 0;
                foreach (Point point in polygon)
                {
                    aliasingPolygon[idx] = new Point(point.X * AntiAliasingSamples, point.Y * AntiAliasingSamples) ;
                    idx++;
                }
            }

            // Iterate over the pixels in the constrained bounding box
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {

                    if (AntiAliasing)
                    {
                        int samleInside = 0;
                        int sampleCount = AntiAliasingSamples * AntiAliasingSamples;
                        for (int Pixely = 0; Pixely < AntiAliasingSamples; Pixely++)
                        {
                            for (int Pixelx = 0; Pixelx < AntiAliasingSamples; Pixelx++)
                            {
                                int ActualX = x * AntiAliasingSamples + Pixelx;
                                int ActualY = y * AntiAliasingSamples + Pixely;
                                
                                if (IsPointInPolygon(ActualX,ActualY,aliasingPolygon))
                                {
                                    samleInside++;
                                }
                            }
                        }
                        if (samleInside > 0) 
                        {
                            int percentage = (int)((samleInside / (float)sampleCount) * 255);

                            ChangePixel(x, y, MergeColorWithAlpha(color, percentage), overrideText, false);
                        }
                    }
                    else
                    {
                        if (IsPointInPolygon(x, y, polygon))
                        {
                            ChangePixel(x, y, color, overrideText, false);
                        }
                    }

                }
            }
        }

        /// <summary>
        /// I aint a mathematician so i do not have any clue as to what is going on here
        /// </summary>
        public bool IsPointInPolygon(int pixelX, int pixelY, IEnumerable<Point> polygon)
        {
            // Get the vertices into an array for easier indexing
            Point[] vertices = polygon.ToArray();
            int n = vertices.Length;
            if (n < 3) return false; // A polygon must have at least 3 vertices

            int crossings = 0;

            // Iterate through all edges (i to i+1, including n-1 to 0)
            for (int i = 0; i < n; i++)
            {
                Point p1 = vertices[i];
                Point p2 = vertices[(i + 1) % n]; // Gets the next vertex, wraps back to 0 for the last edge

                // 1. Check if the horizontal ray at pixelY intersects the vertical span of the edge (p1 to p2)
                // Note: The check must be strict on one side (e.g., p1.Y <= pixelY) to ensure
                // vertices on the ray are counted exactly once.
                bool yInBounds = ((p1.Y <= pixelY && p2.Y > pixelY) || (p2.Y <= pixelY && p1.Y > pixelY));

                if (yInBounds)
                {
                    // 2. Calculate the X-coordinate of the intersection point
                    // This is derived from the line equation:
                    // x_intersect = p1.X + (p2.X - p1.X) * (pixelY - p1.Y) / (p2.Y - p1.Y)

                    // Handle horizontal edges (p1.Y == p2.Y) separately, though they are usually
                    // implicitly handled by the yInBounds check's strict inequality.
                    if (p1.Y == p2.Y)
                    {
                        // If the pixel is on a horizontal edge, it's inside if its X is between the edge X's.
                        // For simplicity and robustness, horizontal edges that coincide with the ray 
                        // are often ignored, as they will be handled by the other edges.
                        // We proceed to only check non-horizontal intersections.
                        continue;
                    }

                    double x_intersect = (double)(p2.X - p1.X) * (pixelY - p1.Y) / (p2.Y - p1.Y) + p1.X;

                    // 3. Check if the intersection point is to the right of the pixel
                    if (x_intersect > pixelX)
                    {
                        crossings++;
                    }
                }
            }

            // If the number of crossings is ODD, the point is inside.
            return (crossings % 2 != 0);
        }

        private PhysicalPixel[,] Populate2DArrayWithPixels(PhysicalPixel[,] input)
        {

            for (int i = 0; i < input.GetLength(0); i++)
            {
                for (int j = 0; j < input.GetLength(1); j++)
                {
                    input[i, j] = new PhysicalPixel();

                    input[i, j].backgroundColor = "#000000"; // 7 chars
                    input[i, j].backgroundColor = input[i, j].backgroundColor.Substring(0, 7); // truncate just in case

                    input[i, j].foregroundColor = "#000000"; // 7 chars
                    input[i, j].foregroundColor = input[i, j].foregroundColor.Substring(0, 7); // truncate just in case
                    input[i, j].chartype = (uint)char.ConvertToUtf32(" ", 0);
                }
            }
            return input;
        }


        private PhysicalPixel[] Flatten(PhysicalPixel[,] arr)
        {
            int rows = arr.GetLength(1);
            int cols = arr.GetLength(0);
            PhysicalPixel[] flat = new PhysicalPixel[cols * rows];
            for (int x = 0; x < cols; x++)
                for (int y = 0; y < rows; y++)
                    flat[y * cols + x] = arr[x, y];
            return flat;
        }


        private static void ValidateRGB(string rgb)
        {
            try
            {
                rgb = rgb.TrimStart('#');

                int r = Convert.ToInt32(rgb.Substring(0, 2), 16);
                int g = Convert.ToInt32(rgb.Substring(2, 2), 16);
                int b = Convert.ToInt32(rgb.Substring(4, 2), 16);


                if (rgb.Length == 8)
                {
                    int alpha = Convert.ToInt32(rgb.Substring(6, 2), 16);
                }
            }
            catch
            {
                throw new Exception($"RGB Code provided is invalid, rgb code: {rgb}");
            }
        }

        private static bool HasTransparencyValue(string rgb)
        {
            if (rgb.Length == 9)
            {
                return true;
            }
            return false;
        }

        private static string MergeColorWithAlpha(string rgb, int alpha)
        {
            if (HasTransparencyValue(rgb))
            {
                int a = Convert.ToInt32(rgb.Substring(7, 2), 16);

                int updatedAlpha = (int)(a * (float)((float)alpha / 255));
                string updatedString = rgb.Remove(rgb.Length - 2);
                updatedString += updatedAlpha.ToString("X2");

                return updatedString;
            }

            return rgb + alpha.ToString("X2");
        }

        private static string MergeColors(string colorRGB, string colorRGBAlpha)
        {
            // Remove '#' if present
            colorRGB = colorRGB.TrimStart('#');
            colorRGBAlpha = colorRGBAlpha.TrimStart('#');

            // Parse background color (assume fully opaque)
            int bgR = Convert.ToInt32(colorRGB.Substring(0, 2), 16);
            int bgG = Convert.ToInt32(colorRGB.Substring(2, 2), 16);
            int bgB = Convert.ToInt32(colorRGB.Substring(4, 2), 16);

            // Parse foreground color
            int fgR = Convert.ToInt32(colorRGBAlpha.Substring(0, 2), 16);
            int fgG = Convert.ToInt32(colorRGBAlpha.Substring(2, 2), 16);
            int fgB = Convert.ToInt32(colorRGBAlpha.Substring(4, 2), 16);

            // Parse alpha
             int fgA = Convert.ToInt32(colorRGBAlpha.Substring(6, 2), 16);

            double alpha = fgA / 255.0;

            // Apply alpha blending formula
            int r = (int)Math.Round(fgR * alpha + bgR * (1 - alpha));
            int g = (int)Math.Round(fgG * alpha + bgG * (1 - alpha));
            int b = (int)Math.Round(fgB * alpha + bgB * (1 - alpha));

            // Clamp values just in case
            r = Math.Clamp(r, 0, 255);
            g = Math.Clamp(g, 0, 255);
            b = Math.Clamp(b, 0, 255);

            // Return result as hex
            return $"#{r:X2}{g:X2}{b:X2}";
        }


        public static class Util
        {
            /// <summary>
            /// Create rgb string from 3 color values (red, green, blue)
            /// </summary>
            public static string RGB(int r, int g, int b)
            {
                return $"#{r:X2}{g:X2}{b:X2}";
            }

            /// <summary>
            /// Create rgb string from 4 color values (red, green, blue, transparency)
            /// </summary>
            public static string RGB(int r, int g, int b, int alpha)
            {
                return $"#{r:X2}{g:X2}{b:X2}{alpha:X2}";
            }

            /// <summary>
            /// Determine if a value lies between two numbers. Ideal for hitboxes
            /// </summary>
            public static bool Between(int min, int current, int max)
            {
                return (min <= current && current <= max);
            }
        }

    }
}

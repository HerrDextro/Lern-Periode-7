using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Drawing;
using System.Xml.Linq;
using Graphic_Renderer.SmartPainterFiles.DataObjects;
using Newtonsoft.Json;
using System.Security.Principal;

namespace Graphic_Renderer.SmartPainterFiles
{
    public class SPainter
    {
        // Position arrays
        string[,] pixel;
        string[,] pixelLast;
        string[,] charType;
        string[,] charTypeLast;
        string[,] letterColor;
        string[,] letterColorLast;

        // Setting default colors
        string defaultTextColor;
        string defaultBGColor;

        // Setting console size
        public int consoleWidth;
        public int consoleHeight;

        public SPainter(int width, int height, string color)
        {
            width *= 2; // Adapting coords for Console

            defaultBGColor = color;

            consoleHeight = height;
            consoleWidth = width / 2;

            defaultTextColor = "White";

            Console.CursorVisible = false;


            pixel = new string[width, height];
            pixel = populateList(pixel, color); // Initialising pixel array

            pixelLast = pixel.Clone() as string[,]; // Initialising pixelLast array

            charType = new string[width, height]; // Initialising charType array
            charType = populateList(charType, "█");

            letterColor = new string[width, height];
            letterColor = populateList(letterColor, defaultTextColor);

            letterColorLast = letterColor.Clone() as string[,];


            // for opening console



        }

        public void updateFrame()
        {
            for (int i = 0; i < pixel.GetLength(1); i++)
            {
                for (int j = 0; j < pixel.GetLength(0); j++)
                {
                    if (pixel[j, i] != pixelLast[j, i] || charType[j, i] != charTypeLast[j, i] || letterColor[j, i] != letterColorLast[j, i])
                    {
                        Console.SetCursorPosition(j, i);
                        if (charType[j, i] == "█")
                        {
                            setColor(pixel[j, i]);
                        }
                        else
                        {
                            setColor(letterColor[j,i]);
                            setBGColor(pixel[j, i]);
                        }
                        Console.Write(charType[j, i]);
                    }
                }
            }
            pixelLast = pixel.Clone() as string[,];
            charTypeLast = charType.Clone() as string[,];
            letterColorLast = letterColor.Clone() as string[,];
        }

        public void updateText() // Update all texts (not done by updateFrame)
        {
            for (int i = 0; i < pixel.GetLength(1); i++)
            {
                for (int j = 0; j < pixel.GetLength(0); j++)
                {
                    if (charType[j, i] != "█")
                    {
                        Console.SetCursorPosition(j, i);

                        setColor(defaultTextColor);
                        setBGColor(pixel[j, i]);

                        Console.Write(charType[j, i]);
                    }
                }
            }
        }
        public void clear() // Clear backlog
        {
            pixel = populateList(pixel, defaultBGColor);
            charType = populateList(charType, "█");
            letterColor = populateList(letterColor, defaultTextColor);
        }

        public void renderFrame() // Initialisation of the Console
        {
            for (int i = 0; i < pixel.GetLength(1) - 1; i++)
            {
                for (int j = 0; j < pixel.GetLength(0); j++)
                {
                    if (charType[j, i] == "█")
                    {
                        setColor(pixel[j, i]);
                    }
                    else
                    {
                        setColor(defaultTextColor);
                        setBGColor(pixel[j, i]);
                    }

                    Console.Write(charType[j, i]);
                }
                Console.WriteLine();
            }
            pixelLast = pixel.Clone() as string[,];
        }
        public void writeText(string text, int xpos, int ypos, string color = "defaultTextColor")
        {
            
            
            for (int i = 0; i < text.Length; i++)
            {
                charType[xpos + i, ypos] = Convert.ToString(text[i]);
                if (color != "defaultTextColor") 
                {
                    letterColor[xpos + i, ypos] = color;
                }
            }
        }
        public void changeTextColor(string color)
        {
            defaultTextColor = color;
        }

        public void changePixel(string color, int xpos, int ypos)
        {
            xpos *= 2; //Adapting coords for Console
            try
            {
                pixel[xpos, ypos] = color;
                pixel[xpos + 1, ypos] = color;
            }
            catch (IndexOutOfRangeException) { }
        }
        public void fillRectangle(string color, int xstart, int ystart, int xsize, int ysize)
        {
            xsize *= 2; //Adapting coords for Console
            xstart *= 2;

            for (int i = 0; i < xsize; i++)
            {
                for (int j = 0; j < ysize; j++)
                {

                    try
                    {
                        pixel[i + xstart, j + ystart] = color;
                        charType[i + xstart, j + ystart] = "█";
                    }
                    catch (IndexOutOfRangeException) { }
                }
            }
        }
        public void saveImage(int xstart, int ystart, int xsize, int ysize, string filepath)
        {
            xsize *= 2;
            xstart *= 2; //Adapting coords for Console


            Image.Pixel[][] pixels = new Image.Pixel[ysize][];
            for (int y = 0; y < ysize; y++)
            {
                pixels[y] = new Image.Pixel[xsize];
                for (int x = 0; x < xsize; x++)
                {
                    string color = pixel[xstart + x, ystart + y];
                    char letter = charType[xstart + x, ystart + y][0];
                    string letterCol = letterColor[xstart + x, ystart + y];

                    pixels[y][x] = new Image.Pixel { color = color, letter = letter, letterColor = letterCol };
                }
            }
            Image image = new Image() { pixels = pixels };

            string json = JsonConvert.SerializeObject(image, Formatting.Indented);

            File.WriteAllText(filepath, json);

        }
        public void loadImage(int xpos, int ypos, string filepath)
        {
            string extension = Path.GetExtension(filepath).ToLower();
            if(extension == ".txt")
            {
                string[] textInpRaw = File.ReadAllLines(filepath);
                xpos *= 2; // Adapting coords for Console

                for (int i = 0; i < textInpRaw.Length; i++)
                {
                    string[] line = textInpRaw[i].Split(";");

                    for (int j = 0; j < line.Length - 1; j++)
                    {
                        try
                        {
                            if (xpos + j >= 0 && ypos + i >= 0 && xpos + j <= consoleWidth * 2 - 1 && ypos + i <= consoleHeight * 2 - 1)
                            {
                                if (line[j] != "none")
                                {
                                    pixel[xpos + j, ypos + i] = line[j];
                                }
                            }
                        }
                        catch (IndexOutOfRangeException) { }
                    }
                }
            }
            else if (extension == ".json")
            {
                string jsonString = File.ReadAllText(filepath);

                xpos *= 2;

                // Throw if invalid JSON
                if (jsonString == null) { throw new InvalidDataException("Targeted file is empty"); }

                Image? imageJson = JsonConvert.DeserializeObject<Image>(jsonString);

                if (imageJson == null) { throw new InvalidDataException("Targeted file is empty"); }

                for (int i = 0; i < imageJson.pixels.GetLength(0); i++) // YES IT DOES COMPLAIN BUT NO IT CANNOT BE NULL
                {
                    for (int j = 0; j < imageJson.pixels[i].GetLength(0); j++)
                    {
                        try
                        {
                            if (xpos + j >= 0 && ypos + i >= 0 && xpos + j <= consoleWidth * 2 - 1 && ypos + i <= consoleHeight * 2 - 1)
                            {
                                var imgPixel = imageJson.pixels[i][j];

                                pixel[xpos + j, ypos + i] = imgPixel.color;
                                charType[xpos + j, ypos + i] = imgPixel.letter.ToString();
                                letterColor[xpos + j, ypos + i] = imgPixel.letterColor;

                            }
                        }
                        catch (IndexOutOfRangeException) { }
                    }
                }

            }


        }
        private string[,] populateList(string[,] list, string input)
        {
            for (int x = 0; x < list.GetLength(0); x++)
            {
                for (int y = 0; y < list.GetLength(1); y++)
                {
                    list[x, y] = input;
                }
            }
            return list;
        }

        private string toSingleString(string[] input)
        {
            string result = string.Empty;

            for (int x = 0; x < input.Length; x++)
            {
                result += input[x] + "\n";
            }
            return result;
        }

        private void setColor(string color)
        {
            try
            {
                Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color, true);
            }
            catch (ArgumentException)
            {
                Console.ForegroundColor = ConsoleColor.White; // Default color if the string is invalid
            }
        }
        private void setBGColor(string color)
        {
            try
            {
                Console.BackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color, true);
            }
            catch (ArgumentException)
            {
                Console.BackgroundColor = ConsoleColor.White; // Default color if the string is invalid
            }
        }
    }
}
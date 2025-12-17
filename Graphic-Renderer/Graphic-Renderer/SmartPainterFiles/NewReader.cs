using System;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static Graphic_Renderer.SmartPainterFiles.DataObjects.Point;
using System.Drawing;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using static Graphic_Renderer.SmartPainterFiles.DataObjects.Image;
using static System.Runtime.InteropServices.JavaScript.JSType;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Numerics;
using System.Runtime.InteropServices.Marshalling;
using Graphic_Renderer.SmartPainterFiles.DataObjects;

namespace Graphic_Renderer.SmartPainterFiles
{
    public class NewReader
    {
        private DataObjects.Point _mousePos;
        private DateTime _lastMouseCheck;


        // -------------------- Keys ------------------------ //

        // Letters
        public PainterKey A { get; } = new(0x41, 30);
        public PainterKey B { get; } = new(0x42, 48);
        public PainterKey C { get; } = new(0x43, 46);
        public PainterKey D { get; } = new(0x44, 32);
        public PainterKey E { get; } = new(0x45, 18);
        public PainterKey F { get; } = new(0x46, 33);
        public PainterKey G { get; } = new(0x47, 34);
        public PainterKey H { get; } = new(0x48, 35);
        public PainterKey I { get; } = new(0x49, 23);
        public PainterKey J { get; } = new(0x4A, 36);
        public PainterKey K { get; } = new(0x4B, 37);
        public PainterKey L { get; } = new(0x4C, 38);
        public PainterKey M { get; } = new(0x4D, 50);
        public PainterKey N { get; } = new(0x4E, 49);
        public PainterKey O { get; } = new(0x4F, 24);
        public PainterKey P { get; } = new(0x50, 25);
        public PainterKey Q { get; } = new(0x51, 16);
        public PainterKey R { get; } = new(0x52, 19);
        public PainterKey S { get; } = new(0x53, 31);
        public PainterKey T { get; } = new(0x54, 20);
        public PainterKey U { get; } = new(0x55, 22);
        public PainterKey V { get; } = new(0x56, 47);
        public PainterKey W { get; } = new(0x57, 17);
        public PainterKey X { get; } = new(0x58, 45);
        public PainterKey Y { get; } = new(0x59, 21);
        public PainterKey Z { get; } = new(0x5A, 44);


        // Numbers
        public PainterKey n0 { get; } = new(0x30, 11);
        public PainterKey n1 { get; } = new(0x31, 2);
        public PainterKey n2 { get; } = new(0x32, 3);
        public PainterKey n3 { get; } = new(0x33, 4);
        public PainterKey n4 { get; } = new(0x34, 5);
        public PainterKey n5 { get; } = new(0x35, 6);
        public PainterKey n6 { get; } = new(0x36, 7);
        public PainterKey n7 { get; } = new(0x37, 8);
        public PainterKey n8 { get; } = new(0x38, 9);
        public PainterKey n9 { get; } = new(0x39, 10);

        // Special Characters
        public PainterKey Space { get; } = new(0x20, 57);
        public PainterKey Enter { get; } = new(0x0D, 28);
        public PainterKey Escape { get; } = new(0x1B, 1);
        public PainterKey ArrowLeft { get; } = new(0x25, 105);
        public PainterKey ArrowUp { get; } = new(0x26, 103);
        public PainterKey ArrowRight { get; } = new(0x27, 106);
        public PainterKey ArrowDown { get; } = new(0x28, 108);
        public PainterKey Shift { get; } = new(0x10, 42);
        public PainterKey Control { get; } = new(0x11, 29);
        public PainterKey Alt { get; } = new(0xA4, 56);
        public PainterKey Tab { get; } = new(0x09, 15);
        public PainterKey Backspace { get; } = new(0x08, 14);
        public PainterKey Capslock { get; } = new(0x14, 58);
        public PainterKey Delete { get; } = new(0x2E, 111);

        // Mouse
        public PainterKey MouseLeft { get; } = new(0x01, 272);
        public PainterKey MouseRight { get; } = new(0x02, 273);
        public PainterKey MouseMiddle { get; } = new(0x04, 274);





        public NewReader(SReader old)
        {
            try
            {
                NativeKey.InitializeLinux("/dev/input/event0");
                NativeMouse.Init();
            }
            catch { }
        }



        /// <summary>
        /// Checks if the specified key (can also include mouse) is down
        /// </summary>
        /// <param name="key">Key code of the key, use NewReader.Key for template keys</param>
        /// <param name="onKeyDown">Function pointer to what to do when key is down</param>
        /// <param name="onKeyUp">Function pointer to what to do when key is up</param>
        /// <param name="tick">Function pointer to what to do always</param>
        /// <returns>A bool that determines if the key is down</returns>
        public bool GetKey(PainterKey key, Action? onKeyDown = null, Action? onKeyUp = null, Action? tick = null)
        {
            bool keyIsDown = NativeKey.IsKeyDown(key);
            if (keyIsDown && onKeyDown != null) onKeyDown();

            if (!keyIsDown && onKeyUp != null) onKeyUp();

            if (tick != null) tick();

            return keyIsDown;
        }

        /// <summary>
        /// Returns the point of the current mouse position in Painter Coordinates
        /// </summary>
        /// <returns>Current mouse pos as a Point</returns>
        public DataObjects.Point GetMousePosition()
        {
            NativeMouse.Update();

            int correctedX = (int)(NativeMouse.X * 0.8666666 - 2);
            int correctedY = (int)(NativeMouse.Y * 0.86 - 3);


            var point = new DataObjects.Point(correctedX, correctedY);
            
            return point;
        }

        /// <summary>
        /// Checks if the user has clicked in a certain area with his mouse
        /// </summary>
        /// <param name="xStart">Smallest x where you can click</param>
        /// <param name="yStart">Smallest y where you can click</param>
        /// <param name="xEnd">Biggest x where you can click</param>
        /// <param name="yEnd">Biggest y where you can click</param>
        /// <param name="allowesKeys">Array of the Key codes of all allowes keys, use NewReader.Key for template keys</param>
        /// <param name="onClicked">Function pointer to what to do if the area is clicked, requires int param for the key code that actually clicked it</param>
        /// <param name="tick">Function pointer to what always to do</param>
        /// <returns>A bool to determine a click has happened</returns>
        /// <exception cref=""></exception>
        public bool GetClickbox(int xStart, int yStart, int xEnd, int yEnd, PainterKey[] allowesKeys, Func<int, Action>? onClicked = null, Func<Action>? tick = null)
        {
            throw new NotImplementedException();
        }

        

    }
}

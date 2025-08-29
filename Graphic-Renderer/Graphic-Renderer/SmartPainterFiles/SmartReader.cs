using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Drawing;
using System.Text;

namespace Graphic_Renderer.SmartPainterFiles
{
    public class SReader
    {
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);


        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT { public int X, Y; }

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(nint hwnd, ref Rect rectangle);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern nint FindWindowByCaption(nint zeroOnly, string lpWindowName);

        private const int WM_MOUSEWHEEL = 0x020A;


        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }




        private const int VK_LBUTTON = 0x01; // Virtual key code for the left mouse button
        private const int VK_RBUTTON = 0x02; // Virtual key code for the right mouse button
        private const int VK_MBUTTON = 0x04; // Virtual key code for the middle mouse button



        private static nint _hookID = nint.Zero;

        [StructLayout(LayoutKind.Sequential)]
        struct Coord
        {
            public short X;
            public short Y;
        }



        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        struct CONSOLE_FONT_INFO_EX
        {
            public uint cbSize;
            public uint nFont;
            public Coord dwFontSize;
            public int FontFamily;
            public int FontWeight;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string FaceName;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetCurrentConsoleFontEx(nint hConsoleOutput, bool bMaximumWindow, ref CONSOLE_FONT_INFO_EX lpConsoleCurrentFontEx);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern nint GetStdHandle(int nStdHandle);

        const int STD_OUTPUT_HANDLE = -11;

        // Full Keyboard Input
        [DllImport("user32.dll")]
        private static extern int ToUnicodeEx(
            uint wVirtKey,
            uint wScanCode,
            byte[] lpKeyState,
            [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff,
            int cchBuff,
            uint wFlags,
            IntPtr dwhkl);

        [DllImport("user32.dll")]
        private static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll")]
        private static extern IntPtr GetKeyboardLayout(uint idThread);
        // ---------------

        // Letters
        public const int A = 0x41;
        public const int B = 0x42;
        public const int C = 0x43;
        public const int D = 0x44;
        public const int E = 0x45;
        public const int F = 0x46;
        public const int G = 0x47;
        public const int H = 0x48;
        public const int I = 0x49;
        public const int J = 0x4A;
        public const int K = 0x4B;
        public const int L = 0x4C;
        public const int M = 0x4D;
        public const int N = 0x4E;
        public const int O = 0x4F;
        public const int P = 0x50;
        public const int Q = 0x51;
        public const int R = 0x52;
        public const int S = 0x53;
        public const int T = 0x54;
        public const int U = 0x55;
        public const int V = 0x56;
        public const int W = 0x57;
        public const int X = 0x58;
        public const int Y = 0x59;
        public const int Z = 0x5A;

        // Numbers
        public const int n0 = 0x30;
        public const int n1 = 0x31;
        public const int n2 = 0x32;
        public const int n3 = 0x33;
        public const int n4 = 0x34;
        public const int n5 = 0x35;
        public const int n6 = 0x36;
        public const int n7 = 0x37;
        public const int n8 = 0x38;
        public const int n9 = 0x39;

        // Special Characters
        public const int space = 0x20;
        public const int enter = 0x0D;
        public const int escape = 0x1B;
        public const int arrowLeft = 0x27;
        public const int arrowUp = 0x26;
        public const int arrowRight = 0x25;
        public const int arrowDown = 0x28;
        public const int shift = 0x10;
        public const int control = 0x12;
        public const int alt = 0x12;
        public const int tab = 0x09;
        public const int backspace = 0x08;
        public const int capslock = 0x14;
        public const int delete = 0x2E;


        private int[] _allKeys = [
            A,
            B,
            C,
            D,
            E,
            F,
            G,
            H,
            I,
            J,
            K,
            L,
            M,
            N,
            O,
            P,
            Q,
            R,
            S,
            T,
            U,
            V,
            W,
            X,
            Y,
            Z,
            n0,
            n1,
            n2,
            n3,
            n4,
            n5,
            n6,
            n7,
            n8,
            n9,
            space,
            enter,
            escape,
            arrowLeft,
            arrowRight,
            arrowUp,
            arrowDown,
            tab,
            backspace,
            delete
            ];

        public int consoleWidth;
        public int consoleHeight;

        // Constructor
        public SReader()
        {
            Console.Title = "GraphicsEngine";
        }

        public int[] getMousePos() // Relative to Window in SPainter Pixels
        {
            // Get Mouse Position

            POINT currentPos;
            GetCursorPos(out currentPos);

            int cursorRawX = currentPos.X;
            int cursorRawY = currentPos.Y;

            // Get Console Window

            nint handle = FindWindowByCaption(nint.Zero, "GraphicsEngine");
            Rect rect = new Rect();

            int windowX;
            int windowY;

            if (GetWindowRect(handle, ref rect))
            {
                windowX = rect.Left;
                windowY = rect.Top;
            }
            else
            {
                windowX = 0;
                windowY = 0;
            }

            // Get Line Size
            var consoleHandle = GetStdHandle(STD_OUTPUT_HANDLE);
            var fontInfo = new CONSOLE_FONT_INFO_EX { cbSize = (uint)Marshal.SizeOf<CONSOLE_FONT_INFO_EX>() };

            double fontY;
            double fontX;

            if (GetCurrentConsoleFontEx(consoleHandle, false, ref fontInfo))
            {
                fontY = fontInfo.dwFontSize.Y;
                fontX = fontInfo.dwFontSize.Y / 2;
            }
            else
            {
                fontY = 1;
                fontX = 1;
            }

            // Substract Window - Cursor
            double cursorX = cursorRawX - windowX;
            double cursorY = cursorRawY - windowY;

            // Divide for accurate Coordinates

            int cursorXLine = Convert.ToInt32(cursorX / fontX);
            int cursorYLine = Convert.ToInt32(cursorY / fontY);

            int cursoradaptX = Convert.ToInt32(Math.Floor(cursorXLine / 2.35 - 1));
            int cursoradaptY = Convert.ToInt32(Math.Ceiling(cursorYLine * 0.85) - 3);

            return [cursoradaptX, cursoradaptY];
        }

        public bool IsLeftMouseButtonDown()
        {
            return (GetAsyncKeyState(VK_LBUTTON) & 0x8000) != 0; // Check high-order bit
        }

        public bool IsRightMouseButtonDown()
        {
            return (GetAsyncKeyState(VK_RBUTTON) & 0x8000) != 0; // Check high-order bit
        }

        public bool isMiddleMouseButtonDown()
        {
            return (GetAsyncKeyState(VK_MBUTTON) & 0x80000) != 0;
        }

        public bool KeyDown(int keyCode)
        {
            return (GetAsyncKeyState(keyCode) & 0x8000) != 0;
        }

        //----------------------------------------------------------------
        // Key Captures
        //
        //----------------------------------------------------------------

        private readonly object _lock = new object();
        private CancellationTokenSource? _cts;
        private Task? _captureTask;
        private string _captureState = "";

        public void StartKeyCapture()
        {
            if (_captureTask != null && !_captureTask.IsCompleted)
            {
                return;
            }
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            _captureTask = Task.Run(() => RunCapture(token),token);
        }
        public string ReadKeyCapture()
        {
            lock (_lock)
            {
                return _captureState;
            }
        }

        public string EndCapture()
        {
            if (_cts == null)
                return "Not running";
            _cts.Cancel();

            try
            {
                _captureTask?.Wait();
            }
            catch (AggregateException) {/* Ignore Exception */ }
            finally
            {
                _cts.Dispose();
                _cts = null;
                _captureTask = null;
            }

            lock (_lock)
            {
                return _captureState;
            }

        }


        
        private void RunCapture(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                string f = DateTime.Now.ToString();
                // Execute Key Capturing
                string key = GetActiveKey();




                lock (_lock)
                {
                    if (key == "backspace")
                    {
                        _captureState = _captureState.Remove(_captureState.Length-1);
                    }
                    else
                    {
                        _captureState += key;
                    }
                    
                }
            }
        }

        private string GetActiveKey()
        {
            foreach (int key in _allKeys)
            {
                if (KeyDown(key))
                {
                    while (KeyDown(key)) { }
                    return KeyCodeToUnicode((uint)key);
                }
                if (KeyDown(backspace))
                {
                    while (KeyDown(backspace)) { }
                    return "backspace";
                }
            }
            return "";
        }


        // idk how it works but it does UPDATE:nt

        private string KeyCodeToUnicode(uint virtualKey)
        {
            byte[] keyboardState = new byte[256];

            // Get the current state of all keys
            if (!GetKeyboardState(keyboardState))
                return "";

            // Manually set the state of the Shift key to "down"
            // VK_SHIFT is the virtual key code for the Shift key
            if (KeyDown(shift) || KeyDown(control))
            {
                keyboardState[0x10] = 0x80; // WA DE HEEEEEEL IS THIS
            }

            uint scanCode = MapVirtualKey(virtualKey, 0);
            StringBuilder sb = new StringBuilder(10);
            IntPtr layout = GetKeyboardLayout(0);

            int result = ToUnicodeEx(virtualKey, scanCode, keyboardState, sb, sb.Capacity, 0, layout);
            if (result > 0)
                return sb.ToString();
            return "";
        }



    }
}
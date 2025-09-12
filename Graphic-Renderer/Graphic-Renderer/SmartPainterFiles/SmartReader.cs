using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using TextCopy;

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

        //[DllImport("user32.dll")]
        //private static extern uint MapVirtualKey(uint uCode, uint uMapType);

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
                fontX = fontY * 0.5;

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

            double cursorXLine = (cursorX / fontX)*1.0;
            double cursorYLine = (cursorY / fontY)*1.0;

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
                string temp = _captureState;
                _captureState = "";
                
                return temp;
            }

        }

        public int CaptureLength { get // Get length of chars
            {
                lock (_lock)
                {
                    return _captureState.Length;
                }
            }
        }
        
        private void RunCapture(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                string f = DateTime.Now.ToString();
                // Execute Key Capturing
                string key = GetKeyInput();

                


                lock (_lock)
                {
                    if (key == "back")
                    {
                        if (_captureState.Length >= 1)
                        {
                            _captureState = _captureState.Remove(_captureState.Length-1);
                        }
                    }
                    else if (key == "paste")
                    {
                        try
                        {
                            string clipboardContent = ClipboardService.GetText();
                            _captureState += clipboardContent;
                        }
                        catch { }
                    }
                    else
                    {
                        _captureState += key; 
                    }
                    
                }
            }
        }

        // Get active key:

        [DllImport("user32.dll")]
        private static extern int ToUnicode(
            uint wVirtKey,
            uint wScanCode,
            byte[] lpKeyState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 4)]
        StringBuilder pwszBuff,
            int cchBuff,
            uint wFlags);


        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint uCode, uint uMapType);

        // Special virtual key codes
        private const int VK_BACK = 0x08;
        private const int VK_CONTROL = 0x11;
        private const int VK_SHIFT = 0x10;
        private const int VK_MENU = 0x12; // Alt
        private const int VK_V = 0x56;

        // Left/right variants
        private const int VK_LSHIFT = 0xA0;
        private const int VK_RSHIFT = 0xA1;
        private const int VK_LCONTROL = 0xA2;
        private const int VK_RCONTROL = 0xA3;
        private const int VK_LMENU = 0xA4;
        private const int VK_RMENU = 0xA5;

        private static bool IsModifier(int key) =>
            key == VK_SHIFT || key == VK_CONTROL || key == VK_MENU ||
            key == VK_LSHIFT || key == VK_RSHIFT ||
            key == VK_LCONTROL || key == VK_RCONTROL ||
            key == VK_LMENU || key == VK_RMENU;

        public static string GetKeyInput()
        {
            for (int key = 0x08; key <= 0xFE; key++)
            {
                if ((GetAsyncKeyState(key) & 0x8000) != 0)
                {
                    if (IsModifier(key))
                        continue;

                    if (key >= 0xF0)
                        continue;

                    // Handle Backspace
                    if (key == VK_BACK)
                    {
                        while ((GetAsyncKeyState(key) & 0x8000) != 0) { }
                        return "back";
                    }

                    // Handle Ctrl+V
                    if ((GetAsyncKeyState(VK_CONTROL) & 0x8000) != 0 && key == VK_V)
                    {
                        while ((GetAsyncKeyState(key) & 0x8000) != 0) { }
                        return "paste";
                    }

                    // Capture keyboard state *before* ToUnicode
                    byte[] keyboardState = new byte[256];
                    if (!GetKeyboardState(keyboardState))
                        return null;

                    // Force modifiers from GetAsyncKeyState
                    if ((GetAsyncKeyState(VK_SHIFT) & 0x8000) != 0)
                        keyboardState[VK_SHIFT] = 0x80;
                    if ((GetAsyncKeyState(VK_CONTROL) & 0x8000) != 0)
                        keyboardState[VK_CONTROL] = 0x80;
                    if ((GetAsyncKeyState(VK_MENU) & 0x8000) != 0)
                        keyboardState[VK_MENU] = 0x80;

                    uint scanCode = MapVirtualKey((uint)key, 0);
                    StringBuilder sb = new StringBuilder(5);

                    int result = ToUnicode((uint)key, scanCode, keyboardState, sb, sb.Capacity, 0);

                    // Wait for key release *after* translating character
                    while ((GetAsyncKeyState(key) & 0x8000) != 0) { }

                    if (result > 0)
                        return sb.ToString();
                }
            }
            return null;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Graphic_Renderer.SmartPainterFiles
{
    public static class NativeMouse
    {
        private static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        private static readonly bool IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        // Position State
        public static double X { get; private set; }
        public static double Y { get; private set; }

        // Linux Specific State
        private static int _linuxMouseFd = -1;
        private static Thread _linuxUpdateThread;
        private static bool _running = false;
        private const int EV_REL = 0x02;
        private const int REL_X = 0x00;
        private const int REL_Y = 0x01;

        /// <summary>
        /// Starts the background tracker. 
        /// On Windows, this does nothing (we query on demand).
        /// On Linux, this opens the device stream.
        /// </summary>
        public static void Init(string linuxDevicePath = "/dev/input/event4")
        {
            if (IsLinux)
            {
                _linuxMouseFd = open(linuxDevicePath, O_RDONLY);
                if (_linuxMouseFd < 0) throw new IOException($"Cannot open {linuxDevicePath}");

                _running = true;
                _linuxUpdateThread = new Thread(LinuxReadLoop);
                _linuxUpdateThread.IsBackground = true;
                _linuxUpdateThread.Start();
            }

            AppDomain.CurrentDomain.ProcessExit += (s, e) => Cleanup();
            Console.CancelKeyPress += (s, e) => Cleanup(); // ctrl+c
        }

        /// <summary>
        /// Updates the X/Y properties. 
        /// Call this once per frame in your main loop.
        /// </summary>
        public static void Update()
        {
            if (IsWindows)
            {
                UpdateWindows();
            }
            // Linux updates happen automatically in the background thread
        }

        public static void Cleanup()
        {
            _running = false;
            if (_linuxMouseFd >= 0)
            {
                close(_linuxMouseFd);
                _linuxMouseFd = -1;
            }
        }

        // ================= WINDOWS IMPLEMENTATION =================
        // Strategy: Get Mouse Pixels -> Get Window Pixels -> Subtract -> Divide by Font Size

        private static void UpdateWindows()
        {
            // 1. Get Global Mouse Pixels
            GetCursorPos(out POINT mousePoint);

            // 2. Get Console Window Pixels
            nint handle = FindWindowByCaption(nint.Zero, "GraphicsEngine");
            //IntPtr hWnd = GetConsoleWindow();
            GetWindowRect(handle, out RECT winRect);

            // 3. Determine Relative Pixel Position
            // Note: We add a small offset (approx 30px top, 8px left) for Titlebar/Borders 
            // strictly speaking, we should use ClientToScreen, but this is usually close enough.
            int relativeX = mousePoint.X - winRect.Left - 8;
            int relativeY = mousePoint.Y - winRect.Top - 30;

            // 4. Convert Pixels to Columns/Rows
            // We need the font size to do this accurately.
            // If we can't get it, we guess 8x16 (standard default)
            int fontW = 8;
            int fontH = 16;

            IntPtr hOut = GetStdHandle(-11);


            CONSOLE_SCREEN_BUFFER_INFOEX csbiex = new();
            csbiex.cbSize = (uint)Marshal.SizeOf(csbiex);
            csbiex.ColorTable = new uint[16];

            if (GetConsoleScreenBufferInfoEx(hOut, ref csbiex))
            {
                fontW = Math.Max(1, (int)csbiex.dwFontSize.X);
                fontH = Math.Max(1, (int)csbiex.dwFontSize.Y);
            }

            // 5. Calculate and Clamp
            X = Math.Clamp((double) relativeX / fontW, (double)0, (double)(Console.WindowWidth));
            Y = Math.Clamp((double)relativeY / (fontH/2), (double)0, (double)((Console.WindowHeight) * 2));

            X = (double)relativeX / fontW;
            Y = (double)relativeY / (fontH/2);

        }

        // ================= LINUX IMPLEMENTATION =================
        // Strategy: Read raw hardware deltas (move +1, move -1) and update a counter.

        private static void LinuxReadLoop()
        {
            // Input event struct is 24 bytes on 64-bit Linux
            byte[] buffer = new byte[24];

            while (_running && _linuxMouseFd >= 0)
            {
                // Read blocks until mouse moves
                int bytesRead = read(_linuxMouseFd, buffer, buffer.Length);
                if (bytesRead < 24) continue;

                // Parse struct input_event
                // offset 16 = type (ushort), offset 18 = code (ushort), offset 20 = value (int)
                ushort type = BitConverter.ToUInt16(buffer, 16);
                ushort code = BitConverter.ToUInt16(buffer, 18);
                int value = BitConverter.ToInt32(buffer, 20);

                if (type == EV_REL)
                {
                    if (code == REL_X) X += value;
                    if (code == REL_Y) Y += value;

                    // Clamp to console bounds
                    // Note: We can't easily know the Console size from a background thread reliably
                    // so we clamp loosely or assume standard size.
                    if (X < 0) X = 0;
                    if (Y < 0) Y = 0;
                    // Optional: Clamp to max width/height if known
                }
            }
        }

        // ================= NATIVE IMPORTS =================

        // ================= NATIVE IMPORTS (CORRECTED) =================

        // --- WINDOWS IMPORTS ---

        // FIXED: GetConsoleWindow is in kernel32.dll
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        // These remain in user32.dll
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        // These remain in kernel32.dll
        [DllImport("kernel32.dll")]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        static extern bool GetCurrentConsoleFont(
            IntPtr hConsoleOutput,
            bool bMaximumWindow,
            ref CONSOLE_FONT_INFO lpConsoleCurrentFont
        );

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern nint FindWindowByCaption(nint zeroOnly, string lpWindowName);

        // --- WINDOWS STRUCTS ---
        [StructLayout(LayoutKind.Sequential)]
        struct POINT { public int X; public int Y; }

        [StructLayout(LayoutKind.Sequential)]
        struct RECT { public int Left; public int Top; public int Right; public int Bottom; }

        [StructLayout(LayoutKind.Sequential)]
        struct CONSOLE_FONT_INFO { public int nFont; public COORD dwFontSize; }

        [StructLayout(LayoutKind.Sequential)]
        public struct COORD { public short X; public short Y; }

        [StructLayout(LayoutKind.Sequential)]
        public struct CONSOLE_SCREEN_BUFFER_INFOEX
        {
            // Must be the size of this structure
            public uint cbSize;
            public COORD dwSize;
            public COORD dwCursorPosition;
            public short wAttributes;
            public SMALL_RECT srWindow;
            public COORD dwMaximumWindowSize;
            public ushort wPopupAttributes;
            public bool bFullscreenSupported;

            // COLORREF is a 32-bit unsigned integer (uint in C#)
            // Array of 16 entries for the color table
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public uint[] ColorTable;

            // The font size you are looking for
            public COORD dwFontSize;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SMALL_RECT
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetConsoleScreenBufferInfoEx(
            IntPtr hConsoleOutput,
            ref CONSOLE_SCREEN_BUFFER_INFOEX lpConsoleScreenBufferInfoEx
        );

        // --- LINUX IMPORTS ---
        [DllImport("libc", SetLastError = true)]
        private static extern int open(string pathname, int flags);

        [DllImport("libc", SetLastError = true)]
        private static extern int close(int fd);

        [DllImport("libc", SetLastError = true)]
        private static extern int read(int fd, byte[] buf, int count);
        private const int O_RDONLY = 0;
    }
}

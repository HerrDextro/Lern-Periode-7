using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphic_Renderer.SmartPainterFiles
{
    using Graphic_Renderer.SmartPainterFiles.DataObjects;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;

    public static class NativeKey
    {
        private static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        private static readonly bool IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        private static int _linuxKeyboardFd = -1;
        private static byte[] _linuxKeyBuffer = new byte[64]; // 512 bits

        /// <summary>
        /// Static Constructor: Runs once automatically. 
        /// Sets up the auto-cleanup and generates the key mappings.
        /// </summary>
        static NativeKey()
        {
            // 1. Register Auto-Cleanup
            AppDomain.CurrentDomain.ProcessExit += (s, e) => Cleanup();
            Console.CancelKeyPress += (s, e) => Cleanup(); // ctrl+c
        }

        /// <summary>
        /// Linux Only: Call this once at startup with your device path.
        /// </summary>
        public static void InitializeLinux(string devicePath)
        {
            if (!IsLinux) return;

            if (_linuxKeyboardFd >= 0) Cleanup(); // Safety check

            _linuxKeyboardFd = open(devicePath, O_RDONLY | O_NONBLOCK);
            if (_linuxKeyboardFd < 0)
            {
                // Note: In a real app, you might want to log this rather than throw, 
                // or search for the next available event file.
                throw new IOException($"Could not open {devicePath}. Check permissions (sudo?) or path.");
            }
        }

        /// <summary>
        /// Automatically called on program exit. 
        /// Can be called manually if you want to stop listening early.
        /// </summary>
        public static void Cleanup()
        {
            if (IsLinux && _linuxKeyboardFd >= 0)
            {
                close(_linuxKeyboardFd);
                _linuxKeyboardFd = -1;
            }
        }

        /// <summary>
        /// The easy-to-use function: "Is this ConsoleKey pressed right now?"
        /// </summary>
        public static bool IsKeyDown(PainterKey key) // TODO not use console but painter keys
        {
            if (key == null) return false;


            int keyCodeAdapted;
            if (IsLinux)
            {
                keyCodeAdapted = key.LinuxKeyCode;
            }
            else if (IsWindows)
            {
                keyCodeAdapted = key.WindowsKeyCode;
            }
            else
            {
                return false;
            }

            return IsNativeKeyDown(keyCodeAdapted);
        }

        /// <summary>
        /// The low-level function using raw integers.
        /// </summary>
        private static bool IsNativeKeyDown(int keyCode)
        {
            if (IsWindows)
            {
                return (GetAsyncKeyState(keyCode) & 0x8000) != 0;
            }
            else if (IsLinux)
            {
                if (_linuxKeyboardFd < 0) return false;

                // Query driver
                if (ioctl(_linuxKeyboardFd, EVIOCGKEY(512), _linuxKeyBuffer) < 0)
                    return false;

                int byteIndex = keyCode / 8;
                int bitIndex = keyCode % 8;

                if (byteIndex >= _linuxKeyBuffer.Length) return false;

                return (_linuxKeyBuffer[byteIndex] & (1 << bitIndex)) != 0;
            }
            return false;
        }


        // ================= P/INVOKE DECLARATIONS =================
        [DllImport("user32.dll")] private static extern short GetAsyncKeyState(int vKey);
        [DllImport("libc", SetLastError = true)] private static extern int open(string pathname, int flags);
        [DllImport("libc", SetLastError = true)] private static extern int close(int fd);
        [DllImport("libc", SetLastError = true)] private static extern int ioctl(int fd, long request, byte[] argp);

        private const int O_RDONLY = 0;
        private const int O_NONBLOCK = 2048;

        private static long EVIOCGKEY(int len)
        {
            return 2147483648 | ((long)len << 16) | (0x45 << 8) | 0x18;
        }
    }
}

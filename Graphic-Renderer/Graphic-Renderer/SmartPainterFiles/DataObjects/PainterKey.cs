using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphic_Renderer.SmartPainterFiles.DataObjects
{
    public class PainterKey
    {
        private int _windowsKeyCode;
        private int _linuxKeyCode;

        public int WindowsKeyCode {  get { return _windowsKeyCode; } }
        public int LinuxKeyCode { get { return _linuxKeyCode; } }

        public PainterKey(int windowsKeyCode, int linuxKeyCode)
        {
            _windowsKeyCode = windowsKeyCode;
            _linuxKeyCode = linuxKeyCode;
        }



    }
}

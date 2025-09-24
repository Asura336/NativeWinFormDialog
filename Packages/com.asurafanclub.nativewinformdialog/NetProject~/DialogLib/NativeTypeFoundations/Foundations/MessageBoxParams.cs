using System;
using System.Runtime.InteropServices;
using DialogLib.Data;

namespace DialogLib.Foundations
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 2)]
    public struct MessageBoxParams
    {
        public IntPtr Owner;
        public UnicodeByteBuffer Text;
        public UnicodeByteBuffer Caption;
        public MessageBoxButtons Buttons;
        public MessageBoxIcon Icon;
        public MessageBoxDefaultButton DefaultButton;
        public MessageBoxOptions Options;
    }
}
using System.Runtime.InteropServices;
using DialogLib.Data;

namespace DialogLib.Foundations
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 2)]
    public struct ColorDialogParams
    {
        public bool AllowFullOpen;
        public bool AnyColor;
        public ColorARGB Color;
        public NativeBufferT<ColorARGB> CustomColors;
        public bool FullOpen;
        public bool SolidColorOnly;
    }
}

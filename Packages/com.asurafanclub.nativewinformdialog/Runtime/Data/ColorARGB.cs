using System.Runtime.InteropServices;
using UnityEngine;

namespace DialogLib.Data
{
    /// <summary>
    /// https://github.com/dotnet/runtime/blob/1d1bf92fcf43aa6981804dc53c5174445069c9e4/src/libraries/System.Drawing.Primitives/src/System/Drawing/Color.cs
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ColorARGB
    {
        public byte b;
        public byte g;
        public byte r;
        public byte a;

        public ColorARGB(byte a, byte r, byte g, byte b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

#if WINDOWS
        public static implicit operator ColorARGB(System.Drawing.Color c) => new ColorARGB(c.A, c.R, c.G, c.B);
        public static implicit operator System.Drawing.Color(ColorARGB c) => System.Drawing.Color.FromArgb(c.a, c.r, c.g, c.b);
#endif

#if UNITY_5_3_OR_NEWER
        public static implicit operator ColorARGB(Color32 c) => new ColorARGB(c.a, c.r, c.g, c.b);
        public static implicit operator Color32(ColorARGB c) => new Color32(c.r, c.g, c.b, c.a);
#endif
    }
}

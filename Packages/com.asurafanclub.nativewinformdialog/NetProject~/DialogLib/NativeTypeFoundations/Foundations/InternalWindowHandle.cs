using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace DialogLib.Foundations
{
    public class InternalWindowHandle(nint handle) : IWin32Window
    {
        public nint Handle { get; } = handle;

        public override string ToString()
        {
            int len = GetWindowTextLength(Handle);
            var sb = new StringBuilder(len + 4);
            _ = GetWindowText(Handle, sb, len + 4);
            return sb.ToString();
        }


        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        static extern int GetWindowTextLength([In] nint hWnd);

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        static extern int GetWindowText([In] nint hWnd, [Out] StringBuilder lpString, [In] int nMaxCount);
    }
}

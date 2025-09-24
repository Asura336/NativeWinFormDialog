using System;
using System.Runtime.InteropServices;

namespace DialogLib.Foundations
{
    public struct ApplicationWindow
    {
        static IntPtr applicationWindowHandle;
        public static IntPtr ApplicationWindowHandle => applicationWindowHandle;

        static ApplicationWindow()
        {
            static bool _call(IntPtr hWnd, IntPtr lParam)
            {
                // 检索此线程对应的第一个窗口
                applicationWindowHandle = hWnd;
                return false;
            }
            uint threadID = GetCurrentThreadId();
            if (threadID > 0)
            {
                EnumThreadWindows(threadID, _call, IntPtr.Zero);
                // 在主线程以外可能找不到窗口，使用回退的方法
                if (applicationWindowHandle == IntPtr.Zero)
                {
                    applicationWindowHandle = GetForegroundWindow();
                }
            }
        }

        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern UInt32 GetCurrentThreadId();

        [return: MarshalAs(UnmanagedType.U1)]
        public delegate bool EnumThreadWndProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool EnumThreadWindows(UInt32 dwThreadId, EnumThreadWndProc lpfn, IntPtr lParam);

        [DllImport("User32.dll", EntryPoint = "GetForegroundWindow")]
        public static extern IntPtr GetForegroundWindow();
    }
}

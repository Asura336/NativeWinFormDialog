using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DialogLib.Util
{
    static unsafe class UnsafeHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T* MAllocT<T>(int elementNumber) where T : unmanaged
        {
            IntPtr ptr = Marshal.AllocHGlobal(elementNumber * sizeof(T));
            return (T*)ptr.ToPointer();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Free(void* pnt)
        {
            if (pnt == null) { return; }
            Marshal.FreeHGlobal(new IntPtr(pnt));
        }
    }
}

using DialogLib.Util;

namespace DialogLib.Data
{
    public unsafe struct NativeBufferT<T> where T : unmanaged
    {
        public int length;
        public int count;
        public T* buffer;

        public static void MAlloc(ref NativeBufferT<T> target, int elementNumber)
        {
            target.length = MathHelper.CeilPow2(elementNumber);
            target.count = 0;
            target.buffer = UnsafeHelper.MAllocT<T>(elementNumber);
        }

        public static void Free(ref NativeBufferT<T> target)
        {
            target.length = 0;
            target.count = 0;
            if (target.buffer != null)
            {
                UnsafeHelper.Free(target.buffer);
            }
        }
    }
}

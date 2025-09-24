using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using DialogLib.Util;

namespace DialogLib.Data
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 2)]
    public unsafe struct UnicodeByteBuffer
    {
        /// <summary>
        /// 缓冲区长度
        /// </summary>
        public int length;
        /// <summary>
        /// 已填充的字节数
        /// </summary>
        public int count;
        /// <summary>
        /// 缓冲区头
        /// </summary>
        public byte* buffer;
        /// <summary>
        /// 为缓冲区指定过堆内存
        /// </summary>
        public bool allocated;

        public static void FillMalloc(ref UnicodeByteBuffer target, string src)
        {
            FillMalloc(ref target, src, Encoding.Unicode);
        }
        public static void FillMalloc(ref UnicodeByteBuffer target, string src, Encoding encoding)
        {
            int count = encoding.GetByteCount(src);
            EnsureCapacity(ref target, count);
            var writeable = new Span<byte>(target.buffer, target.length);
            target.count = encoding.GetBytes(src, writeable);
        }

        public static unsafe void FillMalloc(ref UnicodeByteBuffer target, string[] src)
        {
            FillMalloc(ref target, src, Encoding.Unicode);
        }
        public static unsafe void FillMalloc(ref UnicodeByteBuffer target, string[] src, Encoding encoding)
        {
            if (src.Length == 0) { return; }
            if (src.Length == 1)
            {
                FillMalloc(ref target, src[0]);
                return;
            }

            const int separatorByteCount = 2;

            bool isTinyCounter = src.Length > 256;  // 256 * 4 = 1024(bytes)
            int* counterPnt = isTinyCounter ? null : UnsafeHelper.MAllocT<int>(src.Length);
            Span<int> counter = isTinyCounter ? stackalloc int[src.Length]
                : new Span<int>(counterPnt, src.Length);

            // get buffer length
            int dstBufferLen = 0;
            for (int line = 0; line < src.Length; line++)
            {
                int lineByteCount = encoding.GetByteCount(src[line]) + separatorByteCount;
                counter[line] = lineByteCount;
                dstBufferLen += lineByteCount;
            }
            EnsureCapacity(ref target, dstBufferLen);
            target.count = dstBufferLen;

            // write...
            byte* p = target.buffer;
            for (int line = 0; line < src.Length; line++)
            {
                var writeable = new Span<byte>(p, counter[line]);
                int step = encoding.GetBytes(src[line], writeable);
                p += step;

                // "\n"
                p[0] = 0x0A;
                p[1] = 0x00;

                p += separatorByteCount;
            }

            UnsafeHelper.Free(counterPnt);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void EnsureCapacity(ref UnicodeByteBuffer target, int capacity)
        {
            if (capacity > target.length)
            {
                if (target.allocated)
                {
                    UnsafeHelper.Free(target.buffer);
                }
                target.length = MathHelper.CeilPow2(capacity);
                target.buffer = UnsafeHelper.MAllocT<byte>(target.length);
            }
        }

        public static void Free(ref UnicodeByteBuffer target)
        {
            if (target.length > 0)
            {
                Marshal.FreeHGlobal(new IntPtr(target.buffer));
            }
            target.length = 0;
            target.count = 0;
        }

        public override readonly string ToString()
        {
            return ToString(Encoding.Unicode);
        }

        public readonly string ToString(Encoding encoding)
        {
            return count is 0 ? string.Empty : encoding.GetString(buffer, count);
        }
    }
}
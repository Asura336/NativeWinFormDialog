using System;
using System.Runtime.InteropServices;
using System.Text;
using DialogLib.Data;
using DialogLib.Foundations;
using DialogLib.Util;

namespace DialogLib
{
    public static class MessageBox
    {
        public static unsafe DialogResult Show(string text, string caption = null,
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.None,
            MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1,
            MessageBoxOptions options = 0)
        {
            var @params = new MessageBoxParams
            {
                Owner = ApplicationWindow.ApplicationWindowHandle,
                //Text = text,
                //Caption = caption,
                Buttons = buttons,
                Icon = icon,
                DefaultButton = defaultButton,
                Options = options,
            };
            DialogResult result = default;
            var paramPnt = &@params;

            const int tinyBufferLen = 256;
            var encoding = Encoding.Unicode;

            int _textByteSize = encoding.GetByteCount(text);
            bool _textBufMAlloc = _textByteSize > tinyBufferLen;
            Span<byte> _textBuf = _textBufMAlloc
                ? new Span<byte>(UnsafeHelper.MAllocT<byte>(_textByteSize), _textByteSize)
                : stackalloc byte[_textByteSize];
            encoding.GetBytes(text, _textBuf);

            int _captionByteSize = encoding.GetByteCount(caption);
            bool _captionBufMAlloc = _captionByteSize > tinyBufferLen;
            Span<byte> _captionBuf = _captionBufMAlloc
                ? new Span<byte>(UnsafeHelper.MAllocT<byte>(_captionByteSize), _captionByteSize)
                : stackalloc byte[_captionByteSize];
            encoding.GetBytes(caption, _captionBuf);

            fixed (byte* textBufPnt = _textBuf, captionBufPnt = _captionBuf)
            {
                var _text = &paramPnt->Text;
                _text->allocated = _textBufMAlloc;
                _text->length = _textByteSize;
                _text->count = _textByteSize;
                _text->buffer = textBufPnt;

                var _caption = &paramPnt->Caption;
                _caption->allocated = _captionBufMAlloc;
                _caption->length = _captionByteSize;
                _caption->count = _captionByteSize;
                _caption->buffer = captionBufPnt;

                result = ShowMessageBox(paramPnt);

                if (_textBufMAlloc) { UnsafeHelper.Free(textBufPnt); }
                if (_captionBufMAlloc) { UnsafeHelper.Free(captionBufPnt); }
            }

            return result;
        }

        [DllImport("DialogLib", CharSet = CharSet.Unicode)]
        static extern unsafe DialogResult ShowMessageBox([In, Out] MessageBoxParams* Params);
    }
}
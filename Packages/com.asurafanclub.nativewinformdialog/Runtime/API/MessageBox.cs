using System;
using System.Runtime.InteropServices;
using DialogLib.Data;
using DialogLib.Foundations;

namespace DialogLib
{
    public static class MessageBox
    {
        public static DialogResult Show(string text, string caption = null,
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.None,
            MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1,
            MessageBoxOptions options = 0)
        {
            var @params = new MessageBoxParams
            {
                Owner = ApplicationWindow.ApplicationWindowHandle,
                Text = text,
                Caption = caption,
                Buttons = buttons,
                Icon = icon,
                DefaultButton = defaultButton,
                Options = options,
            };

            var inPnt = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(MessageBoxParams)));
            Marshal.StructureToPtr(@params, inPnt, fDeleteOld: false);
            var res = ShowMessageBox(inPnt);
            Marshal.FreeHGlobal(inPnt);
            return res;
        }

        [DllImport("DialogLib", CharSet = CharSet.Unicode)]
        static extern unsafe DialogResult ShowMessageBox(IntPtr IN);
    }
}
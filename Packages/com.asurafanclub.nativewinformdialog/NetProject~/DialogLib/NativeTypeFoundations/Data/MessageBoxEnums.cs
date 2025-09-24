using System;

namespace DialogLib.Data
{
    /// <summary>
    /// Same as System.Windows.Forms.MessageBoxButtons
    /// </summary>
    public enum MessageBoxButtons
    {
        //
        // 摘要:
        //     The message box contains an OK button.
        OK = 0,
        //
        // 摘要:
        //     The message box contains OK and Cancel buttons.
        OKCancel = 1,
        //
        // 摘要:
        //     The message box contains Abort, Retry, and Ignore buttons.
        AbortRetryIgnore = 2,
        //
        // 摘要:
        //     The message box contains Yes, No, and Cancel buttons.
        YesNoCancel = 3,
        //
        // 摘要:
        //     The message box contains Yes and No buttons.
        YesNo = 4,
        //
        // 摘要:
        //     The message box contains Retry and Cancel buttons.
        RetryCancel = 5,
        //
        // 摘要:
        //     Specifies that the message box contains Cancel, Try Again, and Continue buttons.
        CancelTryContinue = 6
    }

    /// <summary>
    /// Same as System.Windows.Forms.MessageBoxIcon
    /// </summary>
    public enum MessageBoxIcon
    {
        //
        // 摘要:
        //     The message box contains no symbols.
        None = 0,
        //
        // 摘要:
        //     The message box contains a symbol consisting of a white X in a circle with a
        //     red background.
        Hand = 16,
        //
        // 摘要:
        //     The message box contains a symbol consisting of white X in a circle with a red
        //     background.
        Stop = 16,
        //
        // 摘要:
        //     The message box contains a symbol consisting of white X in a circle with a red
        //     background.
        Error = 16,
        //
        // 摘要:
        //     The message box contains a symbol consisting of a question mark in a circle.
        //     The question mark message icon is no longer recommended because it does not clearly
        //     represent a specific type of message and because the phrasing of a message as
        //     a question could apply to any message type. In addition, users can confuse the
        //     question mark symbol with a help information symbol. Therefore, do not use this
        //     question mark symbol in your message boxes. The system continues to support its
        //     inclusion only for backward compatibility.
        Question = 32,
        //
        // 摘要:
        //     The message box contains a symbol consisting of an exclamation point in a triangle
        //     with a yellow background.
        Exclamation = 48,
        //
        // 摘要:
        //     The message box contains a symbol consisting of an exclamation point in a triangle
        //     with a yellow background.
        Warning = 48,
        //
        // 摘要:
        //     The message box contains a symbol consisting of a lowercase letter i in a circle.
        Asterisk = 64,
        //
        // 摘要:
        //     The message box contains a symbol consisting of a lowercase letter i in a circle.
        Information = 64
    }

    /// <summary>
    ///  Same as System.Windows.Forms.MessageBoxDefaultButton
    /// </summary>
    public enum MessageBoxDefaultButton
    {
        //
        // 摘要:
        //     The first button on the message box is the default button.
        Button1 = 0,
        //
        // 摘要:
        //     The second button on the message box is the default button.
        Button2 = 256,
        //
        // 摘要:
        //     The third button on the message box is the default button.
        Button3 = 512,
        //
        // 摘要:
        //     Specifies that the Help button on the message box should be the default button.
        Button4 = 768
    }

    /// <summary>
    /// https://learn.microsoft.com/zh-cn/dotnet/api/system.windows.forms.messageboxoptions?view=windowsdesktop-9.0
    /// </summary>
    [Flags]
    public enum MessageBoxOptions
    {
        /// <summary>
        ///  Specifies that the message box is displayed on the active desktop.
        /// </summary>
        ServiceNotification = 2097152,

        /// <summary>
        ///  Specifies that the message box is displayed on the active desktop.
        /// </summary>
        DefaultDesktopOnly = 131072,

        /// <summary>
        ///  Specifies that the message box text is right-aligned.
        /// </summary>
        RightAlign = 524288,

        /// <summary>
        ///  Specifies that the message box text is displayed with Rtl reading order.
        /// </summary>
        RtlReading = 1048576,
    }
}

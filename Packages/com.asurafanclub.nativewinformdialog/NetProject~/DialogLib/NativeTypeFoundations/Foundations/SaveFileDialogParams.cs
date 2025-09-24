using System;
using System.Runtime.InteropServices;
using DialogLib.Data;

namespace DialogLib.Foundations
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 2)]
    public struct SaveFileDialogParams
    {
        // FileDialog
        [MarshalAs(UnmanagedType.U1)] public bool AddExtension;
        [MarshalAs(UnmanagedType.U1)] public bool AddToRecent;
        [MarshalAs(UnmanagedType.U1)] public bool AutoUpgradeEnabled;
        [MarshalAs(UnmanagedType.U1)] public bool CheckFileExists;
        [MarshalAs(UnmanagedType.U1)] public bool CheckPathExists;
        [MarshalAs(UnmanagedType.U1)] public bool DereferenceLinks;
        [MarshalAs(UnmanagedType.U1)] public bool OkRequiresInteraction;
        [MarshalAs(UnmanagedType.U1)] public bool RestoreDirectory;
        [MarshalAs(UnmanagedType.U1)] public bool ShowHiddenFiles;
        [MarshalAs(UnmanagedType.U1)] public bool ShowPinnedPlaces;
        [MarshalAs(UnmanagedType.U1)] public bool SupportMultiDottedExtensions;
        [MarshalAs(UnmanagedType.U1)] public bool ValidateNames;
        public Guid? ClientGuid;
        public int FilterIndex;
        [MarshalAs(UnmanagedType.LPWStr)] public string DefaultExt;
        [MarshalAs(UnmanagedType.LPWStr)] public string FileName;
        [MarshalAs(UnmanagedType.LPWStr)] public string Filter;
        [MarshalAs(UnmanagedType.LPWStr)] public string InitialDirectory;
        [MarshalAs(UnmanagedType.LPWStr)] public string Title;

        // SaveFileDialog
        [MarshalAs(UnmanagedType.U1)] public bool CheckWriteAccess;
        [MarshalAs(UnmanagedType.U1)] public bool CreatePrompt;
        [MarshalAs(UnmanagedType.U1)] public bool ExpandedMode;
        [MarshalAs(UnmanagedType.U1)] public bool OverwritePrompt;
    }


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 2)]
    public unsafe ref struct SaveFileDialogResult
    {
        public DialogResult DialogResult;
        public UnicodeByteBuffer FileName;
    }
}

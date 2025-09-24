using System;
using System.Runtime.InteropServices;
using DialogLib.Data;

namespace DialogLib.Foundations
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 2)]
    public ref struct OpenFileDialogParams
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
        public UnicodeByteBuffer DefaultExt;
        public UnicodeByteBuffer FileName;
        public UnicodeByteBuffer FileNames;
        public UnicodeByteBuffer Filter;
        public UnicodeByteBuffer InitialDirectory;
        public UnicodeByteBuffer Title;

        // OpenFileDialog
        [MarshalAs(UnmanagedType.U1)] public bool Multiselect;
        [MarshalAs(UnmanagedType.U1)] public bool ReadOnlyChecked;
        [MarshalAs(UnmanagedType.U1)] public bool SelectReadOnly;
        [MarshalAs(UnmanagedType.U1)] public bool ShowPreview;
        [MarshalAs(UnmanagedType.U1)] public bool ShowReadOnly;
    }
}

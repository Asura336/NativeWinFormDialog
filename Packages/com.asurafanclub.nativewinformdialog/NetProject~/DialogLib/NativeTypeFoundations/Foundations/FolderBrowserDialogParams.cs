using System;
using System.Runtime.InteropServices;
using DialogLib.Data;

namespace DialogLib.Foundations
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 2)]
    public ref struct FolderBrowserDialogParams
    {
        [MarshalAs(UnmanagedType.U1)] public bool AddToRecent;
        [MarshalAs(UnmanagedType.U1)] public bool AutoUpgradeEnabled;
        [MarshalAs(UnmanagedType.U1)] public bool Multiselect;
        [MarshalAs(UnmanagedType.U1)] public bool OkRequiresInteraction;
        [MarshalAs(UnmanagedType.U1)] public bool ShowHiddenFiles;
        [MarshalAs(UnmanagedType.U1)] public bool ShowNewFolderButton;
        [MarshalAs(UnmanagedType.U1)] public bool ShowPinnedPlaces;
        [MarshalAs(UnmanagedType.U1)] public bool UseDescriptionForTitle;
        public Environment.SpecialFolder RootFolder;
        public UnicodeByteBuffer Description;
        public UnicodeByteBuffer InitialDirectory;
        public UnicodeByteBuffer SelectedPath;
        public UnicodeByteBuffer SelectedPaths;
        public Guid? ClientGuid;
    }
}

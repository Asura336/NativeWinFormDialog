using System;
using System.Runtime.InteropServices;
using DialogLib.Data;
using DialogLib.Foundations;

namespace DialogLib
{
    public class SaveFileDialog
    {
        // FileDialog
        public bool AddExtension { get; set; } = true;
        public bool AddToRecent { get; set; } = false;
        public bool AutoUpgradeEnabled { get; set; } = true;
        public bool CheckFileExists { get; set; } = false;
        public bool CheckPathExists { get; set; } = true;
        public bool DereferenceLinks { get; set; } = true;
        public bool OkRequiresInteraction { get; set; } = false;
        public bool RestoreDirectory { get; set; } = false;
        public bool ShowHiddenFiles { get; set; } = false;
        public bool ShowPinnedPlaces { get; set; } = false;
        public bool SupportMultiDottedExtensions { get; set; } = false;
        public bool ValidateNames { get; set; } = true;
        public Guid? ClientGuid { get; set; } = null;
        public int FilterIndex { get; set; } = 1;
        public string DefaultExt { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string Filter { get; set; } = string.Empty;
        public string InitialDirectory { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;

        // SaveFileDialog
        public bool CheckWriteAccess { get; set; } = false;
        public bool CreatePrompt { get; set; } = false;
        public bool ExpandedMode { get; set; } = false;
        public bool OverwritePrompt { get; set; } = true;

        public void Reset()
        {
            AddExtension = true;
            AddToRecent = false;
            AutoUpgradeEnabled = true;
            CheckFileExists = false;
            CheckPathExists = true;
            DereferenceLinks = true;
            OkRequiresInteraction = false;
            RestoreDirectory = false;
            ShowHiddenFiles = false;
            ShowPinnedPlaces = false;
            SupportMultiDottedExtensions = false;
            ValidateNames = true;
            ClientGuid = null;
            FilterIndex = 1;
            DefaultExt = string.Empty;
            FileName = string.Empty;
            Filter = string.Empty;
            InitialDirectory = string.Empty;
            Title = string.Empty;

            CheckWriteAccess = false;
            CreatePrompt = false;
            ExpandedMode = false;
            OverwritePrompt = true;
        }

        public unsafe DialogResult ShowDialog()
        {
            GetParams(out var @params);
            DialogResult result = default;

            var inPnt = Marshal.AllocHGlobal(Marshal.SizeOf<SaveFileDialogParams>());
            Marshal.StructureToPtr(@params, inPnt, fDeleteOld: false);

            const int tinyBufferLen = 256;
            var resObj = new SaveFileDialogResult();
            var resObjPnt = &resObj;
            var fileNamePnt = &resObjPnt->FileName;

            var _tinyFileName = stackalloc byte[tinyBufferLen];
            fileNamePnt->buffer = _tinyFileName;
            fileNamePnt->length = tinyBufferLen;

            ShowSaveFileDialog(inPnt, resObjPnt);
            result = resObjPnt->DialogResult;

            FileName = resObjPnt->FileName.ToString();

            if (resObjPnt->FileName.buffer != _tinyFileName)
            {
                UnicodeByteBuffer.Free(ref resObjPnt->FileName);
            }
            Marshal.FreeHGlobal(inPnt);
            return result;
        }


        void GetParams(out SaveFileDialogParams @params)
        {
            @params = new SaveFileDialogParams
            {
                AddExtension = AddExtension,
                AddToRecent = AddToRecent,
                AutoUpgradeEnabled = AutoUpgradeEnabled,
                CheckFileExists = CheckFileExists,
                CheckPathExists = CheckPathExists,
                DereferenceLinks = DereferenceLinks,
                OkRequiresInteraction = OkRequiresInteraction,
                RestoreDirectory = RestoreDirectory,
                ShowHiddenFiles = ShowHiddenFiles,
                ShowPinnedPlaces = ShowPinnedPlaces,
                SupportMultiDottedExtensions = SupportMultiDottedExtensions,
                ValidateNames = ValidateNames,
                ClientGuid = ClientGuid,
                FilterIndex = FilterIndex,
                DefaultExt = DefaultExt,
                FileName = FileName,
                Filter = Filter,
                InitialDirectory = InitialDirectory,
                Title = Title,

                CheckWriteAccess = CheckWriteAccess,
                CreatePrompt = CreatePrompt,
                ExpandedMode = ExpandedMode,
                OverwritePrompt = OverwritePrompt,
            };
        }

        [DllImport("DialogLib", CharSet = CharSet.Unicode)]
        static extern unsafe void ShowSaveFileDialog([In] IntPtr IN,
            SaveFileDialogResult* Res);
    }
}
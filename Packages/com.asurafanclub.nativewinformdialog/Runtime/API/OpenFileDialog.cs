using System;
using System.Runtime.InteropServices;
using DialogLib.Data;
using DialogLib.Foundations;

namespace DialogLib
{
    public class OpenFileDialog
    {
        // FileDialog
        public bool AddExtension { get; set; } = true;
        public bool AddToRecent { get; set; } = false;
        public bool AutoUpgradeEnabled { get; set; } = true;
        public bool CheckFileExists { get; set; } = true;
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

        // OpenFileDialog
        public bool Multiselect { get; set; } = false;
        public bool ReadOnlyChecked { get; set; } = false;
        public bool SelectReadOnly { get; set; } = true;
        public bool ShowPreview { get; set; } = true;
        public bool ShowReadOnly { get; set; } = false;

        // read write
        public string[] FileNames { get; private set; } = Array.Empty<string>();

        public void Reset()
        {
            AddExtension = true;
            AddToRecent = false;
            AutoUpgradeEnabled = true;
            CheckFileExists = true;
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

            Multiselect = false;
            ReadOnlyChecked = false;
            SelectReadOnly = true;
            ShowPreview = true;
            ShowReadOnly = false;

            FileNames = Array.Empty<string>();
        }

        public unsafe DialogResult ShowDialog()
        {
            GetParams(out var @params);
            DialogResult result = default;

            var inPnt = Marshal.AllocHGlobal(Marshal.SizeOf<OpenFileDialogParams>());
            Marshal.StructureToPtr(@params, inPnt, fDeleteOld: false);

            const int tinyBufferLen = 256;
            var resObj = new OpenFileDialogResult();
            var resObjPnt = &resObj;
            var fileNamePnt = &resObjPnt->FileName;
            var fileNamesPnt = &resObjPnt->FileNames;

            var _tinyFileName = stackalloc byte[tinyBufferLen];
            var _tinyFileNames = stackalloc byte[tinyBufferLen];

            fileNamePnt->buffer = _tinyFileName;
            fileNamePnt->length = tinyBufferLen;

            fileNamesPnt->buffer = _tinyFileNames;
            fileNamesPnt->length = tinyBufferLen;

            ShowOpenFileDialog(inPnt, resObjPnt);
            result = resObjPnt->DialogResult;

            FileName = resObjPnt->FileName.ToString();
            if (resObjPnt->FileName.buffer != _tinyFileName)
            {
                UnicodeByteBuffer.Free(ref resObjPnt->FileName);
            }
            if (resObjPnt->FileNames.count > 0)
            {
                FileNames = resObjPnt->FileNames.ToString().Split('\n', StringSplitOptions.RemoveEmptyEntries);
                if (resObjPnt->FileNames.buffer != _tinyFileNames)
                {
                    UnicodeByteBuffer.Free(ref resObjPnt->FileNames);
                }
            }

            Marshal.FreeHGlobal(inPnt);
            return result;
        }

        void GetParams(out OpenFileDialogParams @params)
        {
            @params = new OpenFileDialogParams
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

                Multiselect = Multiselect,
                ReadOnlyChecked = ReadOnlyChecked,
                SelectReadOnly = SelectReadOnly,
                ShowPreview = ShowPreview,
                ShowReadOnly = ShowReadOnly,
            };
        }

        [DllImport("DialogLib", CharSet = CharSet.Unicode)]
        static extern unsafe void ShowOpenFileDialog([In] IntPtr IN,
            OpenFileDialogResult* Res);
    }
}

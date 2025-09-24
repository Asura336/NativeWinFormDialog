using System;
using System.Runtime.InteropServices;
using DialogLib.Data;
using DialogLib.Foundations;

namespace DialogLib
{
    public class FolderBrowserDialog
    {
        public bool AddToRecent { get; set; } = false;
        public bool AutoUpgradeEnabled { get; set; } = true;
        public bool Multiselect { get; set; } = false;
        public bool OkRequiresInteraction { get; set; } = false;
        public bool ShowHiddenFiles { get; set; } = false;
        public bool ShowNewFolderButton { get; set; } = true;
        public bool ShowPinnedPlaces { get; set; } = false;
        public bool UseDescriptionForTitle { get; set; } = false;
        public Environment.SpecialFolder RootFolder { get; set; } = Environment.SpecialFolder.Desktop;
        public Guid? ClientGuid { get; set; } = null;
        public string Description { get; set; } = string.Empty;
        public string InitialDirectory { get; set; } = string.Empty;
        public string SelectedPath { get; set; } = string.Empty;

        // read write
        public string[] SelectedPaths { get; private set; } = Array.Empty<string>();

        public void Reset()
        {
            AddToRecent = false;
            AutoUpgradeEnabled = true;
            Multiselect = false;
            OkRequiresInteraction = false;
            ShowHiddenFiles = false;
            ShowNewFolderButton = true;
            ShowPinnedPlaces = false;
            UseDescriptionForTitle = false;
            RootFolder = Environment.SpecialFolder.Desktop;
            ClientGuid = null;
            Description = string.Empty;
            InitialDirectory = string.Empty;
            SelectedPath = string.Empty;
            SelectedPaths = Array.Empty<string>();
        }

        public unsafe DialogResult ShowDialog()
        {
            GetParams(out var @params);
            DialogResult result = default;

            FolderBrowserDialogParams* @paramsPnt = &@params;
            UnicodeByteBuffer.FillMalloc(ref paramsPnt->Description, Description);
            UnicodeByteBuffer.FillMalloc(ref paramsPnt->InitialDirectory, InitialDirectory);

            //const int tinyBufferLen = 256;
            var selectedPathPnt = @paramsPnt->SelectedPath;
            //var _tinySelectedPath = stackalloc byte[tinyBufferLen];
            //selectedPathPnt.buffer = _tinySelectedPath;
            //selectedPathPnt.length = tinyBufferLen;
            //selectedPathPnt.allocated = false;

            var selectedPathsPnt = @paramsPnt->SelectedPaths;
            //var _tinySelectedPaths = stackalloc byte[tinyBufferLen];
            //selectedPathsPnt.buffer = _tinySelectedPaths;
            //selectedPathsPnt.length = tinyBufferLen;
            //selectedPathsPnt.allocated = false;

            UnicodeByteBuffer.FillMalloc(ref selectedPathPnt, SelectedPath);
            UnicodeByteBuffer.FillMalloc(ref selectedPathsPnt, SelectedPaths);


            result = ShowFolderBrowserDialog(paramsPnt);
            SelectedPath = paramsPnt->SelectedPath.ToString();
            if (paramsPnt->SelectedPaths.count > 0)
            {
                SelectedPaths = paramsPnt->SelectedPaths.ToString().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            }


            UnicodeByteBuffer.Free(ref @paramsPnt->Description);
            UnicodeByteBuffer.Free(ref @paramsPnt->InitialDirectory);
            //if (paramsPnt->SelectedPath.buffer != _tinySelectedPath)
            {
                UnicodeByteBuffer.Free(ref @paramsPnt->SelectedPath);
            }
            //if (paramsPnt->SelectedPaths.buffer != _tinySelectedPaths)
            {
                UnicodeByteBuffer.Free(ref @paramsPnt->SelectedPaths);
            }
            return result;
        }

        void GetParams(out FolderBrowserDialogParams @params)
        {
            @params = new FolderBrowserDialogParams
            {
                AddToRecent = AddToRecent,
                AutoUpgradeEnabled = AutoUpgradeEnabled,
                Multiselect = Multiselect,
                OkRequiresInteraction = OkRequiresInteraction,
                ShowHiddenFiles = ShowHiddenFiles,
                ShowNewFolderButton = ShowNewFolderButton,
                ShowPinnedPlaces = ShowPinnedPlaces,
                UseDescriptionForTitle = UseDescriptionForTitle,
                RootFolder = RootFolder,
                ClientGuid = ClientGuid,
                //Description = Description,  // 字符串类型的 Description 会有乱码，可能编码不一样？
                //InitialDirectory = InitialDirectory,
                //SelectedPath = SelectedPath,
            };
        }

        [DllImport("DialogLib", CharSet = CharSet.Unicode)]
        static extern unsafe DialogResult ShowFolderBrowserDialog([In, Out] FolderBrowserDialogParams* Param);
    }
}

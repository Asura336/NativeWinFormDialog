using System;
using System.Runtime.InteropServices;
using System.Text;
using DialogLib.Data;
using DialogLib.Foundations;
using DialogLib.Util;

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

            FolderBrowserDialogParams* paramPnt = &@params;

            // 尝试用栈缓冲区预分配时程序崩溃，使用堆分配读写缓冲区
            var selectedPathPnt = paramPnt->SelectedPath;
            var selectedPathsPnt = paramPnt->SelectedPaths;
            UnicodeByteBuffer.FillMalloc(ref selectedPathPnt, SelectedPath);
            UnicodeByteBuffer.FillMalloc(ref selectedPathsPnt, SelectedPaths);

            const int tinyBufferLen = 256;
            var encoding = Encoding.Unicode;

            int desc_size = string.IsNullOrEmpty(Description) ? 0 : encoding.GetByteCount(Description);
            bool desc_alloc = desc_size > tinyBufferLen;
            Span<byte> desc_buff = desc_alloc
                ? new Span<byte>(UnsafeHelper.MAllocT<byte>(desc_size), desc_size)
                : desc_size is 0 ? Span<byte>.Empty : stackalloc byte[desc_size];

            int initDir_size = string.IsNullOrEmpty(InitialDirectory) ? 0 : encoding.GetByteCount(InitialDirectory);
            bool initDir_alloc = initDir_size > tinyBufferLen;
            Span<byte> initDir_buff = initDir_alloc
                ? new Span<byte>(UnsafeHelper.MAllocT<byte>(initDir_size), initDir_size)
                : initDir_size is 0 ? Span<byte>.Empty : stackalloc byte[initDir_size];

            fixed (byte* desc_pnt = desc_buff, initDir_pnt = initDir_buff)
            {
                var _desc = &paramPnt->Description;
                _desc->allocated = desc_alloc;
                _desc->length = desc_buff.Length;
                _desc->count = encoding.GetBytes(Description, desc_buff);
                _desc->buffer = desc_pnt;

                var _initDir = &paramPnt->InitialDirectory;
                _initDir->allocated = initDir_alloc;
                _initDir->length = initDir_buff.Length;
                _initDir->count = encoding.GetBytes(InitialDirectory, initDir_buff);
                _initDir->buffer = initDir_pnt;

                result = ShowFolderBrowserDialog(paramPnt);
                SelectedPath = paramPnt->SelectedPath.ToString();
                if (paramPnt->SelectedPaths.count > 0)
                {
                    SelectedPaths = paramPnt->SelectedPaths.ToString()
                        .Split('\n', StringSplitOptions.RemoveEmptyEntries);
                }

                UnicodeByteBuffer.Free(ref paramPnt->Description);
                UnicodeByteBuffer.Free(ref paramPnt->InitialDirectory);
            }

            UnicodeByteBuffer.Free(ref paramPnt->SelectedPath);
            UnicodeByteBuffer.Free(ref paramPnt->SelectedPaths);
            return result;


            ////--------
            //// Origin
            //UnicodeByteBuffer.FillMalloc(ref paramPnt->Description, Description);
            //UnicodeByteBuffer.FillMalloc(ref paramPnt->InitialDirectory, InitialDirectory);

            //var selectedPathPnt = paramPnt->SelectedPath;
            //var selectedPathsPnt = paramPnt->SelectedPaths;
            //UnicodeByteBuffer.FillMalloc(ref selectedPathPnt, SelectedPath);
            //UnicodeByteBuffer.FillMalloc(ref selectedPathsPnt, SelectedPaths);

            //result = ShowFolderBrowserDialog(paramPnt);
            //SelectedPath = paramPnt->SelectedPath.ToString();
            //if (paramPnt->SelectedPaths.count > 0)
            //{
            //    SelectedPaths = paramPnt->SelectedPaths.ToString().Split('\n', StringSplitOptions.RemoveEmptyEntries);
            //}
            //UnicodeByteBuffer.Free(ref paramPnt->Description);
            //UnicodeByteBuffer.Free(ref paramPnt->InitialDirectory);
            //UnicodeByteBuffer.Free(ref paramPnt->SelectedPath);
            //UnicodeByteBuffer.Free(ref paramPnt->SelectedPaths);
            //return result;
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

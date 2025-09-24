using System;
using System.Runtime.InteropServices;
using System.Text;
using DialogLib.Data;
using DialogLib.Foundations;
using DialogLib.Util;

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

            var paramPnt = &@params;

            // 用堆分配初始化读写缓冲区
            var fileNamePnt = paramPnt->FileName;
            UnicodeByteBuffer.FillMalloc(ref fileNamePnt, FileName);

            const int tinyBufferLen = 256;
            var encoding = Encoding.Unicode;

            int defEx_size = string.IsNullOrEmpty(DefaultExt) ? 0 : encoding.GetByteCount(DefaultExt);
            bool defEx_alloc = defEx_size > tinyBufferLen;
            Span<byte> defEx_buff = defEx_alloc
                ? new Span<byte>(UnsafeHelper.MAllocT<byte>(defEx_size), defEx_size)
                : defEx_size is 0 ? Span<byte>.Empty : stackalloc byte[defEx_size];

            int filter_size = string.IsNullOrEmpty(Filter) ? 0 : encoding.GetByteCount(Filter);
            bool filter_alloc = filter_size > tinyBufferLen;
            Span<byte> filter_buff = filter_alloc
                ? new Span<byte>(UnsafeHelper.MAllocT<byte>(filter_size), filter_size)
                : filter_size is 0 ? Span<byte>.Empty : stackalloc byte[filter_size];

            int initDir_size = string.IsNullOrEmpty(InitialDirectory) ? 0 : encoding.GetByteCount(InitialDirectory);
            bool initDir_alloc = initDir_size > tinyBufferLen;
            Span<byte> initDir_buff = initDir_alloc
                ? new Span<byte>(UnsafeHelper.MAllocT<byte>(initDir_size), initDir_size)
                : initDir_size is 0 ? Span<byte>.Empty : stackalloc byte[initDir_size];

            int title_size = string.IsNullOrEmpty(Title) ? 0 : encoding.GetByteCount(Title);
            bool title_alloc = title_size > tinyBufferLen;
            Span<byte> title_buff = title_alloc
                ? new Span<byte>(UnsafeHelper.MAllocT<byte>(title_size), title_size)
                : title_size is 0 ? Span<byte>.Empty : stackalloc byte[title_size];

            fixed (byte* defEx_pnt = defEx_buff, filter_pnt = filter_buff, initDir_pnt = initDir_buff, title_pnt = title_buff)
            {
                var _defEx = &paramPnt->DefaultExt;
                _defEx->allocated = defEx_alloc;
                _defEx->length = defEx_buff.Length;
                _defEx->count = encoding.GetBytes(DefaultExt, defEx_buff);
                _defEx->buffer = defEx_pnt;

                var _filter = &paramPnt->Filter;
                _filter->allocated = filter_alloc;
                _filter->length = filter_buff.Length;
                _filter->count = encoding.GetBytes(Filter, filter_buff);
                _filter->buffer = filter_pnt;

                var _initDir = &paramPnt->InitialDirectory;
                _initDir->allocated = initDir_alloc;
                _initDir->length = initDir_buff.Length;
                _initDir->count = encoding.GetBytes(InitialDirectory, initDir_buff);
                _initDir->buffer = initDir_pnt;

                var _title = &paramPnt->Title;
                _title->allocated = title_alloc;
                _title->length = title_buff.Length;
                _title->count = encoding.GetBytes(Title, title_buff);
                _title->buffer = title_pnt;

                result = ShowSaveFileDialog(paramPnt);

                UnicodeByteBuffer.Free(ref paramPnt->DefaultExt);
                UnicodeByteBuffer.Free(ref paramPnt->Filter);
                UnicodeByteBuffer.Free(ref paramPnt->InitialDirectory);
                UnicodeByteBuffer.Free(ref paramPnt->Title);
            }

            FileName = paramPnt->FileName.ToString();

            UnicodeByteBuffer.Free(ref paramPnt->FileName);

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
                //DefaultExt = DefaultExt,
                //FileName = FileName,
                //Filter = Filter,
                //InitialDirectory = InitialDirectory,
                //Title = Title,

                CheckWriteAccess = CheckWriteAccess,
                CreatePrompt = CreatePrompt,
                ExpandedMode = ExpandedMode,
                OverwritePrompt = OverwritePrompt,
            };
        }

        [DllImport("DialogLib", CharSet = CharSet.Unicode)]
        static extern unsafe DialogResult ShowSaveFileDialog([In, Out] SaveFileDialogParams* Param);
    }
}
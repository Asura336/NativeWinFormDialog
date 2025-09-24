using System;
using System.Runtime.InteropServices;
using System.Text;
using DialogLib.Data;
using DialogLib.Foundations;
using DialogLib.Util;

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

            var paramPnt = &@params;

            // 用堆分配初始化读写缓冲区
            var fileNamePnt = paramPnt->FileName;
            var fileNamesPnt = paramPnt->FileNames;
            UnicodeByteBuffer.FillMalloc(ref fileNamePnt, FileName);
            UnicodeByteBuffer.FillMalloc(ref fileNamesPnt, FileNames);

            const int tinyBufferLen = 256;
            var encoding = Encoding.Unicode;

            int defEx_size = string.IsNullOrEmpty(DefaultExt) ? 0 : encoding.GetByteCount(DefaultExt);
            bool defEx_alloc = defEx_size > tinyBufferLen;
            Span<byte> defEx_buff = defEx_alloc
                ? new Span<byte>(UnsafeHelper.MAllocT<byte>(defEx_size), defEx_size)
                : defEx_size is 0 ? Span<byte>.Empty : stackalloc byte[defEx_size];

            int filter_size = string.IsNullOrEmpty(DefaultExt) ? 0 : encoding.GetByteCount(DefaultExt);
            bool filter_alloc = filter_size > tinyBufferLen;
            Span<byte> filter_buff = filter_alloc
                ? new Span<byte>(UnsafeHelper.MAllocT<byte>(filter_size), filter_size)
                : filter_size is 0 ? Span<byte>.Empty : stackalloc byte[filter_size];

            int initDir_size = string.IsNullOrEmpty(DefaultExt) ? 0 : encoding.GetByteCount(DefaultExt);
            bool initDir_alloc = initDir_size > tinyBufferLen;
            Span<byte> initDir_buff = initDir_alloc
                ? new Span<byte>(UnsafeHelper.MAllocT<byte>(initDir_size), initDir_size)
                : initDir_size is 0 ? Span<byte>.Empty : stackalloc byte[initDir_size];

            int title_size = string.IsNullOrEmpty(DefaultExt) ? 0 : encoding.GetByteCount(DefaultExt);
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

                var _filter = &paramPnt->DefaultExt;
                _filter->allocated = defEx_alloc;
                _filter->length = defEx_buff.Length;
                _filter->count = encoding.GetBytes(DefaultExt, defEx_buff);
                _filter->buffer = defEx_pnt;

                var _initDir = &paramPnt->DefaultExt;
                _initDir->allocated = defEx_alloc;
                _initDir->length = defEx_buff.Length;
                _initDir->count = encoding.GetBytes(DefaultExt, defEx_buff);
                _initDir->buffer = defEx_pnt;

                var _title = &paramPnt->DefaultExt;
                _title->allocated = defEx_alloc;
                _title->length = defEx_buff.Length;
                _title->count = encoding.GetBytes(DefaultExt, defEx_buff);
                _title->buffer = defEx_pnt;

                result = ShowOpenFileDialog(paramPnt);

                UnicodeByteBuffer.Free(ref paramPnt->DefaultExt);
                UnicodeByteBuffer.Free(ref paramPnt->Filter);
                UnicodeByteBuffer.Free(ref paramPnt->InitialDirectory);
                UnicodeByteBuffer.Free(ref paramPnt->Title);
            }

            FileName = paramPnt->FileName.ToString();
            if (paramPnt->FileNames.count > 0)
            {
                FileNames = paramPnt->FileNames.ToString()
                    .Split('\n', StringSplitOptions.RemoveEmptyEntries);
            }

            UnicodeByteBuffer.Free(ref paramPnt->FileName);
            UnicodeByteBuffer.Free(ref paramPnt->FileNames);

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
                //DefaultExt = DefaultExt,
                //FileName = FileName,
                //Filter = Filter,
                //InitialDirectory = InitialDirectory,
                //Title = Title,

                Multiselect = Multiselect,
                ReadOnlyChecked = ReadOnlyChecked,
                SelectReadOnly = SelectReadOnly,
                ShowPreview = ShowPreview,
                ShowReadOnly = ShowReadOnly,
            };
        }

        [DllImport("DialogLib", CharSet = CharSet.Unicode)]
        static extern unsafe DialogResult ShowOpenFileDialog([In, Out] OpenFileDialogParams* Params);
    }
}

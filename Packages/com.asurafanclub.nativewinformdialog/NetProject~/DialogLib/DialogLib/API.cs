using System.Runtime.InteropServices;
using DialogLib.Data;
using DialogLib.Foundations;

namespace DialogLib;

public class API
{
    [UnmanagedCallersOnly(EntryPoint = "ShowMessageBox")]
    public static unsafe Data.DialogResult ShowMessageBox(MessageBoxParams* Param)
    {
        var result = MessageBox.Show(
            owner: new InternalWindowHandle(Param->Owner),
            text: Param->Text.ToString(),
            caption: Param->Caption.ToString(),
            buttons: (System.Windows.Forms.MessageBoxButtons)Param->Buttons,
            icon: (System.Windows.Forms.MessageBoxIcon)Param->Icon,
            defaultButton: (System.Windows.Forms.MessageBoxDefaultButton)Param->DefaultButton,
            options: (System.Windows.Forms.MessageBoxOptions)Param->Options);
        return (Data.DialogResult)result;
    }

    [UnmanagedCallersOnly(EntryPoint = "ShowOpenFileDialog")]
    public static unsafe Data.DialogResult ShowOpenFileDialog(OpenFileDialogParams* Param)
    {
        var ofn = new OpenFileDialog
        {
            AddExtension = Param->AddExtension,
            AddToRecent = Param->AddToRecent,
            AutoUpgradeEnabled = Param->AutoUpgradeEnabled,
            CheckFileExists = Param->CheckFileExists,
            CheckPathExists = Param->CheckPathExists,
            DereferenceLinks = Param->DereferenceLinks,
            OkRequiresInteraction = Param->OkRequiresInteraction,
            RestoreDirectory = Param->RestoreDirectory,
            ShowHiddenFiles = Param->ShowHiddenFiles,
            ShowPinnedPlaces = Param->ShowPinnedPlaces,
            SupportMultiDottedExtensions = Param->SupportMultiDottedExtensions,
            ValidateNames = Param->ValidateNames,
            ClientGuid = Param->ClientGuid,
            FilterIndex = Param->FilterIndex,
            DefaultExt = Param->DefaultExt.ToString(),
            FileName = Param->FileName.ToString(),
            Filter = Param->Filter.ToString(),
            InitialDirectory = Param->InitialDirectory.ToString(),
            Title = Param->Title.ToString(),

            Multiselect = Param->Multiselect,
            ReadOnlyChecked = Param->ReadOnlyChecked,
            SelectReadOnly = Param->SelectReadOnly,
            ShowPreview = Param->ShowPreview,
            ShowReadOnly = Param->ShowReadOnly,
        };
        var ofnRes = ofn.ShowDialog();
        UnicodeByteBuffer.FillMalloc(ref Param->FileName, ofn.FileName);
        if (ofn.Multiselect)
        {
            UnicodeByteBuffer.FillMalloc(ref Param->FileNames, ofn.FileNames);
        }
        return (Data.DialogResult)ofnRes;
    }

    [UnmanagedCallersOnly(EntryPoint = "ShowSaveFileDialog")]
    public static unsafe void ShowSaveFileDialog(IntPtr IN, SaveFileDialogResult* Res)
    {
        var @params = Marshal.PtrToStructure<SaveFileDialogParams>(IN);
        var sfn = new SaveFileDialog
        {
            AddExtension = @params.AddExtension,
            AddToRecent = @params.AddToRecent,
            AutoUpgradeEnabled = @params.AutoUpgradeEnabled,
            CheckFileExists = @params.CheckFileExists,
            CheckPathExists = @params.CheckPathExists,
            DereferenceLinks = @params.DereferenceLinks,
            OkRequiresInteraction = @params.OkRequiresInteraction,
            RestoreDirectory = @params.RestoreDirectory,
            ShowHiddenFiles = @params.ShowHiddenFiles,
            ShowPinnedPlaces = @params.ShowPinnedPlaces,
            SupportMultiDottedExtensions = @params.SupportMultiDottedExtensions,
            ValidateNames = @params.ValidateNames,
            ClientGuid = @params.ClientGuid,
            FilterIndex = @params.FilterIndex,
            DefaultExt = @params.DefaultExt,
            FileName = @params.FileName,
            Filter = @params.Filter,
            InitialDirectory = @params.InitialDirectory,
            Title = @params.Title,

            CheckWriteAccess = @params.CheckWriteAccess,
            CreatePrompt = @params.CreatePrompt,
            ExpandedMode = @params.ExpandedMode,
            OverwritePrompt = @params.OverwritePrompt,
        };

        var sfnRes = sfn.ShowDialog();
        Res->DialogResult = (Data.DialogResult)sfnRes;
        UnicodeByteBuffer.FillMalloc(ref Res->FileName, sfn.FileName);
    }

    [UnmanagedCallersOnly(EntryPoint = "ShowFolderBrowserDialog")]
    public static unsafe Data.DialogResult ShowFolderBrowserDialog(FolderBrowserDialogParams* Param)
    {
        var fb = new FolderBrowserDialog
        {
            AddToRecent = Param->AddToRecent,
            AutoUpgradeEnabled = Param->AutoUpgradeEnabled,
            Multiselect = Param->Multiselect,
            OkRequiresInteraction = Param->OkRequiresInteraction,
            ShowHiddenFiles = Param->ShowHiddenFiles,
            ShowNewFolderButton = Param->ShowNewFolderButton,
            ShowPinnedPlaces = Param->ShowPinnedPlaces,
            UseDescriptionForTitle = Param->UseDescriptionForTitle,
            RootFolder = Param->RootFolder,
            ClientGuid = Param->ClientGuid,
            Description = Param->Description.ToString(),
            InitialDirectory = Param->InitialDirectory.ToString(),
            SelectedPath = Param->SelectedPath.ToString(),
        };

        var ofnRes = fb.ShowDialog();
        UnicodeByteBuffer.FillMalloc(ref Param->SelectedPath, fb.SelectedPath);
        if (Param->Multiselect)
        {
            UnicodeByteBuffer.FillMalloc(ref Param->SelectedPaths, fb.SelectedPaths);
        }

        return (Data.DialogResult)ofnRes;
    }

    [UnmanagedCallersOnly(EntryPoint = "ShowColorDialog")]
    public static unsafe Data.DialogResult ShowColorDialog(ColorDialogParams* Param)
    {
        int[] customColors;
        if (Param->CustomColors.count == 0)
        {
            customColors = null!;
        }
        else
        {
            int* p = (int*)Param->CustomColors.buffer;
            int count = Param->CustomColors.count;
            customColors = new int[count];
            new Span<int>(p, count).CopyTo(customColors);
        }
        var cd = new ColorDialog
        {
            AllowFullOpen = Param->AllowFullOpen,
            AnyColor = Param->AnyColor,
            Color = Param->Color,
            CustomColors = customColors,
            FullOpen = Param->FullOpen,
            SolidColorOnly = Param->SolidColorOnly,
        };
        var res = cd.ShowDialog();

        Param->Color = cd.Color;
        if (Param->CustomColors.length < cd.CustomColors.Length)
        {
            NativeBufferT<ColorARGB>.Free(ref Param->CustomColors);
            NativeBufferT<ColorARGB>.MAlloc(ref Param->CustomColors, cd.CustomColors.Length);
        }
        int* dstPnt = (int*)Param->CustomColors.buffer;
        Param->CustomColors.count = cd.CustomColors.Length;
        var dstSpan = new Span<int>(dstPnt, Param->CustomColors.count);
        new Span<int>(cd.CustomColors).CopyTo(dstSpan);

        return (Data.DialogResult)res;
    }
}

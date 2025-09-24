using System;
using DialogLib;
using DialogLib.Data;
using UnityEngine;

public class TestDialogs : MonoBehaviour
{
    public int x;
    public int y;

    public int width = 200;
    public int height = 400;

    private void OnGUI()
    {
        Rect rect = new Rect(x, y, width, height);
        GUILayout.BeginArea(rect);
        GUILayout.Box("Commands: ");
        GUILayout.BeginVertical();

        if (GUILayout.Button("MessageBox"))
        {
            var res = MessageBox.Show(
                 "Hello World 消息内容",
                 "Message(Native) 消息头",
                 MessageBoxButtons.OKCancel);
            Debug.Log(res);
        }
        if (GUILayout.Button("OpenFileDialog"))
        {
            var ofn = new OpenFileDialog
            {
                Title = "打开文件(Native)",
                DefaultExt = "txt",
                Filter = "文本文件（.txt）|*.txt|所有文件|*.*",
                FileName = "未命名",
                FilterIndex = 2,
                Multiselect = true,
            };
            var res = ofn.ShowDialog();
            Debug.Log(res);
            Debug.Log(ofn.FileName);
            Debug.Log(string.Join("\r\n", ofn.FileNames));
        }
        if (GUILayout.Button("SaveFileDialog"))
        {
            var sfn = new SaveFileDialog
            {
                Title = "保存文件(Native)",
                DefaultExt = "txt",
                Filter = "文本文件（.txt）|*.txt|所有文件|*.*",
                FileName = "未命名",

                ShowPinnedPlaces = false,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                RestoreDirectory = false,
                ShowHiddenFiles = true,
                CreatePrompt = false,
                OverwritePrompt = false,
                OkRequiresInteraction = true,
            };
            var res = sfn.ShowDialog();
            Debug.Log(res);
            Debug.Log(sfn.FileName);
        }
        if (GUILayout.Button("FolderBrowser"))
        {
            var fb = new FolderBrowserDialog
            {
                RootFolder = Environment.SpecialFolder.MyDocuments,
                Multiselect = true,
                Description = "文件夹浏览提示DEFG",
                UseDescriptionForTitle = true,
                ShowPinnedPlaces = true,
                ShowNewFolderButton = true,
                AddToRecent = true,
            };
            var res = fb.ShowDialog();
            Debug.Log(res);
            Debug.Log(fb.SelectedPath);
            Debug.Log(string.Join("\r\n", fb.SelectedPaths));
        }
        if (GUILayout.Button("ColorDialog"))
        {
            var cd = new ColorDialog
            {
                AllowFullOpen = true,
                AnyColor = true,
                FullOpen = true,
                CustomColors = new Color32[] { Color.red, Color.green, Color.blue, Color.cyan },
            };
            var res = cd.ShowDialog();
            Debug.Log(res);
            var color = cd.Color;
            Debug.Log($"Color => <color=#{color.r:X2}{color.g:X2}{color.b:X2}FF>{color}</color>");
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}

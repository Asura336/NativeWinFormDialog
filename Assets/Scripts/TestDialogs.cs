using System;
using System.Collections;
using System.Collections.Generic;
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

        if (GUILayout.Button("MessageBox(Native)"))
        {
            var res = DialogLib.MessageBox.Show(
                 "Hello World 消息内容",
                 "Message(Native) 消息头",
                 DialogLib.Data.MessageBoxButtons.OKCancel);
            Debug.Log(res);
        }
        if (GUILayout.Button("OpenFileDialog(Native)"))
        {
            var ofn = new DialogLib.OpenFileDialog
            {
                Title = "保存文件",
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
        if (GUILayout.Button("SaveFileDialog(Native)"))
        {
            var sfn = new DialogLib.SaveFileDialog
            {
                Title = "保存文件",
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
        if (GUILayout.Button("FolderBrowser(Native)"))
        {
            var fb = new DialogLib.FolderBrowserDialog
            {
                RootFolder = Environment.SpecialFolder.MyDocuments,
                Multiselect = true,
                Description = "文件夹浏览提示DEFG",
                //UseDescriptionForTitle = true,
                ShowPinnedPlaces = true,
                ShowNewFolderButton = true,
                AddToRecent = true,
            };
            var res = fb.ShowDialog();
            Debug.Log(res);
            Debug.Log(fb.SelectedPath);
            Debug.Log(string.Join("\r\n", fb.SelectedPaths));
        }
        if (GUILayout.Button("ColorDialog(Native)"))
        {
            var cd = new DialogLib.ColorDialog
            {
                AllowFullOpen = true,
                AnyColor = true,
                FullOpen = true,
                CustomColors = new Color32[] { Color.red, Color.green, Color.blue, Color.cyan },
            };
            var res = cd.ShowDialog();
            var color = cd.Color;
            Debug.Log($"Color => <color=#{color.r:X2}{color.g:X2}{color.b:X2}FF>{color}</color>");
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}

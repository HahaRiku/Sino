using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Diagnostics;

public class ScriptReaderWindow : EditorWindow
{

    private string fileName;
    private string filePath;

    string test;

    ScriptReaderWindow()
    {
        titleContent = new GUIContent("Script Reader");
    }

    [MenuItem("Window/劇本讀入")]
    static void Init()
    {
        ScriptReaderWindow window = (ScriptReaderWindow)GetWindow(typeof(ScriptReaderWindow));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Space(10);

        //路徑選擇
        GUILayout.BeginHorizontal();
        GUILayout.Label("劇本文檔路徑  " + filePath);
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("..."))
            filePath = EditorUtility.OpenFilePanel("Overwrite with txt", "", "txt");
        GUILayout.EndHorizontal();
        if (fileName == "")
            fileName = Path.GetFileName(filePath);
        GUILayout.Space(10);
        fileName = EditorGUILayout.TextField("新劇本名稱", fileName);

        GUI.skin.button.alignment = TextAnchor.MiddleCenter;
        //確定
        if (GUILayout.Button("按我生成 Story Data"))
        {
            if (File.Exists(filePath))
            {
                string dataAsTxt = File.ReadAllText(filePath);
                int warn = Reader.CheckFormat(dataAsTxt);
                if (warn == 0)
                    Reader.TextToStoryData(fileName, dataAsTxt);
                else if (warn > 0 && EditorUtility.DisplayDialog("帽の提醒", "警告：第" + warn + "行格式有誤", "仍要繼續", "取消"))
                    Reader.TextToStoryData(fileName, dataAsTxt);
                EditorUtility.FocusProjectWindow();
            }
            else
            {
                EditorUtility.DisplayDialog("帽の提醒", "發生錯誤：路徑不存在", "OK");
            }
        }
    }
}
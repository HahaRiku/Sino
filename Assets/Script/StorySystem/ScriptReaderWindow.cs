using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Diagnostics;

public class ScriptReaderWindow : EditorWindow {

    string fileName;
    string filePath;

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
        fileName = EditorGUILayout.TextField("劇本名稱", fileName);

        //路徑選擇
        GUILayout.BeginHorizontal();
        GUILayout.Label("劇本路徑  " + filePath);
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("..."))
            filePath = EditorUtility.OpenFilePanel("Overwrite with txt", "", "txt");
        GUILayout.EndHorizontal();
        GUI.skin.button.alignment = TextAnchor.MiddleCenter;
        //確定
        if (GUILayout.Button("按我生成 Story Data"))
        {
            if (File.Exists(filePath))
            {
                string dataAsTxt = File.ReadAllText(filePath);
                UnityEngine.Debug.Log(dataAsTxt);
            }
            else
            {
                EditorUtility.DisplayDialog("帽の提醒", "發生錯誤：路徑不存在", "OK");
            }
        } 
    }

    private void LoadGameData()
    {

    }

    /*private void SaveGameData()
    {

       // string dataAsJson = JsonUtility.ToJson(gameData);

        string filePath = Application.dataPath + gameDataProjectFilePath;
        //File.WriteAllText(filePath, dataAsJson);

    }*/
}
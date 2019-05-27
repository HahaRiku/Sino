using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public static class Reader
{
    public static T CreateAsset<T>(string assetName, string parentFolder) where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();

        string[] pathSegments = assetName.Split('/');
        string folder = "Assets/" + parentFolder;
        for (int i = 0; i < pathSegments.Length - 1; i++)
        {
            if (!Directory.Exists(Application.dataPath + Path.GetDirectoryName(folder + "/" + pathSegments[i])))
                AssetDatabase.CreateFolder(folder, pathSegments[i]);
            folder += "/" + pathSegments[i]; 
        }
        AssetDatabase.CreateAsset(asset, "Assets/" + parentFolder + "/" + assetName + ".asset");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();

        return asset;
    }

    public static void TextToStoryData(string filename, string txt)
    {
        StoryData story = CreateAsset<StoryData>(filename, "Resources");
        List<string> list = txt.Split('\n').ToList();

        string word = @"""([^<>""]*)""";
        string tag_name = @"^<\s*name\s*=\s*""[^<>=]*""(?: color\s*=\s*""[^<>=]*"")?\s*>$";
        string tag_image = @"^<\s*image\s*=\s*""[^<>=]*""(?: location\s*=\s*""[^<>=]*"")?\s*>$";
        string tag_wait = @"^<\s*wait\s*=\s*""[^<>=]*""\s*>$";
        string tag_script = @"^<\s*script\s*=\s*""[^<>=]*""(?: parameter\s*=\s*""[^<>=]*"")*\s*>$";
        string tag_button = @"^<\s*button\s+text\s*=\s*""[^<>=]*""\s+jump\s*=\s*""[^<>=]*""(?:\s+button\s+text\s*=\s*""[^<>=]*""\s+jump\s*=\s*""[^<>=]*"")*\s*>$";
        string tag_select = @"^<\s*wait\s+select\s*>$";
        string tag_label = @"^<\s*label\s*=\s*""[^<>=]*""\s*>$";
        string tag_jump = @"^<\s*jump\s*=\s*""[^<>=]*""\s*>$";
        string tag_dialog = @"^<\s*dialog (?:open|close)\s*>$";

        string name = "";
        foreach (string t in list)
        {
            string data = t.Trim().Replace("\r", "");
            if (Regex.IsMatch(data, tag_name))
            {
                name = Regex.Matches(data, word)[0].ToString();
                name = name.Substring(1, name.Length - 2);
            }
            else if (Regex.IsMatch(data, tag_image))
                continue;
            else if (Regex.IsMatch(data, tag_wait))
                continue;
            else if (Regex.IsMatch(data, tag_script))
                continue;
            else if (Regex.IsMatch(data, tag_button))
                continue;
            else if (Regex.IsMatch(data, tag_select))
                continue;
            else if (Regex.IsMatch(data, tag_label))
                continue;
            else if (Regex.IsMatch(data, tag_jump))
                continue;
            else if (Regex.IsMatch(data, tag_dialog))
                continue;
            else
            {
                story.StateList.Add(new StoryData.StoryState(name, data));
            }
        }
    }

    public static int CheckFormat(string txt)
    {
        string[] array = txt.Split('\n');
        string word = @"^[^<>""]*$";
        string tag_name = @"^<\s*name\s*=\s*""[^<>=]*""(?: color\s*=\s*""[^<>=]*"")?\s*>$";
        string tag_image = @"^<\s*image\s*=\s*""[^<>=]*""(?: location\s*=\s*""[^<>=]*"")?\s*>$";
        string tag_wait = @"^<\s*wait\s*=\s*""[^<>=]*""\s*>$";
        string tag_script = @"^<\s*script\s*=\s*""[^<>=]*""(?: parameter\s*=\s*""[^<>=]*"")*\s*>$";
        string tag_button = @"^<\s*button\s+text\s*=\s*""[^<>=]*""\s+jump\s*=\s*""[^<>=]*""(?:\s+button\s+text\s*=\s*""[^<>=]*""\s+jump\s*=\s*""[^<>=]*"")*\s*>$";
        string tag_select = @"^<\s*wait\s+select\s*>$";
        string tag_label = @"^<\s*label\s*=\s*""[^<>=]*""\s*>$";
        string tag_jump = @"^<\s*jump\s*=\s*""[^<>=]*""\s*>$";
        string tag_dialog = @"^<\s*dialog (?:open|close)\s*>$";
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = array[i].Trim().Replace("\r", "");
            if (Regex.IsMatch(array[i], word))
                continue;
            else if (Regex.IsMatch(array[i], tag_name))
                continue;
            else if (Regex.IsMatch(array[i], tag_image))
                continue;
            else if (Regex.IsMatch(array[i], tag_wait))
                continue;
            else if (Regex.IsMatch(array[i], tag_script))
                continue;
            else if (Regex.IsMatch(array[i], tag_button))
                continue;
            else if (Regex.IsMatch(array[i], tag_select))
                continue;
            else if (Regex.IsMatch(array[i], tag_label))
                continue;
            else if (Regex.IsMatch(array[i], tag_jump))
                continue;
            else if (Regex.IsMatch(array[i], tag_dialog))
                continue;
            else
                return i + 1;
        }
        return 0;
    }
}

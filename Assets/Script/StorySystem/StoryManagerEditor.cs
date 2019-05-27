using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(StoryManager))]
public class StoryManagerEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        StoryManager myScript = (StoryManager)target;
        if (GUILayout.Button("Begin Story"))
        {
            myScript.BeginStory();
        }
    }
}

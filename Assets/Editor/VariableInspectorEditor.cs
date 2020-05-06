using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VariableInspector))]
public class VariableInspectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        VariableInspector myScript = (VariableInspector)target;
        if (GUILayout.Button("Refresh"))
        {
            myScript.Refresh();
        }
    }

}

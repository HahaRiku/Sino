using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NPC))]
public class NPCEditor : Editor {

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var sO = serializedObject;
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(sO.FindProperty("type"), new GUIContent("NPC種類"));
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(sO.FindProperty("state"), new GUIContent("目前狀態"));
        
        if (sO.FindProperty("type").enumValueIndex == (int)NPC.NpcType.item)
        {
            EditorGUILayout.PropertyField(sO.FindProperty("itemType"), new GUIContent("是否可撿"));
            if(sO.FindProperty("itemType").enumValueIndex == (int)NPC.ItemType.可撿)
                EditorGUILayout.PropertyField(sO.FindProperty("可撿的物品的名字"), new GUIContent("可撿物品名稱"));
            else
                EditorGUILayout.PropertyField(sO.FindProperty("不可撿的物品的敘述"), new GUIContent("不可撿物品敘述"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(sO.FindProperty("冷卻時間"));
            EditorGUILayout.PropertyField(sO.FindProperty("Radius"));
            EditorGUILayout.PropertyField(sO.FindProperty("HintRaius"));
            EditorGUILayout.PropertyField(sO.FindProperty("Offset"));
            EditorGUILayout.Space();
        }
        if (sO.FindProperty("type").enumValueIndex == (int)NPC.NpcType.talk)
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(sO.FindProperty("冷卻時間"));
            EditorGUILayout.PropertyField(sO.FindProperty("Radius"));
            EditorGUILayout.PropertyField(sO.FindProperty("Offset"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(sO.FindProperty("一個點"));
            EditorGUILayout.PropertyField(sO.FindProperty("兩個點"));
            EditorGUILayout.PropertyField(sO.FindProperty("三個點"));
        }
        if (sO.FindProperty("type").enumValueIndex == (int)NPC.NpcType.door)
        {
            EditorGUILayout.PropertyField(sO.FindProperty("門的名字"));
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(sO.FindProperty("門一開始有沒有鎖"));
            if(sO.FindProperty("門一開始有沒有鎖").boolValue)
                EditorGUILayout.PropertyField(sO.FindProperty("需要的鑰匙名字"));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(sO.FindProperty("門要傳送到的場景名稱"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(sO.FindProperty("冷卻時間"));
            EditorGUILayout.PropertyField(sO.FindProperty("Radius"));
            EditorGUILayout.PropertyField(sO.FindProperty("Offset"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(sO.FindProperty("鎖打開"));
            EditorGUILayout.PropertyField(sO.FindProperty("鎖鎖起來"));
        }
        serializedObject.ApplyModifiedProperties();
    }
}


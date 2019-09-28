using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CanEditMultipleObjects]
[CustomEditor(typeof(StoryData))]
public class StoryDataEditor:Editor
{
    ReorderableList list;
    private void OnEnable()
    {
        var serProp = serializedObject.FindProperty("StateList");
        list = new ReorderableList(serProp.serializedObject, serProp);
        list.elementHeight = 120;
        list.drawElementCallback = DrawListElement;
        list.drawHeaderCallback = DrawListHeader;
        list.onAddDropdownCallback = onAddDropdownCallback;
        list.onRemoveCallback = onRemoveCallback;
    }
    void DrawListHeader(Rect rect)
    {
        var spacing = 3;
        var arect = rect;
        arect.x += spacing;
        EditorGUI.LabelField(arect, "List Of State");
    }
    void DrawListElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        #region
        var spacing = 5f;
        var arect = rect;
        var serElem = this.list.serializedProperty.GetArrayElementAtIndex(index);
        var input_w = 120f;
        var label_w = 70f;
        GUIStyle fontStyle = new GUIStyle();
        GUIStyle enumStyle = EditorStyles.popup;
        fontStyle.fontSize = 12;
        var oriStyle = enumStyle.fontSize;
        enumStyle.fontSize = 14;
        enumStyle.fixedHeight = 20;
        enumStyle.CalcScreenSize(new Vector2(10, 10));
        arect.height = 18;
        arect.width = 100;
        EditorGUI.LabelField(arect, "State " + (index + 1));
        EditorGUI.indentLevel++;
        arect.y += arect.height + 3;
        EditorGUI.LabelField(arect, "資料類型: ", fontStyle);
        arect.y += arect.height;
        serElem.FindPropertyRelative("state類型").enumValueIndex = (int)(StoryData.StoryState.type)EditorGUI.EnumPopup(arect, (StoryData.StoryState.type)serElem.FindPropertyRelative("state類型").enumValueIndex, enumStyle);

        arect.y += arect.height + spacing + spacing;
        EditorGUI.LabelField(arect, "繼續條件: ", fontStyle);
        arect.y += arect.height;
        serElem.FindPropertyRelative("continue條件").enumValueIndex = (int)(StoryData.StoryState.condition)EditorGUI.EnumPopup(arect, (StoryData.StoryState.condition)serElem.FindPropertyRelative("continue條件").enumValueIndex, enumStyle);
        enumStyle.fontSize = 10;
        fontStyle.fontSize = 14;
        arect.x += arect.width + 10;
        arect.width = label_w;
        arect.y = rect.y + arect.height;
        input_w = EditorGUIUtility.currentViewWidth - label_w - 180;
        Rect buttonRect = arect;
        buttonRect.xMin = rect.xMax - 20;
        buttonRect.width = 15;
        buttonRect.height = 80;
        #endregion
        if (serElem.FindPropertyRelative("state類型").enumValueIndex == 0)
        {
            EditorGUI.LabelField(arect, "人名: ", fontStyle);
            arect.x += arect.width;
            arect.xMax = rect.xMax - 20;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("Name"), GUIContent.none);
            arect.y += arect.height + spacing;
            arect.width = label_w;
            arect.x -= arect.width;
            EditorGUI.LabelField(arect, "對話內容: ", fontStyle);
            arect.x += arect.width;
            arect.xMax = rect.xMax - 20;
            arect.height = 60;
            serElem.FindPropertyRelative("Text").stringValue = EditorGUI.TextArea(arect, serElem.FindPropertyRelative("Text").stringValue);
        }
        else if (serElem.FindPropertyRelative("state類型").enumValueIndex == 1)
        {
            EditorGUI.LabelField(arect, "角色: ", fontStyle);
            arect.x += arect.width;
            arect.xMax = rect.xMax - 20;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("Character"), GUIContent.none);
            arect.y += arect.height + spacing;
            arect.width = label_w;
            arect.x -= arect.width;
            EditorGUI.LabelField(arect, "x原點: ", fontStyle);
            arect.x += arect.width;
            arect.xMax = rect.xMax - 20;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("OriPositionX"), GUIContent.none);
            arect.y += arect.height + spacing;
            arect.width = label_w;
            arect.x -= arect.width;
            EditorGUI.LabelField(arect, "花費時間: ", fontStyle);
            arect.x += arect.width;
            arect.xMax = rect.xMax - 20;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("Duration"), GUIContent.none);
        }
        else if (serElem.FindPropertyRelative("state類型").enumValueIndex == 2)
        {
            var half_input_w = Mathf.Max((EditorGUIUtility.currentViewWidth - label_w * 3 / 2 - 180) / 2, 0);
            EditorGUI.LabelField(arect, "當變數: ", fontStyle);
            arect.x += arect.width;
            arect.width = half_input_w;
            if (half_input_w < 0) Debug.Log("oops");
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("Flag"), GUIContent.none);
            arect.x += arect.width;
            arect.width = label_w / 2;
            EditorGUI.LabelField(arect, "等於", fontStyle);
            arect.x += arect.width;
            arect.xMax = rect.xMax - 20;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("WhenFlagIs"), GUIContent.none);
            arect.y += arect.height + spacing;
            arect.width = label_w;
            arect.x -= arect.width * 3 / 2 + half_input_w;
            EditorGUI.LabelField(arect, "則跳到: ", fontStyle);
            arect.x += arect.width;
            arect.xMax = rect.xMax - 20;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("ThanJumpTo"), GUIContent.none);
            arect.y += arect.height + spacing;
            arect.width = label_w;
            arect.x -= arect.width;
            EditorGUI.LabelField(arect, "否則跳到: ", fontStyle);
            arect.x += arect.width;
            arect.xMax = rect.xMax - 20;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("ElseJumpTo"), GUIContent.none);
            arect.y += arect.height + spacing;
            arect.width = label_w;
            arect.x -= arect.width;
            EditorGUI.LabelField(arect, "直接跳到: ", fontStyle);
            arect.x += arect.width;
            arect.xMax = rect.xMax - 20;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("JustJump"), GUIContent.none);
        }
        else if (serElem.FindPropertyRelative("state類型").enumValueIndex == 3)
        {
            EditorGUI.LabelField(arect, "變數名稱: ", fontStyle);
            arect.x += arect.width;
            arect.xMax = rect.xMax - 20;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("Variable"), GUIContent.none);
            arect.y += arect.height + spacing;
            arect.width = label_w;
            arect.x -= arect.width;
            EditorGUI.LabelField(arect, "設值: ", fontStyle);
            arect.x += arect.width;
            arect.xMax = rect.xMax - 20;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("Value"), GUIContent.none);
        }
        else if (serElem.FindPropertyRelative("state類型").enumValueIndex == 4)
        {
            var optProp = serElem.FindPropertyRelative("Options");
            fontStyle.fontSize = 12;
            arect.height = 16;

            var small_input_w = (EditorGUIUtility.currentViewWidth - label_w - label_w - 180) / 4;
            for (int i = 0; i < optProp.arraySize; i++)
            {
                arect.width = label_w;
                EditorGUI.LabelField(arect, "選項 " + i, fontStyle);
                arect.x += arect.width;
                arect.width = small_input_w;
                EditorGUI.PropertyField(arect, optProp.GetArrayElementAtIndex(i).FindPropertyRelative("Content"), GUIContent.none);
                arect.x += arect.width;
                arect.width = label_w;
                EditorGUI.LabelField(arect, "跳至標籤: ", fontStyle);
                arect.x += arect.width;
                arect.xMax = rect.xMax - 20;
                EditorGUI.PropertyField(arect, optProp.GetArrayElementAtIndex(i).FindPropertyRelative("JumpTo"), GUIContent.none);
                arect.y += arect.height + spacing - 3;
                arect.x -= label_w + label_w + small_input_w;
            }
            arect.width = label_w;
            arect.x = rect.xMax - 20 - arect.width - arect.width;
            if (GUI.Button(arect, "+"))
            {
                AddOption(index, optProp.arraySize);
            }
            arect.x += arect.width;
            if (GUI.Button(arect, "-"))
            {
                DeleteOption(index, optProp.arraySize - 1);
            }
        }
        else if (serElem.FindPropertyRelative("state類型").enumValueIndex == 5)
        {
            EditorGUI.LabelField(arect, "角色: ", fontStyle);
            arect.x += arect.width;
            arect.xMax = rect.xMax - 20;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("CharName"), GUIContent.none);
            arect.y += arect.height + spacing;
            arect.width = label_w;
            arect.x -= arect.width;
            EditorGUI.LabelField(arect, "心情氣泡: ", fontStyle);
            arect.x += arect.width;
            arect.xMax = rect.xMax - 20;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("Emotion"), GUIContent.none);
        }
        else if (serElem.FindPropertyRelative("state類型").enumValueIndex == 6)
        {
            EditorGUI.LabelField(arect, "標籤名: ", fontStyle);
            arect.x += arect.width;
            arect.xMax = rect.xMax - 20;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("Label"), GUIContent.none);
        }
        else if (serElem.FindPropertyRelative("state類型").enumValueIndex == 7)
        {
            EditorGUI.LabelField(arect, "標籤名: ", fontStyle);
            arect.x += arect.width;
            arect.xMax = rect.xMax - 20;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("LabelJump"), GUIContent.none);
        }
        else if (serElem.FindPropertyRelative("state類型").enumValueIndex == 8)
        {
            EditorGUI.LabelField(arect, "腳本: ", fontStyle);
            arect.x += arect.width;
            arect.xMax = rect.xMax - 20;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("Class"), GUIContent.none);
            arect.y += arect.height + spacing;
            arect.width = label_w;
            arect.x -= arect.width;
            var parProp = serElem.FindPropertyRelative("Parameters");
            for (int i = 0; i < parProp.arraySize; i++)
            {
                EditorGUI.LabelField(arect, "參數 " + i + ": ", fontStyle);
                arect.x += arect.width;
                arect.xMax = rect.xMax - 20;
                EditorGUI.PropertyField(arect, parProp.GetArrayElementAtIndex(i), GUIContent.none);
                arect.y += arect.height + spacing;
                arect.width = label_w;
                arect.x -= arect.width;
            }
        }
        else if (serElem.FindPropertyRelative("state類型").enumValueIndex == 9)
        {
            EditorGUI.LabelField(arect, "等待時間: ", fontStyle);
            arect.x += arect.width;
            arect.xMax = rect.xMax - 20;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("WaitTime"), GUIContent.none);
        }
        else if (serElem.FindPropertyRelative("state類型").enumValueIndex == 10)
        {
            EditorGUI.LabelField(arect, "場景: ", fontStyle);
            arect.x += arect.width;
            arect.xMax = rect.xMax - 20;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("Scene"), GUIContent.none);
            arect.y += arect.height + spacing;
            arect.width = label_w;
            arect.x -= arect.width;
            EditorGUI.LabelField(arect, "人物位置: ", fontStyle);
            arect.x += arect.width;
            arect.xMax = rect.xMax - 20;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("SpawnPoint"), GUIContent.none);
        }
        EditorGUI.indentLevel--;
        /*if (GUI.Button(buttonRect, "-"))
        {
            Delete(index);
        }*/
    }
    void onAddDropdownCallback(Rect buttonRect, ReorderableList list)
    {
        var menu = new GenericMenu();
        menu.AddItem(new GUIContent("新增對話"), false, AddStory);
        menu.AddItem(new GUIContent("新增選項"), false, AddSelectOptions);
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("新增心情"), false, AddEmotion);
        menu.AddItem(new GUIContent("新增動作"), false, AddMovement);
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("新增分支"), false, AddBranch);
        menu.AddItem(new GUIContent("新增變數"), false, AddVariable);
        menu.AddItem(new GUIContent("新增標籤設立"), false, AddLabelSetting);
        menu.AddItem(new GUIContent("新增標籤跳轉"), false, AddLabelJumping);
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("新增轉場"), false, AddTransition);
        menu.AddItem(new GUIContent("新增等待時間"), false, AddWaitTime);
        menu.AddItem(new GUIContent("新增外部腳本"), false, AddOuterScript);
        menu.DropDown(buttonRect);
    }
    void onRemoveCallback(ReorderableList list)
    {
        Debug.Log(list.index);
        if (EditorUtility.DisplayDialog("帽の提醒", "確定要刪掉嗎？", "是", "否"))
            ReorderableList.defaultBehaviours.DoRemoveButton(list);
    }
    void AddStory()
    {
        var index = list.serializedProperty.arraySize;
        list.serializedProperty.arraySize++;
        list.index = index;
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("state類型").enumValueIndex = 0;
        element.FindPropertyRelative("Name").stringValue = "";
        element.FindPropertyRelative("Text").stringValue = "";
        element.FindPropertyRelative("continue條件").enumValueIndex = 0;
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }
    void AddMovement()
    {
        var index = list.serializedProperty.arraySize;
        list.serializedProperty.arraySize++;
        list.index = index;
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("state類型").enumValueIndex = 1;
        element.FindPropertyRelative("Character").stringValue = "";
        element.FindPropertyRelative("OriPositionX").floatValue = 0;
        element.FindPropertyRelative("NewPositionX").floatValue = 0;
        element.FindPropertyRelative("Duration").floatValue = 0;
        element.FindPropertyRelative("continue條件").enumValueIndex = 1;
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }
    void AddBranch()
    {
        var index = list.serializedProperty.arraySize;
        list.serializedProperty.arraySize++;
        list.index = index;
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("state類型").enumValueIndex = 2;
        element.FindPropertyRelative("Flag").stringValue = "";
        element.FindPropertyRelative("WhenFlagIs").intValue = 0;
        element.FindPropertyRelative("JustJump").intValue = 0;
        element.FindPropertyRelative("continue條件").enumValueIndex = 2;
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }
    void AddVariable()
    {
        var index = list.serializedProperty.arraySize;
        list.serializedProperty.arraySize++;
        list.index = index;
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("state類型").enumValueIndex = 3;
        element.FindPropertyRelative("Variable").stringValue = "";
        element.FindPropertyRelative("Value").intValue = 0;
        element.FindPropertyRelative("continue條件").enumValueIndex = 2;
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }
    void AddSelectOptions()
    {
        var index = list.serializedProperty.arraySize;
        list.serializedProperty.arraySize++;
        list.index = index;
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("state類型").enumValueIndex = 4;
        while (element.FindPropertyRelative("Options").arraySize < 2)
            element.FindPropertyRelative("Options").InsertArrayElementAtIndex(0);
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }
    void AddEmotion()
    {
        var index = list.serializedProperty.arraySize;
        list.serializedProperty.arraySize++;
        list.index = index;
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("state類型").enumValueIndex = 5;
        element.FindPropertyRelative("continue條件").enumValueIndex = 1;
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }
    void AddLabelSetting()
    {
        var index = list.serializedProperty.arraySize;
        list.serializedProperty.arraySize++;
        list.index = index;
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("state類型").enumValueIndex = 6;
        element.FindPropertyRelative("continue條件").enumValueIndex = 2;
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }
    void AddLabelJumping()
    {
        var index = list.serializedProperty.arraySize;
        list.serializedProperty.arraySize++;
        list.index = index;
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("state類型").enumValueIndex = 7;
        element.FindPropertyRelative("continue條件").enumValueIndex = 2;
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }
    void AddOuterScript()
    {
        var index = list.serializedProperty.arraySize;
        list.serializedProperty.arraySize++;
        list.index = index;
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("state類型").enumValueIndex = 8;
        element.FindPropertyRelative("continue條件").enumValueIndex = 2;
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }
    void AddWaitTime()
    {
        var index = list.serializedProperty.arraySize;
        list.serializedProperty.arraySize++;
        list.index = index;
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("state類型").enumValueIndex = 9;
        element.FindPropertyRelative("continue條件").enumValueIndex = 1;
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }
    void AddTransition()
    {
        var index = list.serializedProperty.arraySize;
        list.serializedProperty.arraySize++;
        list.index = index;
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        element.FindPropertyRelative("state類型").enumValueIndex = 10;
        element.FindPropertyRelative("continue條件").enumValueIndex = 1;
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }
    void AddOption(int stateListIndex, int arrayNewIndex)
    {
        var element = serializedObject.FindProperty("StateList").GetArrayElementAtIndex(stateListIndex);
        element.FindPropertyRelative("Options").InsertArrayElementAtIndex(arrayNewIndex);
    }
    void DeleteOption(int stateListIndex, int arrayNewIndex)
    {
        var element = serializedObject.FindProperty("StateList").GetArrayElementAtIndex(stateListIndex);
        element.FindPropertyRelative("Options").DeleteArrayElementAtIndex(arrayNewIndex);
    }
    void Delete(int index)
    {
        list.index = index;
        
        onRemoveCallback(list);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        list.DoLayoutList();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
        serializedObject.ApplyModifiedProperties();
    }
}

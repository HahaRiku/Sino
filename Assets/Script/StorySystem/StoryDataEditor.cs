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

        if (serElem.FindPropertyRelative("state類型").enumValueIndex == 0)
        {
            EditorGUI.LabelField(arect, "人名: ", fontStyle);
            arect.x += arect.width;
            arect.width = input_w;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("Name"), GUIContent.none);
            arect.y += arect.height + spacing;
            arect.width = label_w;
            arect.x -= arect.width;
            EditorGUI.LabelField(arect, "對話內容: ", fontStyle);
            arect.x += arect.width;
            arect.width = input_w;
            arect.height = 60;
            serElem.FindPropertyRelative("Text").stringValue = EditorGUI.TextArea(arect, serElem.FindPropertyRelative("Text").stringValue);
            arect.y += spacing;
            arect.x = rect.x;
            arect.height = EditorGUIUtility.singleLineHeight / 5;
            EditorGUI.LabelField(arect, "");
        }
        else if (serElem.FindPropertyRelative("state類型").enumValueIndex == 1)
        {
            EditorGUI.LabelField(arect, "角色: ", fontStyle);
            arect.x += arect.width;
            arect.width = input_w;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("Character"), GUIContent.none);
            arect.y += arect.height + spacing;
            arect.width = label_w;
            arect.x -= arect.width;
            EditorGUI.LabelField(arect, "x原點: ", fontStyle);
            arect.x += arect.width;
            arect.width = input_w;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("OriPositionX"), GUIContent.none);
            arect.y += arect.height + spacing;
            arect.width = label_w;
            arect.x -= arect.width;
            EditorGUI.LabelField(arect, "x終點: ", fontStyle);
            arect.x += arect.width;
            arect.width = input_w;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("NewPositionX"), GUIContent.none);
            arect.y += arect.height + spacing;
            arect.width = label_w;
            arect.x -= arect.width;
            EditorGUI.LabelField(arect, "花費時間: ", fontStyle);
            arect.x += arect.width;
            arect.width = input_w;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("Duration"), GUIContent.none);
            arect.y += spacing;
            arect.x = rect.x;
            arect.height = EditorGUIUtility.singleLineHeight / 5;
            EditorGUI.LabelField(arect, "");
        }
        else if (serElem.FindPropertyRelative("state類型").enumValueIndex == 2)
        {
            EditorGUI.LabelField(arect, "變數: ", fontStyle);
            arect.x += arect.width;
            arect.width = input_w;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("Flag"), GUIContent.none);
            arect.y += arect.height + spacing;
            arect.width = label_w;
            arect.x -= arect.width;
            EditorGUI.LabelField(arect, "當0跳到: ", fontStyle);
            arect.x += arect.width;
            arect.width = input_w;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("WhenFlagFalse"), GUIContent.none);
            arect.y += arect.height + spacing;
            arect.width = label_w;
            arect.x -= arect.width;
            EditorGUI.LabelField(arect, "當1跳到: ", fontStyle);
            arect.x += arect.width;
            arect.width = input_w;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("WhenFlagTrue"), GUIContent.none);
            arect.y += arect.height + spacing;
            arect.width = label_w;
            arect.x -= arect.width;
            EditorGUI.LabelField(arect, "直接跳到: ", fontStyle);
            arect.x += arect.width;
            arect.width = input_w;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("JustJump"), GUIContent.none);
            arect.y += spacing;
            arect.x = rect.x;
            arect.height = EditorGUIUtility.singleLineHeight / 5;
            EditorGUI.LabelField(arect, "");
        }
        else if (serElem.FindPropertyRelative("state類型").enumValueIndex == 3)
        {
            EditorGUI.LabelField(arect, "變數名稱: ", fontStyle);
            arect.x += arect.width;
            arect.width = input_w;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("Variable"), GUIContent.none);
            arect.y += arect.height + spacing;
            arect.width = label_w;
            arect.x -= arect.width;
            EditorGUI.LabelField(arect, "設值: ", fontStyle);
            arect.x += arect.width;
            arect.width = input_w;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("Value"), GUIContent.none);
            arect.y += spacing;
            arect.x = rect.x;
            arect.height = EditorGUIUtility.singleLineHeight / 5;
            EditorGUI.LabelField(arect, "");
        }
        EditorGUI.indentLevel--;
    }
    void onAddDropdownCallback(Rect buttonRect, ReorderableList list)
    {
        var menu = new GenericMenu();
        menu.AddItem(new GUIContent("新增對話"), false, AddStory);
        menu.AddItem(new GUIContent("新增動作"), false, AddMovement);
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("新增分支"), false, AddBranch);
        menu.AddItem(new GUIContent("新增變數"), false, AddVariable);
        menu.DropDown(buttonRect);
    }
    void onRemoveCallback(ReorderableList list)
    {
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
        element.FindPropertyRelative("WhenFlagTrue").floatValue = 0;
        element.FindPropertyRelative("WhenFlagFalse").floatValue = 0;
        element.FindPropertyRelative("JustJump").floatValue = 0;
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
        element.FindPropertyRelative("state類型").enumValueIndex = 2;
        element.FindPropertyRelative("Variable").stringValue = "";
        element.FindPropertyRelative("Value").boolValue = true;
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
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

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(NPCTrigger))]
public class NPCTriggerEditor : Editor
{
    ReorderableList list;

    private void OnEnable()
    {
        var serProp = serializedObject.FindProperty("NPC運作條件List");
        list = new ReorderableList(serProp.serializedObject, serProp);
        list.elementHeight = 75;
        list.drawElementCallback = DrawListElement;
        list.drawHeaderCallback = DrawListHeader;
    }
    void DrawListHeader(Rect rect)
    {
        var spacing = 3;
        var arect = rect;
        arect.x += spacing;
        EditorGUI.LabelField(arect, "變數條件");
    }
    void DrawListElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        var arect = rect;
        var serElem = this.list.serializedProperty.GetArrayElementAtIndex(index);
        var spacing = 3f;
        var label_w = (rect.width - 10) / 3;
        var input_w = rect.width - 10 - label_w;

        arect.height = 18;
        arect.width = label_w;
        arect.x += 10;
        arect.y += 5;
        EditorGUI.LabelField(arect, "NPC運作條件: ");
        arect.x += label_w;
        arect.width = input_w;
        serElem.FindPropertyRelative("NPC運作條件").enumValueIndex = (int)(NPC運作條件Element.NPCCondition)EditorGUI.EnumPopup(arect, (NPC運作條件Element.NPCCondition)serElem.FindPropertyRelative("NPC運作條件").enumValueIndex);
        arect.x -= label_w;
        arect.y += arect.height + spacing;
        if (serElem.FindPropertyRelative("NPC運作條件").enumValueIndex == 0)
        {
            arect.width = label_w;
            EditorGUI.LabelField(arect, "變數名稱: ");
            arect.x += label_w;
            arect.width = input_w;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("條件變數名稱"), new GUIContent(""));
            arect.x -= label_w;
            arect.y += arect.height + spacing;
            arect.width = label_w;
            EditorGUI.LabelField(arect, "變數值: ");
            arect.x += label_w;
            arect.width = input_w;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("條件變數值"), new GUIContent(""));
        }
        else if (serElem.FindPropertyRelative("NPC運作條件").enumValueIndex == 1)
        {
            arect.width = label_w;
            EditorGUI.LabelField(arect, "物件名稱: ");
            arect.x += label_w;
            arect.width = input_w;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("條件物件名稱"), new GUIContent(""));
            arect.x -= label_w;
            arect.y += arect.height + spacing;
            arect.width = label_w;
            EditorGUI.LabelField(arect, "是否存在: ");
            arect.x += label_w;
            arect.width = input_w;
            EditorGUI.PropertyField(arect, serElem.FindPropertyRelative("條件物件存在與否"), new GUIContent(""));
        }
    }  

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var sO = serializedObject;
        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((NPCTrigger)target), typeof(NPCTrigger), false);
        GUI.enabled = true;
        list.DoLayoutList();

        //EditorGUILayout.PropertyField(sO.FindProperty("NPC運作條件List"), new GUIContent("變數條件"));
        //List<NPC運作條件Element> list = new List<NPC運作條件Element>();
        //for(int i=0;i< serializedObject.FindProperty("NPC運作條件List").arraySize; i++)
        //{
        //    var property = serializedObject.FindProperty("NPC運作條件List").GetArrayElementAtIndex(i);
        //    EditorGUILayout.PropertyField(property.FindProperty("type"), new GUIContent("NPC觸發種類"));
        //}

        EditorGUILayout.PropertyField(sO.FindProperty("type"), new GUIContent("NPC觸發種類"));
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(sO.FindProperty("state"), new GUIContent("目前狀態"));
        //EditorList.Show(serializedObject.FindProperty("NPC運作條件List"), EditorListOption.ListLabel | EditorListOption.ElementLabels | EditorListOption.Buttons, "變數條件");
        EditorGUILayout.PropertyField(sO.FindProperty("NPC面向右邊"));
        EditorGUILayout.PropertyField(sO.FindProperty("NPC對話後是否要面向席諾"));
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(sO.FindProperty("冷卻時間"));
        EditorGUILayout.PropertyField(sO.FindProperty("Radius"));
        EditorGUILayout.PropertyField(sO.FindProperty("Offset"));
        //EditorGUILayout.PropertyField(sO.FindProperty("NPC運作條件List"), new GUIContent("變數條件"), includeChildren: true);



        /*if (sO.FindProperty("type").enumValueIndex == (int)NPC.NpcType.item)
        {
            EditorGUILayout.PropertyField(sO.FindProperty("itemType"), new GUIContent("是否可撿"));
            if (sO.FindProperty("itemType").enumValueIndex == (int)NPC.ItemType.可撿)
            {
                EditorGUILayout.PropertyField(sO.FindProperty("可撿的物品的名字"), new GUIContent("可撿物品名稱"));
                EditorGUILayout.PropertyField(sO.FindProperty("是否撿完變不可撿"), new GUIContent("是否對話完變不可撿"));
                if (sO.FindProperty("是否撿完變不可撿").boolValue)
                    EditorGUILayout.PropertyField(sO.FindProperty("不可撿的物品的敘述"), new GUIContent("不可撿物品敘述"));
            }
            else
            {
                EditorGUILayout.PropertyField(sO.FindProperty("不可撿的物品的敘述"), new GUIContent("不可撿物品敘述"));
                EditorGUILayout.PropertyField(sO.FindProperty("是否不撿完變可撿"), new GUIContent("是否對話完變可撿"));
                if (sO.FindProperty("是否不撿完變可撿").boolValue)
                    EditorGUILayout.PropertyField(sO.FindProperty("可撿的物品的名字"), new GUIContent("可撿物品名稱"));
            }

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(sO.FindProperty("冷卻時間"));
            EditorGUILayout.PropertyField(sO.FindProperty("Radius"));
            EditorGUILayout.PropertyField(sO.FindProperty("HintRaius"));
            EditorGUILayout.PropertyField(sO.FindProperty("Offset"));
            EditorGUILayout.Space();
        }
        if (sO.FindProperty("type").enumValueIndex == (int)NPC.NpcType.talk)
        {
            EditorGUILayout.PropertyField(sO.FindProperty("NPC面向右邊"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(sO.FindProperty("冷卻時間"));
            EditorGUILayout.PropertyField(sO.FindProperty("Radius"));
            EditorGUILayout.PropertyField(sO.FindProperty("Offset"));
        }
        if (sO.FindProperty("type").enumValueIndex == (int)NPC.NpcType.door)
        {
            EditorGUILayout.PropertyField(sO.FindProperty("doorType"));
            EditorGUILayout.PropertyField(sO.FindProperty("門的名字"));
            if (sO.FindProperty("doorType").enumValueIndex == 1)
                EditorGUILayout.PropertyField(sO.FindProperty("需要的鑰匙名字"));
            EditorGUILayout.PropertyField(sO.FindProperty("是否有傳送功能"));
            if (sO.FindProperty("是否有傳送功能").boolValue)
            {
                EditorGUILayout.PropertyField(sO.FindProperty("門要傳送到的場景"));
                EditorGUILayout.PropertyField(sO.FindProperty("門要傳送到的場景名字"));
                EditorGUILayout.PropertyField(sO.FindProperty("傳送地點"));
            }
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
    /*private string DrawEmptyMessage()
    {
        GUILayout.Label("List is empty!", EditorStyles.miniLabel);
    }*/
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
        serializedObject.ApplyModifiedProperties();
    }

}
[Flags]
public enum EditorListOption
{
    None = 0,
    ListSize = 1,
    ListLabel = 2,
    ElementLabels = 4,
    Buttons = 8,
    Default = ListSize | ListLabel | ElementLabels,
    NoElementLabels = ListSize | ListLabel,
    All = Default | Buttons
}
public static class EditorList
{

    public static void Show(SerializedProperty list, EditorListOption options = EditorListOption.Default, string label = null)
    {
        if (!list.isArray)
        {
            EditorGUILayout.HelpBox(list.name + " is neither an array nor a list!", MessageType.Error);
            return;
        }

        bool
            showListLabel = (options & EditorListOption.ListLabel) != 0,
            showListSize = (options & EditorListOption.ListSize) != 0;

        if (showListLabel)
        {
            if (label == null)
                EditorGUILayout.PropertyField(list);
            else
                EditorGUILayout.PropertyField(list, new GUIContent(label));
            EditorGUI.indentLevel += 1;
        }
        if (!showListLabel || list.isExpanded)
        {
            SerializedProperty size = list.FindPropertyRelative("Array.size");
            if (showListSize)
            {
                EditorGUILayout.PropertyField(size);
            }
            if (size.hasMultipleDifferentValues)
            {
                EditorGUILayout.HelpBox("Not showing lists with different sizes.", MessageType.Info);
            }
            else
            {
                ShowElements(list, options);
            }
        }
        if (showListLabel)
        {
            EditorGUI.indentLevel -= 1;
        }
    }
    private static GUILayoutOption miniButtonWidth = GUILayout.Width(25f);
    private static void ShowButtons(SerializedProperty list, int index)
    {
        if (GUILayout.Button(moveButtonContent, EditorStyles.miniButtonLeft, miniButtonWidth))
        {
            list.MoveArrayElement(index, index + 1);
        }
        if (GUILayout.Button(duplicateButtonContent, EditorStyles.miniButtonMid, miniButtonWidth))
        {
            list.InsertArrayElementAtIndex(index);
        }
        if (GUILayout.Button(deleteButtonContent, EditorStyles.miniButtonRight, miniButtonWidth))
        {
            int oldSize = list.arraySize;
            list.DeleteArrayElementAtIndex(index);
            if (list.arraySize == oldSize)
            {
                list.DeleteArrayElementAtIndex(index);
            }
        }
    }

    private static GUIContent
        moveButtonContent = new GUIContent("\u21b4", "move down"),
        duplicateButtonContent = new GUIContent("+", "duplicate"),
        deleteButtonContent = new GUIContent("-", "delete"),
        addButtonContent = new GUIContent("+", "add element");

    private static void ShowElements(SerializedProperty list, EditorListOption options)
    {
        bool
            showElementLabels = (options & EditorListOption.ElementLabels) != 0,
            showButtons = (options & EditorListOption.Buttons) != 0;

        for (int i = 0; i < list.arraySize; i++)
        {
            if (showElementLabels)
            {
                if (showButtons)
                {
                    EditorGUILayout.BeginHorizontal();
                }
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("NPC運作條件"), new GUIContent("NPC運作條件"), false);
                if (showButtons)
                {
                    ShowButtons(list, i);
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("條件變數名稱"), new GUIContent("條件變數名稱"), false);
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("條件變數值"), new GUIContent("條件變數值"), false);
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("條件物件名稱"), new GUIContent("條件物件名稱"), false);
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("條件物件存在與否"), new GUIContent("條件物件存在與否"), false);
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), new GUIContent(" " + (i + 1)), true);
            }
            else
            {
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), GUIContent.none, true);
            }
            EditorGUILayout.Space();
        }
        if (showButtons && list.arraySize == 0 && GUILayout.Button(addButtonContent, EditorStyles.miniButton))
        {
            list.arraySize += 1;
        }
    }
}
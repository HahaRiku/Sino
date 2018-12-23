/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(Item_test))]
//[CreateAssetMenu(menuName="MySubMenue/ItemDatabase ")]
public class ItemDatabaseEditor : EditorWindow{
	
	[MenuItem("window/itemDatabase")]
	public static void Init(){
		ItemDatabaseEditor window = (ItemDatabaseEditor)EditorWindow.GetWindow(typeof(ItemDatabaseEditor), true, "ItemDatabase");
        window.Show();
	}
	
	[SerializeField]
	private List<Item_test> items = new List<Item_test>;
	
	
	Vector2 scroll_Items;
    string t = "This is a string inside a Scroll view!";
	
	
	
	void OnGUI(){
		
		EditorGUILayout.BeginHorizontal();
		
		//頂端按鍵等
		//EditorGUILayout.BeginHorizontal();
		(GUILayout.Button("+", GUILayout.ExpandWidth(false))){}
		
		
		//左邊拉條
		EditorGUILayout.BeginVertical("box");
		
        scroll_Items =
            EditorGUILayout.BeginScrollView(scroll_Items, GUILayout.Width(150), GUILayout.Height(200));	//拉條視窗大小
        GUILayout.Label(t);
        EditorGUILayout.EndScrollView();
		
		
        EditorGUILayout.EndVertical();
		
		//右邊資訊
		
		
		
		
		/*
        if (GUILayout.Button("Add More Text", GUILayout.Width(100), GUILayout.Height(100)))				//按鈕，可忽略
            t += " \nAnd this is more text!";
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Clear"))
            t = "";
		
		
		
		
		EditorGUILayout.EndHorizontal();
	}

}*/

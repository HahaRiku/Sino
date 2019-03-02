using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase {
	
	public struct item{
		//int id;// = list.at();
		public string name;
		public GameObject obj;
		public int number;
	};

	public static List<item> itemList = new List<item>();
	
	private void addItem (string objName, string itemName){
		item temp = new item();
		//temp.id = itemList.Count();
		temp.obj = GameObject.Find(objName);
		temp.name = itemName;
		temp.number = 0;
		itemList.Add(temp);
	}

	public void Awake (){
		//Create all items here.
		
		addItem("item_1pick", "test" ); //0
	}
	
}

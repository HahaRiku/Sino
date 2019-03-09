using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : ItemDatabase{
	
	public static List<item> bag = new List<item>(itemList);

	public void getItem (int itemId){
		if (itemId >= bag.Count || itemId < 0){Debug.LogError ("bug"); }
		else {
			bag[itemId].number ++;
		}
		
	}

	public void deleteItem (int itemId){
		if (itemId >= bag.Count || itemId < 0) {
			Debug.LogError ("bug"); }
		else if (bag[itemId].number == 0){
			Debug.LogError ("bug"); }
		else {
			bag[itemId].number --;
		}
		
	}

	//delete items 

}

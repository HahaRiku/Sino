using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_bag : MonoBehaviour{
    
	//private var bag = ;
	private Text itemInBag;
	
	void Start () {
		itemInBag = this.gameObject.GetComponent<Text>();
		itemInBag.text = "Item: ";
	}
	
	void Update () {
        //temp: show items in bag
		foreach (ItemDatabase.item items in Bag.bag){
			if(items.number > 0){
				itemInBag.text += items.name + " , ";
			}
		}
		
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharWithItem : MonoBehaviour {
    
	public GameObject interactItem;
	private bool IsHighlight;
	
	
	void Start () {
		IsHighlight = false;
	}
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Z) && IsHighlight) {		//按下Z && 有物體可互動
            
			if(interactItem.GetComponent<ItemInfo>().actMode == ItemInfo.Act_Mode.look_only){		//look
				effect_ShowInfo(interactItem,true);
			}
			else{		//pick
				pick_TEMP(interactItem);
			}
        }
    }
	
//touching
	void OnTriggerEnter2D(Collider2D col_item){
		if(col_item.transform.tag == "ActableItem" && !IsHighlight){
			interactItem = col_item.gameObject;
			effect_Highlight(interactItem, true);
			IsHighlight = true;
		}
	}
	void OnTriggerStay2D(Collider2D col_item){
		if(col_item.transform.tag == "ActableItem" && !IsHighlight){
			interactItem = col_item.gameObject;
			effect_Highlight(interactItem, true);
			IsHighlight = true;
		}
	}
	
//
	void OnTriggerExit2D(Collider2D col_item){
		if(col_item.transform.tag == "ActableItem" && IsHighlight){
			interactItem = null;
			effect_Highlight(interactItem, false);
			IsHighlight = false;
		}
	}
	
	void effect_Highlight(GameObject item, bool IsHighlight){
		if(IsHighlight){
				item.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load<Sprite>("picName2");}
		else{	item.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load<Sprite>("picName0");}
	}
	void effect_ShowInfo(GameObject item, bool IsOpen){
		item.transform.GetChild(0).gameObject.SetActive(IsOpen);
	}
	
	void pick_TEMP(GameObject item){
		
		//antEnable = false;
		
		effect_ShowInfo(item,true);
		
		//press z
		
		//interactItem.SetActive(false);
		if(item.GetComponent<ItemInfo>().actMode == ItemInfo.Act_Mode.one_time_get){
			item.SetActive(false);
		}
		//antEnable = true;
	}
}

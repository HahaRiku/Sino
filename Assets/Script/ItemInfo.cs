using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour {
    
	public enum HighLightState{
		idle,
		touched,
		describe,
		choosing_action,
		picking,
		picken
	};
	[HideInInspector]
	public HighLightState HLE;
	
	//一單位選項字高=40
	
	public enum Act_Mode{
		look_only,
		one_time_get,
		multi_get
	};
	public Act_Mode actMode;
	public Sprite[] pics;		//1: idle;	2: HL;	3:InfoImg
	public string[] description;
	
	void Start(){
		HLE = HighLightState.idle;
	}
	
//物品狀態&效果
	public void ItemState(){//HighLightState HLState
		switch(HLE){
			case HighLightState.idle:
				transform.GetChild(0).gameObject.SetActive(false);
				gameObject.GetComponent<SpriteRenderer>().sprite = pics[0];
				if(actMode == Act_Mode.look_only){
					transform.GetChild(1).gameObject.SetActive(false);}
					
				break;
			case HighLightState.touched:
				gameObject.GetComponent<SpriteRenderer>().sprite = pics[0];
				transform.GetChild(0).gameObject.SetActive(true);
				break;
			case HighLightState.describe:
				gameObject.GetComponent<SpriteRenderer>().sprite = pics[1];
				if(actMode == Act_Mode.look_only){
					this.gameObject.transform.GetChild(1).gameObject.SetActive(true);}
				//else: by char
				break;
		//others: as char
			case HighLightState.choosing_action:
				break;
			case HighLightState.picking:
				break;
			case HighLightState.picken:
				break;
			default:
				break;
		}
	}
	
//Scope Effect
	void OnTriggerEnter2D(Collider2D player){
		HLE = HighLightState.touched;
		ItemState();
	}
	void OnTriggerExit2D(Collider2D player){
		HLE = HighLightState.idle;
		ItemState();
	}
	
}

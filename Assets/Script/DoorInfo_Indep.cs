using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInfo_Indep : MonoBehaviour {
    
	public enum HighLightState{
		idle,
		touched,
		describe
	};
	[HideInInspector]
	public HighLightState HLE;
	public Vector3 nextScenePos;// = new Vector3;
	
	//一單位選項字高=40
	
	//public Sprite pics;		//1: idle
	public string description;
	public bool IsLocked;
	public Object nextScene;	//抓場景，轉成名稱供LoadScene使用
	public int nextScenePosLabel;
	
	void Start(){
		HLE = HighLightState.idle;
	}
	
//狀態&效果
	public void ItemState(){//HighLightState HLState
		switch(HLE){
			case HighLightState.idle:
				transform.GetChild(0).gameObject.SetActive(false);
			//All doors are look_only
				transform.GetChild(1).gameObject.SetActive(false);
				break;
			case HighLightState.touched:
				transform.GetChild(0).gameObject.SetActive(true);
				break;
			case HighLightState.describe:
			//All doors are look_only
				this.gameObject.transform.GetChild(1).gameObject.SetActive(true);
				break;
		//others: as char
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

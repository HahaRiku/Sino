using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorInfo : MonoBehaviour {
    
	public enum HighLightState{
		idle,
		touched,
		describe
	};
	[HideInInspector]
	public HighLightState HLE;
	
	//一單位選項字高=40
	
	//public Sprite pics;		//1: idle
	public string description;
	public bool IsLocked;
	public Object nextScene;	//抓場景，轉成名稱供LoadScene使用
	public GameStateManager.SpawnPoint nextScenePos;
	public GameStateManager.Facing facing;
	
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
			default:
				break;
		}
	}
	
	void OnTriggerEnter2D(Collider2D player){
		HLE = HighLightState.touched;
		ItemState();
	}
	void OnTriggerStay2D(Collider2D player){
		if(Input.GetKeyDown(KeyCode.Z)){	//(press Z && locked) -> {describe}
			if(IsLocked){
				HLE = HighLightState.describe;
				ItemState();
			}
			else{                           //(press Z && openable) -> [open] + [in]
                //GameStateManager.Instance.黑幕轉場(nextScene.name, nextScenePos);
                FindObjectOfType<GameStateManager>().黑幕轉場(nextScene.name, nextScenePos, facing);
                SceneManager.LoadScene(nextScene.name);
			}
		}
		else if(Input.GetKeyDown(KeyCode.X) && IsLocked){
			//(UseItem() && locked) -> [unlocked] + {touched}
			//目前用X鍵代替用道具		
			
			Debug.Log("門打開了");
			IsLocked = false;
		}
	}
	void OnTriggerExit2D(Collider2D player){
		HLE = HighLightState.idle;
		ItemState();
	}
	
}


/*
DoorState{
	idle,
		(touch) -> {touched}
	touched,
		(press Z && locked) -> {describe}
		(press Z && openable) -> [open] + [in]
		(UseItem() && locked) -> [unlocked] + {touched}
		(untouch) -> {idle}
	describe,
		(press Z) -> {touched}
		(untouch) -> {idle}
};
*/

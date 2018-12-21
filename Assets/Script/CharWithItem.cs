using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharWithItem : MonoBehaviour {
    
	public GameObject interactItem,Info_UI,pickChoose;
	public static bool actEnable;	//會移到GlobalValue裡
	
	void Start () {
		//IsHighlight = false;
		actEnable = true;
		pickChoose.SetActive(false);
	}
	
//pre_funct
	private IEnumerator waitForKeyPress(KeyCode key){
		bool done = false;
		while(!done){
			if(Input.GetKeyDown(key)){	done = true;}
			yield return 0; // wait until next frame, then continue execution from here (loop continues)
		}
	}
	private void SetPickUIInfo(int imgNumber, string description){
		Info_UI.GetComponentInChildren<Image>().sprite = interactItem.GetComponent<ItemInfo>().pics[imgNumber];
		Info_UI.GetComponentInChildren<Text>().text = description;
	}
	private void ActivePickUI(bool IsActive){
		Info_UI.SetActive(IsActive);
	}
	
//Touch: "idle -> touched"
	void OnTriggerEnter2D(Collider2D col_item){
		if(col_item.transform.tag == "ActableItem"){
			interactItem = col_item.gameObject;
			StartCoroutine(F_Touched());
		}
	}
//Untouch: "touched -> idle"
	void OnTriggerExit2D(Collider2D col_item){
		if(col_item.transform.tag == "ActableItem"){
			interactItem = null;
		}
	}
//F_touched
	private IEnumerator F_Touched(){
	//(press Z) -> {describe}
		yield return new WaitForSeconds(Time.deltaTime);
		yield return waitForKeyPress(KeyCode.Z);		
		if(interactItem != null ){
			StartCoroutine(F_Describe());
		}
		//(untouch) -> {idle}: At item
	}
//F_describe
	private IEnumerator F_Describe(){
		interactItem.GetComponent<ItemInfo>().HLE = ItemInfo.HighLightState.describe;
		interactItem.GetComponent<ItemInfo>().ItemState();
	//(unpick) -> [describe_1]
		if(interactItem.GetComponent<ItemInfo>().actMode == ItemInfo.Act_Mode.look_only){
		//(unpick && untouch) -> {idle}: At item
		}
	//(pickable) -> [describe_2]
		else{
			SetPickUIInfo(2,interactItem.GetComponent<ItemInfo>().description[0]);
			ActivePickUI(true);
			
		//(wait press Z) -> [dialogue] + {choosing_action}	//"pick? Y/N"
			actEnable = false;
			yield return waitForKeyPress(KeyCode.Z);
			ActivePickUI(false);
			
			//dialogue
			
			yield return StartCoroutine(F_WhetherPick());
		}
	}
//F_choosing_action
	private IEnumerator F_WhetherPick(){
		pickChoose.SetActive(true);
		int choice = 0;
		bool done = false;
		RectTransform cursor = pickChoose.transform.GetChild(0).gameObject.GetComponent<RectTransform> ();	
		Vector3 cursor_localPos = new Vector3(-75.0f, 20.0f, 0);
		cursor.localPosition = cursor_localPos;
		while(!done){
			if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)){
				choice = (choice+1)%2;
				cursor.localPosition = new Vector3(cursor_localPos.x, cursor_localPos.y - 40.0f * choice, cursor_localPos.z);
			}
			else if(Input.GetKeyDown(KeyCode.Z)){
				done = true;
				pickChoose.SetActive(false);
			//(press Z && "Y") -> //dial + pic
				if(choice == 0){
					StartCoroutine(F_Pick());
				}
			//(press Z && "N") -> {touched}
				else{
					actEnable = true;
					StartCoroutine(F_Touched());
				}
			}
			yield return 0; // wait until choosing
		}
	}
//F_picking & picken
	private IEnumerator F_Pick(){
		SetPickUIInfo(2, "獲得" + interactItem.gameObject.name );
		ActivePickUI(true);
		yield return new WaitForSeconds(Time.deltaTime);
		yield return waitForKeyPress(KeyCode.Z);
			//item++
		ActivePickUI(false);
		actEnable = true;
	//(press Z && 1pick) -> {picken} : -> active(false)
		if(interactItem.GetComponent<ItemInfo>().actMode == ItemInfo.Act_Mode.one_time_get){
			interactItem.GetComponent<SpriteRenderer>().color = new Vector4(255.0f, 255.0f, 255.0f, 0);
			interactItem.SetActive(false);
			interactItem = null;
		}
	//(press Z && mulpick) -> {touched}
		else if(interactItem.GetComponent<ItemInfo>().actMode == ItemInfo.Act_Mode.multi_get){
			StartCoroutine(F_Touched());
		}
		
	}
	
}

/*
ItemState{
	idle,
		(touch) -> {touched}
	touched,
		(press Z) -> {describe}
		(untouch) -> {idle}
	describe,
		(pickable && press Z) -> [dialogue] + {choosing_action}	//"pick? Y/N"
		(unpiuck && untouch) -> {idle}
	choosing_action,
		(press Z && "Y") -> //dial + pic
		(press Z && "N") -> {touched}
	picking,
		(press Z) -> {picken}
		(press Z) -> {touched}
	picken
		-> active(false)
};
*/
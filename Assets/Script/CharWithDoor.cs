using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharWithDoor : MonoBehaviour {
    
	public GameObject interactDoor;
	//public int nextScenePosLabel;
	
	//private Vector3 tempTransPosition = new Vector3(-7.25f, -1.47f, 0.0f);
	
	
//Touch: "idle -> touched"
	void OnTriggerEnter2D(Collider2D col){
		if(col.transform.tag == "Door"){
			interactDoor = col.gameObject;
			StartCoroutine(F_Touched());
		}
	}
//Untouch: "touched -> idle"
	void OnTriggerExit2D(Collider2D col){
		if(col.transform.tag == "Door"){
			interactDoor = null;
		}
	}
//F_touched
	private IEnumerator F_Touched(){
		if(interactDoor != null ){
			//KeyCode key;
			yield return new WaitForSeconds(Time.deltaTime);
			
		//wait Z or X input
			bool done = false;
			while(!done){		
				if(Input.GetKeyDown(KeyCode.Z)){
					done = true;
				//(press Z && locked) -> {describe}
					if(interactDoor.GetComponent<DoorInfo_Indep>().IsLocked){
						F_Describe();
					}
				//(press Z && openable) -> [open] + [in]
					else{
						F_IntoDoor();
					}
				}
			//(UseItem() && locked) -> [unlocked] + {touched}
			//目前用X鍵代替用道具						**************************************************************************************
				else if(Input.GetKeyDown(KeyCode.X) && interactDoor.GetComponent<DoorInfo_Indep>().IsLocked){
					done = true;
					Debug.Log("門打開了");
					interactDoor.GetComponent<DoorInfo_Indep>().IsLocked = false;
				}
				yield return 0;
			}
		}
		//(untouch) -> {idle}: At door
	}
//F_describe
	private void F_Describe(){
		interactDoor.GetComponent<DoorInfo_Indep>().HLE = DoorInfo_Indep.HighLightState.describe;
		interactDoor.GetComponent<DoorInfo_Indep>().ItemState();
	//(unpick) -> [describe_1]: At item
	}
	
//F_intoDoor
	public void F_IntoDoor(){
		
	//用場景名稱傳送，名稱在門上的DoorInfo腳本裡作修改
		
		Temp_GameManager.transPosSet(interactDoor.GetComponent<DoorInfo_Indep>().nextScenePosLabel);
		SceneManager.LoadScene(interactDoor.GetComponent<DoorInfo_Indep>().nextScene.name);
		
		//Temp_sceneinit.transformPointNum = interactDoor.GetComponent<DoorInfo_Indep>().nextScenePosLabel;
		/*
		if(interactDoor.GetComponent<DoorInfo_Indep>().nextScenePos != Vector3.zero){
			//Set Player's position
		}*/
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
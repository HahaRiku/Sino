using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharWithItem : MonoBehaviour {
    
	public GameObject interactItem,Info_UI;
	private bool IsHighlight;
	private bool actEnable;
	
	void Start () {
		IsHighlight = false;
		actEnable = true;
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
				item.GetComponent<SpriteRenderer>().sprite = item.GetComponent<ItemInfo>().pics[1];}
		else{	item.GetComponent<SpriteRenderer>().sprite = item.GetComponent<ItemInfo>().pics[0];}
	}
	void effect_ShowInfo(GameObject item, bool IsOpen){
		if(item.GetComponent<ItemInfo>().actMode == ItemInfo.Act_Mode.look_only){
			item.transform.GetChild(0).gameObject.SetActive(IsOpen);}
		else{
			
		//UI_set & open
			Info_UI.GetComponentInChildren<Image>().sprite = item.GetComponent<ItemInfo>().pics[2];
			Info_UI.GetComponentInChildren<Text>().text = item.GetComponent<ItemInfo>().description;
			Info_UI.SetActive(IsOpen);
		}
	}
	
	void pick_TEMP(GameObject item){
		
		actEnable = false;
		effect_ShowInfo(item,true);
		
		//interactItem.SetActive(false);
		if(item.GetComponent<ItemInfo>().actMode == ItemInfo.Act_Mode.one_time_get){
			item.SetActive(false);
		}
		
		//press z
		StartCoroutine(waitKeyPress(KeyCode.Z));
		
		interactItem = null;
		effect_Highlight(item, false);
		IsHighlight = false;
		//antEnable = true;
	}
	private IEnumerator waitKeyPress( KeyCode code){
		while(!actEnable){
			if(Input.GetKeyDown(code)){
				actEnable = true;
				break;
			}
			yield return 0;
		}
	}
	
}



                  /*
public class CoroutineTest : MonoBehaviour {
	bool bKeyPressed =false;
	// Use this for initialization
	void Start () {
		FirstFunction();
	}

	void FirstFunction(){
		print("before  start coroutine ");
		StartCoroutine(waitKeyPress(KeyCode.A));
		print ("after start coroutine");
	}
	IEnumerator waitKeyPress(KeyCode code){
		while(!bKeyPressed){
			if(Input.GetKeyDown(code)){
				StartGame();
				break;
			}
			print (" wait key press  yield return");
			yield return 0;
		}
		print ("end of wait key press ");
	}
	void StartGame(){
		bKeyPressed = true;
		print("StartGame!!!!!!!!!!!!!!!!!!!");
	}
}*/
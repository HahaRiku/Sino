using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharWithItem : MonoBehaviour {
    
	public GameObject interactItem,Info_UI;
	private bool IsHighlight;
	public bool actEnable;
	private int count = 0;
	
	void Start () {
		IsHighlight = false;
		actEnable = true;
	}
	
	void Update () {
        if(actEnable){
			gameObject.transform.parent.GetComponent<CharacterControl>().enabled = true;
		}else{
			gameObject.transform.parent.GetComponent<CharacterControl>().enabled = false;
		}
		
		
		if (Input.GetKeyDown(KeyCode.Z) && IsHighlight) {		//按下Z && 有物體可互動
            
			if(interactItem.GetComponent<ItemInfo>().actMode == ItemInfo.Act_Mode.look_only){		//look
				effect_ShowInfo(interactItem,true);
			}
			else{		//pick
				StartCoroutine(pick_TEMP(interactItem));
			}
        }
    }
	
//touching
	void OnTriggerEnter2D(Collider2D col_item){
Debug.LogError(col_item.transform.tag);
		
		if(col_item.transform.tag == "ActableItem" && !IsHighlight){
			interactItem = col_item.gameObject;
			effect_Highlight(interactItem, true);
			IsHighlight = true;
		}
	}
	void OnTriggerExit2D(Collider2D col_item){
		if(col_item.transform.tag == "ActableItem" && IsHighlight){
			IsHighlight = false;
			effect_Highlight(interactItem, false);
			effect_ShowInfo(interactItem,false);
			interactItem = null;
		}
	}
	
//HL effect
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
	
	private IEnumerator pick_TEMP(GameObject item){
		
		actEnable = false;
	Debug.LogError(actEnable);
		effect_ShowInfo(item,true);
		
		if(item.GetComponent<ItemInfo>().actMode == ItemInfo.Act_Mode.one_time_get){
			item.GetComponent<SpriteRenderer>().sprite = null;
			
			while(!actEnable){
				if(count == 0 && Input.GetKeyUp(KeyCode.Z)){
					count++;
				}
				else if(count == 1 && Input.GetKeyDown(KeyCode.Z)){
					actEnable = true;
					Debug.LogError(actEnable + "1");
					count = 0;
					
					effect_ShowInfo(item,false);
					effect_Highlight(item, false);
					IsHighlight = false;
					interactItem = null;
					
					break;
				}
				Debug.LogError(actEnable + "2");
				yield return 0;
			}
			//StartCoroutine(waitKeyPress(KeyCode.Z, item));
			item.SetActive(false);
		}
		else{
			while(!actEnable){
				if(count == 0 && Input.GetKeyUp(KeyCode.Z)){
					count++;
				}
				else if(count == 1 && Input.GetKeyDown(KeyCode.Z)){
					actEnable = true;
					Debug.LogError(actEnable + "1");
					count = 0;
					
					effect_ShowInfo(item,false);
					effect_Highlight(item, false);
					IsHighlight = false;
					interactItem = null;
					
					break;
				}
				Debug.LogError(actEnable + "2");
				yield return 0;
			}
		}
	}
	/*
	private IEnumerator waitKeyPress(KeyCode code, GameObject item){
		
		while(!actEnable){
			if(count == 0 && Input.GetKeyUp(code)){
				count++;
			}
			else if(count == 1 && Input.GetKeyDown(code)){
				actEnable = true;
				Debug.LogError(actEnable + "1");
				count++;
				
				effect_ShowInfo(item,false);
				effect_Highlight(item, false);
				IsHighlight = false;
				interactItem = null;
				
				break;
			}
			Debug.LogError(actEnable + "2");
			yield return 0;
		}
	}*/
	
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
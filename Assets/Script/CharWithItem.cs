using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharWithItem : MonoBehaviour {
    
	public GameObject interactItem,Info_UI;
	private bool IsHighlight;
	private bool actEnable;
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
		
	//按下Z && 有物體可互動
		if (Input.GetKeyDown(KeyCode.Z) && IsHighlight) {
			
		//只能看的物體：info綁在item下，直接顯示即可
			if(interactItem.GetComponent<ItemInfo>().actMode == ItemInfo.Act_Mode.look_only){	
				effect_ShowInfo(interactItem,true);}
				
		//可以撿的物體：在function裡處理
			else{
				StartCoroutine(pick_TEMP(interactItem));
			}
        }
    }
	
//碰觸trigger
	void OnTriggerEnter2D(Collider2D col_item){
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
	
//highlight效果
	void effect_Highlight(GameObject item, bool IsHighlight){
		if(IsHighlight){
				item.GetComponent<SpriteRenderer>().sprite = item.GetComponent<ItemInfo>().pics[1];}
		else{	item.GetComponent<SpriteRenderer>().sprite = item.GetComponent<ItemInfo>().pics[0];}
	}
	
//顯示敘述的效果
	void effect_ShowInfo(GameObject item, bool IsOpen){
		
	//只能看的物體 - 直接顯示資訊
		if(item.GetComponent<ItemInfo>().actMode == ItemInfo.Act_Mode.look_only){
			item.transform.GetChild(0).gameObject.SetActive(IsOpen);}
	//撿取物品的UI設定
		else{
			
		//UI_set & open
			Info_UI.GetComponentInChildren<Image>().sprite = item.GetComponent<ItemInfo>().pics[2];
			Info_UI.GetComponentInChildren<Text>().text = item.GetComponent<ItemInfo>().description;
			Info_UI.SetActive(IsOpen);
		}
	}
	
//撿取動作
	private IEnumerator pick_TEMP(GameObject item){
		
	/*物體訊息顯示：
		關閉人物移動 -> 打開UI層的物件訊息 [等待Z鍵輸入] -> 關閉訊息、恢復人物移動*/
		actEnable = false;
		effect_ShowInfo(item,true);
		
	//一次撿取的物品：撿取時關閉地圖上的圖示，訊息結束後刪除物品
		if(item.GetComponent<ItemInfo>().actMode == ItemInfo.Act_Mode.one_time_get){
		
		//關閉圖像
			item.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0f);
		
		//等待輸入&關閉訊息
			while(!actEnable){
				if(count == 0 && Input.GetKeyUp(KeyCode.Z)){
					count++;
				}
				else if(count == 1 && Input.GetKeyDown(KeyCode.Z)){
					actEnable = true;
					count = 0;
					
					effect_ShowInfo(item,false);
					effect_Highlight(item, false);
					IsHighlight = false;
					interactItem = null;
					
					break;
				}
				yield return 0;
			}
			
			item.SetActive(false);
		}
	//重複撿取的物品：撿取前後都不動畫面上的物品
		else{
			while(!actEnable){
				if(count == 0 && Input.GetKeyUp(KeyCode.Z)){
					count++;
				}
				else if(count == 1 && Input.GetKeyDown(KeyCode.Z)){
					actEnable = true;
					count = 0;
					
					effect_ShowInfo(item,false);
					effect_Highlight(item, false);
					IsHighlight = false;
					interactItem = null;
					
					break;
				}
				yield return 0;
			}
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour {
    
	public enum Act_Mode{
		look_only,
		one_time_get,
		multi_get
	};
	public Act_Mode actMode;
	public Sprite[] pics;		//1: idle;	2: HL;	3:InfoImg
	public string description;
	
	/*
	public bool IsHighlight;
	
	void Start(){
		IsHighlight = false;
	}
	*/
}

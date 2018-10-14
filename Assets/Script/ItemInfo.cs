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
	public Sprite[] pics;
	
	/*
	public bool IsHighlight;
	
	void Start(){
		IsHighlight = false;
	}
	*/
}

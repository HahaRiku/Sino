using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_bag : MonoBehaviour {
    
	public GameObject[] itemList;
	private Text itemInBag;
	
	// Use this for initialization
	void Start () {
		itemInBag.text = "Item: ";
	}
	
	// Update is called once per frame
	void Update () {
        
    }
}

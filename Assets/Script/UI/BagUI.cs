using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**************************
 * 
 * press B to open
 * 
 * ***************************/

public class BagUI : MonoBehaviour {

    private GameObject[] itemUI = new GameObject[5];
    private GameObject itemGroup;
    private GameObject rightArrow;
    private GameObject leftArrow;
    private int nowAccessBagIndex = 0;

	// Use this for initialization
	void Start () {
        itemGroup = gameObject.transform.GetChild(0).GetChild(0).gameObject;
        for (int i = 0; i < 5; i++) {
            itemUI[i] = itemGroup.transform.GetChild(i).gameObject;
        }
        rightArrow = itemGroup.transform.GetChild(5).gameObject;
        leftArrow = itemGroup.transform.GetChild(6).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

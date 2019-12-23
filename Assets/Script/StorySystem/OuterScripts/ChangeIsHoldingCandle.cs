using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeIsHoldingCandle : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Change(bool changingValue) {
        FindObjectOfType<PlayerController>().IsHoldingCandle = changingValue;
    }
}

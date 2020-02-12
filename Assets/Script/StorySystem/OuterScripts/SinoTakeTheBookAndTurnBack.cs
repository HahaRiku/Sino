using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinoTakeTheBookAndTurnBack : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeBook() {
        FindObjectOfType<PlayerController>().gameObject.GetComponent<ChangeCharacSprite>().ChangeSprite(2);
    }

    public void TurnBack() {
        FindObjectOfType<PlayerController>().gameObject.GetComponent<ChangeCharacSprite>().ChangeBackToDragonBone();
    }
}

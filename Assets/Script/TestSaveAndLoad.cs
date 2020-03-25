using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSaveAndLoad : MonoBehaviour {
    public bool save = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(save) {
            save = false;
            SaveAndLoad.Save(1);
        }
	}
}

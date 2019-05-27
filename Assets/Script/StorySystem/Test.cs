using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StoryManager.Instance.劇本名稱 = "kopa";
        StoryManager.Instance.BeginStory();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trans : MonoBehaviour {
	public string targetScenename;
	
	void OnTriggerEnter2D(Collider2D col_item){
		SceneManager.LoadScene(targetScenename);
	}
}

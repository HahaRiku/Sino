using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoTrans : MonoBehaviour {
	public string targetScenename;
	public Vector3 nextScenePos;
	
	void OnTriggerEnter2D(Collider2D col_item){
		SceneManager.LoadScene(targetScenename);
		if(nextScenePos != Vector3.zero){
			//Set Player's position
		}
	}
}

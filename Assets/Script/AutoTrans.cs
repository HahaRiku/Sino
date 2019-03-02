using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoTrans : MonoBehaviour {
	
	[SerializeField]
	private Object targetScene;
	public int nextScenePosLabel;	//0=null , 1=L , 2=R , 3=door , 4=stair
	
	public Vector3 nextScenePos;
	
	void OnTriggerEnter2D(Collider2D col_item){
		
		SceneManager.LoadScene(targetScene.name);
		Temp_sceneinit.transformPointNum = nextScenePosLabel;
		
		/*
		if(nextScenePos != Vector3.zero){
			//Set Player's position
		}*/
	}
}

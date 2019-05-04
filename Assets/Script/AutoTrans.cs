using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoTrans : MonoBehaviour {
	
	[SerializeField]
	private Object targetScene;
	public int nextScenePosLabel;	//0=null , 1=L , 2=R , 3=door , 4=stair
	
	void OnTriggerEnter2D(Collider2D col_item){
		Temp_sceneinit.transformPointNum = nextScenePosLabel;
		SceneManager.LoadScene(targetScene.name);
	}
	
}

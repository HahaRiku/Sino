using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoTrans : MonoBehaviour {
	
	[SerializeField]
	private Object targetScene;
	public int nextScenePosLabel;	//0=null , 1=L , 2=R , 3=door , 4=stair , 5=smallDoor	
	
	void OnTriggerEnter2D(Collider2D other){
		
		//Temp_GameManager.transformPointNum = nextScenePosLabel;
		//player = Temp_GameManager.playerObj;
		Temp_GameManager.transPosSet(nextScenePosLabel);
		
		SceneManager.LoadScene(targetScene.name/* , LoadSceneMode.Additive*/);
		
	}
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoTrans : MonoBehaviour {
	
	[SerializeField]
	private Object targetScene;
	[SerializeField]
	private string targetSceneName;
	[SerializeField]
	private float freeX;
	public GameStateManager.SpawnPoint nextScenePos;    //0=null , 1=L , 2=R , 3=door , 4=stair , 5=smallDoor	

    void OnTriggerEnter2D(Collider2D other)
    {
        if((int)nextScenePos == 6){
            FindObjectOfType<GameStateManager>().黑幕轉場(targetSceneName, new Vector3(freeX, -3.2f, 0), GameStateManager.Facing.保留);
        }		
        else
            FindObjectOfType<GameStateManager>().黑幕轉場(targetSceneName, nextScenePos, GameStateManager.Facing.保留);
        //GameStateManager.Instance.黑幕轉場(targetScene.name, nextScenePos);
    }
}

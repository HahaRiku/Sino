using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Temp_GameManager : MonoBehaviour {
	
	static Temp_GameManager instance;
	public GameObject playerObj;
	
	void Awake(){
		if(instance == null){
			instance = this;
			DontDestroyOnLoad(this);
		}
		else if (this != instance){
			Destroy(gameObject);
		}
	}
	
	void Start (){
		playerObj = Instantiate(Resources.Load("Characters/player2")) as GameObject;
		playerObj.transform.position = new Vector3(-3.73f, -4.07f, 0);
		DontDestroyOnLoad(playerObj);
	}
}

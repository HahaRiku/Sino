using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Temp_GameManager : MonoBehaviour {
	
	static Temp_GameManager instance;
	
	public static int transformPointNum;	//傳送點標號
	public static GameObject playerObj;
	public static Vector3[] transPos = {
		new Vector3(5.5f, -4.07f, 0),
		new Vector3(-5.5f, -4.07f, 0),
		new Vector3(0, -4.07f, 0),
		new Vector3(-2.3f, -4.07f, 0),
		new Vector3(-1.5f, -4.07f, 0),
		new Vector3(1.5f, -4.07f, 0)
	};
	
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
		playerObj = Instantiate(Resources.Load("Characters/Sino")) as GameObject;
		//playerObj.transform.position = new Vector3(-3.73f, -4.07f, 0);
		DontDestroyOnLoad(playerObj);
		Debug.Log("546156");
	}
	
	public static void transPosSet(int nextScenePosLabel){
		switch(nextScenePosLabel){
			case 0:
				playerObj.transform.position = transPos[2];
				break;
			case 1:		//to right
				playerObj.transform.position = transPos[0];
				break;
			case 2:		//to left
				playerObj.transform.position = transPos[1];
				break;
			case 3:		//to door
				playerObj.transform.position = transPos[2];
				break;
			case 4:		//to small door
				playerObj.transform.position = transPos[3];
				break;
			case 5:		//to right stair
				playerObj.transform.position = transPos[4];
				break;
			case 6:		//to left stair
				playerObj.transform.position = transPos[5];
				break;
			/*case 6:
				GameObject g = GameObject.Find("Door") as GameObject;
				break;*/
			default:	break;
		}
	}
}

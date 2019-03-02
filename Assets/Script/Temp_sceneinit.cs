using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_sceneinit : MonoBehaviour {

	public static int transformPointNum;	//傳送點標號
	public GameObject player;				//玩家物件
	
	void Start () {
		//player = Instantiate("player");
		
		if(transformPointNum != 0){
			GameObject g = null;
			switch(transformPointNum){
				case 1:
					g = GameObject.Find("L") as GameObject;
					break;
				case 2:
					g = GameObject.Find("R") as GameObject;
					break;
				case 3:
					g = GameObject.Find("Door") as GameObject;
					break;
				case 4:
					g = GameObject.Find("Stair") as GameObject;
					break;
				/*case 5:
					GameObject g = GameObject.Find("Door") as GameObject;
					break;*/
				default:	break;
			}
			
			if(g != null){
				player.transform.position = g.transform.position;
				
			}
			transformPointNum = 0;
		}
	}
	
}

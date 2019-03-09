using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trans_Stair : MonoBehaviour {
	
	[System.Serializable]
	public struct targetscenelist{
		public Object targetScene;
		public int nextScenePosLabel;	//0=null , 1=L , 2=R , 3=door , 4=stair
	};
	[SerializeField]
	private targetscenelist[] sceneList;
	
	public enum stairtype{
		UpAndDown,
		Up,
		Down
	};
	public stairtype stairType;
	
	void OnTriggerEnter2D(Collider2D col_item){
		StartCoroutine(TransUI());
	}
	
	private IEnumerator TransUI(){
		CharWithItem.actEnable = false;		//limit moving
		
		switch(stairType){
			case stairtype.UpAndDown:
				this.transform.GetChild(0).gameObject.SetActive(true);
				this.transform.GetChild(1).gameObject.SetActive(true);
				
				//UI+choose
				bool done = false;
				while(!done){
					if(Input.GetKeyDown(KeyCode.UpArrow)){
						done = true;
						SceneManager.LoadScene(sceneList[0].targetScene.name);
						Temp_sceneinit.transformPointNum = sceneList[0].nextScenePosLabel;
					}
					else if(Input.GetKeyDown(KeyCode.DownArrow)){
						done = true;
						SceneManager.LoadScene(sceneList[1].targetScene.name);
						Temp_sceneinit.transformPointNum = sceneList[1].nextScenePosLabel;
					}
					/*else if(Input.GetKeyDown(KeyCode.X)){	//cancel
						
					}*/
					yield return 0;
				}
				
				break;
			case stairtype.Up:
				this.transform.GetChild(0).gameObject.SetActive(true);
				
				done = false;
				while(!done){
					if(Input.GetKeyDown(KeyCode.UpArrow)){
						done = true;
						SceneManager.LoadScene(sceneList[0].targetScene.name);
						Temp_sceneinit.transformPointNum = sceneList[0].nextScenePosLabel;
					}
					yield return 0;
				}
				break;
			case stairtype.Down:
				this.transform.GetChild(1).gameObject.SetActive(true);
				
				done = false;
				while(!done){
					if(Input.GetKeyDown(KeyCode.DownArrow)){
						done = true;
						SceneManager.LoadScene(sceneList[1].targetScene.name);
						Temp_sceneinit.transformPointNum = sceneList[1].nextScenePosLabel;
					}
					yield return 0;
				}
				break;
			default:	break;
		}
		
	}
	
}

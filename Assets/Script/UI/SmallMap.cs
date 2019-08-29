using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SmallMap : MonoBehaviour {

	//覺得可以和GameStateManager合併，但暫時寫在這裡
	//DontDestroy(this)
	
	public GameObject mapUI;
	public Sprite[] floorImage;
	public GameObject ani_mapShowUpY, ani_mapShowDownY;
	
	private GameObject GM;
	private GameObject[] playerPlaceImage;
	private bool currentShowState,lastShowState;
	private string currentSceneName;
	
	private Vector3[] playerPlace = {
        new Vector3(-28f, -296f, 0),
        new Vector3(-28f, -201.1f, 0),
        new Vector3(-28f, -108.3f, 0),
        new Vector3(-28f, -9.8f, 0),
        new Vector3(-28f, 85.3f, 0),
		new Vector3(-28f, 179f, 0)
    };
	
	void Awake(){
		
		playerPlaceImage = new GameObject[4];
		playerPlaceImage[0] = mapUI.transform.GetChild(1).gameObject;
		playerPlaceImage[1] = mapUI.transform.GetChild(2).gameObject;
		playerPlaceImage[2] = mapUI.transform.GetChild(3).gameObject;
		playerPlaceImage[3] = mapUI.transform.GetChild(4).gameObject;//.gameObject.GetComponent<Image>();
		
	}
	
	void Start(){
		//***以下3個值不該在這裡初始化
		currentShowState = false;
		lastShowState = false;
		currentSceneName = SystemVariables.Scene;
		GM = GameObject.Find("GM");
		
		SmallMapUpdate();
		StartCoroutine(SmallMapShow());
	}
	
	void Update () {
		if(currentSceneName != SystemVariables.Scene){GM = GameObject.Find("GM");}
		if(GM.GetComponent<GameStateManager>().NowStatus == GameStateManager.SceneStatus.自由探索){
			currentShowState = true;
		}
		else{currentShowState = false;}
	}
	private IEnumerator SmallMapShow(){
		while(true){
			if(currentShowState){
				if(lastShowState){
					//Update current player place
					if(currentSceneName != SystemVariables.Scene){
						SmallMapUpdate();}
				}
				else{
					//Open map
					
					//mapUI.SetActive(true);
					//Debug.Log(mapUI.transform.parent.gameObject.name);
					StartCoroutine(SmallMapShowAnimation(true));
					lastShowState = currentShowState;
				}
			}
			else{
				if(lastShowState){
					//Close map
					
					//mapUI.SetActive(false);
					//Debug.Log(mapUI.transform.parent.gameObject.name);
					StartCoroutine(SmallMapShowAnimation(false));
					lastShowState = currentShowState;
				}
			}
			yield return null;
		}
	}
	private IEnumerator SmallMapShowAnimation(bool IsUp){
		if(IsUp){
			while(Vector3.Distance(ani_mapShowUpY.transform.position, mapUI.transform.parent.position) >= 3.0f){
				mapUI.transform.parent.position = Vector3.MoveTowards(mapUI.transform.parent.position, ani_mapShowUpY.transform.position, 3.0f);
				yield return new WaitForFixedUpdate();
			}
		}
		else{
			while(Vector3.Distance(ani_mapShowDownY.transform.position, mapUI.transform.parent.position) >= 3.0f){
				mapUI.transform.parent.position = Vector3.MoveTowards(mapUI.transform.parent.position, ani_mapShowDownY.transform.position, 3.0f);
				yield return new WaitForFixedUpdate();
			}
		}
	}
	
	private void SmallMapUpdate(){
		currentSceneName = SystemVariables.Scene;
		
		//Floor name
		switch(currentSceneName.Substring(0,2)){
			case "1F":
				playerPlaceImage[0].GetComponent<Image>().sprite = floorImage[0];
				break;
			case "2F":
				playerPlaceImage[0].GetComponent<Image>().sprite = floorImage[1];
				break;
			case "3F":
				playerPlaceImage[0].GetComponent<Image>().sprite = floorImage[2];
				break;
			case "B1":
				playerPlaceImage[0].GetComponent<Image>().sprite = floorImage[3];
				break;
			case "B2":
				playerPlaceImage[0].GetComponent<Image>().sprite = floorImage[4];
				break;
			case "B3":
				playerPlaceImage[0].GetComponent<Image>().sprite = floorImage[5];
				break;
			default:break;
		}
		
		//Player place
		if(currentSceneName[currentSceneName.Length -1] == '梯'){
			playerPlaceImage[1].SetActive(false);
			switch(currentSceneName[currentSceneName.Length -3]){
				case '左':
					playerPlaceImage[2].SetActive(true);
					break;
				case '右':
					playerPlaceImage[3].SetActive(true);
					break;
				default:break;
			}
		}
		else{
			playerPlaceImage[2].SetActive(false);
			playerPlaceImage[3].SetActive(false);
			playerPlaceImage[1].SetActive(true);
			
			switch(currentSceneName[currentSceneName.Length -1]){
				case '1':
					playerPlaceImage[1].transform.localPosition = playerPlace[0];
					break;
				case '2':
					playerPlaceImage[1].transform.localPosition = playerPlace[1];
					break;
				case '3':
					playerPlaceImage[1].transform.localPosition = playerPlace[2];
					break;
				case '4':
					playerPlaceImage[1].transform.localPosition = playerPlace[3];
					break;
				case '5':
					playerPlaceImage[1].transform.localPosition = playerPlace[4];
					break;
				case '6':
					playerPlaceImage[1].transform.localPosition = playerPlace[5];
					break;
				default:break;
			}
		}
	}
	
}

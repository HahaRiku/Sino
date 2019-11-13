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
	
	private GameStateManager GM;
	private GameObject[] playerPlaceImage;
	private bool currentShowState,lastShowState;
	private string currentSceneName;
	
	private Vector3[] playerPlace = {
        new Vector3(-21.5f, -339.6f, 0),
        new Vector3(-22.6f, -224.9f, 0),
        new Vector3(-22.6f, -114.3f, 0),
        new Vector3(-19.1f, 12.1f, 0),
        new Vector3(-23.1f, 131.1f, 0),
		new Vector3(-26.6f, 238.2f, 0)
    };
	
	void Awake(){
		
		playerPlaceImage = new GameObject[4];
		playerPlaceImage[0] = mapUI.transform.GetChild(1).gameObject;
		playerPlaceImage[1] = mapUI.transform.GetChild(2).gameObject;
		playerPlaceImage[2] = mapUI.transform.GetChild(3).gameObject;
		playerPlaceImage[3] = mapUI.transform.GetChild(4).gameObject;//.gameObject.GetComponent<Image>();
		
	}
	
	void Start(){
		currentShowState = false;
		lastShowState = false;
		currentSceneName = SystemVariables.Scene;
		GM = GameObject.Find("GM").GetComponent<GameStateManager>();
		
		SmallMapUpdate();
		StartCoroutine(SmallMapShow());
	}
	
	void Update () {
		if(currentSceneName != SystemVariables.Scene){GM = GameObject.Find("GM").GetComponent<GameStateManager>();}
        if (GM != null) {
            if (GM.NowStatus == GameStateManager.SceneStatus.自由探索) {
                currentShowState = true;
            }
            else { currentShowState = false; }
        }
        
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
		switch(GM.SmallMapInfo.floor){
			case 1:
				playerPlaceImage[0].GetComponent<Image>().sprite = floorImage[0];
				break;
			case 2:
				playerPlaceImage[0].GetComponent<Image>().sprite = floorImage[1];
				break;
			case 3:
				playerPlaceImage[0].GetComponent<Image>().sprite = floorImage[2];
				break;
			case -1:
				playerPlaceImage[0].GetComponent<Image>().sprite = floorImage[3];
				break;
			case -2:
				playerPlaceImage[0].GetComponent<Image>().sprite = floorImage[4];
				break;
			case -3:
				playerPlaceImage[0].GetComponent<Image>().sprite = floorImage[5];
				break;
			default:break;
		}
		
		//Player place
		if(GM.SmallMapInfo.corridorPlace == 0 || GM.SmallMapInfo.corridorPlace == 7){
			playerPlaceImage[1].SetActive(false);
			switch(GM.SmallMapInfo.corridorPlace){
				case 0:
					playerPlaceImage[2].SetActive(true);
					break;
				case 7:
					playerPlaceImage[3].SetActive(true);
					break;
				default:break;
			}
		}
		else{
			playerPlaceImage[2].SetActive(false);
			playerPlaceImage[3].SetActive(false);
			playerPlaceImage[1].SetActive(true);
			
			switch(GM.SmallMapInfo.corridorPlace){
				case 1:
					playerPlaceImage[1].transform.localPosition = playerPlace[0];
					break;
				case 2:
					playerPlaceImage[1].transform.localPosition = playerPlace[1];
					break;
				case 3:
					playerPlaceImage[1].transform.localPosition = playerPlace[2];
					break;
				case 4:
					playerPlaceImage[1].transform.localPosition = playerPlace[3];
					break;
				case 5:
					playerPlaceImage[1].transform.localPosition = playerPlace[4];
					break;
				case 6:
					playerPlaceImage[1].transform.localPosition = playerPlace[5];
					break;
				default:break;
			}
		}
	}
	
}

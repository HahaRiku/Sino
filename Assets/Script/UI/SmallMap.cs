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
	
	private Vector2[] playerPlace = {
		new Vector2(-26.6f, 238.2f),
        new Vector2(-23.1f, 131.1f),
        new Vector2(-19.1f, 12.1f),
        new Vector2(-22.6f, -114.3f),
        new Vector2(-22.6f, -224.9f),
        new Vector2(-21.5f, -339.6f)
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
        playerPlaceImage[0].GetComponent<Image>().sprite = floorImage[(int)GM.第幾層];
		
		//Player place
		if((int)GM.從左邊數來第幾個 == 0){
            playerPlaceImage[1].SetActive(false);
            playerPlaceImage[2].SetActive(true);
		}
        else if((int)GM.從左邊數來第幾個 == 7) {
            playerPlaceImage[1].SetActive(false);
            playerPlaceImage[3].SetActive(true);
        }
		else{
			playerPlaceImage[2].SetActive(false);
			playerPlaceImage[3].SetActive(false);
			playerPlaceImage[1].SetActive(true);
			playerPlaceImage[1].transform.localPosition = playerPlace[(int)GM.從左邊數來第幾個 - 1];
		}
	}
	
}

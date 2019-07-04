using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class CharacterControl2 : MonoBehaviour {
    [System.Serializable]
    public struct CharacterObjectAndName {
        public string name;
        public GameObject obj;
    }
	
	
	public enum AnimationState{
		idle,
		stand,
		walk,
		stand_with_candle_left,
		stand_with_candle_right,
		walk_with_candle_left,
		walk_with_candle_right
	};
	public AnimationState aniState;		//偵測玩家動作
	private bool IsWalk_ani;
	public bool IsHoldCandle_ani;	//預設為不拿蠟燭，須從外面腳本改動
	

    public float speed;
    //public Dialog dialogComp;
    public CharacterObjectAndName[] otherCharactersObjects;
    private DragonBones.Animation ani;
    
    void Start() {
        ani = GetComponent<UnityArmatureComponent>().armature.animation;
        IsWalk_ani = false;
		IsHoldCandle_ani = false;
		aniState = AnimationState.stand;		
    }

    // Update is called once per frame
    void Update () {
        //if(CharWithItem.actEnable){
        if (!SystemVariables.lockMoving) {
            if (!(Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))) {				//左右鍵非同時按住
                /*
				if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow)) {	//開始走路
                    IsWalk_ani = true;
                    ani.Play("walk");
                }
				if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow)) {	//開始走路
                    IsWalk_ani = true;
                    ani.Play("walk");
                }*/
				
                if (Input.GetKey(KeyCode.RightArrow)) {		//持續向右
					AnimationController("continuing_right");
					
                    transform.localPosition = new Vector3(transform.localPosition.x + speed, transform.localPosition.y, transform.localPosition.z);
                    if (transform.localScale.x > 0) {
                        transform.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
                    }
                }
                if (Input.GetKey(KeyCode.LeftArrow)) {		//持續向左
                    AnimationController("continuing_left");
					
                    transform.localPosition = new Vector3(transform.localPosition.x - speed, transform.localPosition.y, transform.localPosition.z);
                    if (transform.localScale.x < 0) {
                        transform.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
                    }
                }
                if (!(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))) {		//按鍵放開(停下)
                    AnimationController("stand_or_stop");
					
                }
            }
            else {		//按鍵同時按住
                AnimationController("stand_or_stop");
				
            }
            //}
        }
    }

    public void AutoWalk(string name, float x) {
        StartCoroutine(WalkLerp(name, x));
    }

    IEnumerator WalkLerp(string name, float x) {
        GameObject tempGameObject= null;
        print(name);
        print(x);
        if (name != "席諾") {
            for (int i = 0; i < otherCharactersObjects.Length; i++) {
                if (name == otherCharactersObjects[i].name) {
                    tempGameObject = otherCharactersObjects[i].obj;
                    break;
                }
            }
        }
        else {
            tempGameObject = gameObject;
        }
        ani.Play("walk");
        if (tempGameObject != null) {
            print("123");
            for (float i = transform.position.x; i < x; i += speed) {
                tempGameObject.transform.localPosition = new Vector3(i, tempGameObject.transform.position.y, tempGameObject.transform.position.z);
                yield return null;
            }
        }
        else {
            print("walk name error");
        }
        ani.Play("stand");
        //dialogComp.SetLineDone(true);		//沒在用
        //ani.enabled = false;
    }
	
	
	
	public void AnimationController(string command){
		switch(command){
			case "idle":
				break;
				
			case "stand_or_stop":
				if(aniState == AnimationState.stand || aniState == AnimationState.walk){	//normal
					IsWalk_ani = false;
					aniState = AnimationState.stand;
					ani.Play("stand");
				}else if (aniState == AnimationState.walk_with_candle_left || aniState == AnimationState.stand_with_candle_left){	//candle_left
					IsWalk_ani = false;
					aniState = AnimationState.stand_with_candle_left;
					ani.Play("stand_with_candle_left");
				}else if (aniState == AnimationState.walk_with_candle_right || aniState == AnimationState.stand_with_candle_right){	//candle_right
					IsWalk_ani = false;
					aniState = AnimationState.stand_with_candle_right;
					ani.Play("stand_with_candle_right");
				}
				break;
				
			case "continuing_right":
				if (!IsWalk_ani) {
					IsWalk_ani = true;
					if(IsHoldCandle_ani){
						aniState = AnimationState.walk_with_candle_right;
						ani.Play("walk_with_candle_right");
					}
					else{
						aniState = AnimationState.walk;
						ani.Play("walk");
					}
				}
				break;
				
			case "continuing_left":
				if (!IsWalk_ani) {
					IsWalk_ani = true;
					if(IsHoldCandle_ani){
						aniState = AnimationState.walk_with_candle_left;
						ani.Play("walk_with_candle_left");
					}
					else{
						aniState = AnimationState.walk;
						ani.Play("walk");
					}
				}
				break;

			default:	break;
		}
	}
	
}

/*
	//private int temp_stats = 0;	//0 = idle, 1 = left, 2=right
	
    void Start() {
        ani= gameObject.GetComponent<Animator>();
        IsWalk_ani = false;
		//StartCoroutine(WalkController ());
    }
	
	private void UpdateWalk (int stats) {
		switch(stats){
			case 0:
				IsWalk_ani = false;
				ani.SetBool("Walk", false);
				break;
				
			case 1:
				if (!IsWalk_ani) {
					IsWalk_ani = true;
					ani.SetBool("Walk", true);
				}
				transform.localPosition = new Vector3(transform.localPosition.x - speed, transform.localPosition.y, transform.localPosition.z);
				if (transform.localScale.x < 0) {
					transform.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
				}
				break;
				
			case 2:
				if (!IsWalk_ani) {
					IsWalk_ani = true;
					ani.SetBool("Walk", true);
				}
				transform.localPosition = new Vector3(transform.localPosition.x + speed, transform.localPosition.y, transform.localPosition.z);
				if (transform.localScale.x > 0) {
					transform.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
				}
				break;
				
			default:	break;
		}
	}
	
	private IEnumerator WalkController () {
        //if(CharWithItem.actEnable){//
        while(true){
			
			switch(temp_stats){
				case 0:
					UpdateWalk (0);
					//Debug.Log("414");
					if (Input.GetKey(KeyCode.RightArrow)) {	//2
						temp_stats = 2;
					}
					if (Input.GetKey(KeyCode.LeftArrow)) {
						temp_stats = 1;
					}
					if (!(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))) {
						temp_stats = 0;
					}
					break;
					
				case 1:
					UpdateWalk (1);
					
					if (Input.GetKey(KeyCode.LeftArrow)) {
						temp_stats = 1;
					}
					if (Input.GetKeyDown(KeyCode.RightArrow)) {	//2
						temp_stats = 2;
					}
					if (!(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))) {
						temp_stats = 0;
					}
					break;
					
				case 2:
					UpdateWalk (2);
					
					if (Input.GetKey(KeyCode.RightArrow)) {	//2
						temp_stats = 2;
					}
					if (Input.GetKeyDown(KeyCode.LeftArrow)) {
						temp_stats = 1;
					}
					if (!(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))) {
						temp_stats = 0;
					}
					break;
					
				default:	break;
			}
			
			yield return 0;
        }
    }*/
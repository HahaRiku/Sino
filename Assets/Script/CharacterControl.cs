using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class CharacterControl : MonoBehaviour {
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
	private bool IsWalk_ani;
    private DragonBones.Animation ani;
	
	//---------------接口---------------
	
	public bool IsHoldCandle_ani = false;		//預設為不拿蠟燭，須從外面腳本改動
	public AnimationState aniState = AnimationState.stand;		//偵測玩家動作
    [Range(0.05f, 0.3f)]
	public float speed;
    public CharacterObjectAndName[] otherCharactersObjects;

    private bool controlLock = false;
	
    void Start() {
        ani = GetComponent<UnityArmatureComponent>().armature.animation;
        IsWalk_ani = false;
		IsHoldCandle_ani = false;
		aniState = AnimationState.stand;
		//speed = 0.1f;
    }

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
	
    public void SetIsPlayerCanControl(bool b)
    {
        controlLock = !b;
    }
    public void SetPlayerStopMove()
    {
        controlLock = true;
    }
}
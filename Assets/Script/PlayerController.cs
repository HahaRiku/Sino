using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class PlayerController : MonoBehaviour
{
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
    private DragonBones.Armature arma;

    private bool controlLock = false;
    private int running = 0;

    //---------------接口---------------
    [HideInInspector]
    public bool IsHoldCandle_ani = false;		//預設為不拿蠟燭，須從外面腳本改動
	public AnimationState aniState = AnimationState.stand;		//偵測玩家動作
    [Range(0.05f, 0.3f)]
	public float speed;

    void Start() {
        arma = transform.GetChild(0).GetComponent<UnityArmatureComponent>().armature;
        IsWalk_ani = false;
		IsHoldCandle_ani = false;
		aniState = AnimationState.stand;
    }

    void Update () {
        if (!controlLock) {
            if (!(Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))) {				//左右鍵非同時按住
                if (Input.GetKey(KeyCode.RightArrow)) {		//持續向右
					AnimationController("continuing_right");
                    transform.localPosition = new Vector3(transform.localPosition.x + speed, transform.localPosition.y, transform.localPosition.z);
                }
                if (Input.GetKey(KeyCode.LeftArrow)) {		//持續向左
                    AnimationController("continuing_left");
                    transform.localPosition = new Vector3(transform.localPosition.x - speed, transform.localPosition.y, transform.localPosition.z);
                }
                if (!(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))) {		//按鍵放開(停下)
                    AnimationController("stand_or_stop");
                }
            }
            else {		//按鍵同時按住
                AnimationController("stand_or_stop");
            }
        }
        else
            AnimationController("stand_or_stop");
    }
	
	
	public void AnimationController(string command){
		switch(command){
			case "idle":
				break;
				
			case "stand_or_stop":
				if(aniState == AnimationState.stand || aniState == AnimationState.walk){	//normal
					IsWalk_ani = false;
					aniState = AnimationState.stand;
                    arma.animation.Play("stand");
				}else if (aniState == AnimationState.walk_with_candle_left || aniState == AnimationState.stand_with_candle_left){	//candle_left
					IsWalk_ani = false;
					aniState = AnimationState.stand_with_candle_left;
                    arma.animation.Play("stand_with_candle_left");
				}else if (aniState == AnimationState.walk_with_candle_right || aniState == AnimationState.stand_with_candle_right){	//candle_right
					IsWalk_ani = false;
					aniState = AnimationState.stand_with_candle_right;
                    arma.animation.Play("stand_with_candle_right");
				}
				break;
				
			case "continuing_right":
				if (!IsWalk_ani) {
					IsWalk_ani = true;
                    arma.flipX = true;
                    if (IsHoldCandle_ani){
						aniState = AnimationState.walk_with_candle_right;
                        arma.animation.Play("walk_with_candle_right");
					}
					else{
						aniState = AnimationState.walk;
                        arma.animation.Play("walk");
					}
				}
				break;
				
			case "continuing_left":
				if (!IsWalk_ani) {
                    IsWalk_ani = true;
                    arma.flipX = false;
                    if (IsHoldCandle_ani){
						aniState = AnimationState.walk_with_candle_left;
                        arma.animation.Play("walk_with_candle_left");
					}
					else{
						aniState = AnimationState.walk;
                        arma.animation.Play("walk");
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
}
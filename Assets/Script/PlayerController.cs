using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class PlayerController : MonoBehaviour
{
    private UnityArmatureComponent arma;
    enum CollisionStatus
    {
        none,
        left_collision,
        right_collision
    }
    CollisionStatus nowCollisionStatus;
 
    private bool isPlayerCanControl = false;

    public bool IsHoldingCandle = false;		//預設為不拿蠟燭，須從外面腳本改動

    [Range(0.05f, 2f)]
    public float speed = 1; //速度倍率
    private float _v = 5; //預設每秒x移動距離
    private int flip = 0;

    void Start() {
        arma = transform.GetChild(0).GetComponent<UnityArmatureComponent>();
        AnimationController("idle");
    }

    void Update () {
        if (isPlayerCanControl) {
            if (nowCollisionStatus == CollisionStatus.left_collision && Input.GetKey("left"))
            {
                AnimationController("idle");
                flip = 0;
            }
            if (nowCollisionStatus == CollisionStatus.right_collision && Input.GetKey("right"))
            {
                AnimationController("idle");
                flip = 0;
            }

            if (Input.GetKeyDown("left") && nowCollisionStatus != CollisionStatus.left_collision)
            {
                AnimationController("walk_left");
                flip = -1;
            }
            else if (Input.GetKeyUp("left"))
            {
                if (!Input.GetKey("right"))
                {
                    AnimationController("idle");
                    flip = 0;
                }
                else
                {
                    AnimationController("walk_right");
                    flip = 1;
                }
            }

            if (Input.GetKeyDown("right") && nowCollisionStatus != CollisionStatus.right_collision)
            {
                AnimationController("walk_right");
                flip = 1;
            }
            else if (Input.GetKeyUp("right"))
            {
                if (!Input.GetKey("left"))
                {
                    AnimationController("idle");
                    flip = 0;
                }
                else
                {
                    AnimationController("walk_left");
                    flip = -1;
                }
            }
            transform.Translate(flip * Vector2.right * speed * _v * Time.deltaTime);
            return;
        }
    }
	
	
	public void AnimationController(string command){
		switch(command){				
			case "idle":
                if (arma.animationName == "stand")
                    break;
                if(!IsHoldingCandle)
                    arma.animation.FadeIn("stand", 0.08f);
				else if(arma.armature.flipX)
                    arma.animation.FadeIn("stand_with_candle_left", 0.03f);
                else
                    arma.animation.FadeIn("stand_with_candle_right", 0.03f);
                arma.animationName = "stand";
                break;		
			case "walk_right":
                arma.armature.flipX = true;
                if (!IsHoldingCandle)
                    arma.animation.FadeIn("walk", 0.1f);
                else
                    arma.animation.FadeIn("walk_with_candle_left", -1);
                arma.animationName = "walk";
                break;			
			case "walk_left":
                arma.armature.flipX = false;
                if (!IsHoldingCandle)
                    arma.animation.FadeIn("walk", 0.1f);
                else
                    arma.animation.FadeIn("walk_with_candle_right", -1);
                arma.animationName = "walk";
                break;
            case "backward":
                arma.animation.FadeIn("backward", 0.08f, 1);
                arma.animationName = "backward";
                break;
            default:
                break;
		}
	}
	
    public void SetIsPlayerCanControl(bool b)
    {
        isPlayerCanControl = b;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].point.x <= transform.position.x)
            nowCollisionStatus = CollisionStatus.left_collision;
        else if (collision.contacts[0].point.x > transform.position.x)
            nowCollisionStatus = CollisionStatus.right_collision;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        nowCollisionStatus = CollisionStatus.none;
    }
}
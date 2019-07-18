using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class PlayerController : MonoBehaviour
{
    private UnityArmatureComponent arma;
    private UnityArmatureComponent arma2;

    enum DisplayStatus
    {
        左右,
        正背
    }
    enum CollisionStatus
    {
        none,
        left_collision,
        right_collision
    }
    CollisionStatus nowCollisionStatus;
    DisplayStatus nowDisplayStatus;

    private bool isPlayerCanControl = false;

    public bool IsHoldingCandle = false;		//預設為不拿蠟燭，須從外面腳本改動

    [Range(0.05f, 2f)]
    public float speed = 1; //速度倍率
    private float _v = 5; //預設每秒x移動距離
    private int flip = 0;

    void Start() {
        arma = transform.GetChild(0).GetComponent<UnityArmatureComponent>();
        arma2 = transform.GetChild(1).GetComponent<UnityArmatureComponent>();
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        nowDisplayStatus = DisplayStatus.正背;
        AnimationController("idle");
    }

    void Update () {
        if (isPlayerCanControl) {
            Debug.Log(arma._armature);
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

            if(Input.GetKeyDown("up"))
            {
                AnimationController("reverse_side");
                flip = 0;
            }
            if (Input.GetKeyDown("down"))
            {
                AnimationController("front_side");
                flip = 0;
            }
            transform.Translate(flip * Vector2.right * speed * _v * Time.deltaTime);
            return;
        }
    }
	
	
	public void AnimationController(string command){
        switch (command)
        {
            case "idle":
                if (nowDisplayStatus != DisplayStatus.左右)
                {
                    transform.GetChild(0).gameObject.SetActive(true);
                    transform.GetChild(1).gameObject.SetActive(false);
                    nowDisplayStatus = DisplayStatus.左右;
                }
                if (arma.animationName == "stand")
                    break;
                if (!IsHoldingCandle)
                    arma.animation.FadeIn("stand", 0.08f);
                else if (arma.armature.flipX)
                    arma.animation.FadeIn("stand_with_candle_left", 0.03f);
                else
                    arma.animation.FadeIn("stand_with_candle_right", 0.03f);
                arma.animationName = "stand";
                break;
            case "walk_right":
                if (nowDisplayStatus != DisplayStatus.左右)
                {
                    transform.GetChild(0).gameObject.SetActive(true);
                    transform.GetChild(1).gameObject.SetActive(false);
                    nowDisplayStatus = DisplayStatus.左右;
                }
                arma.armature.flipX = true;
                if (!IsHoldingCandle)
                    arma.animation.FadeIn("walk", 0.1f);
                else
                    arma.animation.FadeIn("walk_with_candle_left", -1);
                arma.animationName = "walk";
                break;
            case "walk_left":
                if (nowDisplayStatus != DisplayStatus.左右)
                {
                    transform.GetChild(0).gameObject.SetActive(true);
                    transform.GetChild(1).gameObject.SetActive(false);
                    nowDisplayStatus = DisplayStatus.左右;
                }
                arma.armature.flipX = false;
                if (!IsHoldingCandle)
                    arma.animation.FadeIn("walk", 0.1f);
                else
                    arma.animation.FadeIn("walk_with_candle_right", -1);
                arma.animationName = "walk";
                break;
            case "backward":
                if (nowDisplayStatus != DisplayStatus.左右)
                {
                    transform.GetChild(0).gameObject.SetActive(true);
                    transform.GetChild(1).gameObject.SetActive(false);
                    nowDisplayStatus = DisplayStatus.左右;
                }
                arma.animation.FadeIn("backward", 0.08f, 1);
                arma.animationName = "backward";
                break;
            case "front_side":
                if (nowDisplayStatus != DisplayStatus.正背)
                {
                    transform.GetChild(0).gameObject.SetActive(false);
                    transform.GetChild(1).gameObject.SetActive(true);
                    nowDisplayStatus = DisplayStatus.正背;
                }
                arma2.animation.FadeIn("front_side");
                arma2.animationName = "front_side";
                arma2.armatureName = "Sino(正反)";
                break;
            case "reverse_side":
                if (nowDisplayStatus != DisplayStatus.正背)
                {
                    transform.GetChild(0).gameObject.SetActive(false);
                    transform.GetChild(1).gameObject.SetActive(true);
                    nowDisplayStatus = DisplayStatus.正背;
                }
                arma2.animation.FadeIn("reverse_side");
                arma2.animationName = "reverse_side";
                arma2.armatureName = "Sino(正反)";
                break;
            default:
                break;
        }
	}
	
    public void SetIsPlayerCanControl(bool b)
    {
        isPlayerCanControl = b;
        if (!b)
        {
            AnimationController("idle");
            flip = 0;
        }
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
	
	public void SetPlayerLight(){	//燈光
		transform.Find("Sino/Spotlight").gameObject.SetActive(IsHoldingCandle);
	}
}
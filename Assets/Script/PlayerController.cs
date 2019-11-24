using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class PlayerController : MonoBehaviour
{
    private UnityArmatureComponent arma;
    private ChangeCharacSprite CharacSpriteController;

    enum DisplayStatus
    {
        左右,
        正,
        背
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
    public Light CandleLight;
    private float candlePosX;

    [Range(0.05f, 2f)]
    public float speed = 1; //速度倍率
    private float _v = 5; //預設每秒x移動距離
    private int flip = 0;
	public bool GetIsRight(){
		return arma.armature.flipX;
	}

    void Start() {
        
        arma = transform.GetChild(0).GetComponent<UnityArmatureComponent>();
        CharacSpriteController = GetComponent<ChangeCharacSprite>();
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        nowDisplayStatus = DisplayStatus.正;
        //AnimationController("idle");
    }

    void Update () {
        if (SystemVariables.Scene == "title") {
            Destroy(gameObject);
        }
        if (isPlayerCanControl) {
            if (nowCollisionStatus == CollisionStatus.left_collision && Input.GetKey("left"))
            {
                AnimationController("idle",GetIsRight());
                flip = 0;
            }
            if (nowCollisionStatus == CollisionStatus.right_collision && Input.GetKey("right"))
            {
                AnimationController("idle",GetIsRight());
                flip = 0;
            }

            if (Input.GetKeyDown("left") && nowCollisionStatus != CollisionStatus.left_collision)
            {
                AnimationController("walk_left",GetIsRight());
                flip = -1;
            }
            else if (Input.GetKeyUp("left"))
            {
                if (!Input.GetKey("right"))
                {
                    AnimationController("idle",GetIsRight());
                    flip = 0;
                }
                else
                {
                    AnimationController("walk_right",GetIsRight());
                    flip = 1;
                }
            }

            if (Input.GetKeyDown("right") && nowCollisionStatus != CollisionStatus.right_collision)
            {
                AnimationController("walk_right",GetIsRight());
                flip = 1;
            }
            else if (Input.GetKeyUp("right"))
            {
                if (!Input.GetKey("left"))
                {
                    AnimationController("idle",GetIsRight());
                    flip = 0;
                }
                else
                {
                    AnimationController("walk_left",GetIsRight());
                    flip = -1;
                }
            }

            if(Input.GetKeyDown("up"))
            {
                AnimationController("reverse_side", GetIsRight());
                flip = 0;
            }
            if (Input.GetKeyDown("down"))
            {
                AnimationController("front_side", GetIsRight());
                flip = 0;
            }
            transform.Translate(flip * Vector2.right * speed * _v * Time.deltaTime);
            if (IsHoldingCandle)
                MoveCandle(candlePosX);

            return;
        }
        if (IsHoldingCandle)
            MoveCandle(candlePosX);
    }
	
	
	public void AnimationController(string command, bool newFlipX){
        arma.armature.flipX = newFlipX;
		switch (command)
        {
            case "idle":
                if (nowDisplayStatus != DisplayStatus.左右)
                {
                    CharacSpriteController.ChangeBackToDragonBone();
                    nowDisplayStatus = DisplayStatus.左右;
                }
                if (arma.animationName == "stand")
                    break;
                if (!IsHoldingCandle)
                {
                    CandleLight.gameObject.SetActive(false);
                    arma.animation.FadeIn("stand", 0.08f);
                }
                else
                {
                    CandleLight.gameObject.SetActive(true);
                    if (arma.armature.flipX)
                    {
                        candlePosX = 0.5f;
                        arma.animation.FadeIn("stand_with_candle_right", 0.03f);
                    }
                    else
                    {
                        candlePosX = -0.5f;
                        arma.animation.FadeIn("stand_with_candle_left", 0.03f);
                    }
                }
                arma.animationName = "stand";
                break;
            case "walk_right":
                if (nowDisplayStatus != DisplayStatus.左右)
                {
                    CharacSpriteController.ChangeBackToDragonBone();
                    nowDisplayStatus = DisplayStatus.左右;
                }
                arma.armature.flipX = true;
                if (!IsHoldingCandle)
                {
                    CandleLight.gameObject.SetActive(false);
                    arma.animation.FadeIn("walk_right", 0.1f);
                }
                else
                {
                    CandleLight.gameObject.SetActive(true);
                    candlePosX = 0.5f;
                    arma.animation.FadeIn("walk_with_candle_right", -1);
                }
                arma.animationName = "walk";
                break;
            case "walk_left":
                if (nowDisplayStatus != DisplayStatus.左右)
                {
                    CharacSpriteController.ChangeBackToDragonBone();
                    nowDisplayStatus = DisplayStatus.左右;
                }
                arma.armature.flipX = false;
                if (!IsHoldingCandle)
                {
                    CandleLight.gameObject.SetActive(false);
                    arma.animation.FadeIn("walk_left", 0.1f);
                }
                else
                {
                    CandleLight.gameObject.SetActive(true);
                    candlePosX = -0.5f;
                    arma.animation.FadeIn("walk_with_candle_left", -1);
                }
                arma.animationName = "walk";
                break;
            case "backward":
                if (nowDisplayStatus != DisplayStatus.左右)
                {
                    CharacSpriteController.ChangeBackToDragonBone();
                    nowDisplayStatus = DisplayStatus.左右;
                }
                CandleLight.gameObject.SetActive(false);
                arma.animation.FadeIn("backward", 0.08f, 1);
                arma.animationName = "backward";
                break;
            case "front_side":
                if (nowDisplayStatus != DisplayStatus.正)
                {
                    if (!IsHoldingCandle)
                    {
                        CandleLight.gameObject.SetActive(false);
                        CharacSpriteController.ChangeSprite(0);
                    }
                    else
                    {
                        CandleLight.gameObject.SetActive(true);
                        candlePosX = -0.1f;
                        CharacSpriteController.ChangeSprite(1);
                    }
                    nowDisplayStatus = DisplayStatus.正;
                }
                break;
            case "reverse_side":
                if (nowDisplayStatus != DisplayStatus.背)
                {
                    if (!IsHoldingCandle)
                    {
                        CandleLight.gameObject.SetActive(false);
                        CharacSpriteController.ChangeSprite(2);
                    }
                    else
                    {
                        CandleLight.gameObject.SetActive(true);
                        candlePosX = 0.1f;
                        CharacSpriteController.ChangeSprite(3);
                    }
                    nowDisplayStatus = DisplayStatus.背;
                }
                break;
            case "stand_up":
                if (nowDisplayStatus != DisplayStatus.左右)
                {
                    CharacSpriteController.ChangeBackToDragonBone();
                    nowDisplayStatus = DisplayStatus.左右;
                }
                CandleLight.gameObject.SetActive(false);
                arma.animation.FadeIn("stand_up", 0.08f, 1);
                arma.animationName = "stand_up";
                break;
            default:
                Debug.Log("動作錯誤");
                break;
        }
	}
	
    public void SetIsPlayerCanControl(bool b)
    {
        isPlayerCanControl = b;
        if (!b)
        {
            //AnimationController("idle",GetIsRight());
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

    private void MoveCandle(float targetPosX)
    {
        float _speed = 10;
        float step = _speed * Time.deltaTime;
        CandleLight.transform.localPosition = new Vector3(Mathf.Lerp(CandleLight.transform.localPosition.x, targetPosX, step), 2.2f, -5.46f);//插值算法也可以
    }
}
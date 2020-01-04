using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DragonBones;

public class BackwardAction : MonoBehaviour {

    //移動設定

    int running = 0;

    public bool IsFinished()
    {
        if (running == 0)
            return true;
        else
            return false;
    }
    public void Move()      //要移動就call這個funct
    {
		float duration = 0.5f;
		
		running++;
        var chara = FindObjectOfType<GameStateManager>().Player;
        if (chara)
            StartCoroutine(MoveTo(chara, duration));
        else
        {
            StartCoroutine(Wait(duration));
            Debug.LogWarning("Warning: 人物移動 \"" + chara.name + "\" is not found!");
        }
    }
    IEnumerator MoveTo(GameObject chara, float duration)
    {
        float ori = chara.transform.position.x;
        float length = 0.5f;//change this***************************
        chara.transform.position = new Vector3(ori, chara.transform.position.y);
        var armature = chara.GetComponentInChildren<UnityArmatureComponent>();
        float time = 0, speed = length / duration;
        if (chara.GetComponent<PlayerController>() != null) {
            var player = chara.GetComponent<PlayerController>();

            if (player.GetIsRight()) {
                length = -length;
                speed = -speed;
            }
                
            player.AnimationController("backward");

        }
        while (time < duration)
        {
            time += Time.fixedDeltaTime;

            chara.transform.Translate(Vector2.right * speed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        if (chara.GetComponent<PlayerController>() != null)
            chara.GetComponent<PlayerController>().AnimationController("idle");
        else
        {
            armature.animation.FadeIn("stand", 0.15f);
            armature.animationName = "stand";
        }
        chara.transform.position = new Vector3(ori + length, chara.transform.position.y);
        running--;
    }
    IEnumerator Wait(float duration)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        running--;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DragonBones;

public class ActionController : MonoBehaviour {

    //移動設定

    int running = 0;

    public bool IsFinished()
    {
        if (running == 0)
            return true;
        else
            return false;
    }
    public void Move(string charaName, float finX, float duration)      //要移動就call這個funct
    {
        running++;
        var chara = GameObject.Find(charaName);
        if (chara)
            StartCoroutine(MoveTo(GameObject.Find(charaName), finX, duration));
        else
        {
            StartCoroutine(Wait(duration));
            if (charaName.Trim() != "")
                Debug.LogWarning("Warning: 人物移動 \"" + charaName + "\" is not found!");
        }
    }
    IEnumerator MoveTo(GameObject chara, float fin, float duration)
    {
        float ori = chara.transform.position.x;
        chara.transform.position = new Vector3(ori, chara.transform.position.y);
        var armature = chara.GetComponent<UnityArmatureComponent>();
        float time = 0, speed = (fin - ori) / duration;
        if (chara.GetComponent<PlayerController>() != null) {
            var player = chara.GetComponent<PlayerController>();
            if (speed < 0)
                player.AnimationController("continuing_left");
            else
                player.AnimationController("continuing_right");
        }
        else
        {
            if (speed < 0)
                armature.armature.flipX = false;
            else
                armature.armature.flipX = true;
            armature.animation.FadeIn("walk", 0.15f);
            armature.animationName = "walk";
        }
        while (time < duration)
        {
            time += Time.fixedDeltaTime;

            chara.transform.Translate(Vector2.right * speed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        if (chara.GetComponent<PlayerController>() != null)
            chara.GetComponent<PlayerController>().AnimationController("stand_or_stop");
        else
        {
            armature.animation.FadeIn("stand", 0.15f);
            armature.animationName = "stand";
        }
        chara.transform.position = new Vector3(fin, chara.transform.position.y);
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


	
	/*
	public void PlayAudio(GameObject listener, AudioClip audioFile, bool needsWait){
		//the audiofile can put at "Assets/Resources/"
		AudioSource auds;
		if(listener.GetComponent<AudioSource>()){
			auds = listener.GetComponent<AudioSource>();
		}
		else{
			auds = listener.AddComponent<AudioSource>();
		}
		
		auds.clip = audioFile;
		
		if(needsWait){
			auds.Play();
		}
		else{
			auds.PlayOneShot();
		}*/

}

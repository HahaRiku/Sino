using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DragonBones;

public class ActionController : MonoBehaviour {
	private int speed;
	private DragonBones.Animation ani;
	
	public IEnumerator WhatherWaiting_move(bool waiting, GameObject character, Vector3 targetPlace){
		//the char needs its Dragonbone Animation
		if(waiting){
			yield return move(character, targetPlace);
		}
		else{
			StartCoroutine(move(character, targetPlace));
			yield return null;
		}
	}
	
	public IEnumerator move(GameObject character, Vector3 targetPlace){
		//the char needs its Dragonbone Animation
		ani = character.GetComponent<UnityArmatureComponent>().armature.animation;
		ani.Play("walk");
		
		while( Vector3.Distance(character.transform.localPosition, targetPlace) < 0.001f){
			character.transform.localPosition = Vector3.MoveTowards(character.transform.localPosition, targetPlace, speed * Time.deltaTime);
			yield return null;
		}
		ani.Play("stand");
		yield return null;
	}
}

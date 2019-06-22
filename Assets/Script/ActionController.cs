using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DragonBones;

public class ActionController : MonoBehaviour {
	private int speed;
	private DragonBones.Animation ani;
	
	/*
	//移動設定class：
	//使用單一一個動作組List管理
	public class GameMoveManager{
		
		//個別人物移動
		public class AnimationSingle{

			//bool IsFinished;
			GameObject character;
			Vector3 targetPlace;
			bool needsWaiting;
			public AnimationSingle(GameObject character, Vector3 targetPlace, bool needsWaiting){
				this.character = character;
				this.targetPlace = targetPlace;
				this.needsWaiting = needsWaiting;
			}

			public IEnumerator RunMove(){
				//the char needs its Dragonbone Animation
				if(needsWaiting){
					yield return move_Single(character, targetPlace);
				}
				else{
					StartCoroutine(move_Single(character, targetPlace));
					yield return null;
				}
			}

			public IEnumerator move_Single(GameObject character, Vector3 targetPlace){
				//the char needs its Dragonbone Animation
				ani = character.GetComponent<UnityArmatureComponent>().armature.animation;
				ani.Play("walk");
				IsFinished = false;
				while( Vector3.Distance(character.transform.localPosition, targetPlace) < 0.001f){
					character.transform.localPosition = Vector3.MoveTowards(character.transform.localPosition, targetPlace, speed * Time.deltaTime);
					yield return null;
				}
				ani.Play("stand");
				IsFinished = true;
				yield return null;
			}
			
		}
		
		public List<AnimationSingle> AnimationList = new List<AnimationSingle>();	//
		public bool isFinished = false;
		//public AnimationManager(){}
		//---------------
		//設定移動( 同一個動作List的ID, 要移動的腳色, 目標座標, 是否等待動作完成 )
		public SetAnimation(GameObject character, Vector3 targetPlace, bool needsWaiting){//int animationListID
			AnimationList.Add(new AnimationSingle(character, targetPlace, needsWaiting));
		}
		void RunAnim(){
			foreach(Anim_S anis in AnimList){
				anis.RunMove();
			}				
			isFinished = true;
			AnimationList.Clear;
		}
	}
	public GameMoveManager animationManager = new GameMoveManager();
	*/
	
	
	public void PlayAudio(GameObject listener, AudioClip audioFile, bool needsWait){
		//the audiofile can put at "Assets/Resources/"
		if(listener.GetComponent<AudioSource>()){
			AudioSource as = listener.GetComponent<AudioSource>();
		}
		else{
			AudioSource as = listener.AddComponent<AudioSource>();
		}
		
		as.clip = audioFile;
		
		if(needsWait){
			as.Play();
		}
		else{
			as.PlayOneShot();
		}
	}
	
	
}

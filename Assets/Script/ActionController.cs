using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DragonBones;

public class ActionController : MonoBehaviour {
	
	
	//移動設定class：
	//使用單一一個動作組List管理
	
	//使用說明
	//1. 設定所有移動順序：依序呼叫 SetSingleMove(要移動的腳色, 目標座標, 是否等待動作完成, 移動速度)
	//		。SetSingleMove負責把一個動作加入動作表
	//		。需要連續動作時，依序呼叫即可
	//2. 設定完移動列表後，呼叫 RunAllMove()
	//3. 欲檢查動作表是否全部完成，調閱 GameMoveManager.isFinished即可。
	
	public class GameMoveManager{
		
		//---------宣告區---------
		
		//個別人物移動
		public class MoveSingle : MonoBehaviour {

			private DragonBones.Animation ani;
			private int speed;
			//bool IsFinished;
			private GameObject character;
			private Vector3 targetPlace;
			private bool needsWaiting;
			public MoveSingle(GameObject character, Vector3 targetPlace, bool needsWaiting, int speed){
				this.character = character;
				this.targetPlace = targetPlace;
				this.needsWaiting = needsWaiting;
				this.speed = speed;
			}

			public IEnumerator RunMove(){
				//the char needs its Dragonbone Animation
				if(needsWaiting){
					yield return this.move_Single();
				}
				else{
					StartCoroutine(this.move_Single());
					yield return null;
				}
			}

			private IEnumerator move_Single(){
				//the char needs its Dragonbone Animation
				ani = character.GetComponent<UnityArmatureComponent>().armature.animation;
				ani.Play("walk");
				//IsFinished = false;
				while( Vector3.Distance(character.transform.localPosition, targetPlace) < 0.001f){
					character.transform.localPosition = Vector3.MoveTowards(character.transform.localPosition, targetPlace, speed * Time.deltaTime);
					yield return null;
				}
				ani.Play("stand");
				//IsFinished = true;
				yield return null;
			}
			
		}
		public List<MoveSingle> MoveList = new List<MoveSingle>();
		public bool isFinished = false;		//檢查動作表是否全部完成
		//public MoveManager(){}
		
		//---------------函式區---------------
		
		//設定移動(要移動的腳色, 目標座標, 是否等待動作完成, 移動速度)
		//把單獨一個移動指令加到動作列表裡
		public void SetSingleMove(GameObject character, Vector3 targetPlace, bool needsWaiting, int speed){//int MoveListID
			MoveList.Add(new MoveSingle(character, targetPlace, needsWaiting, speed));
		}
		
		//依序執行列表裡的移動指令
		//執行完會把 isFinished 設成 true
		public void RunAllMove(){
			foreach(MoveSingle anis in MoveList){
				anis.RunMove();
			}
			isFinished = true;
			MoveList.Clear();
		}
	}
	public GameMoveManager MoveManager = new GameMoveManager();
	
	
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
		}
	}*/
	
	
}

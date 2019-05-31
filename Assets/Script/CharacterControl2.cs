using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class CharacterControl2 : MonoBehaviour {
    [System.Serializable]
    public struct CharacterObjectAndName {
        public string name;
        public GameObject obj;
    }

    public float speed;
    public Dialog dialogComp;
    public CharacterObjectAndName[] otherCharactersObjects;
    private DragonBones.Animation ani;
    private bool aniWalk;

    void Start() {
        ani = GetComponent<UnityArmatureComponent>().armature.animation;
        aniWalk = false;
    }

    // Update is called once per frame
    void Update () {
        //if(CharWithItem.actEnable){
        if (!SystemVariables.lockMoving) {
            if (!(Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))) {
                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                    aniWalk = true;
                    ani.Play("walk");
                }
                /*if ((Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))) {
                    KeyDown = false;
                    ani.SetBool("Walk", false);

                }*/
                if (Input.GetKey(KeyCode.RightArrow)) {
                    if (!aniWalk) {
                        aniWalk = true;
                        ani.Play("walk");
                    }
                    transform.localPosition = new Vector3(transform.localPosition.x + speed, transform.localPosition.y, transform.localPosition.z);
                    if (transform.localScale.x > 0) {
                        transform.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
                    }
                }
                if (Input.GetKey(KeyCode.LeftArrow)) {
                    if (!aniWalk) {
                        aniWalk = true;
                        ani.Play("walk");
                    }
                    transform.localPosition = new Vector3(transform.localPosition.x - speed, transform.localPosition.y, transform.localPosition.z);
                    if (transform.localScale.x < 0) {
                        transform.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
                    }
                }
                if (!(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))) {
                    aniWalk = false;
                    ani.Play("stand");
                }
            }
            else {
                aniWalk = false;
                ani.Play("stand");
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
        dialogComp.SetLineDone(true);
        //ani.enabled = false;
    }
}

/*
	//private int temp_stats = 0;	//0 = idle, 1 = left, 2=right
	
    void Start() {
        ani= gameObject.GetComponent<Animator>();
        aniWalk = false;
		//StartCoroutine(WalkController ());
    }
	
	private void UpdateWalk (int stats) {
		switch(stats){
			case 0:
				aniWalk = false;
				ani.SetBool("Walk", false);
				break;
				
			case 1:
				if (!aniWalk) {
					aniWalk = true;
					ani.SetBool("Walk", true);
				}
				transform.localPosition = new Vector3(transform.localPosition.x - speed, transform.localPosition.y, transform.localPosition.z);
				if (transform.localScale.x < 0) {
					transform.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
				}
				break;
				
			case 2:
				if (!aniWalk) {
					aniWalk = true;
					ani.SetBool("Walk", true);
				}
				transform.localPosition = new Vector3(transform.localPosition.x + speed, transform.localPosition.y, transform.localPosition.z);
				if (transform.localScale.x > 0) {
					transform.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
				}
				break;
				
			default:	break;
		}
	}
	
	private IEnumerator WalkController () {
        //if(CharWithItem.actEnable){//
        while(true){
			
			switch(temp_stats){
				case 0:
					UpdateWalk (0);
					//Debug.Log("414");
					if (Input.GetKey(KeyCode.RightArrow)) {	//2
						temp_stats = 2;
					}
					if (Input.GetKey(KeyCode.LeftArrow)) {
						temp_stats = 1;
					}
					if (!(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))) {
						temp_stats = 0;
					}
					break;
					
				case 1:
					UpdateWalk (1);
					
					if (Input.GetKey(KeyCode.LeftArrow)) {
						temp_stats = 1;
					}
					if (Input.GetKeyDown(KeyCode.RightArrow)) {	//2
						temp_stats = 2;
					}
					if (!(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))) {
						temp_stats = 0;
					}
					break;
					
				case 2:
					UpdateWalk (2);
					
					if (Input.GetKey(KeyCode.RightArrow)) {	//2
						temp_stats = 2;
					}
					if (Input.GetKeyDown(KeyCode.LeftArrow)) {
						temp_stats = 1;
					}
					if (!(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))) {
						temp_stats = 0;
					}
					break;
					
				default:	break;
			}
			
			yield return 0;
        }
    }*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    public enum State {
        範圍外,
        可以講話,
        對話中,
        不能講話,
        講完話冷卻中

    }

    public enum Type {
        item,
        talk,
        door
    }

    public State state = State.範圍外;
    public Type type;
    public GameObject player;
    public Sprite 放大鏡;
    public Sprite 放大鏡_發光;

    private Actor actor;
    private SpriteRenderer 放大鏡SP;
    private bool toggleWithPlayer= false;

	// Use this for initialization
	void Start () {
        actor = GameObject.Find("Actor").GetComponent<Actor>();
        放大鏡SP = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        player = actor.userCharacter;
	}
	
	// Update is called once per frame
	void Update () {
        if (!toggleWithPlayer) {
            if (Mathf.Abs(player.transform.localPosition.x - gameObject.transform.localPosition.x) <= gameObject.transform.localScale.x/2) {
                toggleWithPlayer = true;
            }
        }
        else {
            if (Mathf.Abs(player.transform.localPosition.x - gameObject.transform.localPosition.x) >= gameObject.transform.localScale.x / 2) {
                toggleWithPlayer = false;
            }
        }

        if (state == State.範圍外 && toggleWithPlayer) {
            state = State.可以講話;
            if (type != Type.talk) {
                放大鏡SP.sprite = 放大鏡_發光;
            }
            else {

            }
        }
        else if (state == State.可以講話 && Input.GetKeyDown(KeyCode.Z)) {
            state = State.對話中;
            SystemVariables.lockMoving = true;
            //dialog
        }
        else if (state != State.範圍外 && !toggleWithPlayer) {
            state = State.範圍外;
            if (type != Type.talk) {
                放大鏡SP.sprite = 放大鏡;
            }
            else {

            }
        }
        

	}
}

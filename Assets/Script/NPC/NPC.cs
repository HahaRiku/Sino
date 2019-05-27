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
        NPC,
        door
    }

    public State state = State.範圍外;
    public GameObject player;

    private Actor actor;
    private bool toggleWithPlayer= false;

	// Use this for initialization
	void Start () {
        actor = GameObject.Find("Actor").GetComponent<Actor>();
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
        }
        else if (state == State.可以講話 && Input.GetKeyDown(KeyCode.Z)) {
            state = State.對話中;
            SystemVariables.lockMoving = true;
        }


	}
}

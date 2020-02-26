using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTurnArround : MonoBehaviour {
    public NPCTrigger npcTrigger;

	// Use this for initialization
	void Start () {
        npcTrigger = GetComponent<NPCTrigger>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TurnArround() {
        if(npcTrigger!=null) {
            npcTrigger.NPC面向右邊 = !npcTrigger.NPC面向右邊;
        }
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}

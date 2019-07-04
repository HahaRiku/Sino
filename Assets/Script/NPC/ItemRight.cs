using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRight : MonoBehaviour {

    private NPC NPC;

    void Start() {
        NPC = gameObject.GetComponentInParent<NPC>();
    }
    /*
    void OnTriggerEnter2D(Collider2D collider) {
        NPC.ItemRightTrigger(true);
    }

    void OnTriggerExit2D(Collider2D collider) {
        NPC.ItemRightTrigger(false);
    }*/

}

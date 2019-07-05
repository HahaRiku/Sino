using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CG : MonoBehaviour {
    public Sprite cg;

    public void ShowCG() {
        gameObject.transform.parent.gameObject.SetActive(true);
        gameObject.transform.parent.gameObject.GetComponent<SpriteRenderer>().sprite = cg;
    }

    public void CloseCG() {
        gameObject.transform.parent.gameObject.SetActive(false);
    }

}

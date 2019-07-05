using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CG : MonoBehaviour {
    public Sprite cg;

    public void ShowCG() {
        gameObject.transform.parent.gameObject.SetActive(true);
        gameObject.transform.parent.gameObject.GetComponent<Image>().sprite = cg;
        gameObject.transform.parent.gameObject.GetComponent<Animator>().SetTrigger("FadeIn");
    }

    public void CloseCG() {
        gameObject.transform.parent.gameObject.GetComponent<Animator>().SetTrigger("FadeOut");
    }

}

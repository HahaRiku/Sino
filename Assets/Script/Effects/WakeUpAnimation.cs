using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WakeUpAnimation : MonoBehaviour {

    public float maxValue = 1.2f;
    public float minValue = 0;
    public float gapBetweenFrame = 0.008f;

    public bool start = false;

    private Image blurBase;

	// Use this for initialization
	void Start () {
        blurBase = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        if (start) {
            start = false;
            StartCoroutine(BlurAnimation());
        }
	}

    IEnumerator BlurAnimation() {
        for (float i = maxValue; i >= minValue; i -= gapBetweenFrame) {
            blurBase.material.SetFloat("_Size", i);
            yield return null;
        }
        blurBase.material.SetFloat("_Size", 0);
    }

    public void StartWakeUpAnimation() {
        start = true;
    }
}

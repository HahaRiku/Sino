using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishGM : MonoBehaviour {
    public bool start = false;

    private Image PanelImg;
    private RectTransform BarRT;
    private Animator OpenAni;
    private RectTransform Area;
    private GameObject Bowls;
    private Text Time;

    private bool duringTheGame;

	// Use this for initialization
	void Start () {
        PanelImg = transform.GetChild(0).GetComponent<Image>();
        transform.GetChild(0).GetChild(0).GetComponent<CanvasGroup>().alpha = 0;
        BarRT = transform.GetChild(0).GetChild(1).GetComponent<RectTransform>();
        BarRT.sizeDelta = new Vector2(0, BarRT.sizeDelta.y);
        RectTransform temp = transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<RectTransform>();
        temp.anchoredPosition = new Vector2(temp.anchoredPosition.x, -140);
        Area = BarRT.transform.GetChild(0).GetComponent<RectTransform>();
        Area.transform.parent.gameObject.SetActive(false);
        Bowls.SetActive(false);
        Time.transform.parent.gameObject.SetActive(false);

        OpenAni = transform.GetChild(0).GetComponent<Animator>();

        PanelImg.material.SetFloat("_Size", 0);
        duringTheGame = false;
	}
	
	// Update is called once per frame
	void Update () {
        if(start) {
            start = false;
            StartGame();
        }

		if(duringTheGame) {

        }
	}

    public void StartGame() {
        StartCoroutine(Blur());
        StartCoroutine(WaitStartAniDone());
        OpenAni.SetTrigger("FishOpen");
    }

    IEnumerator Blur () {
        for(float i = 0.0f; i < 4.0f; i += 4.0f/30.0f) {
            PanelImg.material.SetFloat("_Size", i);
            yield return null;
        }
    }

    IEnumerator WaitStartAniDone() {
        yield return new WaitForSeconds(1.0f);
        Area.transform.parent.gameObject.SetActive(true);

        duringTheGame = true;
    }
}

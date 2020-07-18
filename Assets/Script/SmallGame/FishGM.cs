using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishGM : MonoBehaviour {
    public bool start = false;
    public int 墨魚數量;
    public float 每隻墨魚有的墨汁;
    public int 需要的碗數;
    public float 每個碗的容量;
    public float 每秒範圍外減少的量;
    public float 每秒範圍內減少的量;
    public Sprite OriginalFish;
    public Sprite LeftPushFish;
    public Sprite RightPushFish;
    public GameObject bowlPrefab;

    [Tooltip("從1開始放")]
    public Sprite[] Numbers;

    private Image PanelImg;
    private Image FishImg;
    private RectTransform BarRT;
    private Animator OpenAni;
    private RectTransform Range;
    private RectTransform Pointer;
    private Transform Bowls;
    private Image Number;

    private bool duringTheGame;
    
    private enum KeyDownStatus {
        rightKeyDownNow, 
        leftKeyDownNow,
        nothing
    }

    private KeyDownStatus keyDownStatus;

	// Use this for initialization
	void Start () {
        PanelImg = transform.GetChild(0).GetComponent<Image>();
        FishImg = PanelImg.transform.GetChild(0).GetComponent<Image>();
        FishImg.GetComponent<CanvasGroup>().alpha = 0;
        BarRT = PanelImg.transform.GetChild(1).GetComponent<RectTransform>();
        BarRT.sizeDelta = new Vector2(0, BarRT.sizeDelta.y);
        Range = BarRT.transform.GetChild(0).GetComponent<RectTransform>();
        Range.gameObject.SetActive(false);
        Pointer = BarRT.transform.GetChild(1).GetComponent<RectTransform>();
        Bowls = PanelImg.transform.GetChild(2);
        Number = PanelImg.transform.GetChild(3).GetComponent<Image>();
        Number.gameObject.SetActive(false);

        OpenAni = transform.GetChild(0).GetComponent<Animator>();

        PanelImg.material.SetFloat("_Size", 0);
        duringTheGame = false;
        keyDownStatus = KeyDownStatus.nothing;

        StartGame();
	}
	
	// Update is called once per frame
	void Update () {
		if(duringTheGame) {
            if(keyDownStatus == KeyDownStatus.nothing && Input.GetKeyDown(KeyCode.RightArrow)) {
                FishImg.sprite = RightPushFish;
                keyDownStatus = KeyDownStatus.rightKeyDownNow;

            }
            if(keyDownStatus == KeyDownStatus.nothing && Input.GetKeyDown(KeyCode.LeftArrow)) {
                FishImg.sprite = LeftPushFish;
                keyDownStatus = KeyDownStatus.leftKeyDownNow;

            }
            if((keyDownStatus == KeyDownStatus.rightKeyDownNow && Input.GetKeyUp(KeyCode.RightArrow)) || 
                (keyDownStatus == KeyDownStatus.leftKeyDownNow && Input.GetKeyUp(KeyCode.LeftArrow))) {
                FishImg.sprite = OriginalFish;
                keyDownStatus = KeyDownStatus.nothing;

            }
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
        yield return new WaitForSeconds(1.2f);
        Number.gameObject.SetActive(true);
        for(int i = Numbers.Length - 1; i >= 0; i--) {
            Number.sprite = Numbers[i];
            yield return new WaitForSeconds(1.0f);
        }

        Number.gameObject.SetActive(false);
        duringTheGame = true;
    }

    private bool IsPointerInRange() {

    }

}

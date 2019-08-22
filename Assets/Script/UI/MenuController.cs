using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*********************
 * 
 * 掛在MenuCanvas上
 * 
 * *******************/

public class MenuController : MonoBehaviour {
    public Sprite 繼續遊戲;
    public Sprite 繼續遊戲_選取;
    public Sprite 讀取檔案;
    public Sprite 讀取檔案_選取;
    public Sprite 返回標題;
    public Sprite 返回標題_選取;
    public Sprite 離開遊戲;
    public Sprite 離開遊戲_選取;
    public Sprite 返回標題Q;
    public Sprite 離開遊戲Q;
    public Sprite 要;
    public Sprite 要_選取;
    public Sprite 不要;
    public Sprite 不要_選取;

    private enum State {
        繼續遊戲,
        讀取檔案,
        返回標題,
        離開遊戲
    }
    private State state;
    private enum QuesType {
        none,
        返回標題,
        離開遊戲
    }
    private QuesType quesType;
    private bool questionState = true; //true:左(要), false:右(不要)
    private CanvasGroup blackBG;
    private GameObject Main;
    private CanvasGroup question;
    private Image 繼續遊戲img;
    private Image 讀取檔案img;
    private Image 返回標題img;
    private Image 離開遊戲img;
    private Image QuestionImg;
    private Image 要img;
    private Image 不要img;
    private bool open = false;
    private bool MenuAniDone = true;

	// Use this for initialization
	void Start () {
        blackBG = transform.GetChild(0).GetComponent<CanvasGroup>();
        Main = transform.GetChild(1).gameObject;
        question = Main.transform.GetChild(6).GetComponent<CanvasGroup>();
        繼續遊戲img = Main.transform.GetChild(2).GetComponent<Image>();
        讀取檔案img = Main.transform.GetChild(3).GetComponent<Image>();
        返回標題img = Main.transform.GetChild(4).GetComponent<Image>();
        離開遊戲img = Main.transform.GetChild(5).GetComponent<Image>();
        QuestionImg = Main.transform.GetChild(6).GetChild(0).GetComponent<Image>();
        要img = Main.transform.GetChild(6).GetChild(1).GetComponent<Image>();
        不要img = Main.transform.GetChild(6).GetChild(2).GetComponent<Image>();
        state = State.繼續遊戲;
        繼續遊戲img.sprite = 繼續遊戲_選取;
        讀取檔案img.sprite = 讀取檔案;
        返回標題img.sprite = 返回標題;
        離開遊戲img.sprite = 離開遊戲;
        要img.sprite = 要_選取;
        不要img.sprite = 不要;
        open = false;
        Main.transform.localPosition = new Vector2(0, 660);
        blackBG.alpha = 0;
        question.alpha = 0;
        MenuAniDone = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (MenuAniDone) {
            if (!open) {
                if (!SystemVariables.lockMenu && Input.GetKeyDown(KeyCode.Escape)) {
                    state = State.繼續遊戲;
                    繼續遊戲img.sprite = 繼續遊戲_選取;
                    讀取檔案img.sprite = 讀取檔案;
                    返回標題img.sprite = 返回標題;
                    離開遊戲img.sprite = 離開遊戲;
                    quesType = QuesType.none;
                    FindObjectOfType<GameStateManager>().OpenMenu();
                    MenuAniDone = false;
                    StartCoroutine(MenuOpenAni());
                }
            }
            else if (quesType != QuesType.none) {
                if (Input.GetKeyDown(KeyCode.X)) {
                    quesType = QuesType.none;
                    MenuAniDone = false;
                    StartCoroutine(QuestionClose());
                }
                else if (questionState) {
                    if (Input.GetKeyDown(KeyCode.RightArrow)) {
                        questionState = false;
                        要img.sprite = 要;
                        不要img.sprite = 不要_選取;
                    }
                    else if (Input.GetKeyDown(KeyCode.Z)) {
                        //need to auto save
                        if (quesType == QuesType.返回標題) {
                            SceneManager.LoadScene("title");
                        }
                        else if (quesType == QuesType.離開遊戲) {
                            Application.Quit();
                        }
                    }
                }
                else {
                    if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                        questionState = true;
                        要img.sprite = 要_選取;
                        不要img.sprite = 不要;
                    }
                    else if (Input.GetKeyDown(KeyCode.Z)) {
                        quesType = QuesType.none;
                        MenuAniDone = false;
                        StartCoroutine(QuestionClose());
                    }
                }
            }
            else {
                if (Input.GetKeyDown(KeyCode.X)) {
                    FindObjectOfType<GameStateManager>().CloseMenu();
                    MenuAniDone = false;
                    StartCoroutine(MenuCloseAni());
                }
                else if (Input.GetKeyDown(KeyCode.Z)) {
                    if (state == State.繼續遊戲) {
                        FindObjectOfType<GameStateManager>().CloseMenu();
                        MenuAniDone = false;
                        StartCoroutine(MenuCloseAni());
                    }
                    else if (state == State.讀取檔案) {

                    }
                    else if (state == State.返回標題) {
                        questionState = true;
                        要img.sprite = 要_選取;
                        不要img.sprite = 不要;
                        quesType = QuesType.返回標題;
                        QuestionImg.sprite = 返回標題Q;
                        MenuAniDone = false;
                        StartCoroutine(QuestionAppear());
                    }
                    else if (state == State.離開遊戲) {
                        questionState = true;
                        要img.sprite = 要_選取;
                        不要img.sprite = 不要;
                        quesType = QuesType.離開遊戲;
                        QuestionImg.sprite = 離開遊戲Q;
                        MenuAniDone = false;
                        StartCoroutine(QuestionAppear());
                    }
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow)) {
                    if (state == State.繼續遊戲) {
                        state = State.離開遊戲;
                        繼續遊戲img.sprite = 繼續遊戲;
                        離開遊戲img.sprite = 離開遊戲_選取;
                    }
                    else if (state == State.讀取檔案) {
                        state = State.繼續遊戲;
                        讀取檔案img.sprite = 讀取檔案;
                        繼續遊戲img.sprite = 繼續遊戲_選取;
                    }
                    else if (state == State.返回標題) {
                        state = State.讀取檔案;
                        返回標題img.sprite = 返回標題;
                        讀取檔案img.sprite = 讀取檔案_選取;
                    }
                    else if (state == State.離開遊戲) {
                        state = State.返回標題;
                        離開遊戲img.sprite = 離開遊戲;
                        返回標題img.sprite = 返回標題_選取;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow)) {
                    if (state == State.繼續遊戲) {
                        state = State.讀取檔案;
                        繼續遊戲img.sprite = 繼續遊戲;
                        讀取檔案img.sprite = 讀取檔案_選取;
                    }
                    else if (state == State.讀取檔案) {
                        state = State.返回標題;
                        讀取檔案img.sprite = 讀取檔案;
                        返回標題img.sprite = 返回標題_選取;
                    }
                    else if (state == State.返回標題) {
                        state = State.離開遊戲;
                        返回標題img.sprite = 返回標題;
                        離開遊戲img.sprite = 離開遊戲_選取;
                    }
                    else if (state == State.離開遊戲) {
                        state = State.繼續遊戲;
                        離開遊戲img.sprite = 離開遊戲;
                        繼續遊戲img.sprite = 繼續遊戲_選取;
                    }
                }
            }
        }
	}

    IEnumerator MenuOpenAni() {
        for (float i = 0; i < 1; i += 0.05f) {
            blackBG.alpha = i;
            yield return null;
        }
        for (float i = 660; i > 0; i -= 10) {
            Main.transform.localPosition = new Vector2(0, i);
            yield return null;
        }
        open = true;
        MenuAniDone = true;
    }

    IEnumerator MenuCloseAni() {
        for (float i = 0; i < 660; i += 10) {
            Main.transform.localPosition = new Vector2(0, i);
            yield return null;
        }
        for (float i = 1; i > 0; i -= 0.05f) {
            blackBG.alpha = i;
            yield return null;
        }
        open = false;
        MenuAniDone = true;
    }

    IEnumerator QuestionAppear() {
        for (float i = 0; i < 1; i += 0.05f) {
            question.alpha = i;
            yield return null;
        }
        MenuAniDone = true;
    }

    IEnumerator QuestionClose() {
        for (float i = 1; i > 0; i -= 0.05f) {
            question.alpha = i;
            yield return null;
        }
        MenuAniDone = true;
    }
}

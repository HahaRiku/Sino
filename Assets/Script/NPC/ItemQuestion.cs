using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/************************************************
 * 
 * 請把此腳本掛在 要把desc或ques掛上去的Canvas上
 * 再Call function
 * 
 * 問題預設選項是右邊的
 * 
 * **********************************************/

public class ItemQuestion : MonoBehaviour {

    public enum QuesState {
        左,
        右
    }

    public GameObject descPref;
    public GameObject quesPref;
    public QuesState quesState = QuesState.右;

    private GameObject descInScene = null;
    private GameObject quesInScene = null;
    private Text DescriptionD;
    private Text QuestionQ;
    private Text QuestionA1;
    private Text QuestionA2;
    private bool detectDescZ = false;
    private bool detectQues = false;
    private bool isDescDone = true;
    private bool isQuesDone = true;
    private string ques右邊要call的function;
    private string ques左邊要call的function;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (detectDescZ) {
            if (Input.GetKeyDown(KeyCode.Z)) {
                descInScene.SetActive(false);
                isDescDone = true;
                detectDescZ = false;
                descInScene.SetActive(false);
            }
        }

        if (detectQues) {
            if (Input.GetKeyDown(KeyCode.Z)) {
                
                if (quesState == QuesState.右) {     //我先做撿起來的 其他再說(躺 call其他各處的function好麻煩 但還沒有背包系統 我也不能幹嘛(躺
                    //nothing
                }
                else if (quesState == QuesState.左) {
                    //背包新增物品
                }
                print("123");
                isQuesDone = true;
                detectQues = false;
                quesInScene.SetActive(false);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                if (quesState == QuesState.左) {
                    quesState = QuesState.右;
                    //記得新增UI變化
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                if (quesState == QuesState.右) {
                    quesState = QuesState.左;
                    //記得新增UI變化
                }
            }
        }
	}

    public void SetDescription(string d) {
        if (descInScene == null) {
            descInScene = Instantiate(descPref, this.transform);
            DescriptionD = descInScene.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
            descInScene.SetActive(false);
        }

        DescriptionD.text = d;

    }

    public void SetQuestion(string q, string a1, string a2) {
        if (quesInScene == null) {
            quesInScene = Instantiate(quesPref, this.transform);
            QuestionQ = quesInScene.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
            QuestionA1 = quesInScene.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>();
            QuestionA2 = quesInScene.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Text>();
            quesInScene.SetActive(false);
        }

        QuestionQ.text = q;
        QuestionA1.text = a1;
        QuestionA2.text = a2;

    }

    public void DescWork() {
        if (descInScene == null) {
            Debug.Log("請先Call SetDescription(string d)");
        }
        else {
            descInScene.SetActive(true);
            detectDescZ = true;
            isDescDone = false;
        }
    }

    public void QuestionWork() {
        if (quesInScene == null) {
            Debug.Log("請先Call SetQuestion(string q, string a1, string a2)");
        }
        else {
            quesInScene.SetActive(true);
            detectQues = true;
            isQuesDone = false;
        }
    }

    public bool IsDescDone() {
        return isDescDone;
    }

    public bool IsQuesDone() {
        return isQuesDone;
    }
}
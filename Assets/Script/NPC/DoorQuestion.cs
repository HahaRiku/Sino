using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/************************************************
 * 
 * 門用的 跟item分開
 * 
 * 請把此腳本掛在 要把desc或ques掛上去的Canvas上
 * 再Call function
 * (物件部分已經做好Prefab 拉ItemPrefab 再做設定即可)
 * 
 * 
 * 問題預設選項是右邊的(否)
 * 
 * **********************************************/

public class DoorQuestion : MonoBehaviour {

    public enum QuesState {
        左,
        右
    }

    public GameObject quesPref;
    public QuesState quesState = QuesState.右;

    private GameObject quesInScene = null;
    private Text QuestionQ;
    private Text QuestionA1;
    private Text QuestionA2;
    private bool isQuesDone = true;
    private string ques右邊要call的function;
    private string ques左邊要call的function;
    private GameObject camObj;
    private RectTransform rectTransform;
    private bool detectQues;
    private string transportScene;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        if (detectQues) {
            if (Input.GetKeyDown(KeyCode.Z)) {

                if (quesState == QuesState.右) {
                    //nothing
                }
                else if (quesState == QuesState.左) {
                    SceneManager.LoadScene(transportScene);     //可能call水哥的傳送的腳本(?

                }
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

    public void SetQuestion(string q, string a1, string a2, string ts) {
        if (quesInScene == null) {
            print("123");
            quesInScene = Instantiate(quesPref, this.transform);
            quesInScene.GetComponent<RectTransform>().localPosition = new Vector2(0, 80);
            QuestionQ = quesInScene.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
            QuestionA1 = quesInScene.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>();
            QuestionA2 = quesInScene.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Text>();
            quesInScene.SetActive(false);
        }

        QuestionQ.text = q;
        QuestionA1.text = a1;
        QuestionA2.text = a2;
        transportScene = ts;

    }

    public void QuestionWork() {
        if (quesInScene == null) {
            Debug.Log("請先Call SetQuestion(string q, string a1, string a2)");
        }
        else {
            quesInScene.SetActive(true);
            isQuesDone = false;
            Invoke("StartDetectQuestion", 0.5f);
        }
    }

    public bool IsQuesDone() {
        return isQuesDone;
    }

    private void StartDetectQuestion() {
        detectQues = true;
    }

}
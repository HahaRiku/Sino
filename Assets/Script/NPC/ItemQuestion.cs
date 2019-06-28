using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/************************************************
 * 
 * 請把此腳本掛在 要把desc或ques掛上去的Canvas上
 * 再Call function
 * (物件部分已經做好Prefab 拉ItemPrefab 再做設定即可)
 * 
 * 可以撿：
 * 1. call SetDescription_CanPick
 * 2. call SetQuestion
 * 3. call QuestionWork
 * 
 * 不能撿：
 * 1. call SetDescription_CannotPick
 * 2. call DescWork_CannotPick
 * 
 * 
 * 問題預設選項是右邊的(否)
 * 
 * **********************************************/

public class ItemQuestion : MonoBehaviour {

    public enum QuesState {
        左,
        右
    }

    public GameObject descPref_CannotPick;
    public GameObject descPref_CanPick;
    public GameObject quesPref;
    public QuesState quesState = QuesState.右;

    private GameObject descInScene_CannotPick = null;
    private GameObject descInScene_CanPick = null;
    private GameObject quesInScene = null;
    private Text DescriptionD_CannotPick;
    private Text DescriptionD_CanPick;
    private Text DescriptionN_CanPick;
    private string itemName;
    private Text QuestionQ;
    private Text QuestionA1;
    private Text QuestionA2;
    private bool detectDescZ_CannotPick = false;
    private bool detectQues = false;
    private bool isDescDone_CannotPick = true;
    private bool isQuesDone = true;
    private string ques右邊要call的function;
    private string ques左邊要call的function;
    private GameObject camObj;
    private RectTransform rectTransform;
    private GameObject itemPrefab;
    private BoxCollider2D itemCollider;
    private bool descriptionCannotPickDisable = false;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (detectDescZ_CannotPick) {
            if (descriptionCannotPickDisable) {
                isDescDone_CannotPick = true;
                detectDescZ_CannotPick = false;
                descInScene_CannotPick.SetActive(false);   //need to change
            }
        }

        if (detectQues) {
            if (Input.GetKeyDown(KeyCode.Z)) {
                
                if (quesState == QuesState.右) {     //我先做撿起來的 其他再說(躺 call其他各處的function好麻煩 但還沒有背包系統 我也不能幹嘛(躺
                    //nothing
                }
                else if (quesState == QuesState.左) {
                    //背包新增物品
                    BagSystem.SetItemInBagOrNot(itemName, true);
                }
                isQuesDone = true;
                detectQues = false;
                descInScene_CanPick.SetActive(false);
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

    public void SetDescription_CannotPick(string d) {   //only small text
        if (descInScene_CanPick == null) {
            camObj = Camera.main.gameObject;
            rectTransform = gameObject.GetComponent<RectTransform>();
            itemPrefab = gameObject.transform.parent.gameObject;
            itemCollider = itemPrefab.GetComponent<BoxCollider2D>();
            descInScene_CannotPick = Instantiate(descPref_CannotPick, this.transform);
            GameObject tempPanelObj = descInScene_CannotPick.transform.GetChild(0).gameObject;
            GameObject tempTextObj = tempPanelObj.transform.GetChild(0).gameObject;
            DescriptionD_CannotPick = tempTextObj.GetComponent<Text>();
            tempPanelObj.transform.localPosition = new Vector2((itemPrefab.transform.localPosition.x + (camObj.transform.localPosition.x) * -1) / 9 * rectTransform.sizeDelta.x / 2,
                (itemPrefab.transform.localPosition.y + (camObj.transform.localPosition.y) * -1) / 5 * rectTransform.sizeDelta.y / 2);
            float tempPosition = (itemCollider.size.x / 2) / 9 * rectTransform.sizeDelta.x / 2;
            tempTextObj.transform.localPosition = new Vector2(tempPosition, 0);
            
            descInScene_CannotPick.SetActive(false);
        }

        DescriptionD_CannotPick.text = d;

    }

    public void SetDescription_CanPick(string d, string n) {
        if (descInScene_CanPick == null) {
            descInScene_CanPick = Instantiate(descPref_CanPick, this.transform);
            DescriptionD_CanPick = descInScene_CanPick.transform.GetChild(0).GetChild(2).gameObject.GetComponent<Text>();
            DescriptionN_CanPick = descInScene_CanPick.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Text>();
            descInScene_CanPick.SetActive(false);
        }

        DescriptionD_CanPick.text = d;
        DescriptionN_CanPick.text = n;
        itemName = n;

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

    public void DescWork_CannotPick() {
        if (descInScene_CannotPick == null) {
            Debug.Log("請先Call SetDescription(string d)");
        }
        else {
            descInScene_CannotPick.SetActive(true);
            isDescDone_CannotPick = false;
            Invoke("StartDetectDesc", 0.5f);
        }
    }

    public void QuestionWork() {
        if (quesInScene == null) {
            Debug.Log("請先Call SetQuestion(string q, string a1, string a2)");
        }
        else {
            print("question");
            quesInScene.SetActive(true);
            descInScene_CanPick.SetActive(true);
            isQuesDone = false;
            Invoke("StartDetectQuestion", 0.5f);
        }
    }

    public bool IsDescDone_CannotPick() {
        return isDescDone_CannotPick;
    }

    public bool IsQuesDone() {
        return isQuesDone;
    }

    private void StartDetectDesc() {
        detectDescZ_CannotPick = true;
    }

    private void StartDetectQuestion() {
        detectQues = true;
    }

    public void SetDescCannotPickDisable(bool b) {
        descriptionCannotPickDisable = b;
    }
}
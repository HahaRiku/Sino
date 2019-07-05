using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/************************************************
 * 
 * 功能：
 * 控制撿物品面板左右選項的UI演出，以及後續工作。
 * 問題預設選項是右邊的(否)
 * 
 * **********************************************/
public class PickablePanelController : PanelController
{
    public enum QuesState { 左, 右 }
    public QuesState quesState = QuesState.右;
    public Sprite 選項底_選擇;
    public Sprite 選項底_未選;

    private string itemName;
    private bool isInteract = false;

    private Image 選項L;
    private Image 選項R;

    void Start()
    {
        選項L = transform.GetChild(2).GetComponent<Image>();
        選項R = transform.GetChild(3).GetComponent<Image>();
        transform.GetChild(2).GetChild(0).GetComponent<Text>().fontSize = 20;
        transform.GetChild(3).GetChild(0).GetComponent<Text>().fontSize = 25;
    }
    void Update()
    {
        if (isInteract)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (quesState == QuesState.左)
                {
                    BagSystem.SetItemInBagOrNot(itemName, true);
                }
                SetInvisible();
                isInteract = false;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && quesState == QuesState.左)
            {
                quesState = QuesState.右;
                transform.GetChild(2).GetChild(0).GetComponent<Text>().fontSize = 20;
                選項L.sprite = 選項底_未選;
                transform.GetChild(3).GetChild(0).GetComponent<Text>().fontSize = 25;
                選項R.sprite = 選項底_選擇;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && quesState == QuesState.右)
            {
                quesState = QuesState.左;
                transform.GetChild(2).GetChild(0).GetComponent<Text>().fontSize = 25;
                選項L.sprite = 選項底_選擇;
                transform.GetChild(3).GetChild(0).GetComponent<Text>().fontSize = 20;
                選項R.sprite = 選項底_未選;
            }
        }
    }

    public void ShowQuestion(string input) //輸入變數
    {
        itemName = input;
        //根據變數得知物品以及NPC是誰
        quesState = QuesState.右;
        transform.GetChild(2).GetChild(0).GetComponent<Text>().fontSize = 20;
        transform.GetChild(3).GetChild(0).GetComponent<Text>().fontSize = 25;

        SetVisible();
        Invoke("StartInteract", 0.1f);
    }

    void StartInteract(string input) //根據輸入得知物品
    {
        isInteract = true;
        itemName = input;
        quesState = QuesState.右;
        transform.GetChild(2).GetChild(0).GetComponent<Text>().fontSize = 20;
        transform.GetChild(3).GetChild(0).GetComponent<Text>().fontSize = 25;

        SetVisible();
        Invoke("StartInteract", 0.1f);
    }

    public void SetInfo(string name, string desc, Sprite img = null)
    {
        if (img)
            transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = img;
        transform.GetChild(0).GetChild(1).GetComponent<Text>().text = name;
        transform.GetChild(0).GetChild(2).GetComponent<Text>().text = desc;
    }
    public void SetQuest(string content)
    {
        transform.GetChild(1).GetChild(0).GetComponent<Text>().text = content;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/************************************************
 * 
 * 功能：
 * 控制撿取面板左右選項的UI演出，以及後續工作。
 * 問題預設選項是右邊的(否)
 * 
 * **********************************************/

public class ItemQuestion : MonoBehaviour
{

    PickablePanelController pickablePanel;
    public enum QuesState { 左, 右 }
    public QuesState quesState = QuesState.右;

    string itemName;
    bool isInteract = false;

    void Start()
    {
        pickablePanel = GetComponent<PickablePanelController>();
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
                pickablePanel.SetInvisible();
                isInteract = false;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && quesState == QuesState.左)
            {
                quesState = QuesState.右;
                transform.GetChild(2).GetChild(0).GetComponent<Text>().fontSize = 20;
                transform.GetChild(3).GetChild(0).GetComponent<Text>().fontSize = 25;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && quesState == QuesState.右)
            {
                quesState = QuesState.左;
                transform.GetChild(2).GetChild(0).GetComponent<Text>().fontSize = 25;
                transform.GetChild(3).GetChild(0).GetComponent<Text>().fontSize = 20;
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

        pickablePanel.SetVisible();
        Invoke("StartInteract", 0.1f);
    }

    void StartInteract()
    {
        isInteract = true;
    }
}
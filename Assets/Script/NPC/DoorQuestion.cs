using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/************************************************
 * 
 * 功能：
 * 控制開門面板左右選項的UI演出，以及後續工作。
 * 問題預設選項是右邊的(否)
 * 
 * **********************************************/

public class DoorQuestion : MonoBehaviour {

    OpenDoorPanelController openDoorPanel;
    public enum QuesState { 左, 右 }
    public QuesState quesState = QuesState.右;

    string sceneName;
    bool isInteract = false;

    void Start() {
        openDoorPanel = GetComponent<OpenDoorPanelController>();
        transform.GetChild(1).GetChild(0).GetComponent<Text>().fontSize = 20;
        transform.GetChild(2).GetChild(0).GetComponent<Text>().fontSize = 25;
    }

    void Update() {

        if (isInteract)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (quesState == QuesState.左)
                {
                    SceneManager.LoadScene(sceneName);     //可能call水哥的傳送的腳本(?
                }
                openDoorPanel.SetInvisible();
                isInteract = false;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && quesState == QuesState.左)
            {
                quesState = QuesState.右;
                //記得新增UI變化
                transform.GetChild(1).GetChild(0).GetComponent<Text>().fontSize = 20;
                transform.GetChild(2).GetChild(0).GetComponent<Text>().fontSize = 25;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && quesState == QuesState.右)
            {
                quesState = QuesState.左;
                transform.GetChild(1).GetChild(0).GetComponent<Text>().fontSize = 25;
                transform.GetChild(2).GetChild(0).GetComponent<Text>().fontSize = 20;
                //記得新增UI變化
            }
        }
    }

    public void ShowQuestion(string input)
    {
        sceneName = input;
        //根據變數得知物品以及NPC是誰
        quesState = QuesState.右;
        transform.GetChild(1).GetChild(0).GetComponent<Text>().fontSize = 20;
        transform.GetChild(2).GetChild(0).GetComponent<Text>().fontSize = 25;

        openDoorPanel.SetVisible();
        Invoke("StartInteract", 0.1f);
    }

    void StartInteract()
    {
        isInteract = true;
    }
}
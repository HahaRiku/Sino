using UnityEngine;
using UnityEngine.UI;

/************************************************
 * 
 * 功能：
 * 控制開門面板左右選項的UI演出，以及後續工作。
 * 問題預設選項是右邊的(否)
 * 
 * **********************************************/
public class OpenDoorPanelController : PanelController
{
    public enum QuesState { 左, 右 }
    public QuesState quesState = QuesState.右;
    public Sprite 選項底_選擇;
    public Sprite 選項底_未選;

    private string sceneName;
    private GameStateManager.SpawnPoint newScenePos;
    private bool isInteract = false;
    private Image 選項L;
    private Image 選項R;

    void Start()
    {
        transform.GetChild(1).GetChild(0).GetComponent<Text>().fontSize = 20;
        transform.GetChild(2).GetChild(0).GetComponent<Text>().fontSize = 25;
        選項L = transform.GetChild(1).GetComponent<Image>();
        選項R = transform.GetChild(2).GetComponent<Image>();
    }

    void Update()
    {
        if (isInteract)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (quesState == QuesState.左)
                {

                    FindObjectOfType<GameStateManager>().黑幕轉場(sceneName, newScenePos);
                    //GameStateManager.Instance.黑幕轉場(sceneName, newScenePos);
                }
                isInteract = false;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && quesState == QuesState.左)
            {
                quesState = QuesState.右;
                //記得新增UI變化
                transform.GetChild(1).GetChild(0).GetComponent<Text>().fontSize = 20;
                transform.GetChild(2).GetChild(0).GetComponent<Text>().fontSize = 25;
                選項L.sprite = 選項底_未選;
                選項R.sprite = 選項底_選擇;

            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && quesState == QuesState.右)
            {
                quesState = QuesState.左;
                transform.GetChild(1).GetChild(0).GetComponent<Text>().fontSize = 25;
                transform.GetChild(2).GetChild(0).GetComponent<Text>().fontSize = 20;
                選項R.sprite = 選項底_未選;
                選項L.sprite = 選項底_選擇;
            }
        }
    }

    public void ShowQuestion(string input, GameStateManager.SpawnPoint point)
    {
        sceneName = input;
        newScenePos = point;
        //根據變數得知傳送點
        quesState = QuesState.右;
        transform.GetChild(1).GetChild(0).GetComponent<Text>().fontSize = 20;
        transform.GetChild(2).GetChild(0).GetComponent<Text>().fontSize = 25;

        SetVisible();
        Invoke("StartInteract", 0.1f);
    }

    void StartInteract()
    {
        isInteract = true;
    }
    public void SetQuest(string content)
    {
        transform.GetChild(0).GetChild(0).GetComponent<Text>().text = content;
    }
}

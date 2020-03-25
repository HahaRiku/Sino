using UnityEngine;
using UnityEngine.UI;

public class NPCFunction : MonoBehaviour {
    public FunctionElement[] functionList;

    private int functionElementIndex = 0;

    private NPCTrigger trigger;
    private bool execute = false;
    private bool functionDone = false;

    private PickablePanelController PickablePanel;
    private UnPickablePanelController UnpickablePanel;
    private OpenDoorPanelController OpenDoorPanel;
    private BagUI bagUI;
    private GameStateManager GM;

    void OnEnable() {
        trigger = GetComponent<NPCTrigger>();
    }

    // Use this for initialization
    void Start () {
        PickablePanel = FindObjectOfType<PickablePanelController>();
        UnpickablePanel = FindObjectOfType<UnPickablePanelController>();
        OpenDoorPanel = FindObjectOfType<OpenDoorPanelController>();
        bagUI = FindObjectOfType<BagUI>();
        GM = FindObjectOfType<GameStateManager>();
        functionElementIndex = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (execute) {
            if (functionList[functionElementIndex].type == FunctionElement.FunctionType.不可撿物件) {
                if (UnpickablePanel.IsVisible() && trigger.CheckIsPlayerInRange(trigger.Radius))
                    return;
                UnpickablePanel.SetInvisible();
                string temp = string.Concat(SystemVariables.Scene, "_", gameObject.name);
                SystemVariables.AddIntVariable(temp, 1);
                execute = false;
                functionDone = true;
                GM.FinEvent();
            }
            else if (functionList[functionElementIndex].type == FunctionElement.FunctionType.可撿物件) {
                if(!PickablePanel.IsVisible()) {
                    trigger.撿了物品 = true;
                    if (functionElementIndex < functionList.Length - 1) {
                        if(bagUI.IsGetItemAniDone()) {
                            functionElementIndex++;
                            ExecuteFunctionElement();
                        }
                        else {
                            return;
                        }
                    }
                    else {
                        execute = false;
                        functionDone = true;
                        GM.FinEvent();
                    }
                }
            }
            else if (functionList[functionElementIndex].type == FunctionElement.FunctionType.傳送) {
                if(!OpenDoorPanel.IsVisible()) {
                    if (functionElementIndex < functionList.Length - 1) {
                        functionElementIndex++;
                        ExecuteFunctionElement();
                    }
                    else {
                        execute = false;
                        functionDone = true;
                        GM.FinEvent();
                    }
                }
            }
            else if (functionList[functionElementIndex].type == FunctionElement.FunctionType.故事系統) {
                if(GetComponent<StoryManager>().IsStoryFinish()) {
                    if (functionElementIndex < functionList.Length - 1) {
                        functionElementIndex++;
                        ExecuteFunctionElement();
                    }
                    else {
                        trigger.functionList最後是做故事系統 = true;
                        execute = false;
                        functionDone = true;
                        GM.FinEvent();
                    }
                }
            }
        }
	}

    void 面板定位() {
        var screenPos = Camera.main.WorldToViewportPoint(transform.position);
        var targetRT = UnpickablePanel.transform.GetChild(0).GetComponent<RectTransform>();
        var targetText = UnpickablePanel.transform.GetChild(0).GetChild(2).GetComponent<Text>();
        UnpickablePanel.GetComponent<RectTransform>().anchorMax = screenPos;
        UnpickablePanel.GetComponent<RectTransform>().anchorMin = screenPos;

        float 間距 = 3;
        if (screenPos.x < 0.5) {
            if (screenPos.y > 0.55) {
                //在左上角
                targetRT.pivot = Vector2.up;
                targetRT.anchoredPosition = new Vector2(間距, -間距);
                targetText.alignment = TextAnchor.UpperLeft;
            }
            else {
                //在左下角
                targetRT.pivot = Vector2.zero;
                targetRT.anchoredPosition = new Vector2(間距, 間距);
                targetText.alignment = TextAnchor.LowerLeft;
            }
        }
        else if (screenPos.x >= 0.5) {
            if (screenPos.y > 0.55) {
                //在右上角
                targetRT.pivot = Vector2.one;
                targetRT.anchoredPosition = new Vector2(-間距, -間距);
                targetText.alignment = TextAnchor.UpperRight;
            }
            else {
                //在右下角
                targetRT.pivot = Vector2.right;
                targetRT.anchoredPosition = new Vector2(-間距, 間距);
                targetText.alignment = TextAnchor.LowerRight;
            }
        }
    }

    public void Execute() {
        execute = true;
        functionDone = false;
        functionElementIndex = 0;
        ExecuteFunctionElement();
    }

    void ExecuteFunctionElement() {
        GM.StartEvent();
        if (functionList[functionElementIndex].type == FunctionElement.FunctionType.不可撿物件) {
            GM.FinEvent();
            面板定位();
            UnpickablePanel.SetInfo(functionList[functionElementIndex].不可撿的物品的敘述);
            UnpickablePanel.SetVisible();
        }
        else if (functionList[functionElementIndex].type == FunctionElement.FunctionType.可撿物件) {
            PickablePanel.SetInfo(functionList[functionElementIndex].可撿的物品的名字, BagSystem.ReturnDescByName(functionList[functionElementIndex].可撿的物品的名字), BagSystem.ReturnSpriteByName(functionList[functionElementIndex].可撿的物品的名字));
            PickablePanel.ShowQuestion(functionList[functionElementIndex].可撿的物品的名字);
        }
        else if (functionList[functionElementIndex].type == FunctionElement.FunctionType.傳送) {
            OpenDoorPanel.ShowQuestion(functionList[functionElementIndex].要傳送到的場景名字, functionList[functionElementIndex].傳送地點);
        }
        else if (functionList[functionElementIndex].type == FunctionElement.FunctionType.故事系統) {
            Debug.Log("1");
	    GetComponent<StoryManager>().劇本 = functionList[functionElementIndex].劇本;
	    Debug.Log(GetComponent<StoryManager>().劇本.StateList.Count);
            GetComponent<StoryManager>().BeginStory();
        }
    }

    public bool IsFunctionDone() {
        return functionDone;
    }
}
[System.Serializable]
public class FunctionElement {
    public enum FunctionType { 可撿物件, 不可撿物件, 故事系統, 傳送 };
    public FunctionType type;

    public string 可撿的物品的名字;
    [TextArea] public string 不可撿的物品的敘述;
    public string 要傳送到的場景名字;
    public GameStateManager.SpawnPoint 傳送地點;
    public StoryData 劇本;
}

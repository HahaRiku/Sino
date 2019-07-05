using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    public enum NpcState { 範圍外, 可以講話, 對話中, 不能講話, 講完話冷卻中 }
    public enum NpcType { item, talk, door }
    public enum ItemType { 可撿, 不可撿 }

    public NpcState state = NpcState.範圍外;
    public NpcType type;
    public ItemType itemType;

    public float 冷卻時間 = 2f;
    public string 可撿的物品的名字;
    public string 不可撿的物品的敘述;

    public string 門的名字;
    public string 需要的鑰匙名字;
    public bool 門一開始有沒有鎖;   //true: 有鎖, false: 沒鎖
    public string 門要傳送到的場景名稱;

    public GameObject player;

    public Sprite 一個點;
    public Sprite 兩個點;
    public Sprite 三個點;

    public Sprite 鎖打開;
    public Sprite 鎖鎖起來;

    public ItemQuestion ItemQuestion;
    public DoorQuestion DoorQuestion;

    private Actor actor;
    private Transform 白點TF;
    private SpriteRenderer 白點SP;
    private SpriteRenderer 點點點SP;
    private SpriteRenderer 鎖SP;
    private bool toggleWithPlayer= false;
    private bool itemToggleLeft = false;
    private bool itemToggleRight = false;
    private bool waitItemQuesDone = false;
    private bool doorUnlockingAniDone = true;
    private bool doorCannotOpenAniDone = true;
    private bool stopDotDotDotAni = true;
    private float 白點亮度差距 = 0.8f;

    public float Radius = 1.5f;
    public float HintRaius = 2.0f;
    public float Offset = 0;

    private PickablePanelController PickablePanel;
    private UnPickablePanelController UnpickablePanel;
    private OpenDoorPanelController OpenDoorPanel;

    void Start () {
        actor = GameObject.Find("Actor").GetComponent<Actor>();
        if (type == NpcType.item) {
            白點TF = gameObject.transform.GetChild(0);
            白點TF.localScale = new Vector2(0, 0);
            白點SP = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
            白點SP.color = new Color(白點SP.color.r, 白點SP.color.g, 白點SP.color.b, 0);
            print(白點SP.color.a);
        }
        else if (type == NpcType.talk) {
            點點點SP = gameObject.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
        }
        else if (type == NpcType.door) {
            鎖SP = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        }
        player = actor.userCharacter;
        ItemQuestion = FindObjectOfType<ItemQuestion>();
        DoorQuestion = FindObjectOfType<DoorQuestion>();
        PickablePanel = FindObjectOfType<PickablePanelController>();
        UnpickablePanel = FindObjectOfType<UnPickablePanelController>();
        OpenDoorPanel = FindObjectOfType<OpenDoorPanelController>();
    }

    void Update()
    {
        if (!SystemVariables.lockNPCinteract)
        {
            if (state == NpcState.不能講話)
                return;

            if (state == NpcState.範圍外)
            {
                if (CheckIsPlayerInRange(Radius)) {
                    state = NpcState.可以講話;
                    if (type == NpcType.talk) {
                        stopDotDotDotAni = false;
                        StartCoroutine(DotDotDot());
                    }
                    else if (type == NpcType.item) {
                        float temp = (1.0f / (Radius + HintRaius + 白點亮度差距)) * (Radius - Mathf.Abs(player.transform.position.x - transform.position.x))    //various part
                            + (1.0f / (Radius + HintRaius + 白點亮度差距)) * (HintRaius + 白點亮度差距);    //another triangle part
                        白點TF.localScale = new Vector2(temp * 2.25f, temp * 2.25f);
                        白點SP.color = new Color(白點SP.color.r, 白點SP.color.g, 白點SP.color.b, temp);
                    }
                    else if (type == NpcType.door && SystemVariables.IsDoorStatusExisted(門的名字))
                        鎖SP.sprite = SystemVariables.doorLockOrNot[門的名字] ? 鎖鎖起來 : 鎖打開;
                    else {
                        SystemVariables.AddDoorStatus(門的名字, 門一開始有沒有鎖);
                        鎖SP.sprite = 門一開始有沒有鎖 ? 鎖鎖起來 : 鎖打開;
                    }
                }
                else if (type == NpcType.item) {
                    float temp = (1.0f / (Radius + HintRaius + 白點亮度差距)) * (Radius + HintRaius - Mathf.Abs(player.transform.position.x - transform.position.x));
                    白點TF.localScale = CheckIsPlayerInRange(HintRaius) ? new Vector2(temp * 2.25f, temp * 2.25f) : new Vector2(0, 0);
                    白點SP.color = CheckIsPlayerInRange(HintRaius) ? new Color(白點SP.color.r, 白點SP.color.g, 白點SP.color.b, temp) :  
                        new Color(白點SP.color.r, 白點SP.color.g, 白點SP.color.b, 0);
                }
                else if (type == NpcType.talk)
                    stopDotDotDotAni = true;
                else if (type == NpcType.door)
                    鎖SP.sprite = null;
            }
            else if (state == NpcState.可以講話)
            {
                if (!CheckIsPlayerInRange(Radius))
                    state = NpcState.範圍外;
                else if (Input.GetKeyDown(KeyCode.Z)) {
                    state = NpcState.對話中;
                    SystemVariables.lockBag = true;
                    if (type == NpcType.item && itemType == ItemType.不可撿) {
                        面板定位();
                        UnpickablePanel.SetInfo(不可撿的物品的敘述);
                        UnpickablePanel.SetVisible();
                        return;
                    }
                    SystemVariables.lockMoving = true;
                    if (type == NpcType.item) {
                        PickablePanel.SetInfo(可撿的物品的名字, BagSystem.ReturnDescByName(可撿的物品的名字));
                        ItemQuestion.ShowQuestion(可撿的物品的名字);
                    }
                    else if (type == NpcType.talk) {
                        stopDotDotDotAni = true;
                        GetComponent<StoryManager>().BeginStory();
                    }
                    else if (type == NpcType.door) {
                        if (SystemVariables.doorLockOrNot[門的名字]) {  //lock
                            if (BagSystem.IsItemInBag(需要的鑰匙名字)) {
                                doorUnlockingAniDone = false;
                                //unlocking animation??
                                //I don't know how to do it with only two pictures. If the animation done, the two statements below will be replaced.
                                鎖SP.sprite = 鎖打開;
                                SystemVariables.AddDoorStatus(門的名字, false);
                                doorUnlockingAniDone = true;
                            }
                            else {
                                //左右晃動的動畫
                                doorCannotOpenAniDone = false;
                                StartCoroutine(CannotOpenDoorAni());
                            }
                        }
                        else {  //unlock
                            DoorQuestion.ShowQuestion(門要傳送到的場景名稱);
                        }
                    }
                }
                else {
                    if (type == NpcType.item) {
                        float temp = (1.0f / (Radius + HintRaius + 白點亮度差距)) * (Radius - Mathf.Abs(player.transform.position.x - transform.position.x))    //various part
                                + (1.0f / (Radius + HintRaius + 白點亮度差距)) * (HintRaius + 白點亮度差距);    //another triangle part
                        白點TF.localScale = new Vector2(temp * 2.25f, temp * 2.25f);
                        白點SP.color = new Color(白點SP.color.r, 白點SP.color.g, 白點SP.color.b, temp);
                    }
                }
            }
            else if (state == NpcState.對話中)
            {
                if (type == NpcType.item && itemType == ItemType.可撿 && !PickablePanel.IsVisible())
                {
                    StartCoroutine(WaitAndResumeTalk());
                }
                else if (type == NpcType.item && itemType == ItemType.不可撿)
                {
                    if (UnpickablePanel.IsVisible() && CheckIsPlayerInRange(Radius))
                        return;
                    UnpickablePanel.SetInvisible();
                    StartCoroutine(WaitAndResumeTalk());
                }
                else if (type == NpcType.talk && GetComponent<StoryManager>().IsStoryFinish())
                {
                    StartCoroutine(WaitAndResumeTalk());
                }
                else if (type == NpcType.door && !OpenDoorPanel.IsVisible())
                {
                    StartCoroutine(WaitAndResumeTalk()); //門真的有需要wait嗎?
                }
            }
            else if (state == NpcState.講完話冷卻中)
            {
                if (type == NpcType.item)
                {
                    state = NpcState.範圍外;
                }            
            }
        }
    }
    bool CheckIsPlayerInRange(float radius)
    {
        Vector2 _range = new Vector2(transform.position.x + Offset - radius, transform.position.x + Offset + radius);
        float x = player.transform.position.x;
        //Debug.Log(x+" "+ _range.x+" "+ _range.y);
        if (x < _range.x)
            return false;
        if (x > _range.y)
            return false;
        return true;
    }
    IEnumerator WaitAndResumeTalk()
    {
        SystemVariables.lockBag = false;
        SystemVariables.lockMoving = false;
        state = NpcState.講完話冷卻中;
        if (type == NpcType.talk)
            yield return new WaitForSeconds(冷卻時間);
        else
            yield return new WaitForSeconds(0.1f);
        state = NpcState.可以講話;
    }

    /* * *
     * 定位規則：
     * 1. 不貼或跑出畫面邊緣(太左就會在右角、太高就會在下角)
     * 2. 
     * 
     * * */

    void 面板定位()
    {
        var canvasRT = UnpickablePanel.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        var screenPos = Camera.main.WorldToViewportPoint(transform.position);
        var targetRT = UnpickablePanel.transform.GetChild(0).GetComponent<RectTransform>();
        UnpickablePanel.GetComponent<RectTransform>().anchorMax = screenPos;
        UnpickablePanel.GetComponent<RectTransform>().anchorMin = screenPos;

        float 間距 = 3;

        

        //在右上角
        targetRT.pivot = Vector2.zero;
        targetRT.anchoredPosition = new Vector2(間距, 間距);

        //在右下角
        targetRT.pivot = Vector2.right;
        targetRT.anchoredPosition = new Vector2(間距, -間距);

        //在左上角
        targetRT.pivot = Vector2.up;
        targetRT.anchoredPosition = new Vector2(-間距, 間距);

        //在左下角
        targetRT.pivot = Vector2.one;
        targetRT.anchoredPosition = new Vector2(-間距, -間距);

    }
    private IEnumerator CannotOpenDoorAni()
    {
        for (float i = 0; i >= -0.3f; i -= 0.1f)
        {
            鎖SP.gameObject.transform.localPosition = new Vector2(i, 鎖SP.gameObject.transform.localPosition.y);
            yield return null;
        }
        for (float i = -0.3f; i <= 0.3f; i += 0.1f)
        {
            鎖SP.gameObject.transform.localPosition = new Vector2(i, 鎖SP.gameObject.transform.localPosition.y);
            yield return null;
        }
        for (float i = 0.3f; i >= 0; i -= 0.1f)
        {
            鎖SP.gameObject.transform.localPosition = new Vector2(i, 鎖SP.gameObject.transform.localPosition.y);
            yield return null;
        }
        yield return new WaitForSeconds(0.05f);
        for (float i = 0; i >= -0.3f; i -= 0.1f)
        {
            鎖SP.gameObject.transform.localPosition = new Vector2(i, 鎖SP.gameObject.transform.localPosition.y);
            yield return null;
        }
        for (float i = -0.3f; i <= 0.3f; i += 0.1f)
        {
            鎖SP.gameObject.transform.localPosition = new Vector2(i, 鎖SP.gameObject.transform.localPosition.y);
            yield return null;
        }
        for (float i = 0.3f; i >= 0; i -= 0.1f)
        {
            鎖SP.gameObject.transform.localPosition = new Vector2(i, 鎖SP.gameObject.transform.localPosition.y);
            yield return null;
        }
        doorCannotOpenAniDone = true;
    }

    private IEnumerator DotDotDot() {
        while (true) {
            if (stopDotDotDotAni) {
                break;
            }
            else if (點點點SP.sprite == null) {
                點點點SP.sprite = 一個點;
                yield return new WaitForSeconds(0.5f);      //分散放 可以改時間
            }
            else if (點點點SP.sprite == 一個點) {
                點點點SP.sprite = 兩個點;
                yield return new WaitForSeconds(0.5f);
            }
            else if (點點點SP.sprite == 兩個點) {
                點點點SP.sprite = 三個點;
                yield return new WaitForSeconds(0.5f);
            }
            else if (點點點SP.sprite == 三個點) {
                點點點SP.sprite = 一個點;
                yield return new WaitForSeconds(0.5f);
            }
            
        }
        點點點SP.sprite = null;
    }

    void OnDrawGizmos()
    {
        float cHeight = 12;

        Vector3 pos = new Vector3(transform.position.x+Offset, Camera.main.transform.position.y, 0);
        float region_width = Radius * 2.0f;
        if (type == NpcType.item)
        {
            Gizmos.color = new Color32(0xFF, 0xF3, 0x9D, 0xA0); ;
            Gizmos.DrawCube(pos, new Vector3(region_width, cHeight, 1.0f));

            Gizmos.color = new Color32(0xFF, 0xFF, 0xFF, 0x4D);
            pos = new Vector3(transform.position.x + Offset - HintRaius + (HintRaius - Radius) / 2, Camera.main.transform.position.y, 0);
            Gizmos.DrawCube(pos, new Vector3(HintRaius - Radius, cHeight, 1.0f));
            pos = new Vector3(transform.position.x + Offset + HintRaius - (HintRaius - Radius) / 2, Camera.main.transform.position.y, 0);
            Gizmos.DrawCube(pos, new Vector3(HintRaius - Radius, cHeight, 1.0f));
        }
        else if (type == NpcType.talk)
        {
            Gizmos.color = new Color32(0x00, 0xC8, 0xE5, 0x4A);
            Gizmos.DrawCube(pos, new Vector3(region_width, cHeight, 1.0f)); 
        }
        else if (type == NpcType.door)
        {
            if(門一開始有沒有鎖)
                Gizmos.color = new Color32(0xE3, 0x00, 0x00, 0x4A);
            else
                Gizmos.color = new Color32(0x70, 0xE5, 0x00, 0x4A);
            Gizmos.DrawCube(pos, new Vector3(region_width, cHeight, 1.0f));
        }
    }

}

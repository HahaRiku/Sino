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
    public Sprite 淺白點;
    public Sprite 白點;

    public Sprite 一個點;
    public Sprite 兩個點;
    public Sprite 三個點;

    public Sprite 鎖打開;
    public Sprite 鎖鎖起來;

    public ItemQuestion ItemQuestion;
    public DoorQuestion DoorQuestion;

    private Actor actor;
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

    public float Radius = 1.5f;
    public float HintRaius = 2.0f;
    public float Offset = 0;

    private PickablePanelController PickablePanel;
    private UnPickablePanelController UnpickablePanel;
    
    void Start () {
        actor = GameObject.Find("Actor").GetComponent<Actor>();
        if (type == NpcType.item) {
            白點SP = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
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
    }

    void Update()
    {
        if (!SystemVariables.lockNPCinteract)
        {
            if (state == NpcState.不能講話)
                return;

            if (state == NpcState.範圍外)
            {
                if (CheckIsPlayerInRange(Radius))
                {
                    state = NpcState.可以講話;
                    if (type == NpcType.talk)
                    {
                        //點點點animation play -> maybe ienumerator
                        stopDotDotDotAni = false;
                        StartCoroutine(DotDotDot());
                    }
                    else if (type == NpcType.item)
                    {
                        白點SP.sprite = 白點;
                    }
                    else if (type == NpcType.door)
                    {
                        //access system variables and determine what sprite it is
                        if (SystemVariables.IsDoorStatusExisted(門的名字))
                        {
                            鎖SP.sprite = SystemVariables.doorLockOrNot[門的名字] ? 鎖鎖起來 : 鎖打開;
                        }
                        else
                        {
                            SystemVariables.AddDoorStatus(門的名字, 門一開始有沒有鎖);
                            鎖SP.sprite = 門一開始有沒有鎖 ? 鎖鎖起來 : 鎖打開;
                        }
                    }
                }
                else if (type == NpcType.item)
                {
                    白點SP.sprite = CheckIsPlayerInRange(HintRaius) ? 淺白點 : null;
                }
            }
            else if (state == NpcState.可以講話)
            {
                if (!CheckIsPlayerInRange(Radius))
                    state = NpcState.範圍外;
                else if(Input.GetKeyDown(KeyCode.Z))
                {
                    state = NpcState.對話中;
                    SystemVariables.lockBag = true;
                    if (type == NpcType.item && itemType == ItemType.不可撿)
                    {
                        UnpickablePanel.SetInfo(可撿的物品的名字, BagSystem.ReturnDescByName(可撿的物品的名字));
                        UnpickablePanel.SetVisible();
                        return;
                    }
                    SystemVariables.lockMoving = true;
                    if (type == NpcType.item)
                    {
                        ItemQuestion.ShowQuestion(可撿的物品的名字);
                    }
                    else if (type == NpcType.talk)
                    {
                        stopDotDotDotAni = true;
                        GetComponent<StoryManager>().BeginStory();
                    }
                    else if (type == NpcType.door)
                    {
                        if (SystemVariables.doorLockOrNot[門的名字])
                        {  //lock
                            if (BagSystem.IsItemInBag(需要的鑰匙名字))
                            {
                                doorUnlockingAniDone = false;
                                //unlocking animation??
                                //I don't know how to do it with only two pictures. If the animation done, the two statements below will be replaced.
                                鎖SP.sprite = 鎖打開;
                                SystemVariables.AddDoorStatus(門的名字, false);
                                doorUnlockingAniDone = true;
                            }
                            else
                            {
                                //左右晃動的動畫
                                doorCannotOpenAniDone = false;
                                StartCoroutine(CannotOpenDoorAni());
                            }
                        }
                        else
                        {  //unlock
                            DoorQuestion.ShowQuestion(門要傳送到的場景名稱);
                        }
                    }
                }
            }
            else if (state == NpcState.對話中)
            {
                if (type == NpcType.item && itemType == ItemType.可撿)
                {
                    if (!PickablePanel.IsVisible())
                        StartCoroutine(WaitAndResumeTalk());
                }
                else if (type == NpcType.item &&itemType == ItemType.不可撿)
                {
                    if (!UnpickablePanel.IsVisible())
                        StartCoroutine(WaitAndResumeTalk());
                    else if (!CheckIsPlayerInRange(Radius))
                    {
                        UnpickablePanel.SetInvisible();
                        StartCoroutine(WaitAndResumeTalk());
                    }
                }
                else if (type == NpcType.talk && GetComponent<StoryManager>().IsStoryFinish())
                {
                    StartCoroutine(WaitAndResumeTalk());
                }
                else if (type == NpcType.door)
                {
                    StartCoroutine(WaitAndResumeTalk()); //門真的有需要wait嗎?
                }
            }
        }
    }
    bool CheckIsPlayerInRange(float radius)
    {
        Vector2 _range = new Vector2(transform.position.x + Offset - radius, transform.position.x + Offset + radius);
        float x = player.transform.position.x;
        if (x < _range.x)
            return false;
        if (x > _range.y)
            return false;
        return true;
    }
    IEnumerator WaitAndResumeTalk()
    {
        SystemVariables.lockBag = true;
        SystemVariables.lockMoving = false;
        state = NpcState.講完話冷卻中;
        yield return new WaitForSeconds(冷卻時間);
        state = NpcState.可以講話;
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
        float cHeight = 20;

        Vector3 pos = new Vector3(transform.position.x+Offset, Camera.main.transform.position.y, 0);
        float region_width = Radius * 2.0f;

        Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 0.3f);
        Gizmos.DrawCube(pos, new Vector3(region_width, cHeight, 1.0f));

        Gizmos.color = new Color(0.0f, 1.0f, 1.0f, 0.3f);

        pos = new Vector3(transform.position.x + Offset - HintRaius + (HintRaius - Radius) / 2, Camera.main.transform.position.y, 0);
        Gizmos.DrawCube(pos, new Vector3(HintRaius - Radius, cHeight, 1.0f));

        pos = new Vector3(transform.position.x + Offset + HintRaius - (HintRaius - Radius) / 2, Camera.main.transform.position.y, 0);
        Gizmos.DrawCube(pos, new Vector3(HintRaius - Radius, cHeight, 1.0f));
    }

}

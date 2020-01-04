using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    public enum NpcState { 範圍外, 可以講話, 對話中, 不能講話, 講完話冷卻中 }
    public enum NpcTriggerType { 白點, 點點點, 鎖, 碰觸觸發 }
    public enum NpcDoingType { 可撿物件, 不可撿物件, 故事系統, 傳送 }
    public enum NpcType { item, talk, door }
    public enum ItemType { 可撿, 不可撿 }
    public enum DoorType { 開啟, 關閉 }

    public NpcState state = NpcState.範圍外;
    public NpcTriggerType triggerType;
    public NpcDoingType doingType;
    public NpcType type;
    public ItemType itemType;
    public DoorType doorType;

    public float 冷卻時間 = 2f;
    public string 可撿的物品的名字;
    [TextArea] public string 不可撿的物品的敘述;
    public bool 是否撿完變不可撿 = false;
    public bool 是否不撿完變可撿 = false;

    public bool NPC面向右邊 = true;

    public string 門的名字;
    public string 需要的鑰匙名字;
    public Object 門要傳送到的場景;
	public string 門要傳送到的場景名字;
    public bool 是否有傳送功能 = true;
    public GameStateManager.SpawnPoint 傳送地點;

    private GameObject player;

    public Sprite 鎖打開;
    public Sprite 鎖鎖起來;
    
    /*private Transform 白點TF;
    private SpriteRenderer 白點SP;
    private SpriteRenderer 點點點SP;
    private SpriteRenderer 鎖SP;
    private float 白點亮度差距 = 0.8f;*/
    private Animator ChildAni;

    public float Radius = 1.5f;
    public float HintRaius = 2.0f;
    public float Offset = 0;

    private GameStateManager GM;
    private PickablePanelController PickablePanel;
    private UnPickablePanelController UnpickablePanel;
    private OpenDoorPanelController OpenDoorPanel;

    private DragonBones.Armature playerArma;

    void Start () {
        /*if (type == NpcType.item) {
            白點TF = gameObject.transform.GetChild(0);
            白點TF.localScale = new Vector2(0, 0);
            白點SP = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
            白點SP.color = new Color(白點SP.color.r, 白點SP.color.g, 白點SP.color.b, 0);
        }
        else if (type == NpcType.talk) {
            點點點SP = gameObject.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
            ChildAni = gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
        }
        else if (type == NpcType.door) {
            鎖SP = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        }*/
        GM = FindObjectOfType<GameStateManager>();
        PickablePanel = FindObjectOfType<PickablePanelController>();
        UnpickablePanel = FindObjectOfType<UnPickablePanelController>();
        OpenDoorPanel = FindObjectOfType<OpenDoorPanelController>();
        player = GM.Player;
        playerArma = player.transform.GetChild(0).GetComponent<DragonBones.UnityArmatureComponent>().armature;
        ChildAni = gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
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
                        //dotAni.SetBool("Dot", true);
                        ChildAni.SetBool("play", true);
                    }
                    else if (type == NpcType.item) {
                        /*float temp = (1.0f / (Radius + HintRaius + 白點亮度差距)) * (Radius - Mathf.Abs(player.transform.position.x - transform.position.x))    //various part
                            + (1.0f / (Radius + HintRaius + 白點亮度差距)) * (HintRaius + 白點亮度差距);    //another triangle part
                        白點TF.localScale = new Vector2(temp * 1f, temp * 1f);
                        白點SP.color = new Color(白點SP.color.r, 白點SP.color.g, 白點SP.color.b, temp);*/
                        ChildAni.SetBool("play", true);
                    }
                    else if (type == NpcType.door && SystemVariables.IsLockStatusExisted(門的名字))
                        /*鎖SP.sprite = SystemVariables.lockLockOrNot[門的名字] ? 鎖鎖起來 : 鎖打開;*/
                        ChildAni.SetBool("play", true);
                    else {
                        SystemVariables.AddLockStatus(門的名字, doorType == DoorType.開啟 ? false : true);
                        /*鎖SP.sprite = doorType == DoorType.開啟 ? 鎖打開 : 鎖鎖起來;*/
                        ChildAni.SetBool("play", true);
                    }
                }
                else if (type == NpcType.item) {
                    /*float temp = (1.0f / (Radius + HintRaius + 白點亮度差距)) * (Radius + HintRaius - Mathf.Abs(player.transform.position.x - transform.position.x));
                    白點TF.localScale = CheckIsPlayerInRange(HintRaius) ? new Vector2(temp * 2.25f, temp * 2.25f) : new Vector2(0, 0);
                    白點SP.color = CheckIsPlayerInRange(HintRaius) ? new Color(白點SP.color.r, 白點SP.color.g, 白點SP.color.b, temp) :  
                        new Color(白點SP.color.r, 白點SP.color.g, 白點SP.color.b, 0);*/
                    ChildAni.SetBool("play", false);
                }
                else if (type == NpcType.talk)
                    ChildAni.SetBool("play", false); //dotAni.SetBool("Dot", false);
                else if (type == NpcType.door)
                    ChildAni.SetBool("play", false);  //鎖SP.sprite = null;
            }
            else if (state == NpcState.可以講話)
            {
                if (!CheckIsPlayerInRange(Radius))
                    state = NpcState.範圍外;
                else if (Input.GetKeyDown(KeyCode.Z)) {
                    state = NpcState.對話中;
                    SystemVariables.lockBag = true;
                    //flipX = false -> faceLeft, flipX = true -> faceRight
                    playerArma.flipX = (player.transform.position.x - transform.position.x) >= 0 ? false : true;

                    if (type == NpcType.item && itemType == ItemType.不可撿) {
                        面板定位();
                        UnpickablePanel.SetInfo(不可撿的物品的敘述);
                        UnpickablePanel.SetVisible();
                        return;
                    }
                    GM.StartEvent();
                    if (type == NpcType.item) {
                        PickablePanel.SetInfo(可撿的物品的名字, BagSystem.ReturnDescByName(可撿的物品的名字), BagSystem.ReturnSpriteByName(可撿的物品的名字));
                        PickablePanel.ShowQuestion(可撿的物品的名字);
                    }
                    else if (type == NpcType.talk) {
                        ChildAni.SetBool("play", false); //dotAni.SetBool("Dot", false);
                        //席諾面向
                        if ((player.transform.position.x - transform.position.x) >= 0) {
                            if (!NPC面向右邊) {
                                NPC面向右邊 = true;
                                transform.localScale = new Vector2(-1 * transform.localScale.x, transform.localScale.y);
                            }
                        }
                        else {
                            if (NPC面向右邊) {
                                NPC面向右邊 = false;
                                transform.localScale = new Vector2(-1 * transform.localScale.x, transform.localScale.y);
                            }
                        }
                        GetComponent<StoryManager>().BeginStory();
                    }
                    else if (type == NpcType.door) {
                        if (SystemVariables.lockLockOrNot[門的名字]) {  //lock
                            if (BagSystem.IsItemInBag(需要的鑰匙名字)) {
                                ChildAni.SetBool("play", false); //鎖SP.sprite = 鎖打開;
                                SystemVariables.AddLockStatus(門的名字, false);                
                                doorType = DoorType.開啟;
                            }
                            else  //左右晃動的動畫                            
                                ChildAni.SetTrigger("play"); // StartCoroutine(CannotOpenDoorAni());
                        }
                        else if(是否有傳送功能) //unlock
                            OpenDoorPanel.ShowQuestion(門要傳送到的場景名字, 傳送地點);
                    }
                }
                //else {
                //    if (type == NpcType.item) {
                //        float temp = (1.0f / (Radius + HintRaius + 白點亮度差距)) * (Radius - Mathf.Abs(player.transform.position.x - transform.position.x))    //various part
                //                + (1.0f / (Radius + HintRaius + 白點亮度差距)) * (HintRaius + 白點亮度差距);    //another triangle part
                //        白點TF.localScale = new Vector2(temp * 1f, temp * 1f); ///原先是乘2.25
                //        白點SP.color = new Color(白點SP.color.r, 白點SP.color.g, 白點SP.color.b, temp);
                //    }
                //}
            }
            else if (state == NpcState.對話中)
            {
                if (type == NpcType.item && itemType == ItemType.可撿 && !PickablePanel.IsVisible())
                {
                    StartCoroutine(WaitAndResumeTalk());
                    if (是否撿完變不可撿)
                        itemType = ItemType.不可撿;
                }
                else if (type == NpcType.item && itemType == ItemType.不可撿)
                {
                    if (UnpickablePanel.IsVisible() && CheckIsPlayerInRange(Radius))
                        return;
                    UnpickablePanel.SetInvisible();
                    string temp = string.Concat(SystemVariables.Scene, "_", gameObject.name);
                    SystemVariables.AddIntVariable(temp, 1);
                    StartCoroutine(WaitAndResumeTalk());
                    if (是否不撿完變可撿)
                        itemType = ItemType.可撿;
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
        if (x < _range.x)
            return false;
        if (x > _range.y)
            return false;
        return true;
    }
    IEnumerator WaitAndResumeTalk()
    {
        SystemVariables.lockBag = false;
        if (type != NpcType.item || (type == NpcType.item && itemType == ItemType.不可撿)) {
            GM.FinEvent();
        }
        state = NpcState.講完話冷卻中;
        
        if (type == NpcType.talk)
            yield return new WaitForSeconds(冷卻時間);
        else
            yield return new WaitForSeconds(0.1f);
        if (type == NpcType.door && doorType == DoorType.開啟) {
            string temp = string.Concat(SystemVariables.Scene, "_", gameObject.name);
            SystemVariables.AddIntVariable(temp, 1);
        }
        state = NpcState.可以講話;
    }

    void 面板定位()
    {
        var screenPos = Camera.main.WorldToViewportPoint(transform.position);
        var targetRT = UnpickablePanel.transform.GetChild(0).GetComponent<RectTransform>();
        UnpickablePanel.GetComponent<RectTransform>().anchorMax = screenPos;
        UnpickablePanel.GetComponent<RectTransform>().anchorMin = screenPos;

        float 間距 = 3;
        if(screenPos.x<0.5)
        {
            if(screenPos.y>0.55)
            {
                //在右下角
                targetRT.pivot = Vector2.right;
                targetRT.anchoredPosition = new Vector2(間距, -間距);
            }
            else
            {
                //在右上角
                targetRT.pivot = Vector2.zero;
                targetRT.anchoredPosition = new Vector2(間距, 間距);
            }
        }
        else if(screenPos.x >= 0.5)
        {
            if (screenPos.y > 0.55)
            {
                //在左下角
                targetRT.pivot = Vector2.one;
                targetRT.anchoredPosition = new Vector2(-間距, -間距);
            }
            else
            {
                //在左上角
                targetRT.pivot = Vector2.up;
                targetRT.anchoredPosition = new Vector2(-間距, 間距);
            }
        }
    }
    /*private IEnumerator CannotOpenDoorAni()
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
    }*/

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
            if(doorType == DoorType.關閉)
                Gizmos.color = new Color32(0xE3, 0x00, 0x00, 0x4A);
            else
                Gizmos.color = new Color32(0x70, 0xE5, 0x00, 0x4A);
            Gizmos.DrawCube(pos, new Vector3(region_width, cHeight, 1.0f));
        }
    }

    public void SetThisLockLock(bool l) {
        SystemVariables.AddLockStatus(門的名字, l);
        if(l) {
            doorType = DoorType.關閉;
        }
        else {
            doorType = DoorType.開啟;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/****************************
 * 
 * 白點
 * 點點點
 * 鎖
 * 碰觸
 * 
 * ***************************/

public class NPCTrigger : MonoBehaviour {

    public enum TriggerType { 白點, 點點點, 鎖, 碰觸 }
    public enum NpcState { 範圍外, 可以講話, 對話中, 不能講話, 講完話冷卻中 }
    public enum LockStatus { 沒鎖, 有鎖 }

    public TriggerType type;
    public NpcState state = NpcState.範圍外;
    public LockStatus lockStatus;

    public float 冷卻時間 = 2f;

    public string 鎖的名字;
    public string 需要的鑰匙名字;

    public Sprite 鎖打開;
    public Sprite 鎖鎖起來;

    public float Radius = 1.5f;
    public float HintRaius = 2.0f;
    public float Offset = 0;

    private Transform 白點TF;
    private SpriteRenderer 白點SP;
    private SpriteRenderer 點點點SP;
    private SpriteRenderer 鎖SP;
    private float 白點亮度差距 = 0.8f;
    private Animator dotAni;

    private NPCFunction function;
    private GameStateManager GM;
    private GameObject player;

    private DragonBones.Armature playerArma;

    private bool doThingsOnLock = false;


    void OnEnable() {
        function = GetComponent<NPCFunction>();
    }

    // Use this for initialization
    void Start() {
        if (type == TriggerType.白點) {
            白點TF = gameObject.transform.GetChild(0);
            白點TF.localScale = new Vector2(0, 0);
            白點SP = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
            白點SP.color = new Color(白點SP.color.r, 白點SP.color.g, 白點SP.color.b, 0);
        }
        else if (type == TriggerType.點點點) {
            點點點SP = gameObject.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
            dotAni = gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
        }
        else if (type == TriggerType.鎖) {
            鎖SP = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
            if (SystemVariables.IsLockStatusExisted(鎖的名字)) {
                lockStatus = SystemVariables.lockLockOrNot[鎖的名字] ? LockStatus.有鎖 : LockStatus.沒鎖;
            }
        }
        GM = FindObjectOfType<GameStateManager>();
        player = GM.Player;
        function = GetComponent<NPCFunction>();
        playerArma = player.transform.GetChild(0).GetComponent<DragonBones.UnityArmatureComponent>().armature;

    }

    // Update is called once per frame
    void Update() {
        if (!SystemVariables.lockNPCinteract) {
            if (state == NpcState.不能講話) {
                return;
            }
            if (state == NpcState.範圍外) {
                if (CheckIsPlayerInRange(Radius)) {
                    if (type == TriggerType.碰觸) {
                        state = NpcState.對話中;
                    }
                    else {
                        state = NpcState.可以講話;
                        if (type == TriggerType.點點點) {
                            dotAni.SetBool("Dot", true);
                        }
                        else if (type == TriggerType.白點) {
                            float temp = (1.0f / (Radius + HintRaius + 白點亮度差距)) * (Radius - Mathf.Abs(player.transform.position.x - transform.position.x))    //various part
                                + (1.0f / (Radius + HintRaius + 白點亮度差距)) * (HintRaius + 白點亮度差距);    //another triangle part
                            白點TF.localScale = new Vector2(temp * 2.25f, temp * 2.25f);
                            白點SP.color = new Color(白點SP.color.r, 白點SP.color.g, 白點SP.color.b, temp);
                        }
                        else if (type == TriggerType.鎖 && SystemVariables.IsLockStatusExisted(鎖的名字))
                            鎖SP.sprite = SystemVariables.lockLockOrNot[鎖的名字] ? 鎖鎖起來 : 鎖打開;
                        else {
                            SystemVariables.AddLockStatus(鎖的名字, (lockStatus == LockStatus.沒鎖) ? false : true);
                            鎖SP.sprite = (lockStatus == LockStatus.沒鎖) ? 鎖打開 : 鎖鎖起來;
                        }
                    }
                }
                else if (type == TriggerType.白點) {
                    float temp = (1.0f / (Radius + HintRaius + 白點亮度差距)) * (Radius + HintRaius - Mathf.Abs(player.transform.position.x - transform.position.x));
                    白點TF.localScale = CheckIsPlayerInRange(HintRaius) ? new Vector2(temp * 2.25f, temp * 2.25f) : new Vector2(0, 0);
                    白點SP.color = CheckIsPlayerInRange(HintRaius) ? new Color(白點SP.color.r, 白點SP.color.g, 白點SP.color.b, temp) :
                        new Color(白點SP.color.r, 白點SP.color.g, 白點SP.color.b, 0);
                }
                else if (type == TriggerType.點點點)
                    dotAni.SetBool("Dot", false);
                else if (type == TriggerType.鎖)
                    鎖SP.sprite = null;
            }
            else if (state == NpcState.可以講話) {
                if (!CheckIsPlayerInRange(Radius))
                    state = NpcState.範圍外;
                else if (type == TriggerType.碰觸) {
                    state = NpcState.對話中;
                    SystemVariables.lockBag = true;
                    playerArma.flipX = (player.transform.position.x - transform.position.x) >= 0 ? false : true;
                    function.Execute();
                }
                else if (Input.GetKeyDown(KeyCode.Z)) {
                    state = NpcState.對話中;
                    SystemVariables.lockBag = true;
                    //flipX = false -> faceLeft, flipX = true -> faceRight
                    playerArma.flipX = (player.transform.position.x - transform.position.x) >= 0 ? false : true;

                    if (type == TriggerType.鎖 && SystemVariables.lockLockOrNot[鎖的名字]) {
                        if (BagSystem.IsItemInBag(需要的鑰匙名字)) {
                            鎖SP.sprite = 鎖打開;
                            SystemVariables.AddLockStatus(鎖的名字, false);
                            lockStatus = LockStatus.沒鎖;
                        }
                        else  //左右晃動的動畫
                            StartCoroutine(CannotOpenDoorAni());
                        doThingsOnLock = true;
                    }
                    else {
                        function.Execute();
                    }
                }
                else {
                    if (type == TriggerType.白點) {
                        float temp = (1.0f / (Radius + HintRaius + 白點亮度差距)) * (Radius - Mathf.Abs(player.transform.position.x - transform.position.x))    //various part
                                + (1.0f / (Radius + HintRaius + 白點亮度差距)) * (HintRaius + 白點亮度差距);    //another triangle part
                        白點TF.localScale = new Vector2(temp * 2.25f, temp * 2.25f);
                        白點SP.color = new Color(白點SP.color.r, 白點SP.color.g, 白點SP.color.b, temp);
                    }
                }
            }
            else if (state == NpcState.對話中) {
                if (doThingsOnLock || function.IsFunctionDone()) {
                    doThingsOnLock = false;
                    state = NpcState.講完話冷卻中;
                    StartCoroutine(WaitAndResumeTalk());
                }
            }
        }
    }

    bool CheckIsPlayerInRange(float radius) {
        Vector2 _range = new Vector2(transform.position.x + Offset - radius, transform.position.x + Offset + radius);
        float x = player.transform.position.x;
        if (x < _range.x)
            return false;
        if (x > _range.y)
            return false;
        return true;
    }

    private IEnumerator CannotOpenDoorAni() {
        for (float i = 0; i >= -0.3f; i -= 0.1f) {
            鎖SP.gameObject.transform.localPosition = new Vector2(i, 鎖SP.gameObject.transform.localPosition.y);
            yield return null;
        }
        for (float i = -0.3f; i <= 0.3f; i += 0.1f) {
            鎖SP.gameObject.transform.localPosition = new Vector2(i, 鎖SP.gameObject.transform.localPosition.y);
            yield return null;
        }
        for (float i = 0.3f; i >= 0; i -= 0.1f) {
            鎖SP.gameObject.transform.localPosition = new Vector2(i, 鎖SP.gameObject.transform.localPosition.y);
            yield return null;
        }
        yield return new WaitForSeconds(0.05f);
        for (float i = 0; i >= -0.3f; i -= 0.1f) {
            鎖SP.gameObject.transform.localPosition = new Vector2(i, 鎖SP.gameObject.transform.localPosition.y);
            yield return null;
        }
        for (float i = -0.3f; i <= 0.3f; i += 0.1f) {
            鎖SP.gameObject.transform.localPosition = new Vector2(i, 鎖SP.gameObject.transform.localPosition.y);
            yield return null;
        }
        for (float i = 0.3f; i >= 0; i -= 0.1f) {
            鎖SP.gameObject.transform.localPosition = new Vector2(i, 鎖SP.gameObject.transform.localPosition.y);
            yield return null;
        }
    }

    IEnumerator WaitAndResumeTalk() {
        SystemVariables.lockBag = false;
        /*if (type != NpcType.item || (type == NpcType.item && itemType == ItemType.不可撿)) {
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
        state = NpcState.可以講話;*/
        yield return new WaitForSeconds(冷卻時間);
        state = NpcState.可以講話;
    }
}
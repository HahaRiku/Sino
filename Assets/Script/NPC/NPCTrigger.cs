using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/****************************
 * 
 * 白點
 * 點點點
 * 鎖
 * 碰觸
 * 
 * ***************************/

public class NPCTrigger : MonoBehaviour {

    
    public enum TriggerType { 白點, 點點點, 鎖, 碰觸, 耳朵 }
    public enum NpcState { 範圍外, 可以講話, 對話中, 不能講話, 講完話冷卻中 }
    public enum LockStatus { 沒鎖, 有鎖 }
    public enum WhenNPCEnd { 換到其他NPC, 關掉, 不改變}

    public List<NPC運作條件Element> NPC運作條件List = new List<NPC運作條件Element>();
    private bool conditionTrue = true;

    public TriggerType type;
    public NpcState state = NpcState.範圍外;
    public LockStatus lockStatus;

    public bool NPC面向右邊 = true;
    public bool NPC對話後是否要面向席諾 = true;

    public float 冷卻時間 = 0.1f;

    public string 鎖的名字;
    public string 需要的鑰匙名字;

    public float Radius = 1.5f;
    public float Offset = 0;

    public WhenNPCEnd 門鎖解鎖後;
    public WhenNPCEnd 撿取了物件後;
    public WhenNPCEnd 故事系統後;
    public GameObject 切換後的NPC;

    private NPCFunction function;
    private GameStateManager GM;
    private GameObject player;
    private Animator animator;

    private DragonBones.Armature playerArma;

    //private bool doThingsOnLock = false;
    public bool 撿了物品 { get; set; }
    public bool functionList最後是做故事系統 { get; set; }

    void Start() {
        function = GetComponent<NPCFunction>();
        if(type != TriggerType.碰觸) {
            animator = transform.GetChild(0).GetComponent<Animator>();
            if (animator == null)
                animator = gameObject.AddComponent<Animator>();
        }
        GM = FindObjectOfType<GameStateManager>();
        player = GM.Player;
        playerArma = player.transform.GetChild(0).GetComponent<DragonBones.UnityArmatureComponent>().armature;
        functionList最後是做故事系統 = false;
        撿了物品 = false;
        if (type == TriggerType.鎖)
        {
            if (string.IsNullOrEmpty(鎖的名字))
            {
                Debug.LogError("錯誤：鎖的名字不得為空！");
                enabled = false;
            }
            if (SystemVariables.IsLockStatusExisted(鎖的名字))
                animator.SetBool("locked", SystemVariables.lockLockOrNot[鎖的名字] ? true : false);
            else
            {
                bool b = lockStatus == LockStatus.有鎖 ? true : false;
                SystemVariables.AddLockStatus(鎖的名字, b);
                animator.SetBool("locked", b);
            }
        }
    }

    void Update() {
        conditionTrue = true;
        foreach (NPC運作條件Element npce in NPC運作條件List) {
            if (npce.NPC運作條件 == NPC運作條件Element.NPCCondition.變數) {
                if (!SystemVariables.IsIntVariableExisted(npce.條件變數名稱)) {
                    conditionTrue = false;
                    break;
                }
                else {
                    if (SystemVariables.otherVariables_int[npce.條件變數名稱] != npce.條件變數值) {
                        conditionTrue = false;
                        break;
                    }
                }
            }
            else if (npce.NPC運作條件 == NPC運作條件Element.NPCCondition.背包物件存在與否) {
                if (BagSystem.IsItemInBag(npce.條件物件名稱) != npce.條件物件存在與否) {
                    conditionTrue = false;
                    break;
                }
            }
        }

        if (!SystemVariables.lockNPCinteract) {
            if (state == NpcState.不能講話) {
                return;
            }
            if (state == NpcState.範圍外) {
                if (conditionTrue && !SystemVariables.lockOtherNPC) {
                    if (CheckIsPlayerInRange(Radius)) {
                        state = NpcState.可以講話;
                        if(type != TriggerType.碰觸)
                            animator.SetBool("play", true);
                    }
                    else if (type != TriggerType.碰觸)
                        animator.SetBool("play", false);
                }
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
                    if (type == TriggerType.鎖 && SystemVariables.lockLockOrNot[鎖的名字]) {
                        if (BagSystem.IsItemInBag(需要的鑰匙名字)) {
                            animator.SetTrigger("unlock");
                            animator.SetBool("locked", false);
                            SystemVariables.AddLockStatus(鎖的名字, false);
                        }
                        else  //左右晃動的動畫
                            animator.SetTrigger("locking");
                        return;
                    }
                    state = NpcState.對話中;
                    SystemVariables.lockBag = true;
                    animator.SetBool("play", false);
                    GM.StartEvent();
                    //flipX = false -> faceLeft, flipX = true -> faceRight
                    playerArma.flipX = (player.transform.position.x - transform.position.x) >= 0 ? false : true;

                    if (NPC對話後是否要面向席諾) {
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
                    }
                    function.Execute();
                }
            }
            else if (state == NpcState.對話中) {
                if (function.IsFunctionDone()) {
                    GM.FinEvent();
                    state = NpcState.講完話冷卻中;
                    StartCoroutine(WaitAndResumeTalk());
                }
            }
        }
    }

    public bool CheckIsPlayerInRange(float radius) {
        Vector2 _range = new Vector2(transform.position.x + Offset - radius, transform.position.x + Offset + radius);
        float x = player.transform.position.x;
        if (x < _range.x)
            return false;
        if (x > _range.y)
            return false;
        return true;
    }

    IEnumerator WaitAndResumeTalk() {
        SystemVariables.lockBag = false;
        if (!animator.GetBool("locked")) {
            if (門鎖解鎖後 == WhenNPCEnd.換到其他NPC) {
                gameObject.SetActive(false);
                切換後的NPC.SetActive(true);
            }
            else if (門鎖解鎖後 == WhenNPCEnd.關掉) {
                gameObject.SetActive(false);
            }
        }
        else if (撿了物品) {
            if (撿取了物件後 == WhenNPCEnd.換到其他NPC) {
                gameObject.SetActive(false);
                切換後的NPC.SetActive(true);
            }
            else if (撿取了物件後 == WhenNPCEnd.關掉) {
                gameObject.SetActive(false);
            }
        }
        else if(functionList最後是做故事系統) {
            if(故事系統後 == WhenNPCEnd.換到其他NPC) {
                gameObject.SetActive(false);
                切換後的NPC.SetActive(true);
            }
            else if(故事系統後 == WhenNPCEnd.關掉) {
                gameObject.SetActive(false);
            }
        }
        yield return new WaitForSeconds(冷卻時間);
        
        state = NpcState.範圍外;
    }
    void OnDrawGizmos()
    {
        float cHeight = 12;

        Vector3 pos = new Vector3(transform.position.x + Offset, Camera.main.transform.position.y, 0);
        float region_width = Radius * 2.0f;

        Gizmos.color = new Color32(0x00, 0xC8, 0xE5, 0x4A);
        Gizmos.DrawCube(pos, new Vector3(region_width, cHeight, 1.0f));
    }
}

[System.Serializable]
public class NPC運作條件Element {
    public enum NPCCondition { 無, 變數, 背包物件存在與否 }
    public NPCCondition NPC運作條件;
    public string 條件變數名稱;
    public int 條件變數值;
    public string 條件物件名稱;
    public bool 條件物件存在與否;
}
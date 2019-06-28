using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    public enum State {
        範圍外,
        快接近_僅限item,
        可以講話,
        對話中,
        不能講話,
        講完話冷卻中

    }

    public enum Type {
        item,
        talk,
        door
    }

    public enum ItemType {
        可撿,
        不可撿
    }

    public State state = State.範圍外;
    public Type type;
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
    

    // Use this for initialization
    void Start () {
        actor = GameObject.Find("Actor").GetComponent<Actor>();
        if (type == Type.item) {
            白點SP = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        }
        else if (type == Type.talk) {
            點點點SP = gameObject.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
        }
        else if (type == Type.door) {
            鎖SP = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        }
        player = actor.userCharacter;

        if (type == Type.item) {
            if (itemType == ItemType.可撿) {
                ItemQuestion.SetDescription_CanPick(BagSystem.ReturnDescByName(可撿的物品的名字), 可撿的物品的名字);
                ItemQuestion.SetQuestion("是否要拾取？", "是", "否");
            }
            else if (itemType == ItemType.不可撿) {
                ItemQuestion.SetDescription_CannotPick(不可撿的物品的敘述);
            }
        }
        else if (type == Type.door) {
            DoorQuestion.SetQuestion("要打開門嗎", "好啊", "不要" ,門要傳送到的場景名稱);
        }
        
    }

    // Update is called once per frame
    void Update() {

        if (state == State.範圍外 && (itemToggleLeft || itemToggleRight) && type == Type.item) {
            state = State.快接近_僅限item;
            白點SP.sprite = 淺白點;
        }
        else if ((state == State.範圍外 || state == State.快接近_僅限item) && toggleWithPlayer) {
            state = State.可以講話;
            if (type == Type.talk) {
                //點點點animation play -> maybe ienumerator

                stopDotDotDotAni = false;
                StartCoroutine(DotDotDot());

            }
            else if (type == Type.item) {
                白點SP.sprite = 白點;
            }
            else if (type == Type.door) {
                //access system variables and determine what sprite it is
                if (SystemVariables.IsDoorStatusExisted(門的名字)) {
                    if (SystemVariables.doorLockOrNot[門的名字]) {
                        鎖SP.sprite = 鎖鎖起來;
                    }
                    else {
                        鎖SP.sprite = 鎖打開;
                    }
                }
                else {
                    SystemVariables.AddDoorStatus(門的名字, 門一開始有沒有鎖);
                    if (門一開始有沒有鎖) {
                        鎖SP.sprite = 鎖鎖起來;
                    }
                    else {
                        鎖SP.sprite = 鎖打開;
                    }
                }

            }
        }
        else if (state == State.可以講話 && !SystemVariables.lockNPCinteract && Input.GetKeyDown(KeyCode.Z)) {
            state = State.對話中;
            SystemVariables.lockBag = true;


            if (type == Type.door) {
                SystemVariables.lockMoving = true;
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
                    DoorQuestion.QuestionWork();
                }
            }
            else if (type == Type.item) {
                //if it can be got, description and question
                //if it can not, only description
                if (itemType == ItemType.可撿) {
                    SystemVariables.lockMoving = true;
                    ItemQuestion.QuestionWork();
                }
                else if (itemType == ItemType.不可撿) {
                    ItemQuestion.DescWork_CannotPick();
                }
            }
            else if (type == Type.talk) {
                SystemVariables.lockMoving = true;

                //點點點animation stop
                stopDotDotDotAni = true;

                //dialog
                print("call dialog");
            }
        }
        else if (state != State.快接近_僅限item && state != State.範圍外 && type == Type.item && (itemToggleLeft || itemToggleRight) && !toggleWithPlayer) {
            if (itemType == ItemType.不可撿) {
                ItemQuestion.SetDescCannotPickDisable(true);
                //state = State.講完話冷卻中;
                Invoke("ResumeTalk", 冷卻時間);
            }
            state = State.快接近_僅限item;
            白點SP.sprite = 淺白點;
        }
        else if (state != State.範圍外 && type == Type.item && !itemToggleLeft && !itemToggleRight && !toggleWithPlayer) {
            if (itemType == ItemType.不可撿) {
                ItemQuestion.SetDescCannotPickDisable(true);
                //state = State.講完話冷卻中;
                Invoke("ResumeTalk", 冷卻時間);
            }
            state = State.範圍外;
            白點SP.sprite = null;
        }
        else if (state != State.範圍外 && !toggleWithPlayer && type != Type.item) {
            state = State.範圍外;
            if (type == Type.talk) {

                //點點點animation stop
                stopDotDotDotAni = true;
            }
            else if (type == Type.door) {
                鎖SP.sprite = null;
            }
        }
        else if (state == State.對話中) {
            if (type == Type.item && itemType == ItemType.可撿) {
                if (ItemQuestion.IsQuesDone()) {
                    state = State.講完話冷卻中;
                    SystemVariables.lockMoving = false;
                    Invoke("ResumeTalk", 冷卻時間);
                }
            }
            else if (type == Type.item && itemType == ItemType.不可撿) {
                if (!toggleWithPlayer) {
                    ItemQuestion.SetDescCannotPickDisable(true);
                    state = State.講完話冷卻中;
                    Invoke("ResumeTalk", 冷卻時間);
                }
            }
            else if (type == Type.talk/* && detectDialogDone*/) {
                state = State.講完話冷卻中;
                SystemVariables.lockMoving = false;
                Invoke("ResumeTalk", 冷卻時間);
            }
            else if (type == Type.door && doorUnlockingAniDone && doorCannotOpenAniDone && DoorQuestion.IsQuesDone()) {
                state = State.講完話冷卻中;
                SystemVariables.lockMoving = false;
                Invoke("ResumeTalk", 冷卻時間);
            }
            SystemVariables.lockBag = false;
        }
        

	}

    void ResumeTalk() {
        if (type == Type.item && itemType == ItemType.不可撿) {
            ItemQuestion.SetDescCannotPickDisable(false);
        }
        state = State.可以講話;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        toggleWithPlayer = true;
    }

    void OnTriggerExit2D(Collider2D collider) {
        toggleWithPlayer = false;
    }

    public void ItemLeftTrigger(bool enter) {
        itemToggleLeft = enter;
    }

    public void ItemRightTrigger(bool enter) {
        itemToggleRight = enter;
    }

    /*private IEnumerator UnlockingAni() {

    }*/

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
}

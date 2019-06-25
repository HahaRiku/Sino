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

    public enum DoorState {
        打開的,
        關起來的
    }

    public State state = State.範圍外;
    public Type type;
    public ItemType itemType;
    public float 冷卻時間 = 2f;
    public string 不可撿的物品的敘述;
    public GameObject player;
    public Sprite 淺白點;
    public Sprite 白點;
    public Sprite 一個點;
    public Sprite 兩個點;
    public Sprite 三個點;
    public Sprite 鎖打開;
    public Sprite 鎖鎖起來;
    public ItemQuestion ItemQuestion;

    private Actor actor;
    private SpriteRenderer 白點SP;
    private SpriteRenderer 點點點SP;
    private SpriteRenderer 鎖SP;
    private bool toggleWithPlayer= false;
    private bool itemToggleLeft = false;
    private bool itemToggleRight = false;
    private bool waitItemQuesDone = false;
    

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
                ItemQuestion.SetDescription_CanPick("這個到時候應該是從背包系統抓ㄅ");
                ItemQuestion.SetQuestion("是否要拾取？", "是", "否");
            }
            else if (itemType == ItemType.不可撿) {
                ItemQuestion.SetDescription_CannotPick(不可撿的物品的敘述);
            }
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
                //點點點animation play
            }
            else if (type == Type.item) {
                白點SP.sprite = 白點;
            }
            else if (type == Type.door) {
                //access bag system and determine what sprite it is
            }
        }
        else if (state == State.可以講話 && Input.GetKeyDown(KeyCode.Z)) {
            state = State.對話中;
            

            if (type == Type.door) {
                SystemVariables.lockMoving = true;

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
                //dialog
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
            }
            else if (type == Type.door) {

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
            else if (type == Type.talk) {

            }
            else if (type == Type.door/* && detectDialogDone*/) {

            }
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
}

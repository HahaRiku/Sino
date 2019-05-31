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
    public GameObject player;
    public Sprite 淺白點;
    public Sprite 白點;
    public ItemQuestion ItemQuestion;

    private Actor actor;
    private SpriteRenderer 白點SP;
    private bool toggleWithPlayer= false;
    private bool itemToggleLeft = false;
    private bool itemToggleRight = false;
    private bool waitItemQuesDone = false;
    

    // Use this for initialization
    void Start () {
        actor = GameObject.Find("Actor").GetComponent<Actor>();
        白點SP = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        player = actor.userCharacter;

        if (type == Type.item) {
            ItemQuestion.SetDescription("這個到時候應該是從背包系統抓ㄅ");
            if (itemType == ItemType.可撿) {
                ItemQuestion.SetQuestion("是否要拿起？", "是", "否");
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

            }
            else if (type == Type.item) {
                白點SP.sprite = 白點;
            }
            else if (type == Type.door) {

            }
        }
        else if (state == State.可以講話 && Input.GetKeyDown(KeyCode.Z)) {
            state = State.對話中;
            SystemVariables.lockMoving = true;

            if (type == Type.door) {

            }
            else if (type == Type.item) {
                //if it can be got, description and question
                //if it can not, only description
                ItemQuestion.DescWork();
            }
            else if (type == Type.talk) {
                //dialog
            }
        }
        else if (state != State.快接近_僅限item && state != State.範圍外 && type == Type.item && (itemToggleLeft || itemToggleRight) && !toggleWithPlayer) {
            state = State.快接近_僅限item;
            白點SP.sprite = 淺白點;
        }
        else if (state != State.範圍外 && type == Type.item && !itemToggleLeft && !itemToggleRight && !toggleWithPlayer) {
            state = State.範圍外;
            白點SP.sprite = null;
        }
        else if (state != State.範圍外 && !toggleWithPlayer && type != Type.item) {
            state = State.範圍外;
            if (type == Type.talk) {

            }
            else if (type == Type.door) {

            }
        }
        else if (state == State.對話中 && type == Type.item && itemType == ItemType.不可撿 && ItemQuestion.IsDescDone()) {
            state = State.講完話冷卻中;
            SystemVariables.lockMoving = false;
            Invoke("ResumeTalk", 冷卻時間);
        }
        else if (state == State.對話中 && type == Type.item && itemType == ItemType.可撿 && ItemQuestion.IsDescDone() && !waitItemQuesDone) {
            //maintain at State.對話中
            //do 可撿 的question
            ItemQuestion.QuestionWork();
            waitItemQuesDone = true;
        }
        else if (state == State.對話中 && type == Type.item && itemType == ItemType.可撿 && waitItemQuesDone && ItemQuestion.IsQuesDone()) {
            state = State.講完話冷卻中;
            SystemVariables.lockMoving = false;
            Invoke("ResumeTalk", 冷卻時間);
            waitItemQuesDone = false;
        }
        else if (state == State.對話中 && type == Type.door) {

        }
        else if (state == State.對話中 && type == Type.talk) {

        }
        

	}

    void ResumeTalk() {
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCFunction : MonoBehaviour {

    public enum FunctionType { 可撿物件, 不可撿物件, 故事系統, 傳送 };

    public FunctionType type;
    public UnityEvent Event;

    private NPCTrigger trigger;
    private bool execute = false;
    private bool functionDone = false;

    void OnEnable() {
        trigger = GetComponent<NPCTrigger>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (execute) {
            if (type == FunctionType.不可撿物件) {

            }
        }
	}

    public void Execute() {
        execute = true;
    }

    public bool IsFunctionDone() {
        return functionDone;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Dialog : MonoBehaviour {
    enum State {
        Dialog,
        Explore
    }

    public TextAsset file;
    public bool defaultStateIsDialog;
    public int currentLine;
    public Animator canvasAni ;

    private Text textName;
    private Text dialog;
    private string textOfFile;
    private int nowPointerOfOutput;
    private State state;
    public bool nowStateShouldBeDialog;

    // Use this for initialization
    void Start() {
        if (defaultStateIsDialog) {
            state = State.Dialog;
        }
        else {
            state = State.Explore;
        }
        currentLine = 1;
        nowPointerOfOutput = 0;
        textName = gameObject.transform.GetChild(0).GetComponent<Text>();
        dialog = gameObject.transform.GetChild(1).GetComponent<Text>();
        textOfFile = file.text;
        //dialog.text = file.text;
        ChangeDialog();
        
    }

    // Update is called once per frame
    void Update() {
        if (nowStateShouldBeDialog) {
            print(state);
            if (state == State.Explore) {
                canvasAni.SetBool("Dialog", true);
                state = State.Dialog;
            }
            else {
                if (Input.GetKeyDown(KeyCode.Z)) {
                    if (nowPointerOfOutput < textOfFile.Length) {
                        currentLine += 1;
                        ChangeDialog();
                    }
                }
            }
        }
        else {
            if (state == State.Dialog) {
                canvasAni.SetBool("Dialog", false);
                state = State.Explore;
                WaitAndPressZToNextLine();
            }
        }
        
    }

    IEnumerator WaitAndPressZToNextLine() {
        yield return new WaitForSeconds(1.0f);
        if (Input.GetKeyDown(KeyCode.Z)) {
            if (nowPointerOfOutput < textOfFile.Length) {
                currentLine += 1;
                ChangeDialog();
            }
        }
    }

    void ChangeDialog() {
        int nameStart=0, nameEnd=0;
        while (nowPointerOfOutput != textOfFile.Length && textOfFile[nowPointerOfOutput] != '\n') {
            if (textOfFile[nowPointerOfOutput] == '【') {
                nameStart = nowPointerOfOutput + 1;
            }
            else if (textOfFile[nowPointerOfOutput] == '】') {
                nameEnd = nowPointerOfOutput - 1;
            }
            nowPointerOfOutput += 1 ;
        }
        textName.text = textOfFile.Substring(nameStart, nameEnd+1-nameStart);
        dialog.text = textOfFile.Substring(nameEnd+2, nowPointerOfOutput-nameEnd-2);
        nowPointerOfOutput += 1;
    }
}

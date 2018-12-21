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
    public Animator BGAni;
    public Sprite BackGround;

    private Text textName;
    private Text dialog;
    private string textOfFile;
    private int nowPointerOfOutput;
    private State state;
    private bool nowStateShouldBeDialog;
    private bool lineDone;

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
        lineDone = true;
        //dialog.text = file.text;
    }

    // Update is called once per frame
    void Update() {
        if (nowStateShouldBeDialog) {
            if (state == State.Explore) {   //切換到dialog
                canvasAni.SetBool("Dialog", true);
                BGAni.SetBool("Dialog", true);
                state = State.Dialog;
                ChangeDialog();
            }
            else {  //當下即是dialog
                if (Input.GetKeyDown(KeyCode.Z)) {
                    if (nowPointerOfOutput < textOfFile.Length) {
                        if (!NextIsExplore()) {
                            currentLine += 1;
                            ChangeDialog();
                        }
                        else WhatType(false, "");
                    }
                }
            }
        }
        else {
            if (state == State.Dialog) {    //切換到explore
                canvasAni.SetBool("Dialog", false);
                BGAni.SetBool("Dialog", false);
                state = State.Explore;
            }
            else {  //當下即是explore
                if (nowPointerOfOutput < textOfFile.Length && lineDone) {
                    currentLine += 1;
                    WhatType(false, "");
                }
            }
        }
        
    }

    IEnumerator WaitAndPressZToNextLine() {     //for waiting the time of changing dialog and explore
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
                print(nameStart);
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

    void WhatType(bool transition, string transType) { //transType 0: not transition, 1: 亮光
        int typeStart = 0, typeEnd = 0;
        string type;
        string description;
        while (nowPointerOfOutput < textOfFile.Length && textOfFile[nowPointerOfOutput] != '\n') {
            if (textOfFile[nowPointerOfOutput] == '《') {
                typeStart = nowPointerOfOutput + 1;
            }
            else if (textOfFile[nowPointerOfOutput] == '》') {
                typeEnd = nowPointerOfOutput - 1;
            }
            nowPointerOfOutput += 1;
        }
        type = textOfFile.Substring(typeStart, typeEnd + 1 - typeStart);
        description = textOfFile.Substring(typeEnd + 2, nowPointerOfOutput - typeEnd - 2);
        nowPointerOfOutput += 1;
        if (transition) {
            if (transType == "亮光") {

            }
        }
        else if (type == "Dialog") {
            nowStateShouldBeDialog = true;
        }
        else if (type == "Explore") {
            nowStateShouldBeDialog = false;
        }
        else if (type == "等待完成探索") {
            lineDone = false;
            SystemVariables.ExploreDone = false;
            StartCoroutine(WaitExploreDone());
        }
        else if (type == "其他腳本") {
            lineDone = false;

            lineDone = true;
        }
        else if (type == "轉場") {
            lineDone = false;
            WhatType(true, description);    //要讀是什麼場景
        }
        else if (type == "換圖") {
            lineDone = false;
            int i = 0;
            for (i = 0 ; description[i]!=',' ; i++) {

            }
            string tempName = description.Substring(0, i);
            string tempSpriteIndex = description.Substring(i + 1, description.Length);
            ChangeSprite(tempName, tempSpriteIndex);
        }
        else if (type == "動畫") {
            lineDone = false;
            CharacterAnimation();
            lineDone = true;
        }
    }

    void ChangeSprite(string name, string spriteIndex) {

    }

    void CharacterAnimation() {

    }

    bool NextIsExplore() {
        if (textOfFile[nowPointerOfOutput] == '《') {
            return true;
        }
        else return false;
    }

    IEnumerator WaitExploreDone() {
        while (!SystemVariables.ExploreDone) {
            yield return null;
        }
        lineDone = true;
    }
}

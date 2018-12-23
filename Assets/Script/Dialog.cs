﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Dialog : MonoBehaviour {
    enum State {
        Dialog,
        Explore
    }

    [System.Serializable]
    public struct BGSpriteAndName {
        public string BGName;
        public Sprite BGSprite;
    }

    [System.Serializable]
    public struct CharacterSpriteAndName {
        public string CharacterName;
        public string CharacterSpriteName;
        public Sprite CharacterSprtie;
    }

    [System.Serializable]
    public struct CharacterObjectAndName {
        public string CharacterName;
        public SpriteRenderer CharacterSpriteComponent;
    }

    /**************************
     * B1醫務室
     * 回憶_舊街道
     * 穿越_B1醫務室外走廊
     * ************************/

    public TextAsset file;
    public bool defaultStateIsDialog;
    public int currentLine;
    public BGSpriteAndName[] BGs;
    public CharacterSpriteAndName[] CharacterSprites;
    public CharacterObjectAndName[] CharacterObjects;
    public Animator canvasAni ;
    public Animator BGAni;
    public Animator WhiteAni;
    public Animator BlackAni;
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
        int nameStart=0, nameEnd=0, returnLine=0;
        while (nowPointerOfOutput != textOfFile.Length && textOfFile[nowPointerOfOutput] != '\n') {
            if (textOfFile[nowPointerOfOutput] == '\\' && textOfFile[nowPointerOfOutput + 1] == 'N') {
                returnLine = nowPointerOfOutput;
                nowPointerOfOutput += 2;
            }
            else if (textOfFile[nowPointerOfOutput] == '【') {
                nameStart = nowPointerOfOutput + 1;
                nowPointerOfOutput += 1;
            }
            else if (textOfFile[nowPointerOfOutput] == '】') {
                nameEnd = nowPointerOfOutput - 1;
                nowPointerOfOutput += 1;
            }
        }
        textName.text = textOfFile.Substring(nameStart, nameEnd+1-nameStart);
        if (returnLine == 0) {
            dialog.text = textOfFile.Substring(nameEnd + 2, nowPointerOfOutput - nameEnd - 2);
        }
        else {
            string str1, str2;
            str1= textOfFile.Substring(nameEnd + 2, returnLine - nameEnd - 2);
            str2= textOfFile.Substring(returnLine + 2, nowPointerOfOutput - returnLine - 2);
            dialog.text = string.Concat(str1, "\n", str2);
        }
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
                WhiteAni.SetBool("White", true);
                for (int i = 0; i < BGs.Length; i++) {
                    if (BGs[i].BGName==description) {
                        ChangeBGSprite(BGs[i].BGSprite);
                        break;
                    }
                }
            }
            else if (transType == "黑屏") {
                BlackAni.SetBool("Black", true);
                for (int i = 0; i < BGs.Length; i++) {
                    if (BGs[i].BGName == description) {
                        ChangeBGSprite(BGs[i].BGSprite);
                        break;
                    }
                }
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
            SystemVariables.PlayerCanMove = true;
            StartCoroutine(WaitExploreDone());
        }
        else if (type == "其他腳本") {
            lineDone = false;

            lineDone = true;
        }
        else if (type == "轉場") {
            lineDone = false;
            WhatType(true, description);    //再看看要讀是什麼場景
        }
        else if (type == "換圖") {
            lineDone = false;
            int i = 0;
            for (i = 0 ; description[i]!=',' ; i++) {
                
            }
            string tempName = description.Substring(0, i);
            string tempSpriteName = description.Substring(i + 1, description.Length-i-1);
            ChangeSprite(tempName, tempSpriteName);
        }
        else if (type == "動畫") {
            lineDone = false;
            CharacterAnimation();
            lineDone = true;
        }
    }

    void ChangeSprite(string name, string spriteName) {
        for (int i = 0; i < CharacterSprites.Length; i++) {
            if (CharacterSprites[i].CharacterName == name && CharacterSprites[i].CharacterSpriteName == spriteName) {
                for (int j = 0; j < CharacterObjects.Length; j++) {
                    if (CharacterObjects[j].CharacterName == name) {
                        CharacterObjects[j].CharacterSpriteComponent.sprite = CharacterSprites[i].CharacterSprtie;
                    }
                }
            }
        }
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
        SystemVariables.PlayerCanMove = false;
    }

    IEnumerator ChangeBGSprite(Sprite wantToChange) {
        yield return new WaitForSeconds(0.5f);
        BackGround = wantToChange;
    }
}

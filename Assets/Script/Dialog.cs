using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Dialog : MonoBehaviour {
    enum State {
        None,
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
        public Sprite CharacterSprite;
    }

    [System.Serializable]
    public struct CharacterObjectAndName {
        public string CharacterName;
        public Transform CharacterTransform;
        public SpriteRenderer CharacterSpriteComponent;
    }

    [System.Serializable]
    public struct TaChiE {      //立ち絵
        public string characterName;
        public string spriteName;
        public Sprite sprite; 
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
    public TaChiE[] TaChiEs;
    public CharacterSpriteAndName[] CharacterSprites;
    public CharacterObjectAndName[] CharacterObjects;
    public Animator canvasAni ;
    public Animator BGAni;
    public Animator WhiteAni;
    public Animator BlackAni;
    public Animator BlurAni;
    public Animator PlayerDialogAni;
    public SpriteRenderer BackGround;
    public CharacterControl CharControl;
    public GameObject rightTachie;
    public GameObject leftTachie;
    public Animator PlayerAni;

    private Text textName;
    private Text dialog;
    private string textOfFile;
    private int nowPointerOfOutput;
    private State state;
    private bool nowStateShouldBeDialog;
    private bool lineDone;
    private Sprite recordedSprite;
    private Image rightTachieImageComp;
    private Image leftTachieImageComp;
    private string rightTachieCharacterName;
    private string leftTachieCharacterName;
    private CanvasGroup rightTachieCanvasGroup;
    private CanvasGroup leftTachieCanvasGroup;

    // Use this for initialization
    void Start() {
        if (defaultStateIsDialog) {
            nowStateShouldBeDialog = true;
        }
        else {
            nowStateShouldBeDialog = false;
        }
        state = State.None;
        currentLine = 0;
        nowPointerOfOutput = 0;
        textName = gameObject.transform.GetChild(0).GetComponent<Text>();
        dialog = gameObject.transform.GetChild(1).GetComponent<Text>();
        textOfFile = file.text;
        print(textOfFile);
        lineDone = true;
        rightTachieImageComp = rightTachie.GetComponent<Image>();
        leftTachieImageComp = leftTachie.GetComponent<Image>();
        rightTachieCanvasGroup = rightTachie.GetComponent<CanvasGroup>();
        leftTachieCanvasGroup = leftTachie.GetComponent<CanvasGroup>();
        rightTachieCanvasGroup.alpha = 0.5f;
        leftTachieCanvasGroup.alpha = 0.5f;
        PlayerAni.enabled = false;
        //dialog.text = file.text;
    }

    // Update is called once per frame
    void Update() {
        //print(nowPointerOfOutput);
        //print(textOfFile[nowPointerOfOutput]);
        if (nowStateShouldBeDialog) {
            if (state != State.Dialog) {   //切換到dialog
                canvasAni.SetBool("Dialog", true);
                PlayerDialogAni.SetBool("Dialog", true);
                BGAni.SetBool("Dialog", true);
                state = State.Dialog;
                if (nowPointerOfOutput < textOfFile.Length - 1) {
                    ChangeDialog();
                    currentLine += 1;
                }
            }
            else if (nowPointerOfOutput < textOfFile.Length - 1) {
                if (Input.GetKeyDown(KeyCode.Z)) {
                    if (!NextIsExplore()) {
                        ChangeDialog();
                        currentLine += 1;
                    }
                    else {
                        WhatType(false, "");
                        currentLine += 1;
                    }
                }
            }
        }
        else {
            if (state != State.Explore) {    //切換到explore
                canvasAni.SetBool("Dialog", false);
                PlayerDialogAni.SetBool("Dialog", false);
                BGAni.SetBool("Dialog", false);
                state = State.Explore;
            }
            else if (nowPointerOfOutput < textOfFile.Length - 1 && lineDone) {
                currentLine += 1;
                WhatType(false, "");
            }
        }
    }

    IEnumerator WaitAndPressZToNextLine() {     //for waiting the time of changing dialog and explore
        yield return new WaitForSeconds(1.0f);
        if (Input.GetKeyDown(KeyCode.Z)) {
            if (nowPointerOfOutput < textOfFile.Length - 1) {
                currentLine += 1;
                ChangeDialog();
            }
        }
    }

    void ChangeDialog() {
        int nameStart=0, nameEnd=0, returnLine=0;
        //print(nowPointerOfOutput);
        while (nowPointerOfOutput < textOfFile.Length - 1 && textOfFile[nowPointerOfOutput] != '\n') {
            //print(nowPointerOfOutput);
            //print(textOfFile[nowPointerOfOutput]);
            if (textOfFile[nowPointerOfOutput] == '\\' && textOfFile[nowPointerOfOutput + 1] == 'N') {
                returnLine = nowPointerOfOutput;
                nowPointerOfOutput += 1;
            }
            else if (textOfFile[nowPointerOfOutput] == '【') {
                nameStart = nowPointerOfOutput + 1;
            }
            else if (textOfFile[nowPointerOfOutput] == '】') {
                nameEnd = nowPointerOfOutput - 1;
            }
            nowPointerOfOutput += 1;
        }
        /*print(nameStart);
        print(nameEnd);
        print(nowPointerOfOutput);*/
        textName.text = textOfFile.Substring(nameStart, nameEnd+1-nameStart);
        if (rightTachieCharacterName == textName.text) {
            rightTachieCanvasGroup.alpha = 1;
            leftTachieCanvasGroup.alpha = 0.5f;
        }
        else if (leftTachieCharacterName == textName.text) {
            leftTachieCanvasGroup.alpha = 1;
            rightTachieCanvasGroup.alpha = 0.5f;
        }
        else {
            leftTachieCanvasGroup.alpha = 0.5f;
            rightTachieCanvasGroup.alpha = 0.5f;
        }
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
        while (nowPointerOfOutput < textOfFile.Length /*- 1*/ && textOfFile[nowPointerOfOutput] != '\n') {
            if (textOfFile[nowPointerOfOutput] == '《') {
                typeStart = nowPointerOfOutput + 1;
            }
            else if (textOfFile[nowPointerOfOutput] == '》') {
                typeEnd = nowPointerOfOutput - 1;
            }
            nowPointerOfOutput += 1;
            /*print(textOfFile[nowPointerOfOutput]-0);
            print(nowPointerOfOutput);*/
        }
        type = textOfFile.Substring(typeStart, typeEnd + 1 - typeStart);
        description = textOfFile.Substring(typeEnd + 2, nowPointerOfOutput - typeEnd - 2);
        nowPointerOfOutput += 1;
        //print(nowPointerOfOutput);
        if (transition) {
            if (transType == "亮光") {
                lineDone = false;
                WhiteAni.SetBool("White", true);
                for (int i = 0; i < BGs.Length; i++) {
                    if (BGs[i].BGName == description) {
                        StartCoroutine(ChangeBGSprite(BGs[i].BGSprite));
                        break;
                    }
                }
            }
            else if (transType == "黑屏") {
                lineDone = false;
                BlackAni.SetBool("Black", true);
                for (int i = 0; i < BGs.Length; i++) {
                    if (BGs[i].BGName == description) {
                        StartCoroutine(ChangeBGSprite(BGs[i].BGSprite));
                        break;
                    }
                }
            }
            else if (transType == "模糊") {
                lineDone = false;
                BlurAni.SetBool("Blur", true);
                for (int i = 0; i < BGs.Length; i++) {
                    if (BGs[i].BGName == description) {
                        StartCoroutine(ChangeBGSprite(BGs[i].BGSprite));
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
            lineDone = true;
            /*SystemVariables.ExploreDone = false;
            SystemVariables.PlayerCanMove = true;
            PlayerAni.enabled = true;
            StartCoroutine(WaitExploreDone());*/
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
            for (i = 0; description[i] != ','; i++) {

            }
            string tempName = description.Substring(0, i);
            string tempSpriteName = description.Substring(i + 1, description.Length - i - 1);
            ChangeSprite(tempName, tempSpriteName);
            StartCoroutine(ChangePicDelay());
        }
        else if (type == "存圖") {
            lineDone = false;
            for (int i = 0; i < CharacterObjects.Length; i++) {
                if (CharacterObjects[i].CharacterName == description) {
                    for (int j = 0; j < CharacterSprites.Length; j++) {
                        if (CharacterSprites[j].CharacterName == description && CharacterSprites[j].CharacterSpriteName == "default") {
                            CharacterSprites[j].CharacterSprite = CharacterObjects[i].CharacterSpriteComponent.sprite;
                        }
                    }
                }
            }
            lineDone = true;
        }
        else if (type == "移動") {
            lineDone = false;
            //PlayerAni.enabled = true;
            int i = 0;
            for (i = 0; description[i] != ','; i++) {

            }
            string tempName = description.Substring(0, i);
            string tempPosition = description.Substring(i + 1, description.Length - i - 1);
            print(description);
            print(tempPosition);
            CharControl.AutoWalk(tempName, StringToInt(tempPosition));
        }
        else if (type == "換立繪") {
            //print(description);
            lineDone = false;
            int i = 0;
            int characterNameStart = 0, spriteTypeStart = 0;
            for (i = 0; i < description.Length; i++) {
                if (description[i] == ',') {
                    characterNameStart = i + 1;
                }
                else if (description[i] == '.') {
                    spriteTypeStart = i + 1;
                }
            }
            string rightOrLeft = description.Substring(0, characterNameStart - 1);
            string characName = description.Substring(characterNameStart, spriteTypeStart - 1 - characterNameStart);
            string spriteType = description.Substring(spriteTypeStart, description.Length - spriteTypeStart);
            /*print(rightOrLeft);
            print(characName);
            print(spriteType.Length);*/
            for (int j = 0; j < TaChiEs.Length; j++) {
                if (TaChiEs[j].characterName == characName && TaChiEs[j].spriteName == spriteType) {
                    if (rightOrLeft == "右") {
                        rightTachieImageComp.sprite = TaChiEs[j].sprite;
                        rightTachieCharacterName = TaChiEs[j].characterName;
                    }
                    else if (rightOrLeft == "左") {
                        leftTachieImageComp.sprite = TaChiEs[j].sprite;
                        leftTachieCharacterName = TaChiEs[j].characterName;
                    }
                }
            }
            lineDone = true;
        }
        else if (type == "角色出現") {
            lineDone = false;
            for (int i = 0; i < CharacterObjects.Length; i++) {
                if (CharacterObjects[i].CharacterName == description) {
                    CharacterObjects[i].CharacterSpriteComponent.color = new Color(1f, 1f, 1f, 1f);
                }
            }
            lineDone = true;
        }
        else if (type == "角色消失") {
            lineDone = false;
            for (int i = 0; i < CharacterObjects.Length; i++) {
                if (CharacterObjects[i].CharacterName == description) {
                    CharacterObjects[i].CharacterSpriteComponent.color = new Color(1f, 1f, 1f, 0f);
                }
            }
            lineDone = true;
        }
        else if (type == "等待") {
            lineDone = false;
            StartCoroutine(WaitTime(StringToInt(description)));
        }
    }

    void ChangeSprite(string name, string spriteName) {
        for (int i = 0; i < CharacterSprites.Length; i++) {
            if (CharacterSprites[i].CharacterName == name && CharacterSprites[i].CharacterSpriteName == spriteName) {
                for (int j = 0; j < CharacterObjects.Length; j++) {
                    if (CharacterObjects[j].CharacterName == name) {
                        print("do");
                        CharacterObjects[j].CharacterSpriteComponent.sprite = CharacterSprites[i].CharacterSprite;
                    }
                }
            }
        }
    }

    bool NextIsExplore() {
        if (textOfFile[nowPointerOfOutput] == '《') {
            return true;
        }
        else return false;
    }

    IEnumerator WaitExploreDone() {
        while (/*!SystemVariables.ExploreDone*/true) {
            yield return null;
        }
        lineDone = true;
        //SystemVariables.PlayerCanMove = false;
        PlayerAni.enabled = false;
    }

    IEnumerator ChangeBGSprite(Sprite wantToChange) {
        yield return new WaitForSeconds(1f);
        BackGround.sprite = wantToChange;
        yield return new WaitForSeconds(0.5f);
        lineDone = true;
    }

    IEnumerator ChangePicDelay() {
        yield return new WaitForSeconds(2);
        lineDone = true;
    }

    IEnumerator WaitTime(float time) {
        yield return new WaitForSeconds(time);
        lineDone = true;
    }

    public void SetLineDone(bool b) {
        lineDone = b;
    }

    int StringToInt(string str) {
        int result=0;
        for (int i = 0; i < str.Length; i++) {
            result = result * 10 + str[i] - 48;
        }
        return result;
    }
}

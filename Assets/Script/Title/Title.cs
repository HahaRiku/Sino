using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour {
    public enum SelectedButton {
        Start,
        Load,
        Exit
    }

    public Sprite[] 背景_內;

    [System.Serializable]
    public struct ButtonSprite {
        public Sprite notSelected;
        public Sprite selected;
    }
    public ButtonSprite[] buttonSprites;

    public Image[] buttonImageComp;
    public SpriteRenderer 背景_內SpriteComp;

    private SelectedButton state;
    private bool stateChanged ;
    private int bg_index;

	// Use this for initialization
	void Start () {
        bg_index = 0;
        state = SelectedButton.Start;
        if (state == SelectedButton.Start) {
            buttonImageComp[0].sprite = buttonSprites[0].selected;
            buttonImageComp[1].sprite = buttonSprites[1].notSelected;
            buttonImageComp[2].sprite = buttonSprites[2].notSelected;
        }
        else if (state == SelectedButton.Load) {
            buttonImageComp[0].sprite = buttonSprites[0].notSelected;
            buttonImageComp[1].sprite = buttonSprites[1].selected;
            buttonImageComp[2].sprite = buttonSprites[2].notSelected;
        }
        else if (state == SelectedButton.Exit) {
            buttonImageComp[0].sprite = buttonSprites[0].notSelected;
            buttonImageComp[1].sprite = buttonSprites[1].notSelected;
            buttonImageComp[2].sprite = buttonSprites[2].selected;
        }
    }
	
	// Update is called once per frame
	void Update () {
        StartCoroutine(ChangeBG());


        if (Input.GetKeyDown(KeyCode.Z)) {
            if (state == SelectedButton.Exit) {
                Application.Quit();
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            if (state == SelectedButton.Start) {
                buttonImageComp[0].sprite = buttonSprites[0].notSelected;
                buttonImageComp[1].sprite = buttonSprites[1].selected;
                buttonImageComp[2].sprite = buttonSprites[2].notSelected;
                state = SelectedButton.Load;
            }
            else if (state == SelectedButton.Load) {
                buttonImageComp[0].sprite = buttonSprites[0].notSelected;
                buttonImageComp[1].sprite = buttonSprites[1].notSelected;
                buttonImageComp[2].sprite = buttonSprites[2].selected;
                state = SelectedButton.Exit;
            }
            else if (state == SelectedButton.Exit) {
                buttonImageComp[0].sprite = buttonSprites[0].selected;
                buttonImageComp[1].sprite = buttonSprites[1].notSelected;
                buttonImageComp[2].sprite = buttonSprites[2].notSelected;
                state = SelectedButton.Start;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (state == SelectedButton.Start) {
                buttonImageComp[0].sprite = buttonSprites[0].notSelected;
                buttonImageComp[1].sprite = buttonSprites[1].notSelected;
                buttonImageComp[2].sprite = buttonSprites[2].selected;
                state = SelectedButton.Exit;
            }
            else if (state == SelectedButton.Load) {
                buttonImageComp[0].sprite = buttonSprites[0].selected;
                buttonImageComp[1].sprite = buttonSprites[1].notSelected;
                buttonImageComp[2].sprite = buttonSprites[2].notSelected;
                state = SelectedButton.Start;
            }
            else if (state == SelectedButton.Exit) {
                buttonImageComp[0].sprite = buttonSprites[0].notSelected;
                buttonImageComp[1].sprite = buttonSprites[1].selected;
                buttonImageComp[2].sprite = buttonSprites[2].notSelected;
                state = SelectedButton.Load;
            }
        }
        /*if (stateChanged) {

        }*/
	}

    IEnumerator ChangeBG() {
        yield return new WaitForSeconds(8);
        背景_內SpriteComp.sprite = 背景_內[bg_index];
        bg_index++;
        if (bg_index == 背景_內.Length) {
            bg_index = 0;
        }
    }

    /*public void ChangeState(SelectedButton s) {
        state = s;
        stateChanged = true;
    }*/
}

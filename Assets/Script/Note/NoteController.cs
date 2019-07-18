using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/********************************
 * 
 * KeyDown -> start 計時
 * KeyUp -> stop 計時
 * 
 * ********************************/

public class NoteController : MonoBehaviour {
    public enum PressKeyState {
        None,
        RightArrowDown,
        RightArrowUp,
        LeftArrowDown,
        LeftArrowUp
    }

    public float 按幾秒為長按 = 0.5f;
    public float 長按幾秒後翻到最前或後 = 3.0f;
    public PressKeyState pressKeyState = PressKeyState.None;

    private AutoFlip autoFlip;
    private Book controledNote;

    private float timeCountingStart;
    public bool 進度條decreaseDone = true;

	// Use this for initialization
	void Start () {
        進度條decreaseDone = true;
        autoFlip = GetComponent<AutoFlip>();
        controledNote = GetComponent<Book>();
	}
	
	// Update is called once per frame
	void Update () {

        if (pressKeyState == PressKeyState.None) {
            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                timeCountingStart = Time.time;
                pressKeyState = PressKeyState.RightArrowDown;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                timeCountingStart = Time.time;
                pressKeyState = PressKeyState.LeftArrowDown;
            }
        }
        else if (pressKeyState == PressKeyState.RightArrowDown) {
            if ((Time.time - timeCountingStart) >= 按幾秒為長按) {
                //進度條更新
            }
            if ((Time.time - timeCountingStart) >= 按幾秒為長按 + 長按幾秒後翻到最前或後) {
                autoFlip.Mode = FlipMode.RightToLeft;
                autoFlip.DelayBeforeStarting = 0;
                autoFlip.PageFlipTime = 0.01f;
                autoFlip.TimeBetweenPages = 0.1f;
                autoFlip.AnimationFramesCount = 5;
                autoFlip.StartFlipping();
                pressKeyState = PressKeyState.RightArrowUp;
                進度條decreaseDone = false;
                StartCoroutine(進度條decrease());
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow)) {
                pressKeyState = PressKeyState.RightArrowUp;
                if ((Time.time - timeCountingStart) < 按幾秒為長按) {
                    autoFlip.DelayBeforeStarting = 0.2f;
                    autoFlip.PageFlipTime = 0.01f;
                    autoFlip.TimeBetweenPages = 0.1f;
                    autoFlip.AnimationFramesCount = 15;
                    autoFlip.FlipRightPage();
                }
                else if((Time.time - timeCountingStart) < 按幾秒為長按 + 長按幾秒後翻到最前或後) {
                    //進度條----
                    進度條decreaseDone = false;
                    StartCoroutine(進度條decrease());
                }
            }
        }
        else if (pressKeyState == PressKeyState.LeftArrowDown) {
            if ((Time.time - timeCountingStart) >= 按幾秒為長按) {
                //進度條++
            }
            if ((Time.time - timeCountingStart) >= 按幾秒為長按 + 長按幾秒後翻到最前或後) {
                autoFlip.Mode = FlipMode.LeftToRight;
                autoFlip.DelayBeforeStarting = 0;
                autoFlip.PageFlipTime = 0.01f;
                autoFlip.TimeBetweenPages = 0.1f;
                autoFlip.AnimationFramesCount = 5;
                autoFlip.StartFlipping();
                pressKeyState = PressKeyState.LeftArrowUp;
                進度條decreaseDone = false;
                StartCoroutine(進度條decrease());
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow)) {
                pressKeyState = PressKeyState.LeftArrowUp;
                if ((Time.time - timeCountingStart) < 按幾秒為長按) {
                    autoFlip.DelayBeforeStarting = 0.2f;
                    autoFlip.PageFlipTime = 0.01f;
                    autoFlip.TimeBetweenPages = 0.1f;
                    autoFlip.AnimationFramesCount = 15;
                    autoFlip.FlipLeftPage();
                }
                else if ((Time.time - timeCountingStart) < 按幾秒為長按 + 長按幾秒後翻到最前或後) {
                    //進度條----
                    進度條decreaseDone = false;
                    StartCoroutine(進度條decrease());
                }
            }
        }
        else if (進度條decreaseDone && !autoFlip.toEndIsFlipping && (pressKeyState == PressKeyState.RightArrowUp || pressKeyState == PressKeyState.LeftArrowUp) && !(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))) {
            pressKeyState = PressKeyState.None;
        }

	}

    IEnumerator 進度條decrease() {
        for (float t = 長按幾秒後翻到最前或後; t > 0 ; t -= 0.05f) {
            //animation?
            print("test");
            yield return null;
        }
        進度條decreaseDone = true;
    }
}

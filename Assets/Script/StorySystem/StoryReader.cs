using System.Collections;
using System.Collections.Generic;
//using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoryReader : MonoBehaviour
{
    /* 
     * 功能說明：
     *  控制打字動畫，以及對話框的開啟、關閉。
     */

    public DialogPanelController dialogPanel;
    public Text NameText;
    public Text DialogText;

    [Range(0.01f, 0.1f)]
    float intervalTime = 0.05f;
    float timer;

    string currentString;
    int charIndex;

    bool isReadStory = false;
    bool isForceFinish = false;
    bool isFinish = false;
    bool isEnd = false;

    void OnEnable()
    {
        dialogPanel = FindObjectOfType<DialogPanelController>();
        if (dialogPanel == null)
        {
            Debug.Log("Error: No Dialog has found.");
            enabled = false;
            return;
        }
        NameText = dialogPanel.transform.GetChild(1).GetComponent<Text>();
        DialogText = dialogPanel.transform.GetChild(0).GetComponent<Text>();
        DialogText.text = "";
    }
    public bool IsFinished()
    {
        return !isReadStory;
    }
    public void StartTyping(StoryData.StoryState story)
    {
        isFinish = false;
        isForceFinish = false;
        DialogText.text = "";
        charIndex = 0;
        if (!dialogPanel.IsVisible())
            dialogPanel.SetVisible();
        NameText.text = story.Name;
        currentString = story.Text;
        isReadStory = true;
    }
    public void ClosePanel()
    {
        dialogPanel.SetInvisible();
    }
    void Update()
    {
        if (isReadStory)
        {
            if (isFinish)
            {
                isReadStory = false;
                return;
            }
            //if (Input.GetKeyDown(KeyCode.Z) || (EventSystem.current.currentSelectedGameObject == clickRegion && Input.GetMouseButtonDown(0)))
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(0))
            {
                if (!isFinish && charIndex > 2)
                    isForceFinish = true;
            }
            if (!isFinish)
            {
                if (charIndex == currentString.Length)
                {
                    isFinish = true;
                    return;
                }
                if (isForceFinish)
                {
                    DialogText.text = currentString;
                    isFinish = true;
                    return;
                }
                timer += Time.deltaTime;
                if (timer >= intervalTime)
                {
                    if (charIndex > 0 && charIndex % 27 == 0)
                        DialogText.text += "\n";
                    DialogText.text += currentString[charIndex++];
                    timer = 0;
                }
            }
        }
    }
}
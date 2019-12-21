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
    int charIndex_Write;
    bool tagFlag;

    bool isReadStory = false;
    bool isForceFinish = false;
    bool isFinish = false;

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
        charIndex_Write = 0;
        tagFlag = false;
        if (!dialogPanel.IsVisible())
            dialogPanel.SetVisible();
        if (story.Name.Trim() == "")
        {
            NameText.text = "";
            currentString = story.Text;
        }
        else
        {
            NameText.text = story.Name;
            currentString = "\n" + story.Text;
        }
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
            else if (Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(0))
            {
                if (charIndex > 2)
                    isForceFinish = true;
            }
            if (charIndex == currentString.Length)
            {
                isFinish = true;
                return;
            }
            else if (isForceFinish)
            {
                DialogText.text = currentString;
                isFinish = true;
                return;
            }
            timer += Time.deltaTime;
            if (timer >= intervalTime)
            {
                if (charIndex_Write > 0 && charIndex_Write % 27 == 0)
                    DialogText.text += "\n";

                if (currentString[charIndex] == '<')
                    tagFlag = true;
                else if (currentString[charIndex] == '>' )
                    tagFlag = false;
                if (!tagFlag && currentString[charIndex] != '>')                    
                    charIndex_Write++;

                DialogText.text += currentString[charIndex++];
                timer = 0;
            }
        }
    }
}
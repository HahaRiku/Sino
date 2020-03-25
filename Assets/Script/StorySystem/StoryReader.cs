using System.Collections;
using System.Collections.Generic;
//using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using System.Text.RegularExpressions;

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

    //tag檢索
    private List<string> tagList;
    Dictionary<string, string> tagDict = new Dictionary<string, string>() {
        { "<color=#([0-9A-Z]{8})>","</color>"},    { "<b>","</b>"},
        { "<size=#([0-9]+)>","</size>"}
    };


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
        tagList = new List<string>();
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

                //<tag deal>
                if (currentString[charIndex] == '<')    //遇到tag => 讀入整串tag
                {
                    int tagIndex;
                    for (tagIndex = charIndex; currentString[tagIndex] != '>' && tagIndex < currentString.Length; tagIndex++) ; //找tag字串的結尾
                    if (currentString[tagIndex] == '>')    //找到完整tag =>
                    {
                        string tag = currentString.Substring(charIndex, tagIndex - charIndex+1);    //存Tag字串
                        if (tag[1] == '/' && tagDict.ContainsValue(tag))  //字串==endTag => 比對對稱tag
                        {
                            bool error = true;
                            for(int i = 0; i < tagList.Count; i++)
                            {
                                //tagList有值 = tag[endTag] => tag和endTag對稱 => pop掉對稱tag
                                if ( Regex.IsMatch(tagList[i], tagDict.FirstOrDefault(x => x.Value == tag).Key) )  
                                {
                                    error = false;
                                    tagList.Remove(tagList[i]); //pop掉tag
                                    charIndex = tagIndex;   //更新讀入index
                                    i = tagList.Count;
                                }
                            }
                            if (error) { Debug.LogError("Error: tag不對稱"); } //endTag > tag => ERROR Message
                            //</endTag>
                        }
                        else   //字串==新tag => 加入tagList
                        {
                            tagList.Add(tag);
                            charIndex = tagIndex;   //更新讀入index
                            //</新tag>
                        }
                    }
                    else if (tagIndex == currentString.Length) //有 '<' && 沒有 '>' 的情況
                    {
                        //Debug.LogError("Error: tag錯誤");   // or 正常字串
                    }
                }

                //輸入
                if (currentString[charIndex] != '>')    //非處理tag
                {
                    charIndex_Write++;
                    //if (tagList.Any())  //若有tag被啟用(tagList中有tag)，在所有輸入字前加上<tag>
                    //{
                        foreach (string tl in tagList)
                        {
                            DialogText.text += tl;
                            //Debug.Log(charIndex);
                        }
                    //}
                    DialogText.text += currentString[charIndex++];
                    //if (tagList.Any())  //若有tag被啟用(tagList中有tag)，在所有輸入字尾加上</tag>
                    //{
                    for (int i = tagList.Count - 1; i >= 0; i--)
                    {
                        string temp = tagDict.FirstOrDefault(x => Regex.IsMatch(tagList[i], x.Key)).Value;
                        Debug.Log(temp);
                        DialogText.text += temp;
                    }
                    //}
                }
                else  //處理完tag的情況 => 不輸入當前字元(='>')
                    charIndex++;
                // </tag deal>
                
                timer = 0;
            }
        }
    }
}
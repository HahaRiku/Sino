using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionControl : MonoBehaviour {

    /* 
     * 功能說明：
     *  控制選項指標動畫，打字動畫，以及對話框的開啟、關閉。
     */

    public OptionsPanelController optionsPanel;
    OptionPointer pointer;
    public SelectOption[] _options;

    bool isSelectOptions = false;

    void OnEnable()
    {
        optionsPanel = FindObjectOfType<OptionsPanelController>();
        if (optionsPanel == null)
        {
            Debug.Log("Error: No OptionsPanel has found.");
            enabled = false;
            return;
        }
        pointer = optionsPanel.transform.GetChild(1).GetComponent<OptionPointer>();
    }
    public bool IsFinished()
    {
        return !isSelectOptions;
    }
    public void ShowOptions(StoryData.StoryState story)
    {  
        _options = story.Options;
        pointer.Initial(_options.Length);
        for (int i = 0;i< _options.Length;i++)
        {
            Text optionText = optionsPanel.transform.GetChild(0).GetChild(i).GetComponent<Text>();
            optionText.text = _options[i].Content;
        }
        GetComponent<StoryManager>().PauseStory();
        if (!optionsPanel.IsVisible())
            optionsPanel.SetVisible();
        StartCoroutine(WaitAndShow());
    }
    public void ClosePanel()
    {
        optionsPanel.SetInvisible();
    }
    void Update()
    {
        if (isSelectOptions)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                ClosePanel();
                GetComponent<StoryManager>().JumpToLabel(_options[pointer.GetSelectedIndex()].JumpTo);
                GetComponent<StoryManager>().UnpauseStory();
                isSelectOptions = false;
                return;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                pointer.PointerUp();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                pointer.PointerDown();
            }
        }
    }

    IEnumerator WaitAndShow()
    {
        yield return new WaitForSeconds(0.1f);
        isSelectOptions = true;
    }
}

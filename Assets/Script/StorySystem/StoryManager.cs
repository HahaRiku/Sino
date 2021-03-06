﻿using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

//[RequireComponent(typeof(StoryReader))]
public class StoryManager : MonoBehaviour
{
    /* 
     * 功能說明：
     *  負責解讀StoryData，跟StoryReader一起使用
     *  對話開始、選項切換、人物移動都由這裏控管
     */

    public StoryData 劇本;
    List<StoryData.StoryState> stories;
    StoryReader reader;
    DialogPanelController dialogPanel;
    ActionController moveContol;
    OptionControl optionControl;
    OuterScriptControl scriptControl;

    public int nowIndex = 0;
    int listCount = 0;

    bool isStoryFinish = false;
    bool PauseLock = true;

    public GameStateManager GM;
    ClickableRegion clickableRegion;
    public bool 一開始就執行;

    Dictionary<string, int> LabelPairs = new Dictionary<string, int>();

    float _timer = 0; //快跳模式下，state跳轉計時器
    float _waitTime = 0.2f; //快跳模式下，state跳轉所需等待時間

    bool isMovieMode = false;
    protected StoryManager() { }

    public bool IsStoryFinish()
    {
        return isStoryFinish;
    }

    public void BeginStory(float waitTime = 0)
    {  
        GM.StartEvent();
        GM.SetStoryManager(this);
        isStoryFinish = false;
        PauseLock = true;
        StartCoroutine(WaitAndWork(waitTime, "ExecuteState"));
    }

    public void PauseStory()
    {
        PauseLock = true;
    }

    public void UnpauseStory()
    {
        PauseLock = false;
    }

    public void StopStory()
    {
        GM.FinEvent();
        GM.SetStoryManager(null);
        isStoryFinish = true;
        PauseLock = true;
        nowIndex = listCount;
    }

    void OnEnable()
    {
        if (劇本 == null)
        {
            Debug.Log("Error: No 劇本 has found.");
            enabled = false;
            return;
        }
        stories = 劇本.StateList;
        if (stories == null)
        {
            Debug.Log("Error: No stories has found.");
            enabled = false;
            return;
        }
        if (GetComponent<StoryReader>() == null)
            gameObject.AddComponent<StoryReader>();
        if (GetComponent<ActionController>() == null)
            gameObject.AddComponent<ActionController>();
        if (GetComponent<OptionControl>() == null)
            gameObject.AddComponent<OptionControl>();
        reader = GetComponent<StoryReader>();
        dialogPanel = FindObjectOfType<DialogPanelController>();
        if (dialogPanel == null) {
            Debug.Log("Error: No Dialog has found.");
            enabled = false;
            return;
        }
        moveContol = GetComponent<ActionController>();
        optionControl = GetComponent<OptionControl>();
        scriptControl = FindObjectOfType<OuterScriptControl>();
        GM = FindObjectOfType<GameStateManager>();
        listCount = stories.Count;
        if (一開始就執行)
            BeginStory(0.5f);
    }
    private void Start()
    {
        clickableRegion = FindObjectOfType<ClickableRegion>();
    }

    void Update()
    {
        if (isStoryFinish)
            return;
        else if (!PauseLock)
        {
            if (IsThisStateFinished())
            {
                if (Input.GetKeyDown(KeyCode.Z) || (Input.GetMouseButtonDown(0) && clickableRegion.IsMouseEnter) || 長按判定跳轉())
                {
                    if(CheckStateCondition(StoryData.StoryState.condition.完成等待滑鼠或鍵盤點擊))
                        return;
                }
                if (CheckStateCondition(StoryData.StoryState.condition.等待此完成))
                    return;
            }
            CheckStateCondition(StoryData.StoryState.condition.直接繼續);
        }
    }
    bool IsThisStateFinished()
    {
        if (stories[nowIndex].state類型 == StoryData.StoryState.type.人物移動)
            return moveContol.IsFinished();
        else if (stories[nowIndex].state類型 == StoryData.StoryState.type.故事對話)
            return reader.IsFinished();
        else if (stories[nowIndex].state類型 == StoryData.StoryState.type.心情氣泡)
            return true;
        else if (stories[nowIndex].state類型 == StoryData.StoryState.type.等待時間)
            return IsWaitingFinish();
        else
            return true;
    }

    bool CheckStateCondition(StoryData.StoryState.condition condition)
    {
        if (stories[nowIndex].continue條件 == condition)
        {
            if (nowIndex + 1 == listCount)
            {
                reader.ClosePanel();
                isStoryFinish = true;
                StopStory();
                return true;
            }
            ExecuteState(++nowIndex);
            return true;
        }
        return false;
    }

    void ExecuteState(int index)
    {
        nowIndex = index;
        if (stories[index].state類型 == StoryData.StoryState.type.故事對話)
        {
            if (index < listCount - 1 && stories[index + 1].state類型 == StoryData.StoryState.type.鏡頭模式 && stories[nowIndex].Mode == StoryData.StoryState.CameraMode.下降)
                reader.ToMoveCameraNextTime();
            reader.StartTyping(stories[nowIndex]);
        }
        else if (stories[index].state類型 == StoryData.StoryState.type.人物移動)
        {
            if (dialogPanel.IsVisible())
            {
                reader.ClosePanel();
            }
            moveContol.Move(stories[index].Character, stories[index].NewPositionX, stories[index].Duration);
        }
        else if (stories[index].state類型 == StoryData.StoryState.type.分支)
        {
            if (stories[nowIndex].bCondition == StoryData.StoryState.BranchCondition.變數)
            {
                var 變數名稱 = stories[nowIndex].Flag.Trim();
                if (stories[nowIndex].JustJump > 0)
                    ExecuteState(stories[nowIndex].JustJump);
                else if (!SystemVariables.otherVariables_int.ContainsKey(變數名稱))
                {
                    Debug.Log("不存在的變數: " + 變數名稱);
                    var ElseState = stories[nowIndex].ElseJumpTo.Trim();
                    if (CheckIsStateID(ElseState))
                        JumpToStateID(ElseState);
                    else
                        JumpToLabel(ElseState);
                }
                else
                {
                    var 變數value = SystemVariables.otherVariables_int[變數名稱];
                    string JumpState;
                    if (變數value == stories[nowIndex].WhenFlagIs)
                        JumpState = stories[nowIndex].ThanJumpTo.Trim();
                    else
                        JumpState = stories[nowIndex].ElseJumpTo.Trim();

                    if (CheckIsStateID(JumpState))
                        JumpToStateID(JumpState);
                    else
                        JumpToLabel(JumpState);
                }
            }
            else
            {
                var 物件名稱 = stories[nowIndex].Item.Trim();
                if (stories[nowIndex].JustJump > 0)
                    ExecuteState(stories[nowIndex].JustJump);
                else if (!BagSystem.IsItemNameExisted(物件名稱))
                {
                    Debug.Log("不存在的物件: " + 物件名稱);
                    var ElseState = stories[nowIndex].ElseJumpTo.Trim();
                    if (CheckIsStateID(ElseState))
                        JumpToStateID(ElseState);
                    else
                        JumpToLabel(ElseState);
                }
                else
                {
                    var 物件存在與否 = BagSystem.IsItemInBag(物件名稱);
                    string JumpState;
                    if (物件存在與否 == stories[nowIndex].WhenItemExisted)
                        JumpState = stories[nowIndex].ThanJumpTo.Trim();
                    else
                        JumpState = stories[nowIndex].ElseJumpTo.Trim();

                    if (CheckIsStateID(JumpState))
                        JumpToStateID(JumpState);
                    else
                        JumpToLabel(JumpState);
                }
            }
        }
        else if (stories[index].state類型 == StoryData.StoryState.type.指派變數)
        {
            SystemVariables.AddIntVariable(stories[nowIndex].Variable, stories[nowIndex].Value);
        }
        else if (stories[index].state類型 == StoryData.StoryState.type.等待時間)
        {
            StartCoroutine(Wait(stories[nowIndex].WaitTime));
        }
        else if (stories[index].state類型 == StoryData.StoryState.type.跳轉標籤)
        {
            JumpToLabel(stories[nowIndex].LabelJump.Trim());
        }
        else if (stories[index].state類型 == StoryData.StoryState.type.轉換場景)
        {
            var 場景名 = stories[nowIndex].Scene;
            if (!Application.CanStreamedLevelBeLoaded(場景名))
            {
                Debug.LogError("Error：場景 \"" + 場景名 + "\" is not found.");
                return;
            }
            if ((int)stories[nowIndex].SpawnPoint == 6)
            {
                GM.黑幕轉場(場景名, new Vector3(stories[nowIndex].SpawnPointFreeX, -3.2f, 0), stories[nowIndex].Facing);
            }
            else
                GM.黑幕轉場(場景名, stories[nowIndex].SpawnPoint, stories[nowIndex].Facing);
        }
        else if (stories[index].state類型 == StoryData.StoryState.type.外部腳本)
        {
            string 腳本 = stories[nowIndex].Class.Trim();
            var storyEvent = scriptControl.Events.Find(x => x.Name.Trim() == 腳本);
            if (storyEvent == null)
            {
                Debug.LogError("Error：外部腳本 \"" + 腳本 + "\" is not found.");
                return;
            }
            storyEvent.Event.Invoke();
        }
        else if (stories[index].state類型 == StoryData.StoryState.type.出現選項)
        {
            optionControl.ShowOptions(stories[nowIndex]);
        }
        else if (stories[index].state類型 == StoryData.StoryState.type.鏡頭模式)
        {
            if (index < listCount - 1 && stories[index + 1].state類型 == StoryData.StoryState.type.故事對話 && stories[nowIndex].Mode == StoryData.StoryState.CameraMode.上移)
                reader.ToMoveCameraNextTime();
            else if (dialogPanel.IsVisible() && stories[nowIndex].Mode == StoryData.StoryState.CameraMode.下降)
                reader.ToMoveCameraNextTime();
            else if (stories[nowIndex].Mode == StoryData.StoryState.CameraMode.上移)
                dialogPanel.GetComponent<DialogAnimation>().StartMovieOpeningAnim();
            else if (stories[nowIndex].Mode == StoryData.StoryState.CameraMode.下降 && stories[index - 1].state類型 != StoryData.StoryState.type.故事對話)
                dialogPanel.GetComponent<DialogAnimation>().StartMovieEndingAnim();
            /*if (stories[nowIndex].Mode == StoryData.StoryState.CameraMode.上移)
            {
                if (stories[index + 1].state類型 == StoryData.StoryState.type.故事對話)
                    reader.SetMode(true);
                else
                    dialogPanel.GetComponent<DialogAnimation>().StartMovieOpeningAnim();
            }
            else if (stories[nowIndex].Mode == StoryData.StoryState.CameraMode.下降)
            {
                if (stories[index + 1].state類型 == StoryData.StoryState.type.故事對話)
                    dialogPanel.GetComponent<DialogAnimation>().StartMovieEndingAnim();
            }*/
        }
    }
    IEnumerator WaitAndWork(float waitTime, string funcName)
    {
        LabelClear();
        LabelUpdate();
        yield return new WaitForSecondsRealtime(waitTime);
        PauseLock = false;
        ExecuteState(0);
    }

    void LabelUpdate()
    {
        int i = 0;
        foreach (StoryData.StoryState state in stories)
        {
            if (state.state類型 == StoryData.StoryState.type.設置標籤)
                LabelPairs.Add(state.Label.Trim(), i);
            i++;
        }
    }

    void LabelClear()
    {
        LabelPairs.Clear();
    }
    bool CheckIsStateID(string s)
    {
        return Regex.IsMatch(s, @"^\d+$");
    }
    bool 長按判定跳轉()
    {
        if (clickableRegion.IsMouseHold)  //長按判定 -> 開啟快跳模式
        {
            if (_timer >= _waitTime)
            {
                _timer = 0;
                return true;
            }
            else
                _timer += Time.deltaTime;
        }
        else
            _timer = 0;
        return false;
    }

    int isWaiting = 0;
    bool IsWaitingFinish()
    {
        return isWaiting == 0 ? true : false;
    }
    IEnumerator Wait(float waitTime)
    {
        isWaiting++;
		yield return new WaitForSecondsRealtime (waitTime * Time.timeScale);
        isWaiting--;
    }

    public void JumpToLabel(string 標籤名稱)
    {
        if (!LabelPairs.ContainsKey(標籤名稱))
        {
            Debug.LogError("Error：Label \"" + 標籤名稱 + "\" is not found.");
            return;
        }
        nowIndex = LabelPairs[標籤名稱];
        ExecuteState(nowIndex);
    }

    public void JumpToStateID(string ID)
    {
        int nextState_Or_YouCanSayIndexPlusOne = int.Parse(ID);
        if (nextState_Or_YouCanSayIndexPlusOne > listCount || nextState_Or_YouCanSayIndexPlusOne <= 0)
        {
            Debug.LogError("分支State " + (nowIndex + 1) + " 想跳到StateList外: " + nextState_Or_YouCanSayIndexPlusOne);
            StopStory();
            return;
        }
        nowIndex = nextState_Or_YouCanSayIndexPlusOne - 1;
        ExecuteState(nowIndex);
    }
}

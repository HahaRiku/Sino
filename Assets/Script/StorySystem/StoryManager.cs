using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

//[RequireComponent(typeof(StoryReader))]
public class StoryManager : Singleton<StoryManager>
{
    /* 
     * 功能說明：
     *  負責解讀StoryData，跟StoryReader一起使用
     *  對話開始、選項切換、人物移動都由這裏控管
     */

    public string 劇本名稱;
    StoryData 劇本;
    List<StoryData.StoryState> stories;
    //GameObject clickRegion;
    StoryReader reader;
    MovementControl moveContol;
  
    public int nowIndex = 0;
    int listCount = 0;

    bool isStoryFinish = false;
    bool PauseLock = true;

    TwoF_GameManager GM;
    //public bool 一開始就執行;

    Dictionary<string, int> LabelPairs = new Dictionary<string, int>();

    protected StoryManager() { }

    public bool IsStoryFinish()
    {
        return isStoryFinish;
    }

    public void BeginStory()
    {  
        if(!(劇本 = (StoryData)Resources.Load(劇本名稱)))
        {
            Debug.LogError("Error: \"" + 劇本名稱 + "\" 劇本讀入錯誤！請檢查 Assets/Resources 是否存在此檔案名稱。");
            enabled = false;
            return;
        }
        /*if (GM == null)
            GM = GameObject.Find("GM").GetComponent<TwoF_GameManager>();
        GM.StartEvent();
        GM.SetStoryManager(this);*/
        stories = 劇本.StateList;
        listCount = stories.Count;
        isStoryFinish = false;
        PauseLock = true;
        StartCoroutine(WaitAndWork(0.5f, "ExecuteState"));
        //ExecuteState(0);
    }

    public void PauseStory()
    {
        PauseLock = true;
    }

    public void StopStory()
    {
        /*if (GM == null)
            GM = GameObject.Find("GM").GetComponent<TwoF_GameManager>();
        GM.FinEvent();
        GM.SetStoryManager(null);*/
        PauseLock = true;
        nowIndex = listCount;
    }

    void OnEnable()
    {
        if (GetComponent<StoryReader>() == null)
            gameObject.AddComponent<StoryReader>();
        if (GetComponent<MovementControl>() == null)
            gameObject.AddComponent<MovementControl>();
        reader = GetComponent<StoryReader>();
        moveContol = GetComponent<MovementControl>();
        /*if(一開始就執行)
            BeginStory();*/
    }

    void Update()
    {
        if (isStoryFinish)
            return;
        else if (!PauseLock)
        {
            if (IsThisStateFinished())
            {
                if (Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(0))
                {
                    if(CheckStateCondition(StoryData.StoryState.condition.完成等待滑鼠或鍵盤點擊))
                        return;
                }
                CheckStateCondition(StoryData.StoryState.condition.等待此完成);
            }
            CheckStateCondition(StoryData.StoryState.condition.直接繼續);
        }
    }
    bool IsThisStateFinished()
    {
        if (stories[nowIndex].state類型 == StoryData.StoryState.type.人物移動)
            return moveContol.IsFinished();
        else if (stories[nowIndex].state類型 == StoryData.StoryState.type.故事對話)
            return IsTalkFinished();
        else if (stories[nowIndex].state類型 == StoryData.StoryState.type.出現選項)
            return true;
        else if (stories[nowIndex].state類型 == StoryData.StoryState.type.心情氣泡)
            return true;
        else if (stories[nowIndex].state類型 == StoryData.StoryState.type.等待時間)
            return IsWaitingFinish();
        else
            return true;
    }
    /*
    bool MoveFinished()
    {
        return 
    }*/

    bool IsTalkFinished()
    {
        return reader.IsFinished();
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
            reader.StartTyping(stories[nowIndex]);
        }
        else if (stories[index].state類型 == StoryData.StoryState.type.人物移動)
        {
            reader.ClosePanel();
            //moveContol.Move(stories[nowIndex].Character, stories[nowIndex].OriPositionX, stories[nowIndex].NewPositionX, stories[nowIndex].Duration);
        }
        else if (stories[index].state類型 == StoryData.StoryState.type.分支)
        {
            var 變數名稱 = stories[nowIndex].Flag;
            if (stories[nowIndex].JustJump > 0)
                ExecuteState(stories[nowIndex].JustJump);
            else if (!SystemVariables.otherVariables_int.ContainsKey(變數名稱))
            {
                Debug.Log("不存在的變數: " + 變數名稱);
                var ElseState = stories[nowIndex].ElseJumpTo.Trim();
                if (CheckIsStateID(ElseState))
                {
                    int nextState_Or_YouCanSayIndexPlusOne = int.Parse(ElseState);
                    if (nextState_Or_YouCanSayIndexPlusOne > listCount || nextState_Or_YouCanSayIndexPlusOne <= 0)
                    {
                        Debug.LogError("分支State " + (nowIndex + 1) + " 想跳到StateList外: " + nextState_Or_YouCanSayIndexPlusOne);
                        StopStory();
                        return;
                    }
                    nowIndex = nextState_Or_YouCanSayIndexPlusOne - 1;
                }
                else
                {
                    if (!LabelPairs.ContainsKey(ElseState))
                    {
                        Debug.LogError("Error：Label \"" + ElseState + "\" is not found.");
                        StopStory();
                        return;
                    }
                    nowIndex = LabelPairs[ElseState];
                }
                ExecuteState(nowIndex);
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
                {
                    int nextState_Or_YouCanSayIndexPlusOne = int.Parse(JumpState);
                    if (nextState_Or_YouCanSayIndexPlusOne > listCount || nextState_Or_YouCanSayIndexPlusOne <= 0)
                    {
                        Debug.LogError("分支State " + (nowIndex + 1) + " 想跳到StateList外: " + nextState_Or_YouCanSayIndexPlusOne);
                        StopStory();
                        return;
                    }
                    nowIndex = nextState_Or_YouCanSayIndexPlusOne - 1;
                }
                else
                {
                    if (!LabelPairs.ContainsKey(JumpState))
                    {
                        Debug.LogError("Error：Label \"" + JumpState + "\" is not found.");
                        return;
                    }
                    nowIndex = LabelPairs[JumpState];
                }
                ExecuteState(nowIndex);
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
            var 標籤名稱 = stories[nowIndex].LabelJump;
            if (!LabelPairs.ContainsKey(標籤名稱))
            {
                Debug.LogError("Error：Label \"" + 標籤名稱 + "\" is not found.");
                return;
            }

            nowIndex = LabelPairs[標籤名稱];
            ExecuteState(nowIndex);
        }
    }
    IEnumerator WaitAndWork(float waitTime, string funcName)
    {
        LabelClear();
        LabelUpdate();
        yield return new WaitForSecondsRealtime(waitTime);
        /*if(funcName!=null)
        {
            Invoke(funcName, 0);
        }*/
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

    int isWaiting = 0;
    bool IsWaitingFinish()
    {
        return isWaiting == 0 ? true : false;
    }
    IEnumerator Wait(float waitTime)
    {
        isWaiting++;
        yield return new WaitForSecondsRealtime(waitTime);
        isWaiting--;
    }
}

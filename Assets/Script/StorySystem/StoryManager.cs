using System.Collections;
using System.Collections.Generic;
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
    //GameObject clickRegion;
    StoryReader reader;
    MovementControl moveContol;
  
    public int nowIndex = 0;
    int listCount = 0;

    bool isStoryFinish = false;
    bool PauseLock = true;

    TwoF_GameManager GM;
    public bool 一開始就執行;

    public bool IsStoryFinish()
    {
        return isStoryFinish;
    }

    public void BeginStory()
    {
        /*if (GM == null)
            GM = GameObject.Find("GM").GetComponent<TwoF_GameManager>();
        GM.StartEvent();
        GM.SetStoryManager(this);*/
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
        if (GetComponent<MovementControl>() == null)
            gameObject.AddComponent<MovementControl>();
        reader = GetComponent<StoryReader>();
        moveContol = GetComponent<MovementControl>();
        
        listCount = stories.Count;
        if(一開始就執行)
            BeginStory();
    }

    void Update()
    {
        if (isStoryFinish)
            return;
        else if (!PauseLock)
        {
            if (IsThisStateFinished())
            {
                if (stories[nowIndex].state類型 == StoryData.StoryState.type.故事對話 || Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(0))
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
        else //if(stories[nowIndex + 1].state類型 == StoryData.StoryState.type.故事對話)
            return TalkFinished();
    }
    /*
    bool MoveFinished()
    {
        return 
    }*/

    bool TalkFinished()
    {
        return reader.IsFinished();
    }

    bool CheckStateCondition(StoryData.StoryState.condition condition)
    {
        if (nowIndex + 1 < listCount && nowIndex + 1 > 0)
        {
            if (stories[nowIndex].continue條件 == condition)
            {
                ExecuteState(++nowIndex);
                return true;
            }
        }
        else if (nowIndex + 1 == listCount)
        {
            if (stories[nowIndex].continue條件 == condition)
            {
                reader.ClosePanel();
                isStoryFinish = true;
                StopStory();
                return true;
            }
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
            moveContol.Move(stories[nowIndex].Character, stories[nowIndex].OriPositionX, stories[nowIndex].NewPositionX, stories[nowIndex].Duration);
        }
        else if (stories[index].state類型 == StoryData.StoryState.type.分支)
        {
            var 變數名稱 = stories[nowIndex].Flag;
            if (stories[nowIndex].JustJump > 0)
                ExecuteState(stories[nowIndex].JustJump);
            else if (!Variables.VarList.ContainsKey(變數名稱))
            {    //Debug.Log("不存在的變數: " + 變數名稱);
                int nextState_Or_YouCanSayIndexPlusOne = stories[nowIndex].WhenFlagFalse;
                if (nextState_Or_YouCanSayIndexPlusOne > listCount || nextState_Or_YouCanSayIndexPlusOne <= 0)
                {
                    Debug.Log("分支State " + (nowIndex + 1) + " 想跳到StateList外: " + nextState_Or_YouCanSayIndexPlusOne);
                    StopStory();
                }
                else
                {
                    nowIndex = nextState_Or_YouCanSayIndexPlusOne - 1;
                }
                ExecuteState(nowIndex);
            }
            else
            { 
                if (Variables.VarList[變數名稱])
                {
                    int nextState_Or_YouCanSayIndexPlusOne = stories[nowIndex].WhenFlagTrue;
                    if (nextState_Or_YouCanSayIndexPlusOne > listCount || nextState_Or_YouCanSayIndexPlusOne <= 0)
                    {
                        Debug.Log("分支State " + (nowIndex + 1) + " 想跳到StateList外: " + nextState_Or_YouCanSayIndexPlusOne);
                        StopStory();
                    }
                    else
                    {
                        nowIndex = nextState_Or_YouCanSayIndexPlusOne - 1;
                    }
                }
                else
                {
                    int nextState_Or_YouCanSayIndexPlusOne = stories[nowIndex].WhenFlagFalse;
                    if (nextState_Or_YouCanSayIndexPlusOne > listCount || nextState_Or_YouCanSayIndexPlusOne <= 0)
                    {
                        Debug.Log("分支State " + (nowIndex + 1) + " 想跳到StateList外: " + nextState_Or_YouCanSayIndexPlusOne);
                        StopStory();
                    }
                    else
                    {
                        nowIndex = nextState_Or_YouCanSayIndexPlusOne - 1;
                    }
                }
                ExecuteState(nowIndex);
            }
        }
        else if (stories[index].state類型 == StoryData.StoryState.type.指派變數)
        {
            Variables.AddVariable(stories[nowIndex].Variable, stories[nowIndex].Value);
        }
    }
    IEnumerator WaitAndWork(float waitTime, string funcName)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        /*if(funcName!=null)
        {
            Invoke(funcName, 0);
        }*/
        PauseLock = false;
        ExecuteState(0);
    }
}

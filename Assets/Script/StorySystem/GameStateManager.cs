using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateManager : Singleton<GameStateManager> {

    public enum SceneStatus
    {
        演出中,
        自由探索
    };
    //指向目前玩家可以使用角色 null情況代表演出中
    public SceneStatus NowStatus;
    public GameObject Player;
    public StoryManager ActingStorySystem;
    bool PlayerControlLock = false;

    void Start()
    {
        Application.targetFrameRate = 60;
        if (Player == null)
            Player = Instantiate(Resources.Load("Characters/Sino")) as GameObject;
        DontDestroyOnLoad(Player);
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if(Player != null && Player.GetComponent<CharacterControl>() != null)
        {
            Player.GetComponent<CharacterControl>().SetIsPlayerCanControl(!PlayerControlLock);
            SystemVariables.lockMoving = PlayerControlLock;
        }
    }

    //==================狀態控制====================

    public void StartEvent()
    {
        NowStatus = SceneStatus.演出中;
        PlayerControlLock = true;
    }
       
    public void FinEvent()
    {
        NowStatus = SceneStatus.自由探索;
        PlayerControlLock = false;
    }

    public void SetStoryManager(StoryManager s)
    {
        ActingStorySystem = s;
    }

    //==================轉場控制====================

    public static Vector3[] transPos = {
        new Vector3(5.5f, -4.07f, 0),
        new Vector3(-5.5f, -4.07f, 0),
        new Vector3(0, -4.07f, 0),
        new Vector3(-2.3f, -4.07f, 0),
        new Vector3(-1.5f, -4.07f, 0),
        new Vector3(1.5f, -4.07f, 0)
    };

    public enum SpawnPoint { 右側, 左側, 中間, 中間偏左, 樓梯左側, 樓梯右側}

    public void 黑幕轉場(string sceneName, SpawnPoint point)
    {
        StartEvent();
        Player.GetComponent<CharacterControl>().SetPlayerStopMove();
        StartCoroutine(Loading(sceneName, transPos[(int)point]));
    }

    public void 黑幕轉場(string sceneName, Vector3 point)
    {
        StartEvent();
        Player.GetComponent<CharacterControl>().SetPlayerStopMove();
        StartCoroutine(Loading(sceneName, point));
    }

    IEnumerator Loading(string sceneName, Vector3 point)
    {
        GameObject blackImg = GameObject.Find("Canvas/Black");
        blackImg.GetComponent<Animator>().SetTrigger("FadeIn");
        yield return new WaitUntil(() => blackImg.GetComponent<Image>().color.a == 1);
        //完成淡入
        Player.transform.position = point;
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

        //進度條跑完
        async.allowSceneActivation = true;
        blackImg.GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitUntil(() => blackImg.GetComponent<Image>().color.a == 0);
        //完成淡出
        if (ActingStorySystem == null)
            FinEvent();
    }
}

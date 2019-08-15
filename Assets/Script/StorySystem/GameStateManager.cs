using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour {

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

    void Awake()
    {
        Application.targetFrameRate = 60;
        Player = GameObject.Find("Sino");
        if (Player == null)
        {
            Player = Instantiate(Resources.Load("Characters/Sino")) as GameObject;
            Player.name = "Sino";
        }
        DontDestroyOnLoad(Player);
        SystemVariables.Scene = SceneManager.GetActiveScene().name;
    }

    void Update()
    {
        if(Player != null && Player.GetComponent<PlayerController>() != null)
        {
            Player.GetComponent<PlayerController>().SetIsPlayerCanControl(!PlayerControlLock);
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

    public void OpenBag()
    {
        PlayerControlLock = true;
    }

    public void CloseBag() {
        PlayerControlLock = false;
    }

    public void SetStoryManager(StoryManager s)
    {
        ActingStorySystem = s;
    }

    //==================轉場控制====================

    public static Vector3[] transPos = {
        new Vector3(5.5f, -3.2f, 0),
        new Vector3(-5.5f, -3.2f, 0),
        new Vector3(0, -3.2f, 0),
        new Vector3(-2.3f, -3.2f, 0),
        new Vector3(-1.5f, -3.2f, 0),
        new Vector3(1.5f, -3.2f, 0)
    };

    public enum SpawnPoint { 右側, 左側, 中間, 中間偏左, 樓梯左側, 樓梯右側}

    public void 黑幕轉場(string sceneName, SpawnPoint point)
    {
        StartEvent();
        StartCoroutine(Loading(sceneName, transPos[(int)point]));
    }

    public void 黑幕轉場(string sceneName, Vector3 point)
    {
        StartEvent();
        StartCoroutine(Loading(sceneName, point));
        Destroy(Player);
    }

    IEnumerator Loading(string sceneName, Vector3 point)
    {
        GameObject blackImg = GameObject.Find("Canvas/Black");
        blackImg.GetComponent<Animator>().SetTrigger("FadeIn");
        yield return new WaitUntil(() => blackImg.GetComponent<Image>().color.a == 1);
        //完成淡入
        Player.transform.position = point;
        SceneManager.LoadScene(sceneName);
        /*AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

        //進度條跑完
        async.allowSceneActivation = true;*/
        blackImg.GetComponent<Animator>().SetTrigger("FadeOut");
        /*yield return new WaitUntil(() => blackImg.GetComponent<Image>().color.a == 0);
        //完成淡出
        if (ActingStorySystem == null)
            FinEvent();*/
    }
}

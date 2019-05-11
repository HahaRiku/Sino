using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoF_GameManager : MonoBehaviour {

    public enum SceneStatus
    {
        演出中,
        自由探索
    };
    //指向目前玩家可以使用角色 null情況代表演出中
    public SceneStatus NowStatus;
    public GameObject Player;
    public StoryManager ActingStorySystem;
    //public ObjectStatusDescriptor[] ObjectDescList;
    bool PlayerControlLock = false;

    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        /*//GlobalSysData.GamePause = false;
        if (!GlobalSysData.GamePause && GlobalSysData.PlayerPosTmp != Vector3.zero)
        {
            UserCharater.transform.position = GlobalSysData.PlayerPosTmp;
            GlobalSysData.PlayerPosTmp = new Vector3(0, 0, 0);
        }*/
    }

    void Update()
    {
        /*if (Player != null && Player.GetComponent<PlayerController>() != null)
        {

            //Player.GetComponent<PlayerController>().SetIsPlayerCanControl(!PlayerControlLock);

        }
        /*
        //print(GlobalSysData.PlayerPosTmp);
        if (UserCharater != null && UserCharater.activeSelf && UserCharater.GetComponent<WalkingControl>() != null)
        {
            if (GlobalSysData.GamePause || PlayerControlLock)
                UserCharater.GetComponent<WalkingControl>().IO.GetComponent<MoveNewVer1>().movelock = true;
            else
                UserCharater.GetComponent<WalkingControl>().IO.GetComponent<MoveNewVer1>().movelock = false;
        }

        GlobalSysData.ActorPtr = this;
        GlobalSysData.Scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;*/
    }

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
        /*if (GlobalSysData.isLoadingStart)
            e = GlobalSysData.ECTmp;*/
    }
}

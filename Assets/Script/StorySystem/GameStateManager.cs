using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour {

    public enum SceneStatus
    {
        自由探索,
		演出中
    };
    //指向目前玩家可以使用角色 null情況代表演出中
    public SceneStatus NowStatus;
    public GameObject Player;
    public StoryManager ActingStorySystem;
    bool PlayerControlLock = false;

    //小地圖參數，floor=樓層(-3~3)，corridorPlace=走廊位置 左到右(0~7)
    
    public enum Floor { 地下三樓, 地下二樓, 地下一樓, 一樓, 二樓, 三樓}
    public enum PosIndex { 一, 二, 三, 四, 五, 六, 七, 八 }
    [Space(10)]
    public Floor 第幾層;
    public PosIndex 從左邊數來第幾個;

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
        SystemVariables.lockBag = true;
    }
       
    public void FinEvent()
    {
        NowStatus = SceneStatus.自由探索;
        PlayerControlLock = false;
        SystemVariables.lockBag = false;
    }

    public void OpenBag()
    {
        PlayerControlLock = true;
        SystemVariables.lockNPCinteract = true;
        SystemVariables.lockMenu = true;
    }

    public void CloseBag() {
        PlayerControlLock = false;
        SystemVariables.lockNPCinteract = false;
        SystemVariables.lockMenu = false;
    }

    public void OpenMenu() {
        PlayerControlLock = true;
        SystemVariables.lockNPCinteract = true;
        SystemVariables.lockBag = true;
    }

    public void CloseMenu() {
        PlayerControlLock = false;
        SystemVariables.lockNPCinteract = false;
        SystemVariables.lockBag = false;
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
        new Vector3(1.5f, -3.2f, 0),
        new Vector3(0.0f, -3.2f, 0)
    };

    public enum SpawnPoint { 右側, 左側, 中間, 中間偏左, 樓梯左側, 樓梯右側, 其他}
	public enum Facing { 保留, 左, 右, 反向}

    public void 黑幕轉場(string sceneName, SpawnPoint point, Facing facing)
    {
        StartEvent();
        StartCoroutine(Loading(sceneName, transPos[(int)point], facing));
    }

    public void 黑幕轉場(string sceneName, Vector3 point, Facing facing)
    {
        StartEvent();
        StartCoroutine(Loading(sceneName, point, facing));
        Destroy(Player);
    }

    IEnumerator Loading(string sceneName, Vector3 point, Facing facing)
    {
        GameObject blackImg = GameObject.Find("Canvas/Black");
        blackImg.GetComponent<Animator>().SetTrigger("FadeIn");
        yield return new WaitUntil(() => blackImg.GetComponent<Image>().color.a == 1);
        //完成淡入
        Player.transform.position = point;
		PlayerController playerScript = Player.GetComponent<PlayerController>();
		if(facing == Facing.左){playerScript.AnimationController("idle", false) ;}
		else if(facing == Facing.右){playerScript.AnimationController("idle", true);}
		else if(facing == Facing.反向){playerScript.AnimationController("idle", !playerScript.GetIsRight());}
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

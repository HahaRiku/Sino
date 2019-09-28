using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StoryData", menuName = "The ways of freedom/Story Data", order = 0)]
public class StoryData : ScriptableObject
{
    public List<StoryState> StateList = new List<StoryState>();

    [System.Serializable]
    public class StoryState
    {
        public enum type { 故事對話, 人物移動, 分支, 指派變數, 出現選項, 心情氣泡, 設置標籤, 跳轉標籤, 外部腳本, 等待時間, 轉換場景 }
        public type state類型;

        public enum condition { 完成等待滑鼠或鍵盤點擊, 等待此完成, 直接繼續 }
        public condition continue條件;

        //故事對話
        public string Name;
        public string Text;
        public Sprite Image; //立繪
        public Vector2 Location; //立繪位置
        public bool IsFlip; //立繪左右相反

        //人物移動
        public string Character;
        public float OriPositionX;
        public float NewPositionX;
        public float Duration;

        //分支
        //ex: year (2019)、2020、20、19 => jump to state 19
        public string Flag; 
        public int WhenFlagIs; 
        public string ThanJumpTo;
        public string ElseJumpTo;
        public int JustJump;

        //指派變數
        public string Variable;
        public int Value;

        //轉換場景
        public string Scene;
        public GameStateManager.SpawnPoint SpawnPoint;

        //選項
        public SelectOption[] Options = new SelectOption[2];

        //心情氣泡
        public string CharName;
        public string Emotion;

        //設置標籤
        /// <summary>
        /// 設置的標籤名稱
        /// </summary>
        public string Label;

        //跳轉標籤
        /// <summary>
        /// 用來跳轉的標籤名稱
        /// </summary>
        public string LabelJump;

        //外部腳本
        public string Class;
        public float[] Parameters = new float[3];

        //等待時間
        public float WaitTime;


        public StoryState(string Name, string Text)
        {
            this.Name = Name;
            this.Text = Text;
            state類型 = type.故事對話;
            continue條件 = condition.完成等待滑鼠或鍵盤點擊;
        }

        public StoryState(string Character, int OriPositionX, int NewPositionX, int Duration)
        {
            this.Character = Character;
            this.OriPositionX = OriPositionX;
            this.NewPositionX = NewPositionX;
            this.Duration = Duration;
            state類型 = type.人物移動;
            continue條件 = condition.等待此完成;
        }
    }
    public StoryState this[int key]
    {
        get
        {
            return StateList[key];
        }
        set
        {
            StateList[key] = value;
        }
    }

}
[System.Serializable]
public struct SelectOption
{
    public string Content; //選項文字
    public string JumpTo; //選擇跳轉label名
}
using System.Collections.Generic;
using UnityEngine;

public class StoryData : ScriptableObject
{
    public List<StoryState> StateList = new List<StoryState>();

    [System.Serializable]
    public class StoryState
    {
        public enum type { 故事對話, 人物移動, 分支, 指派變數 }
        public type state類型;

        public enum condition { 完成等待滑鼠或鍵盤點擊, 等待此完成, 直接繼續 }
        public condition continue條件;

        public string Name;
        public string Text;
        public Sprite Image;

        public string Character;
        public float OriPositionX;
        public float NewPositionX;
        public float Duration;

        public string Flag;
        public int WhenFlagTrue;
        public int WhenFlagFalse;
        public int JustJump;

        public string Variable;
        public bool Value;

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
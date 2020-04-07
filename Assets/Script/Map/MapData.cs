using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapData")]
public class MapData : ScriptableObject {
    public Floor[] floors;
}

[System.Serializable]
public class Floor {
    public enum FloorType {
        F3, 
        F2,
        F1,
        B1,
        B2,
        B3
    }
    public FloorType 樓層;
    public Sprite floorNum;
    public Room[] rooms;
}

[System.Serializable]
public class Room {
    public enum Type {
        只有走廊,
        有房間
    }
    public int 從左數來第幾間 = 1; //左樓梯: -1, 右樓梯: -2
    public Type 房間類型;
    public string 走廊場景名稱;
    public string 房間場景名稱_如果有;
    public bool 進去過了;
    public bool 走過;
    public float 房間左邊界Pos;
    public Sprite icon;
}

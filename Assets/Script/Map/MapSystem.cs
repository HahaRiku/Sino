using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapSystem {
    public static MapData data = Resources.Load<MapData>("MapData/MapData");
    public static bool dirty = true;

    public static bool HaveEnteredTheRoom(string roomName, Floor.FloorType floorType) {
        foreach(Floor f in data.floors) {
            if(f.樓層 == floorType) {
                foreach(Room r in f.rooms) {
                    if(r.房間類型 == Room.Type.有房間 && r.房間場景名稱_如果有 == roomName) {
                        return r.進去過了;
                    }
                }
            }
        }
        Debug.Log("MapSysteml：找不到所指房間：" + roomName +" in " + floorType.ToString() + "樓");
        return false;
    }

    public static void SetEnteredTheRoom(string roomName, Floor.FloorType floorType) {
        foreach(Floor f in data.floors) {
            if(f.樓層 == floorType) {
                foreach(Room r in f.rooms) {
                    if(r.房間類型 == Room.Type.有房間 && r.房間場景名稱_如果有 == roomName) {
                        r.進去過了 = true;
                        dirty = true;
                        return;
                    }
                }
            }
        }
        Debug.Log("MapSysteml：找不到所指房間" + roomName + " in " + floorType.ToString() + "樓");
        return;
    }

    public static void SetPassedBy(string corridorName, Floor.FloorType floorType) {
        foreach(Floor f in data.floors) {
            if(f.樓層 == floorType) {
                foreach(Room r in f.rooms) {
                    if(r.走廊場景名稱 == corridorName) {
                        r.走過 = true;
                        dirty = true;
                        return;
                    }
                }
            }
        }
        Debug.Log("MapSysteml：找不到所指走廊" + corridorName + " in " + floorType.ToString() + "樓");
        return;
    }

    public static bool HavePassedBy(string corridorName, Floor.FloorType floorType) {
        foreach (Floor f in data.floors) {
            if (f.樓層 == floorType) {
                foreach (Room r in f.rooms) {
                    if (r.走廊場景名稱 == corridorName) {
                        return r.走過;
                    }
                }
            }
        }
        Debug.Log("MapSysteml：找不到所指走廊" + corridorName + " in " + floorType.ToString() + "樓");
        return false;
    }

    public static void Flush() {
        foreach(Floor f in data.floors) {
            foreach(Room r in f.rooms) {
                r.走過 = false;
                r.進去過了 = false;
            }
        }
    }
}

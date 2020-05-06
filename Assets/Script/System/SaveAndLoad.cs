using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[System.Serializable]
public class SavedData {

    public string scene;
    public long playedTime;
    public long systemTime;
    public float playerX;
    public string globalVariables_int;
    public string lockStatus;
    public bool lockMoving;
    public bool lockNpcInteract;
    public bool lockBag;
    public bool lockMenu;
    public bool lockOtherNpc;

}

public static class SaveAndLoad {

    public static void Save(int id) {
        string scene = SystemVariables.Scene;
        long playedTime = SystemVariables.PlayedTime;
        long SystemTime = (Int64)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        string GlobalVars_int = JsonUtility.ToJson(SystemVariables.otherVariables_int, false);
        string LockStatus = JsonUtility.ToJson(SystemVariables.lockLockOrNot, false);

        bool lockMoving = SystemVariables.lockMoving;
        bool lockNPCinteract = SystemVariables.lockNPCinteract;
        bool lockBag = SystemVariables.lockBag;
        bool lockMenu = SystemVariables.lockMenu;
        bool lockOtherNPC = SystemVariables.lockOtherNPC;

        SavedData SavedD = new SavedData {
            scene = scene,
            playedTime = playedTime,
            systemTime = SystemTime,
            globalVariables_int = GlobalVars_int,
            lockStatus = LockStatus,
            lockMoving = lockMoving,
            lockNpcInteract = lockNPCinteract,
            lockBag = lockBag,
            lockMenu = lockMenu,
            lockOtherNpc = lockOtherNPC
        };

        string data = JsonUtility.ToJson(SavedD, false);

        string fileName = "save" + id + ".json";
        Debug.Log(id);
        StreamWriter file = new StreamWriter(Path.Combine(Application.persistentDataPath, fileName));
        file.Write(data);
        file.Close();
    }

    public static SavedData Load(int id) {
        string fileName = "save" + id + ".json";
        StreamReader file = new StreamReader(Path.Combine(Application.persistentDataPath, fileName));
        string loadJson = file.ReadToEnd();
        file.Close();

        SavedData loadData = new SavedData();
        loadData = JsonUtility.FromJson<SavedData>(loadJson);

        SystemVariables.Scene = loadData.scene;
        SystemVariables.PlayedTime = loadData.playedTime;
        SystemVariables.otherVariables_int = JsonUtility.FromJson<Dictionary<string, int>>(loadData.globalVariables_int);
        SystemVariables.lockLockOrNot = JsonUtility.FromJson<Dictionary<string, bool>>(loadData.lockStatus);
        SystemVariables.lockMoving = loadData.lockMoving;
        SystemVariables.lockNPCinteract = loadData.lockNpcInteract;
        SystemVariables.lockBag = loadData.lockBag;
        SystemVariables.lockMenu = loadData.lockMenu;
        SystemVariables.lockOtherNPC = loadData.lockOtherNpc;

        return loadData;
    }
}

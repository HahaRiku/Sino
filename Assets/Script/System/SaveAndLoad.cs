using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class SavedData {

    public string scene;
    public long playedTime;
    public long systemTime;
    public float playerX;
    public string globalVariables;

}

public class SaveAndLoad {

    public void Save(int id) {
        string savedFileName = id.ToString();
        string scene = SystemVariables.Scene;
        long playedTime = SystemVariables.PlayedTime;
        long SystemTime = (Int64)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        string GlobalVars = JsonUtility.ToJson(SystemVariables.otherVariables, false);
        //float playerX= //I need Actor or something

        SavedData SavedD = new SavedData {
            scene = scene,
            playedTime = playedTime,
            systemTime = SystemTime,
            //playerX
            globalVariables = GlobalVars
        };

        string data = JsonUtility.ToJson(SavedD, false);
    }

}

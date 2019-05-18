using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class SystemVariables {

    public static string Scene;
    public static long gameStartTime=0;
    public static long PlayedTime = 0;

    [SerializeField]
    public static Dictionary<string, int> OtherVariables = new Dictionary<string, int>();

}

public static class Init {
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad() {
        SystemVariables.gameStartTime = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
    }
}

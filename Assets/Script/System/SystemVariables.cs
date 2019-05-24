using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public static class SystemVariables {

    public static string Scene;
    public static long startGameTime= 0;
    public static long PlayedTime= 0;

    [SerializeField]
    public static Dictionary<string, int> otherVariables = new Dictionary<string, int>();

    public static void AddVariable(string varName, int varValue) {
        if (otherVariables.ContainsKey(varName)) {
            otherVariables[varName] = varValue;
        }
        else {
            otherVariables.Add(varName, varValue);
        }
    }

    public static void RemoveVariable(string varName) {
        if (otherVariables.ContainsKey(varName)) {
            otherVariables.Remove(varName);
        }
    }

    public static bool IsVariableExisted(string varName) {
        return otherVariables.ContainsKey(varName);
    }

    public static void FlushVariables() {
        otherVariables.Clear();
    }

}

public static class Init {
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad() {
        //SceneDescInit.Init();
        SystemVariables.startGameTime = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
    }
}

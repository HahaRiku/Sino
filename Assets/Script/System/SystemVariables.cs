using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public static class SystemVariables {

    public static string Scene;
    public static long startGameTime= 0;
    public static long PlayedTime= 0;
    public static bool lockMoving = false;

    [SerializeField]
    public static Dictionary<string, int> otherVariables_int = new Dictionary<string, int>();

    [SerializeField]
    public static Dictionary<string, bool> otherVariables_bool = new Dictionary<string, bool>();

    public static void AddIntVariable(string varName, int varValue) {
        if (otherVariables_int.ContainsKey(varName)) {
            otherVariables_int[varName] = varValue;
        }
        else {
            otherVariables_int.Add(varName, varValue);
        }
    }

    public static void AddBoolVariable(string varName, bool varValue) {
        if (otherVariables_bool.ContainsKey(varName)) {
            otherVariables_bool[varName] = varValue;
        }
        else {
            otherVariables_bool.Add(varName, varValue);
        }
    }

    public static void RemoveIntVariable(string varName) {
        if (otherVariables_int.ContainsKey(varName)) {
            otherVariables_int.Remove(varName);
        }
    }

    public static void RemoveBoolVariable(string varName) {
        if (otherVariables_bool.ContainsKey(varName)) {
            otherVariables_bool.Remove(varName);
        }
    }

    public static bool IsIntVariableExisted(string varName) {
        return otherVariables_int.ContainsKey(varName);
    }

    public static bool IsBoolVariableExisted(string varName) {
        return otherVariables_bool.ContainsKey(varName);
    }

    public static void FlushIntVariables() {
        otherVariables_int.Clear();
    }

    public static void FlushBoolVariables() {
        otherVariables_bool.Clear();
    }

}

public static class Init {
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad() {
        //SceneDescInit.Init();
        SystemVariables.startGameTime = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
    }
}

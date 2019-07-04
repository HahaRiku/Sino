using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public static class SystemVariables {

    public static string Scene;
    public static long startGameTime= 0;
    public static long PlayedTime= 0;
    public static bool lockMoving = false;
    public static bool lockNPCinteract = false;
    public static bool lockBag = false;

    [SerializeField]
    public static Dictionary<string, int> otherVariables_int = new Dictionary<string, int>();

    [SerializeField]
    public static Dictionary<string, bool> otherVariables_bool = new Dictionary<string, bool>();

    [SerializeField]
    public static Dictionary<string, bool> doorLockOrNot = new Dictionary<string, bool>();      //string������W�r, bool��O�_���

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

    public static void AddDoorStatus(string varName, bool varValue) {
        if (doorLockOrNot.ContainsKey(varName)) {
            doorLockOrNot[varName] = varValue;
        }
        else {
            doorLockOrNot.Add(varName, varValue);
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

    public static void RemoveDoorStatus(string varName) {
        if (doorLockOrNot.ContainsKey(varName)) {
            doorLockOrNot.Remove(varName);
        }
    }

    public static bool IsIntVariableExisted(string varName) {
        return otherVariables_int.ContainsKey(varName);
    }

    public static bool IsBoolVariableExisted(string varName) {
        return otherVariables_bool.ContainsKey(varName);
    }

    public static bool IsDoorStatusExisted(string varName) {
        return doorLockOrNot.ContainsKey(varName);
    }

    public static void FlushIntVariables() {
        otherVariables_int.Clear();
    }

    public static void FlushBoolVariables() {
        otherVariables_bool.Clear();
    }

    public static void FlushDoorStatus() {
        doorLockOrNot.Clear();
    }

}

public static class Init {
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad() {
        //SceneDescInit.Init();
        SystemVariables.startGameTime = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        SystemVariables.FlushIntVariables();
        SystemVariables.FlushBoolVariables();
        SystemVariables.FlushDoorStatus();
        BagSystem.ResetItemsInBag();
    }
}

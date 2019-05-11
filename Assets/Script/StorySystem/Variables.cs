using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class Variables
{
    public static Dictionary<string, bool> VarList = new Dictionary<string, bool>();

    public static void AddVariable(string key, bool val)
    {
        if (VarList.ContainsKey(key))
            VarList[key] = val;
        else
            VarList.Add(key, val);
    }
    public static bool IsVarExisted(string key)
    {
        return VarList.ContainsKey(key);
    }
    public static void FlushVariables()
    {
        VarList.Clear();
    }
}

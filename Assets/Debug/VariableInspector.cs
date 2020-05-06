using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableInspector : MonoBehaviour
{
    public List<vk> VariableList = new List<vk>();
    public string 想要改的變數名字;
    public int 想要改的值;
    void Start()
    {
        Refresh();
    }

    public void SetVariable()
    {
        if (SystemVariables.IsIntVariableExisted(想要改的變數名字))
        {
            var oriValue = SystemVariables.otherVariables_int[想要改的變數名字];
            SystemVariables.AddIntVariable(想要改的變數名字, 想要改的值);
            Debug.Log(想要改的變數名字 + "已從" + oriValue + "改為" + 想要改的值);
            SystemVariables.AddIntVariable(想要改的變數名字, 想要改的值);
        }
        else
        {
            SystemVariables.AddIntVariable(想要改的變數名字, 想要改的值);
            Debug.Log(想要改的變數名字 + "已改為" + 想要改的值);
        }
    }

    public void Refresh()
    {
        VariableList.Clear();
        foreach (var pair in SystemVariables.otherVariables_int)
        {
            VariableList.Add(new vk { varName = pair.Key, varValue = pair.Value });
        }
    }
}
[System.Serializable]
public struct vk
{
    public string varName;
    public int varValue;
}

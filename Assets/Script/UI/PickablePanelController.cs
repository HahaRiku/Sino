using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickablePanelController : UnPickablePanelController
{
    public void SetQuest(string content)
    {
        transform.GetChild(1).GetChild(0).GetComponent<Text>().text = content;
    }
}

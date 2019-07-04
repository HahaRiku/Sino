using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickablePanelController : PanelController
{
    public void SetInfo(string name, string desc, Sprite img = null)
    {
        if (img)
            transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = img;
        transform.GetChild(0).GetChild(1).GetComponent<Text>().text = name;
        transform.GetChild(0).GetChild(2).GetComponent<Text>().text = desc;
    }
    public void SetQuest(string content)
    {
        transform.GetChild(1).GetChild(0).GetComponent<Text>().text = content;
    }
}

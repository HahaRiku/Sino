using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsPanelController : PanelController
{
    public void SetOptionCount(int length)
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(200, 35 + 50 * length);
    }
}

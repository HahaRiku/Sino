using UnityEngine;
using UnityEngine.UI;

public class OpenDoorPanelController : PanelController {

    public void SetQuest(string content)
    {
        transform.GetChild(0).GetChild(0).GetComponent<Text>().text = content;
    }
}

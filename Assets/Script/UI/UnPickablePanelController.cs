using UnityEngine;
using UnityEngine.UI;

public class UnPickablePanelController : PanelController
{
    bool isInteract = false;
    void Update()
    {
        if (isInteract)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                SetInvisible();
                isInteract = false;
            }
            if(!IsVisible())
                isInteract = false;
        }
    }
    public void SetInfo(string name, string desc, Sprite img = null)
    {
        if (img)
            transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = img;
        transform.GetChild(0).GetChild(1).GetComponent<Text>().text = name;
        transform.GetChild(0).GetChild(2).GetComponent<Text>().text = desc;
    }

    public void SetInfo(string desc)
    {
        transform.GetChild(0).GetChild(2).GetComponent<Text>().text = desc;
    }

    public new void SetVisible()
    {
        GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<CanvasGroup>().interactable = true;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        _v = true;

        Invoke("StartInteract", 0.1f);
    }
    void StartInteract()
    {
        isInteract = true;
    }
}

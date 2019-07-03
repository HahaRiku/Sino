using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogPanelController : MonoBehaviour
{
    bool _v;

    public bool IsVisible()
    {
        return _v;
    }
    public void SetInvisible()
    {
        //GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().interactable = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        GetComponent<DialogAnimation>().StartCloseAnim();
        _v = false;
    }
    public void SetVisible()
    {
        //GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<CanvasGroup>().interactable = true;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        GetComponent<DialogAnimation>().StartOpenAnim();
        _v = true;
    }
}

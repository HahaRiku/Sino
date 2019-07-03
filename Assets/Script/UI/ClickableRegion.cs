using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableRegion : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool IsMouseClick = false;
    public bool IsMouseEnter = false;
    public bool IsMouseHold = false;

    float timer = 0;
    float 長按時間 = 0.5f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsMouseEnter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsMouseEnter = false;
    }

     void Update()
    {
        if (IsMouseClick)
        {
            if (timer < 長按時間)
                timer += Time.deltaTime;
            else if (!IsMouseHold)
                IsMouseHold = true;
        }
        else if(Input.GetMouseButtonDown(0))
        {
            IsMouseClick = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            IsMouseClick = false;
            IsMouseHold = false;
            timer = 0;
        }
    }
}

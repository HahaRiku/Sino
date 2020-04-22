using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OptionPointer : MonoBehaviour
{
    Transform optionsPanel;
    int _count;

    public int _index = 0;

    private void Start()
    {
        optionsPanel = transform.parent.GetChild(0);
    }
    public void Initial(int numOfOptions)
    {
        _count = numOfOptions;
        for (int i = 0; i < optionsPanel.childCount; i++)
        {
            if (i < _count)
                optionsPanel.GetChild(i).gameObject.SetActive(true);
            else
                optionsPanel.GetChild(i).gameObject.SetActive(false);
        }
        SetSelectedIndex(0); //每當開啟選項，預設選擇第一個
    }
    public void PointerUp()
    {
        _index = _index == 0 ? _count - 1 : _index - 1;
        GetComponent<RectTransform>().anchoredPosition =
            new Vector2(GetComponent<RectTransform>().anchoredPosition.x,
            optionsPanel.GetChild(_index).GetComponent<RectTransform>().anchoredPosition.y);
    }

    public void PointerDown()
    {
        _index = _index == _count - 1 ? 0 : _index + 1;
        GetComponent<RectTransform>().anchoredPosition =
            new Vector2(GetComponent<RectTransform>().anchoredPosition.x,
            optionsPanel.GetChild(_index).GetComponent<RectTransform>().anchoredPosition.y);
    }

    public int GetSelectedIndex()
    {
        return _index;
    }

    public void SetSelectedIndex(int index)
    {
        _index = index;
        GetComponent<RectTransform>().anchoredPosition =
            new Vector2(GetComponent<RectTransform>().anchoredPosition.x,
            optionsPanel.GetChild(_index).GetComponent<RectTransform>().anchoredPosition.y);
    }
}

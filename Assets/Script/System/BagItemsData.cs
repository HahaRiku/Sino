using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BagItemData")]
public class BagItemsData : ScriptableObject {

    public List<BagItem> bagItemList = new List<BagItem>();

}

[System.Serializable]
public class BagItem {

    public string name;
    public Sprite sprite;
    [TextArea]
    public string desc;
    public bool inBag;

}

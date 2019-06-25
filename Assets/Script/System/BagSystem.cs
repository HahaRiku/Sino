using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class BagSystem {

    [SerializeField]
    public struct BagItem {
        public string name;
        public Sprite sprite;
        public string desc;
        public bool existed;
    }

    [SerializeField]
    public static List<BagItem> bagItemList = new List<BagItem>();

    public static void AddItemToBag(BagItem item) {
        bagItemList.Add(item);
    }

    public static void SetItemExisted(string name, bool existed) {
        BagItem tempItem;
        foreach (BagItem item in bagItemList) {
            if (name == item.name) {
                tempItem = item;
                break;
            }
        }
        tempItem.existed = existed;
    }

    public static void DeleteItemFromBag(string name) {
        foreach (BagItem item in bagItemList) {
            if (name == item.name) {
                bagItemList.Remove(item);
                break;
            }
        }
        
    }

    public static void ClearBag() {
        bagItemList.Clear();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**********************************
 * 
 * BagItemData的資料只有bool inBag會在遊戲中動到
 * data內資料被更動(基本上只有inBag會被更動)的話，會寫BagUI dirty
 * 
 * *******************************/


public static class BagSystem {

    public static BagItemsData data = Resources.Load<BagItemsData>("BagItemData/BagItemData");

    public static bool bagUIDirty = false ;

    public static void SetItemInBagOrNot(string name, bool inbag) {     //要設bagUI的dirty
        foreach (BagItem item in data.bagItemList) {
            if (name == item.name) {
                item.inBag = inbag;
                break;
            }
        }

        bagUIDirty = true;

    }

    public static void ResetItemsInBag() {      //開始新遊戲的時候用到(也要設bagUI dirty
        foreach (BagItem item in data.bagItemList) {
            item.inBag = false;
        }

        bagUIDirty = true;

    }

    public static string ReturnDescByName(string n) {
        foreach (BagItem item in data.bagItemList) {
            if (n == item.name) {
                return item.desc;
            }
        }
        Debug.Log("注意！可撿的物件NPC輸入錯名字：" + n);
        return "";
    }

    public static bool IsItemInBag(string n) {
        foreach (BagItem item in data.bagItemList) {
            if (n == item.name) {
                if (item.inBag) {
                    return true;
                }
                return false;
            }
        }
        return false;
    }

}

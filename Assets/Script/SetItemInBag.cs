using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetItemInBag : MonoBehaviour {

    public void SetItemInBagFunc(string itemName) {
        BagSystem.SetItemInBagOrNot(itemName, true);
    }

    public void SetItemNotInBagFunc(string itemName) {
        BagSystem.SetItemInBagOrNot(itemName, false);
    }

    public void GetItemAni(string itemName) {
        gameObject.GetComponent<BagUI>().GetItemAni(itemName);
    }
}

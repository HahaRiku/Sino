using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetItemInBag : MonoBehaviour {

    public void SetItemInBagFunc(string itemName) {
        BagSystem.SetItemInBagOrNot(itemName, true);
    }

    public void GetItemAni(string itemName) {
        FindObjectOfType<BagUI>().GetItemAni(itemName);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetItemInBag : MonoBehaviour {

    public string ItemName;

    public void SetInBag() {
        BagSystem.SetItemInBagOrNot(ItemName, true);
    }

}

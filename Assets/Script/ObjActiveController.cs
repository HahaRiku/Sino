using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjActiveController : MonoBehaviour {

    public enum Condition {
        變數,
        道具是否在背包
    }

    [System.Serializable]
    public struct Element {
        public Condition 要使用哪種判斷;
        public string 變數名;
        public int 如果上面的變數為多少才設置;
        public string 道具名稱;
        public bool 道具是否在背包;
        public GameObject 要設定Active的物件;
        public bool Active的值;
    }

    public List<Element> 檢查的序列 = new List<Element>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (檢查的序列.Count != 0) {
            foreach (Element e in 檢查的序列) {
                if (e.要使用哪種判斷 == Condition.變數) {
                    if (SystemVariables.IsIntVariableExisted(e.變數名)) {
                        if (SystemVariables.otherVariables_int[e.變數名] == e.如果上面的變數為多少才設置) {
                            e.要設定Active的物件.SetActive(e.Active的值);
                            檢查的序列.Remove(e);
                            return;
                        }
                    }
                }
                else if (e.要使用哪種判斷 == Condition.道具是否在背包) {
                    if (BagSystem.IsItemInBag(e.道具名稱) == e.道具是否在背包) {
                        e.要設定Active的物件.SetActive(e.Active的值);
                        檢查的序列.Remove(e);
                        return;
                    }
                }
            }
        }
	}
}

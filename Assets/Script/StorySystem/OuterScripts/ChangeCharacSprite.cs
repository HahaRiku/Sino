using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***********************************
 * 
 * 掛在要換圖的角色的gameObject上
 * 每個角色的架構都是
 * 
 * *********************************/

public class ChangeCharacSprite : MonoBehaviour {

    public GameObject SpriteRenderer在的GameObject;
    public Sprite[] 要換的sprite丟在這;
    public GameObject 如果有DragonBone;

    private SpriteRenderer SR;

	// Use this for initialization
	void Start () {
        SR = SpriteRenderer在的GameObject.GetComponent<SpriteRenderer>();
	}

    public void ChangeSprite(int index) {
        SR.sprite = 要換的sprite丟在這[index];
        if (如果有DragonBone != null) {
            如果有DragonBone.SetActive(false);
        }
        SpriteRenderer在的GameObject.SetActive(true);
    }

    public void ChangeBackToDragonBone() {
        SpriteRenderer在的GameObject.SetActive(false);
        如果有DragonBone.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour {
    public float speed;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.RightArrow)) {
            MoveRight();
        }
        else if (Input.GetKey(KeyCode.LeftArrow)) {
            MoveLeft();
        }
    }

    public void MoveRight() {
        transform.localPosition = new Vector3(transform.localPosition.x + speed, transform.localPosition.y, transform.localPosition.z);
        if (transform.localScale.x < 0) {
            transform.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
        }
    }

    public void MoveLeft() {
        transform.localPosition = new Vector3(transform.localPosition.x - speed, transform.localPosition.y, transform.localPosition.z);
        if (transform.localScale.x > 0) {
            transform.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
        }
    }
}

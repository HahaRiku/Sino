using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour {
    public float speed;
    private Animator ani;
    private bool KeyDown;

    void Start() {
        ani= gameObject.GetComponent<Animator>();
        KeyDown = false;
    }

    // Update is called once per frame
    void Update () {
        //if(CharWithItem.actEnable){
        if (!KeyDown)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                KeyDown = true;
                ani.SetBool("Walk", true);
            }
        }
        else {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.localPosition = new Vector3(transform.localPosition.x + speed, transform.localPosition.y, transform.localPosition.z);
                if (transform.localScale.x > 0)
                {
                    transform.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
                }
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.localPosition = new Vector3(transform.localPosition.x - speed, transform.localPosition.y, transform.localPosition.z);
                if (transform.localScale.x < 0)
                {
                    transform.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
                }
            }
            if ((Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow)))
            {
                KeyDown = false;
                ani.SetBool("Walk", false);
            }
        }
		//}
    }
}

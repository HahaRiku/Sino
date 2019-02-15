using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour {
    public float speed;
    public Dialog dialogComp;
    private Animator ani;
    private bool aniWalk;

    void Start() {
        ani= gameObject.GetComponent<Animator>();
        aniWalk = false;
    }

    // Update is called once per frame
    void Update () {
        //if(CharWithItem.actEnable){
        if (!(Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))) {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                aniWalk = true;
                ani.SetBool("Walk", true);
            }
            /*if ((Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))) {
                KeyDown = false;
                ani.SetBool("Walk", false);

            }*/
            if (Input.GetKey(KeyCode.RightArrow)) {
                if (!aniWalk) {
                    aniWalk = true;
                    ani.SetBool("Walk", true);
                }
                transform.localPosition = new Vector3(transform.localPosition.x + speed, transform.localPosition.y, transform.localPosition.z);
                if (transform.localScale.x > 0) {
                    transform.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
                }
            }
            if (Input.GetKey(KeyCode.LeftArrow)) {
                if (!aniWalk) {
                    aniWalk = true;
                    ani.SetBool("Walk", true);
                }
                transform.localPosition = new Vector3(transform.localPosition.x - speed, transform.localPosition.y, transform.localPosition.z);
                if (transform.localScale.x < 0) {
                    transform.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
                }
            }
            if (!(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))) {
                aniWalk = false;
                ani.SetBool("Walk", false);
            }
        }
        else {
            aniWalk = false;
            ani.SetBool("Walk", false);
        }
        //}
    }

    public void AutoWalk(float x) {
        StartCoroutine(WalkLerp(x));
    }

    IEnumerator WalkLerp(float x) {
        ani.SetBool("Walk", true);
        for (float i = transform.position.x; i < x; i += speed) {
            transform.localPosition = new Vector3(i, transform.position.y, transform.position.z);
            yield return null;
        }
        ani.SetBool("Walk", false);
        dialogComp.SetLineDone(true);
    }
}

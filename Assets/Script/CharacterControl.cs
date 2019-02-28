using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour {
    [System.Serializable]
    public struct CharacterObjectAndName {
        public string name;
        public GameObject obj;
    }

    public float speed;
    public Dialog dialogComp;
    public CharacterObjectAndName[] otherCharactersObjects;
    private Animator ani;
    private bool KeyDown;

    void Start() {
        ani= gameObject.GetComponent<Animator>();
        KeyDown = false;
    }

    // Update is called once per frame
    void Update () {
        //if(CharWithItem.actEnable){
        if(SystemVariables.PlayerCanMove) {
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
		}
    }

    public void AutoWalk(string name, float x) {
        StartCoroutine(WalkLerp(name, x));
    }

    IEnumerator WalkLerp(string name, float x) {
        ani.SetBool("Walk", true);
        for (float i = transform.position.x; i < x; i += speed) {
            transform.localPosition = new Vector3(i, transform.position.y, transform.position.z);
            yield return null;
        }
        ani.SetBool("Walk", false);
        dialogComp.SetLineDone(true);
        //ani.enabled = false;
    }
}

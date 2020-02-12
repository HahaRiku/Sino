using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HideIntoBarrel : MonoBehaviour {
    private Image 必須躲起來;
    private Image Number;
    private Animator ani;

    private bool start;
    private float startTime;

	// Use this for initialization
	void Start () {
        必須躲起來 = transform.GetChild(0).GetComponent<Image>();
        Number = transform.GetChild(1).GetComponent<Image>();
        必須躲起來.gameObject.SetActive(false);
        Number.gameObject.SetActive(false);
        ani = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		if(start) {
            if(SystemVariables.IsIntVariableExisted("序章-3_躲酒桶Finish")) {
                if (SystemVariables.otherVariables_int["序章-3_躲酒桶Finish"] == 1) {
                    gameObject.SetActive(false);
                }
            }
            else {
                float currentTime = Time.time - startTime;
                if (currentTime >= 5.5f) {
                    if (SystemVariables.IsIntVariableExisted("序章-3_躲酒桶Finish")) {
                        if (SystemVariables.otherVariables_int["序章-3_躲酒桶Finish"] == 1) {
                            start = false;
                        }
                        else {
                            SceneManager.LoadScene("GG");
                        }
                    }
                    else {
                        SceneManager.LoadScene("GG");
                    }
                }
                else if (currentTime >= 2.5f) {
                    ani.SetBool("start", false);
                }
            }
        }
	}

    public void GameStart() {
        start = true;
        ani.SetBool("start", true);
        startTime = Time.time;
    }
}

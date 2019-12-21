using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAnimation : MonoBehaviour {

    private GameObject bird;
    private Animator birdAni;

    void Start () {
        bird = GameObject.Find("bird");//*
        birdAni = bird.GetComponent<Animator>();
    }
	
    public void ChangeBirdAnimation(string anim)
    {
        switch (anim)
        {
            case "wing":
                birdAni.SetTrigger("wing");
                break;
            case "jump":
                birdAni.SetInteger("jumping", 0);
                birdAni.SetTrigger("jump");
                break;
            case "jumpEnd":
                birdAni.SetInteger("jumping", 1);
                break;
            default:break;
        }
    }    
}

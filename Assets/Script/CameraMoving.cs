using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour {
    public GameObject player;

    private Vector3 offset;


	// Use this for initialization
	void Start () {
        transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        offset = transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = player.transform.position + offset;
	}
}

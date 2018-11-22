using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itembutton : MonoBehaviour {
    public int id;
    List<int> idofitem = new List<int>(10);

    public List<int> Idofitem
    {
        get
        {
            return idofitem;
        }

        set
        {
            idofitem = value;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (id > 0)
            {
                id -= id;
            }
            else
            {
                id = 0;
            }
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            id += id;
        }
    }
}

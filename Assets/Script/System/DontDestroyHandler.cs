using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyHandler : MonoBehaviour
{
    public GameObject PreFab;
    public GameObject clone;

    void Awake()
    {
        if (GameObject.Find(PreFab.name) == null)
        {
            clone = Instantiate(PreFab);
            clone.name = PreFab.name;
        }
        else
        {
            clone = GameObject.Find(PreFab.name);
        }
        DontDestroyOnLoad(clone);
    }
}

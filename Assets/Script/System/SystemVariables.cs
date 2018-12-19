using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class SystemVariables {

    [SerializeField]
    public static List<Sprite> sino = new List<Sprite>();
    [SerializeField]
    public static List<Sprite> doctor = new List<Sprite>();

    public enum Progress {
        序幕,

    }

    public static bool ExploreDone=false ;


}

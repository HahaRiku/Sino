using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NotePagesData")]
public class NotePagesData : ScriptableObject {

    public List<NoteOnePage> notePagesList = new List<NoteOnePage>();

}

[System.Serializable]
public class NoteOnePage {

    public Sprite[] sprites;

}

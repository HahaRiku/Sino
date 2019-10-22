using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNote : MonoBehaviour {

    public bool testGetMission;
    public bool testGetMission2;
    public bool testGetMission3;
    public bool testOpenNote;
    public bool testCloseNote;

    public NoteController noteController;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (testGetMission) {
            testGetMission = false;
            NotePagesSystem.NewPage();
            NotePagesSystem.ChangeSprite(0, 0);
        }
        if(testGetMission2) {
            testGetMission2 = false;
            NotePagesSystem.NewPage();
            NotePagesSystem.ChangeSprite(1, 0);
        }
        if(testGetMission3) {
            testGetMission3 = false;
            NotePagesSystem.NewPage();
            NotePagesSystem.ChangeSprite(2, 0);
        }
        if (testOpenNote) {
            testOpenNote = false;
            noteController.OpenNote();
        }
        if (testCloseNote) {
            testCloseNote = false;
            noteController.CloseNote();
        }
    }

}

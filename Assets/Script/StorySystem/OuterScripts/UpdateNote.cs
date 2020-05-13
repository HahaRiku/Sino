using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateNote : MonoBehaviour {
    [System.Serializable]
    public struct UpdateInfo {
        public bool 開新一頁;
        public int NotePagesList的index;
        public int 該NotePages裡的index;
    }

    public UpdateInfo[] updateInfo;

    public void DoUpdate(int indexInUpdateInfo) {
        if(updateInfo[indexInUpdateInfo].開新一頁) {
            NotePagesSystem.NewPage();
        }
        NotePagesSystem.ChangeSprite(updateInfo[indexInUpdateInfo].NotePagesList的index, updateInfo[indexInUpdateInfo].該NotePages裡的index);
    }
}

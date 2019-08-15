using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**************************
 * 
 * 任務接取順序固定
 * 結束一個任務才能夠接下一個
 * 
 * 總頁數必定是偶數
 * 
 * 接到新任務: call NewPage() + ChangeSprite()
 * 單純換內容(sprite): 直接call ChangeSprite()
 * 
 * ***********************/

public static class NotePagesSystem {

    public static NotePagesData data = Resources.Load<NotePagesData>("NotePagesData/NotePagesData");
    private static int totalPages_data = (data.notePagesList.Count % 2 == 1) ? data.notePagesList.Count + 1 : data.notePagesList.Count;  //不算第0頁的總頁數
    public static int totalPages_current = 0;   //0, 1, 2, 3....
    public static Sprite[] bookPages = new Sprite[totalPages_data + 2]; //加上用不到的第0頁和最後一頁

    private static Sprite transparent = Resources.Load<Sprite>("Transparent");

    public static bool dirty = false;

    public static void NewPage() {
        totalPages_current += 1;
    }

    //因為接取任務是線性的 所以只換最新的任務
    //indexInPagesData: notePagesList的element index ((一個element包了一頁會替換的所有sprite
    //spriteIndex: 該element的哪一個sprite ((有鑑於可能不會照順序塞
    public static void ChangeSprite(int indexInPagesData, int spriteIndex) {
        if (totalPages_current > 0) {
            bookPages[totalPages_current] = data.notePagesList[indexInPagesData].sprites[spriteIndex];
        }
        dirty = true;
    }

    public static void ResetNotePages() {
        totalPages_current = 0;
        bookPages = new Sprite[totalPages_data + 2];
        for (int i = 0; i < totalPages_data + 2; i++) {
            bookPages[i] = transparent;
        }
    }

}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**************************
 * 
 * press B to open
 * 
 * ***************************/

public class BagUI : MonoBehaviour {

    public Sprite 框;
    public Sprite 有物件的底;
    public GameObject DescriptionPrefab;
    public Sprite 透明的圖;
    public Sprite 右箭頭;
    public Sprite 左箭頭;

    public struct ElementInPage {
        public string name;
        public Sprite sprite;
        public string desc;
        public bool used;
    }

    private ElementInPage[,] currentBag;    //the items now exactly in bag  //not including all of the items in BagItemData

    private Image[] bottomUI = new Image[5];
    private Image[] itemUI = new Image[5];
    private Image[] kuangUI = new Image[5];
    private Image rightArrow;
    private Image leftArrow;
    private GameObject itemGroup;
    private CanvasGroup itemGroupCanvasGroup;
    private CanvasGroup noItemHintCanvasGroup;
    private GameObject itemDescription;
    private CanvasGroup itemDescCanvasGroup;
    private Image itemDescImage;
    private Text itemDescName;
    private Text itemDescD;
    private int nowAccessBagIndex = 0;
    private int totalPageNum_allItem;   //1~
    private int totalPageNum_current;   //1~
    private int totalItemNum_current;
    private bool open = false;
    private bool pageOneDirty = true;
    private int currentPage = 0;    //0~totalPageNum_current-1
    private int currentElement = 0; //0~4
    private bool openAndCloseAnimationDone = true;
    private RectTransform bagBottom;
    private bool changePageDone = true;
    private bool changePageMidDone = true;  //Mid: 換頁時 不透明度變為0的那時候
    private int notYetChangePageKuangAndItsNum = -1;
    private bool readingDescription = false;
    private bool descAniDone = true;

    //從NPC or 在劇情中 拿到物件 背包會打開關掉 顯示該物件的動畫(物件放在最後 動畫結束後才重新整理
    private bool getItemAniStart = false;
    private bool getItemAniDone = true;
    private string getItemAniName;

	// Use this for initialization
	void Start () {
        bagBottom = gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        noItemHintCanvasGroup = gameObject.transform.GetChild(0).GetChild(1).gameObject.GetComponent<CanvasGroup>();
        itemGroup = gameObject.transform.GetChild(0).GetChild(0).gameObject;
        itemGroupCanvasGroup = itemGroup.GetComponent<CanvasGroup>();
        for (int i = 0; i < 5; i++) {
            bottomUI[i] = itemGroup.transform.GetChild(i).gameObject.GetComponent<Image>();
            itemUI[i] = bottomUI[i].gameObject.transform.GetChild(0).GetComponent<Image>();
            kuangUI[i] = itemUI[i].gameObject.transform.GetChild(0).GetComponent<Image>();
        }
        rightArrow = itemGroup.transform.GetChild(5).gameObject.GetComponent<Image>();
        leftArrow = itemGroup.transform.GetChild(6).gameObject.GetComponent<Image>();

        itemDescription = gameObject.transform.GetChild(1).gameObject;
        itemDescCanvasGroup = itemDescription.GetComponent<CanvasGroup>();
        itemDescImage = itemDescription.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>();
        itemDescName = itemDescription.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Text>();
        itemDescD = itemDescription.transform.GetChild(0).GetChild(2).gameObject.GetComponent<Text>();

        totalPageNum_allItem = BagSystem.data.bagItemList.Count / 5 + 1;
        currentBag = new ElementInPage[totalPageNum_allItem, 5];

        for (int i = 0; i < totalPageNum_allItem; i++) {
            for (int j = 0; j < 5; j++) {
                currentBag[i, j].used = false;
            }
        }

        int itemIndex = 0;
        foreach (BagItem item in BagSystem.data.bagItemList) {
            if (item.inBag) {
                int pageIndex = itemIndex / 5;
                int elementIndex = itemIndex % 5;
                currentBag[pageIndex, elementIndex].name = item.name;
                currentBag[pageIndex, elementIndex].sprite = item.sprite;
                currentBag[pageIndex, elementIndex].desc = item.desc;
                currentBag[pageIndex, elementIndex].used = true;
                itemIndex += 1;
            }
        }
        totalPageNum_current = Mathf.CeilToInt(itemIndex / 5.0f);
        //print(totalPageNum_current);
        totalItemNum_current = itemIndex;
        pageOneDirty = true;

        //print(totalItemNum_current);

        bagBottom.localPosition = new Vector2(bagBottom.localPosition.x, 280);
        itemDescCanvasGroup.alpha = 0;
        noItemHintCanvasGroup.alpha = 1;

    }
	
	// Update is called once per frame
	void Update () {

        if (openAndCloseAnimationDone) {
            if (!open) {
                if (getItemAniStart) {
                    getItemAniStart = false;

                    //implemented by coroutine
                    StartCoroutine(GetItemAnimation(getItemAniName));

                }
                else if(getItemAniDone) {
                    if (BagSystem.bagUIDirty) {     //refresh the order of the items in bag  //the order of the items in bagUI is the same as the order in BagItemData
                        for (int i = 0; i < totalPageNum_allItem; i++) {
                            for (int j = 0; j < 5; j++) {
                                currentBag[i, j].used = false;
                            }
                        }

                        int itemIndex = 0;
                        foreach (BagItem item in BagSystem.data.bagItemList) {
                            if (item.inBag) {
                                int pageIndex = itemIndex / 5;
                                int elementIndex = itemIndex % 5;
                                currentBag[pageIndex, elementIndex].name = item.name;
                                currentBag[pageIndex, elementIndex].sprite = item.sprite;
                                currentBag[pageIndex, elementIndex].desc = item.desc;
                                currentBag[pageIndex, elementIndex].used = true;
                                itemIndex += 1;
                            }
                        }
                        totalPageNum_current = Mathf.CeilToInt(itemIndex / 5.0f);
                        totalItemNum_current = itemIndex;
                        pageOneDirty = true;
                        BagSystem.bagUIDirty = false;
                    }
                    else if (Input.GetKeyDown(KeyCode.B) && !SystemVariables.lockBag) {
                        FindObjectOfType<GameStateManager>().OpenBag();
                        SystemVariables.lockNPCinteract = true;
                        open = true;
                        currentPage = 0;
                        currentElement = 0;
                        if (totalItemNum_current == 0) {
                            noItemHintCanvasGroup.alpha = 1;
                            itemGroupCanvasGroup.alpha = 0;
                        }
                        else {
                            noItemHintCanvasGroup.alpha = 0;
                            itemGroupCanvasGroup.alpha = 1;
                            ChangeKuang(currentElement);
                            //set 

                            //set image
                            if (pageOneDirty) {
                                ChangeItem(0);
                            }
                            if (totalPageNum_current == 1) {
                                rightArrow.sprite = 透明的圖;
                                leftArrow.sprite = 透明的圖;
                            }
                            else {
                                rightArrow.sprite = 右箭頭;
                                leftArrow.sprite = 透明的圖;
                            }
                        }

                        //open animation
                        openAndCloseAnimationDone = false;
                        //do animation
                        StartCoroutine(OpenAnimation());
                    }
                }
            }
            else if (readingDescription) {  //bag is open and in reading description status
                if (descAniDone) {
                    if (Input.GetKeyDown(KeyCode.X)) {
                        readingDescription = false;

                        descAniDone = false;
                        StartCoroutine(DescriptionClose());
                    }
                }
            }
            else if (descAniDone) {

                if (notYetChangePageKuangAndItsNum > -1 && changePageMidDone) {
                    ChangeKuang(notYetChangePageKuangAndItsNum);
                    //change all itemUI
                    ChangeItem(currentPage);
                    if (currentPage == 0) {
                        if (totalPageNum_current == 1) {
                            rightArrow.sprite = 透明的圖;
                            leftArrow.sprite = 透明的圖;
                        }
                        else {
                            rightArrow.sprite = 右箭頭;
                            leftArrow.sprite = 透明的圖;
                        }
                    }
                    else if (currentPage == totalPageNum_current - 1) {
                        rightArrow.sprite = 透明的圖;
                        leftArrow.sprite = 左箭頭;
                    }
                    else {
                        rightArrow.sprite = 右箭頭;
                        leftArrow.sprite = 左箭頭;
                    }
                    
                    notYetChangePageKuangAndItsNum = -1;
                }

                if (changePageDone) {
                    if (totalItemNum_current != 0) {
                        if (Input.GetKeyDown(KeyCode.RightArrow)) {
                            if (currentPage == totalPageNum_current - 1) {
                                if (currentElement < (totalItemNum_current - 1) % 5) {
                                    currentElement += 1;
                                    ChangeKuang(currentElement);
                                }
                                else {
                                    //不能按的音效?
                                }
                            }
                            else if (currentPage < totalPageNum_current - 1) {
                                if (currentElement == 4) {  //the next page
                                    currentElement = 0;
                                    currentPage += 1;

                                    changePageDone = false;
                                    changePageMidDone = false;
                                    notYetChangePageKuangAndItsNum = currentElement;
                                    StartCoroutine(ChangePage());

                                }
                                else if (currentElement < 4) {
                                    currentElement += 1;
                                    ChangeKuang(currentElement);
                                }
                            }
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                            if (currentPage == 0) {
                                if (currentElement > 0) {
                                    currentElement -= 1;
                                    ChangeKuang(currentElement);
                                }
                                else {
                                    //不能按的音效?
                                }
                            }
                            else if (currentPage > 0) {
                                if (currentElement == 0) {  //the last page
                                    currentElement = 4;
                                    currentPage -= 1;

                                    changePageDone = false;
                                    changePageMidDone = false;
                                    notYetChangePageKuangAndItsNum = currentElement;
                                    StartCoroutine(ChangePage());

                                }
                                else if (currentElement > 0) {
                                    currentElement -= 1;
                                    ChangeKuang(currentElement);
                                }
                            }
                        }
                        else if (Input.GetKeyDown(KeyCode.Z)) {
                            itemDescImage.sprite = currentBag[currentPage, currentElement].sprite;
                            itemDescName.text = currentBag[currentPage, currentElement].name;
                            itemDescD.text = currentBag[currentPage, currentElement].desc;

                            readingDescription = true;
                            descAniDone = false;
                            StartCoroutine(DescriptionShowUp());
                        }
                    }
                    
                    if (Input.GetKeyDown(KeyCode.B)) {
                        FindObjectOfType<GameStateManager>().CloseBag();
                        SystemVariables.lockNPCinteract = false;
                        open = false;
                        StartCoroutine(CloseAnimation());

                    }
                }

            }
        }

	}

    IEnumerator OpenAnimation() {
        for (int i = 280; i >= 165; i-=5) {
            bagBottom.localPosition = new Vector2(bagBottom.localPosition.x, i);
            yield return null;
        }
        openAndCloseAnimationDone = true;
    }

    IEnumerator CloseAnimation() {
        for (int i = 165; i <= 280; i += 5) {
            bagBottom.localPosition = new Vector2(bagBottom.localPosition.x, i);
            yield return null;
        }
        openAndCloseAnimationDone = true;
    }

    IEnumerator GetItemAnimation(string name) {
        FindObjectOfType<GameStateManager>().OpenBag();
        //put the got item into the last of the currentBag
        int pageIndex = totalItemNum_current / 5;
        int elementIndex = totalItemNum_current % 5;
        foreach (BagItem item in BagSystem.data.bagItemList) {
            if (item.name == name) {
                if (item.inBag) {
                    currentBag[pageIndex, elementIndex].name = item.name;
                    currentBag[pageIndex, elementIndex].sprite = item.sprite;
                    currentBag[pageIndex, elementIndex].desc = item.desc;
                    currentBag[pageIndex, elementIndex].used = true;
                }
                else {
                    Debug.Log("得到背包物件" + name + "時，忘了add該物件進去BagSystem");
                    
                }
            }
        }
        totalItemNum_current += 1;
        totalPageNum_current = Mathf.CeilToInt(totalItemNum_current / 5.0f);
 
        //set the page and kuang showed on the bagUI is the last page and the last one
        noItemHintCanvasGroup.alpha = 0;
        itemGroupCanvasGroup.alpha = 1;
        ChangeItem(pageIndex);
        ChangeKuang(elementIndex);
        rightArrow.sprite = 透明的圖;
        if (totalPageNum_current == 1) {
            leftArrow.sprite = 透明的圖;
        }
        else {
            leftArrow.sprite = 左箭頭;
        }

        //open
        openAndCloseAnimationDone = false;
        for (int i = 280; i >= 165; i -= 5) {
            bagBottom.localPosition = new Vector2(bagBottom.localPosition.x, i);
            yield return null;
        }
        openAndCloseAnimationDone = true;

        //wait
        yield return new WaitForSeconds(1.25f);

        //close
        openAndCloseAnimationDone = false;
        for (int i = 165; i <= 280; i += 5) {
            bagBottom.localPosition = new Vector2(bagBottom.localPosition.x, i);
            yield return null;
        }
        openAndCloseAnimationDone = true;

        getItemAniDone = true;
        
        FindObjectOfType<GameStateManager>().CloseBag();
    }

    IEnumerator ChangePage() {
        for (float i = 1; i >= 0; i -= 0.1f) {
            itemGroupCanvasGroup.alpha = i;
            yield return null;
        }
        changePageMidDone = true;
        for (float i = 0; i <= 1; i += 0.1f) {
            itemGroupCanvasGroup.alpha = i;
            yield return null;
        }
        changePageDone = true;
    }

    IEnumerator DescriptionShowUp() {
        for (float i = 0; i <= 1; i += 0.2f) {
            itemDescCanvasGroup.alpha = i;
            yield return null;
        }
        descAniDone = true;
    }

    IEnumerator DescriptionClose() {
        for (float i = 1; i >= 0; i -= 0.2f) {
            itemDescCanvasGroup.alpha = i;
            yield return null;
        }
        descAniDone = true;
    }

    private void ChangeKuang(int index) {       //index: 0~4
        for (int i = 0; i < 5; i++) {
            if (i == index) {
                kuangUI[i].sprite = 框;
            }
            else {
                kuangUI[i].sprite = 透明的圖;
            }
        }
    }

    private void ChangeItem(int pageIndex) {    //pageIndex : 0~
        for (int i = 0; i < 5; i++) {
            if (currentBag[pageIndex, i].used) {
                itemUI[i].sprite = currentBag[pageIndex, i].sprite;
                bottomUI[i].sprite = 有物件的底;
            }
            else {
                itemUI[i].sprite = 透明的圖;
                bottomUI[i].sprite = 透明的圖;
            }
        }
    }

    public void GetItemAni(string name) {
        getItemAniStart = true;
        getItemAniDone = false;
        getItemAniName = name;
    }
}

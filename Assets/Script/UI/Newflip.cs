using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
[RequireComponent(typeof(Saving))]
public class Newflip : MonoBehaviour
{
    public FlipMode Mode;
    public Image Larrow,Rarrow,Pen1,Pen2,Pen3,Pen4,Pen5,Pen6,Pen7,Pen8, hidden1,hidden2,hidden3,hidden4,pic,Title,SaveLoad;
    public float PageFlipTime = 1,TimeBetweenPages = 1, DelayBeforeStarting = 0;
    public bool AutoStartFlip = true;
    public Saving ControledBook;
    public int AnimationFramesCount = 40,chossenFile=0,S0L1;
    private int rightpageNumber = 0, leftpageNumber = 0,chossenTrueFile = 1,YN = 0;
    bool isFlipping = false, isSaving = false, opening = false;
    public Text f1text,f2text,f3text,f4text,f5text,f6text,f7text,f8text;
    public Text ht1,ht2,ht3,ht4,TitleText;
    public Animator SavingCheck;
    public Sprite yes, yesC, no, noC, Cover ,CoverAuto ,nA ,n1 ,n2 ,n3 ,n4 ,n5 ,n6 ,n7 ,n8 ,n9, n10, n11, n12, n13, n14, n15, n16, n17, n18;

    private string room = "醫護室",DateAndTime = "00:00:00";


    // Use this for initialization
    void Start()
    {
        pic.enabled = false;
        Larrow.enabled = false;
        Rarrow.enabled = true;
        pens_out();
        refresh();
        Title.enabled = false;
        SaveLoad.enabled = true;
        TitleText.enabled = false;
        GameObject.Find("Cover").GetComponent<Image>().sprite = Cover;
        chossenFile = 1;
        if (!ControledBook)
            ControledBook = GetComponent<Saving>();
        if (AutoStartFlip)
            StartFlipping();
        ControledBook.OnFlip.AddListener(new UnityEngine.Events.UnityAction(PageFlipped));
    }
    void PageFlipped()
    {
        isFlipping = false;
        refresh();
        if (ControledBook.currentPage == 4)
        {
            pic.enabled = true;
            ht1.enabled = false;
            ht2.enabled = false;
            ht3.enabled = false;
            ht4.enabled = false;
            hidden1.enabled = false;
            hidden2.enabled = false;
            hidden3.enabled = false;
            hidden4.enabled = false;
            Larrow.enabled = true;
            Rarrow.enabled = false;
        }
        else
        {
            if (ControledBook.currentPage == 0)
            {
                Title.enabled = false;
                TitleText.enabled = false;
                SaveLoad.enabled = true;
                Larrow.enabled = false;
                Rarrow.enabled = true;
            }
            else
            {
                Title.enabled = true;
                TitleText.enabled = true;
                SaveLoad.enabled = false;
                Larrow.enabled = true;
                Rarrow.enabled = true;
            }
            pic.enabled = false;
            ht1.enabled = true;
            ht2.enabled = true;
            ht3.enabled = true;
            ht4.enabled = true;
            hidden1.enabled = true;
            hidden2.enabled = true;
            hidden3.enabled = true;
            hidden4.enabled = true;
        }
    }
    public void StartFlipping()
    {
        StartCoroutine(FlipToEnd());
    }
    public void FlipRightPage()
    {
        if (isFlipping) return;
        if (ControledBook.currentPage >= 4) return;
        isFlipping = true;
        if (ControledBook.currentPage == 2)
        {
            pic.enabled = true;
            ht1.enabled = false;
            ht2.enabled = false;
            ht3.enabled = false;
            ht4.enabled = false;
            hidden1.enabled = false;
            hidden2.enabled = false;
            hidden3.enabled = false;
            hidden4.enabled = false;
            Larrow.enabled = true;
            Rarrow.enabled = false;
        }
        //右半內文事先改變
        GameObject.Find("Save5").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage + 2) * 4 + 3) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
        GameObject.Find("Save6").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage + 2) * 4 + 4) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
        GameObject.Find("Save7").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage + 2) * 4 + 5) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
        GameObject.Find("Save8").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage + 2) * 4 + 6) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
        //cover變化
        //右邊翻頁
        f1text.text = "<i>#" + ((ControledBook.currentPage + 2) * 4 - 1) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
        f2text.text = "<i>#" + ((ControledBook.currentPage + 2) * 4 + 0) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
        f3text.text = "<i>#" + ((ControledBook.currentPage + 2) * 4 + 1) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
        f4text.text = "<i>#" + ((ControledBook.currentPage + 2) * 4 + 2) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
        //左邊不變
        f5text.text = "<i>#" + ((ControledBook.currentPage + 0) * 4 + 3) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
        f6text.text = "<i>#" + ((ControledBook.currentPage + 0) * 4 + 4) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
        f7text.text = "<i>#" + ((ControledBook.currentPage + 0) * 4 + 5) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
        f8text.text = "<i>#" + ((ControledBook.currentPage + 0) * 4 + 6) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
        chossenFile = 0;
        float frameTime = PageFlipTime / AnimationFramesCount;
        float xc = (ControledBook.EndBottomRight.x + ControledBook.EndBottomLeft.x) / 2;
        float xl = ((ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x) / 2) * 0.9f;
        //float h =  ControledBook.Height * 0.5f;
        float h = Mathf.Abs(ControledBook.EndBottomRight.y) * 0.9f;
        float dx = (xl) * 2 / AnimationFramesCount;
        StartCoroutine(FlipRTL(xc, xl, h, frameTime, dx));
    }
    public void FlipLeftPage()
    {
        if (isFlipping) return;
        if (ControledBook.currentPage <= 0) return;
        isFlipping = true;
        if (ControledBook.currentPage == 2)
        {
            GameObject.Find("Save2").GetComponentInChildren<Text>().text = "<i>Auto Save\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
            Title.enabled = false;
            TitleText.enabled = false;
            SaveLoad.enabled = true;
            Larrow.enabled = false;
            Rarrow.enabled = true;
            chossenFile = 1;
        }
        else
        {
            GameObject.Find("Save2").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage - 2) * 4 + 0) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
            chossenFile = 0;
        }
        GameObject.Find("Save1").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage - 2) * 4 - 1) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
        GameObject.Find("Save3").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage - 2) * 4 + 1) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
        GameObject.Find("Save4").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage - 2) * 4 + 2) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
        f1text.text = "<i>#" + ((ControledBook.currentPage + 0) * 4 - 1) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
        f2text.text = "<i>#" + ((ControledBook.currentPage + 0) * 4 + 0) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
        f3text.text = "<i>#" + ((ControledBook.currentPage + 0) * 4 + 1) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
        f4text.text = "<i>#" + ((ControledBook.currentPage + 0) * 4 + 2) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
        f5text.text = "<i>#" + ((ControledBook.currentPage - 2) * 4 + 3) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
        f6text.text = "<i>#" + ((ControledBook.currentPage - 2) * 4 + 4) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
        f7text.text = "<i>#" + ((ControledBook.currentPage - 2) * 4 + 5) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
        f8text.text = "<i>#" + ((ControledBook.currentPage - 2) * 4 + 6) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
        float frameTime = PageFlipTime / AnimationFramesCount;
        float xc = (ControledBook.EndBottomRight.x + ControledBook.EndBottomLeft.x) / 2;
        float xl = ((ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x) / 2) * 0.9f;
        //float h =  ControledBook.Height * 0.5f;
        float h = Mathf.Abs(ControledBook.EndBottomRight.y) * 0.9f;
        float dx = (xl) * 2 / AnimationFramesCount;
        StartCoroutine(FlipLTR(xc, xl, h, frameTime, dx));
    }
    IEnumerator FlipToEnd()
    {
        yield return new WaitForSeconds(DelayBeforeStarting);
        float frameTime = PageFlipTime / AnimationFramesCount;
        float xc = (ControledBook.EndBottomRight.x + ControledBook.EndBottomLeft.x) / 2;
        float xl = ((ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x) / 2) * 0.9f;
        //float h =  ControledBook.Height * 0.5f;
        float h = Mathf.Abs(ControledBook.EndBottomRight.y) * 0.9f;
        //y=-(h/(xl)^2)*(x-xc)^2          
        //               y         
        //               |          
        //               |          
        //               |          
        //_______________|_________________x         
        //              o|o             |
        //           o   |   o          |
        //         o     |     o        | h
        //        o      |      o       |
        //       o------xc-------o      -
        //               |<--xl-->
        //               |
        //               |
        float dx = (xl) * 2 / AnimationFramesCount;
        switch (Mode)
        {
            case FlipMode.RightToLeft:
                while (ControledBook.currentPage < ControledBook.TotalPageCount)
                {
                    StartCoroutine(FlipRTL(xc, xl, h, frameTime, dx));
                    yield return new WaitForSeconds(TimeBetweenPages);
                }
                break;
            case FlipMode.LeftToRight:
                while (ControledBook.currentPage > 0)
                {
                    StartCoroutine(FlipLTR(xc, xl, h, frameTime, dx));
                    yield return new WaitForSeconds(TimeBetweenPages);
                }
                break;
        }
    }
    IEnumerator FlipRTL(float xc, float xl, float h, float frameTime, float dx)
    {
        float x = xc + xl;
        float y = (-h / (xl * xl)) * (x - xc) * (x - xc);

        ControledBook.DragRightPageToPoint(new Vector3(x, y, 0));
        for (int i = 0; i < AnimationFramesCount; i++)
        {
            y = (-h / (xl * xl)) * (x - xc) * (x - xc);
            ControledBook.UpdateBookRTLToPoint(new Vector3(x, y, 0));
            yield return new WaitForSeconds(frameTime);
            x -= dx;
        }
        ControledBook.ReleasePage();
    }
    IEnumerator FlipLTR(float xc, float xl, float h, float frameTime, float dx)
    {
        float x = xc - xl;
        float y = (-h / (xl * xl)) * (x - xc) * (x - xc);
        ControledBook.DragLeftPageToPoint(new Vector3(x, y, 0));
        for (int i = 0; i < AnimationFramesCount; i++)
        {
            y = (-h / (xl * xl)) * (x - xc) * (x - xc);
            ControledBook.UpdateBookLTRToPoint(new Vector3(x, y, 0));
            yield return new WaitForSeconds(frameTime);
            x += dx;
        }
        ControledBook.ReleasePage();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (opening == true || isFlipping == true)
            {
                return;
            }
            if (isSaving)
            {
                switch (YN)
                {
                    case 0:
                        break;
                    case 1:
                        YN = 0;
                        break;
                }
                refresh();
                return; 
            }
            if (ControledBook.currentPage == 4)
            {
                return;
            }
            if (chossenFile == 0 || chossenFile == 1 || chossenFile == 2 || chossenFile == 3)
            {
                chossenFile += 4;
            }
            else
            {
                FlipRightPage();
            }
            refresh();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
            if (opening == true || isFlipping == true)
            {
                return;
            }
            if (isSaving)
            {
                switch (YN)
                {
                    case 0:
                        YN = 1;
                        break;
                    case 1:
                        break;
                }
                refresh();
                return;
            }
            if(ControledBook.currentPage == 0 && chossenFile == 4)
            {
                chossenFile = 1;
            }
            else if (chossenFile == 4 || chossenFile == 5 || chossenFile == 6 || chossenFile == 7)
            {
                chossenFile -= 4;
            }
            else
            {
                FlipLeftPage();
            }
            refresh();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (opening == true || isFlipping == true)
            {
                return;
            }
            if (isSaving) return;
            if (ControledBook.currentPage == 0 && chossenFile == 1)
            {
                return;
            }
            if (chossenFile == 0)
            {
                chossenFile = 7;
                FlipLeftPage();
            }
            else
            {
                chossenFile--;
            }
            refresh();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (opening == true || isFlipping == true)
            {
                return;
            }
            if (isSaving) return;
            if (chossenFile == 3)
            {
                if (ControledBook.currentPage == 4)
                {
                    return;
                }
            }
            if (chossenFile == 7)
            {
                if (ControledBook.currentPage == 4)
                {
                return;
                }
                else
                {
                    chossenFile = 0;
                    FlipRightPage();
                }
            }
            else
            {
                chossenFile++;
            }
            refresh();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if(opening == true || isFlipping == true)
            {
                return;
            }
            else if (isSaving==false)
            {
                if (ControledBook.currentPage == 0 && chossenFile == 0)
                {
                    return;
                }
                else if (ControledBook.currentPage == 0 && chossenFile == 1 && S0L1 == 0)
                {
                    return;
                }
                isSaving = true;
                opening = true;
                chossenTrueFile = ControledBook.currentPage * 4 + chossenFile - 1;
                YN = 0;
                refresh();
                SavingCheck.SetBool("OPENorCLOSE", true);
                StartCoroutine(Wait1s(1));
            }
            else
            {
                if(YN == 1)
                {
                    if(S0L1 == 0)
                    {
                        
                        ChangeFileName();
                    }
                    else
                    {

                    }
                }
                else
                {

                }
                opening = true;
                refresh();
                SavingCheck.SetBool("OPENorCLOSE", false);
                StartCoroutine(Wait1s(0));
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (isSaving == false || opening == true || isFlipping == true)
            {
                return;
            }
            else
            {
                opening = true;
                refresh();
                SavingCheck.SetBool("OPENorCLOSE", false);
                StartCoroutine(Wait1s(0));
            }
        }
    }

    void refresh ()
    {
        if (isFlipping == false)
        {
            pens_out();
            switch (chossenTrueFile)
            {
                case 0:
                    GameObject.Find("Text").GetComponent<Image>().sprite = nA;
                    break;
                case 1:
                    GameObject.Find("Text").GetComponent<Image>().sprite = n1;
                    break;
                case 2:
                    GameObject.Find("Text").GetComponent<Image>().sprite = n2;
                    break;
                case 3:
                    GameObject.Find("Text").GetComponent<Image>().sprite = n3;
                    break;
                case 4:
                    GameObject.Find("Text").GetComponent<Image>().sprite = n4;
                    break;
                case 5:
                    GameObject.Find("Text").GetComponent<Image>().sprite = n5;
                    break;
                case 6:
                    GameObject.Find("Text").GetComponent<Image>().sprite = n6;
                    break;
                case 7:
                    GameObject.Find("Text").GetComponent<Image>().sprite = n7;
                    break;
                case 8:
                    GameObject.Find("Text").GetComponent<Image>().sprite = n8;
                    break;
                case 9:
                    GameObject.Find("Text").GetComponent<Image>().sprite = n9;
                    break;
                case 10:
                    GameObject.Find("Text").GetComponent<Image>().sprite = n10;
                    break;
                case 11:
                    GameObject.Find("Text").GetComponent<Image>().sprite = n11;
                    break;
                case 12:
                    GameObject.Find("Text").GetComponent<Image>().sprite = n12;
                    break;
                case 13:
                    GameObject.Find("Text").GetComponent<Image>().sprite = n13;
                    break;
                case 14:
                    GameObject.Find("Text").GetComponent<Image>().sprite = n14;
                    break;
                case 15:
                    GameObject.Find("Text").GetComponent<Image>().sprite = n15;
                    break;
                case 16:
                    GameObject.Find("Text").GetComponent<Image>().sprite = n16;
                    break;
                case 17:
                    GameObject.Find("Text").GetComponent<Image>().sprite = n17;
                    break;
                case 18:
                    GameObject.Find("Text").GetComponent<Image>().sprite = n18;
                    break;
            }
            if (chossenTrueFile == 0)
            {
                GameObject.Find("Cover").GetComponent<Image>().sprite = CoverAuto;
            }
            else 
            {
                GameObject.Find("Cover").GetComponent<Image>().sprite = Cover;
            }
            if (isSaving == false)
            {
                switch (chossenFile)
                {
                    case 0:
                        Pen1.enabled = true;
                        break;
                    case 1:
                        Pen2.enabled = true;
                        break;
                    case 2:
                        Pen3.enabled = true;
                        break;
                    case 3:
                        Pen4.enabled = true;
                        break;
                    case 4:
                        Pen5.enabled = true;
                        break;
                    case 5:
                        Pen6.enabled = true;
                        break;
                    case 6:
                        Pen7.enabled = true;
                        break;
                    case 7:
                        Pen8.enabled = true;
                        break;
                }
            }
            else
            {
                pens_out();
                switch (YN)
                {
                    case 0:
                        GameObject.Find("YES").GetComponent<Image>().sprite = yes;
                        GameObject.Find("NO").GetComponent<Image>().sprite = noC;
                        break;
                    case 1:
                        GameObject.Find("YES").GetComponent<Image>().sprite = yesC;
                        GameObject.Find("NO").GetComponent<Image>().sprite = no;
                        break;
                }
            }
        }
        else
        {
            pens_out();
        }
    }

    void ChangeFileName()
    {
        switch (chossenFile)
        {
            case 0:
                GameObject.Find("Save1").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage) * 4 - 1) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
                break;
            case 1:
                GameObject.Find("Save2").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage) * 4 + 0) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
                break;
            case 2:
                GameObject.Find("Save3").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage) * 4 + 1) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
                break;
            case 3:
                GameObject.Find("Save4").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage) * 4 + 2) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
                break;
            case 4:
                GameObject.Find("Save5").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage) * 4 + 3) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
                break;
            case 5:
                GameObject.Find("Save6").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage) * 4 + 4) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
                break;
            case 6:
                GameObject.Find("Save7").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage) * 4 + 5) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
                break;
            case 7:
                GameObject.Find("Save8").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage) * 4 + 6) + "\n\t" + room + "\n <size=15>遊玩時長:" + DateAndTime + "</size></i>";
                break;
        }
    }

    void pens_out ()
    {
        Pen1.enabled = false;
        Pen2.enabled = false;
        Pen3.enabled = false;
        Pen4.enabled = false;
        Pen5.enabled = false;
        Pen6.enabled = false;
        Pen7.enabled = false;
        Pen8.enabled = false;
    }

    IEnumerator Wait1s(int a)
    {
        if (a == 0)
        {
            yield return new WaitForSeconds(0.25f);
            isSaving = false;
        }
        else if (a == 1)
        {
            yield return new WaitForSeconds(0.2f);
        }
        opening = false;
        refresh();
    }
}

//Debug.Log(DateTime.Now.ToString("yyyy/MM/dd") + " " + DateTime.Now.ToString("HH:mm"));

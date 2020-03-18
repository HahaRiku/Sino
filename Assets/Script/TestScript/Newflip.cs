using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
[RequireComponent(typeof(Saving))]
public class Newflip : MonoBehaviour
{
    public FlipMode Mode;
    public Image Larrow,Rarrow,Pen1,Pen2,Pen3,Pen4,Pen5,Pen6,Pen7,Pen8, PicY, PicN, hidden1,hidden2,hidden3,hidden4,pic,Title,SaveLoad;
    public float PageFlipTime = 1,TimeBetweenPages = 1, DelayBeforeStarting = 0;
    public bool AutoStartFlip = true;
    public Saving ControledBook;
    public int AnimationFramesCount = 40,chossenFile=0;
    private int rightpageNumber = 0, leftpageNumber = 0,chossenTrueFile = 1,YN = 0;
    bool isFlipping = false,isSaving = false;
    public Text f1text,f2text,f3text,f4text, f5text,f6text,f7text,f8text;
    public Text ht1,ht2,ht3,ht4,saveYN,TitleText;
    public Animator SavingCheck;


    // Use this for initialization
    void Start()
    {
        pic.enabled = false;
        Larrow.enabled = false;
        Rarrow.enabled = true;
        Pen1.enabled = false;
        Pen2.enabled = false;
        Pen3.enabled = false;
        Pen4.enabled = false;
        Pen5.enabled = false;
        Pen6.enabled = false;
        Pen7.enabled = false;
        Pen8.enabled = false;
        PicY.enabled = false;
        PicN.enabled = true;
        Title.enabled = false;
        SaveLoad.enabled = true;
        TitleText.enabled = false;
        saveYN.text = "是否覆蓋此檔案?";
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
            }
            else
            {
                Title.enabled = true;
                TitleText.enabled = true;
                SaveLoad.enabled = false;
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
        GameObject.Find("Save5").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage + 2) * 4 + 3) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        GameObject.Find("Save6").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage + 2) * 4 + 4) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        GameObject.Find("Save7").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage + 2) * 4 + 5) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        GameObject.Find("Save8").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage + 2) * 4 + 6) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f1text.text = "<i>#" + ((ControledBook.currentPage + 2) * 4 - 1) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f2text.text = "<i>#" + ((ControledBook.currentPage + 2) * 4 + 0) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f3text.text = "<i>#" + ((ControledBook.currentPage + 2) * 4 + 1) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f4text.text = "<i>#" + ((ControledBook.currentPage + 2) * 4 + 2) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f5text.text = "<i>#" + ((ControledBook.currentPage + 0) * 4 + 3) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f6text.text = "<i>#" + ((ControledBook.currentPage + 0) * 4 + 4) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f7text.text = "<i>#" + ((ControledBook.currentPage + 0) * 4 + 5) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f8text.text = "<i>#" + ((ControledBook.currentPage + 0) * 4 + 6) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
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
            GameObject.Find("Save2").GetComponentInChildren<Text>().text = "<i>Auto Save\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
            Larrow.enabled = false;
            Rarrow.enabled = true;
            Title.enabled = false;
            TitleText.enabled = false;
            SaveLoad.enabled = true;
            chossenFile = 1;
        }
        else
        {
            GameObject.Find("Save2").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage - 2) * 4 + 0) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
            chossenFile = 0;
        }
        GameObject.Find("Save1").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage - 2) * 4 - 1) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        GameObject.Find("Save3").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage - 2) * 4 + 1) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        GameObject.Find("Save4").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage - 2) * 4 + 2) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f1text.text = "<i>#" + ((ControledBook.currentPage + 0) * 4 - 1) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f2text.text = "<i>#" + ((ControledBook.currentPage + 0) * 4 + 0) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f3text.text = "<i>#" + ((ControledBook.currentPage + 0) * 4 + 1) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f4text.text = "<i>#" + ((ControledBook.currentPage + 0) * 4 + 2) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f5text.text = "<i>#" + ((ControledBook.currentPage - 2) * 4 + 3) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f6text.text = "<i>#" + ((ControledBook.currentPage - 2) * 4 + 4) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f7text.text = "<i>#" + ((ControledBook.currentPage - 2) * 4 + 5) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f8text.text = "<i>#" + ((ControledBook.currentPage - 2) * 4 + 6) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
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
        if (isFlipping == false)
        {
            Pen1.enabled = false;
            Pen2.enabled = false;
            Pen3.enabled = false;
            Pen4.enabled = false;
            Pen5.enabled = false;
            Pen6.enabled = false;
            Pen7.enabled = false;
            Pen8.enabled = false;
            saveYN.text = "是否覆蓋 <i>檔案" + chossenTrueFile + "</i> ?";
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
                if (ControledBook.currentPage == 0)
                {
                    Larrow.enabled = false;
                    Rarrow.enabled = true;
                }
                else if (ControledBook.currentPage == 4)
                {
                    Larrow.enabled = true;
                    Rarrow.enabled = false;
                }
                else
                {
                    Larrow.enabled = true;
                    Rarrow.enabled = true;
                }
            }
            else
            {
                Pen1.enabled = false;
                Pen2.enabled = false;
                Pen3.enabled = false;
                Pen4.enabled = false;
                Pen5.enabled = false;
                Pen6.enabled = false;
                Pen7.enabled = false;
                Pen8.enabled = false;
                switch (YN)
                {
                    case 0:
                        PicY.enabled = false;
                        PicN.enabled = true;
                        break;
                    case 1:
                        PicY.enabled = true;
                        PicN.enabled = false;
                        break;
                }
            }
        }
        else
        {
            Pen1.enabled = false;
            Pen2.enabled = false;
            Pen3.enabled = false;
            Pen4.enabled = false;
            Pen5.enabled = false;
            Pen6.enabled = false;
            Pen7.enabled = false;
            Pen8.enabled = false;
            PicY.enabled = false;
            PicN.enabled = false;
        }    

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (isFlipping) return;
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
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
            if (isFlipping) return;
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
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (isFlipping) return;
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
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (isFlipping) return;
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
            }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if(isSaving==false)
            {
                chossenTrueFile = ControledBook.currentPage * 4 + chossenFile - 1;
                YN = 0;
                SavingCheck.SetBool("OPENorCLOSE", true);
                isSaving = true;
            }
            else
            {
                SavingCheck.SetBool("OPENorCLOSE", false);
                StartCoroutine(Wait1s());
            }
        }
    }

    IEnumerator Wait1s()
    {
        yield return new WaitForSeconds(1f);
        isSaving = false;
    }
}

//Debug.Log(DateTime.Now.ToString("yyyy/MM/dd") + " " + DateTime.Now.ToString("HH:mm"));

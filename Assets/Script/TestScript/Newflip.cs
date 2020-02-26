﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[RequireComponent(typeof(Saving))]
public class Newflip : MonoBehaviour
{
    public FlipMode Mode;
    public Image Larrow,Rarrow,Pen1,Pen2,Pen3,Pen4,Pen5,Pen6,Pen7,Pen8,hidden1,hidden2,hidden3,hidden4,pic;
    public float PageFlipTime = 1;
    public float TimeBetweenPages = 1;
    public float DelayBeforeStarting = 0;
    public bool AutoStartFlip = true;
    public Saving ControledBook;
    public int AnimationFramesCount = 40,chossenFile=0;
    private int  rightpageNumber = 0, leftpageNumber = 0;
    bool isFlipping = false;
    public Text f1text,ht1;
    public Text f2text,ht2;
    public Text f3text,ht3;
    public Text f4text,ht4;
    public Text f5text;
    public Text f6text;
    public Text f7text;
    public Text f8text;

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
        chossenFile = 0;
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
        GameObject.Find("Save5").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage + 2) * 4 + 4) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        GameObject.Find("Save6").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage + 2) * 4 + 5) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        GameObject.Find("Save7").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage + 2) * 4 + 6) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        GameObject.Find("Save8").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage + 2) * 4 + 7) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f1text.text = "<i>#" + ((ControledBook.currentPage + 2) * 4 + 0) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f2text.text = "<i>#" + ((ControledBook.currentPage + 2) * 4 + 1) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f3text.text = "<i>#" + ((ControledBook.currentPage + 2) * 4 + 2) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f4text.text = "<i>#" + ((ControledBook.currentPage + 2) * 4 + 3) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f5text.text = "<i>#" + ((ControledBook.currentPage + 0) * 4 + 4) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f6text.text = "<i>#" + ((ControledBook.currentPage + 0) * 4 + 5) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f7text.text = "<i>#" + ((ControledBook.currentPage + 0) * 4 + 6) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f8text.text = "<i>#" + ((ControledBook.currentPage + 0) * 4 + 7) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
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
            Larrow.enabled = false;
            Rarrow.enabled = true;
        }
        
        if (ControledBook.currentPage == 2)
        {
            GameObject.Find("Save1").GetComponentInChildren<Text>().text = "<i>Auto Save\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        }
        else
        {
            GameObject.Find("Save1").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage - 2) * 4 + 0) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        }
        GameObject.Find("Save2").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage - 2) * 4 + 1) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        GameObject.Find("Save3").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage - 2) * 4 + 2) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        GameObject.Find("Save4").GetComponentInChildren<Text>().text = "<i>#" + ((ControledBook.currentPage - 2) * 4 + 3) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f1text.text = "<i>#" + ((ControledBook.currentPage + 0) * 4 + 0) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f2text.text = "<i>#" + ((ControledBook.currentPage + 0) * 4 + 1) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f3text.text = "<i>#" + ((ControledBook.currentPage + 0) * 4 + 2) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f4text.text = "<i>#" + ((ControledBook.currentPage + 0) * 4 + 3) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f5text.text = "<i>#" + ((ControledBook.currentPage - 2) * 4 + 4) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f6text.text = "<i>#" + ((ControledBook.currentPage - 2) * 4 + 5) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f7text.text = "<i>#" + ((ControledBook.currentPage - 2) * 4 + 6) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        f8text.text = "<i>#" + ((ControledBook.currentPage - 2) * 4 + 7) + "\n\t主人房\n <size=15>2020/11/11 00:00</size></i>";
        chossenFile = 0;
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
        if (isFlipping==false)
            {
            Pen1.enabled = false;
            Pen2.enabled = false;
            Pen3.enabled = false;
            Pen4.enabled = false;
            Pen5.enabled = false;
            Pen6.enabled = false;
            Pen7.enabled = false;
            Pen8.enabled = false;
            switch(chossenFile)
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
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            FlipRightPage();
            //GetComponent<Animator>(pen).Stop();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            FlipLeftPage();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (isFlipping) return;
            if (chossenFile == 0)
            {
                if (ControledBook.currentPage == 0)
                {
                    return;
                }
                else
                {
                    chossenFile = 7;
                    FlipLeftPage();
                }
                
            }
            else
            {
                chossenFile--;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (isFlipping) return;
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
    }
}

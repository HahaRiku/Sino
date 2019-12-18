using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[RequireComponent(typeof(Saving))]
public class Newflip : MonoBehaviour
{
    public FlipMode Mode;
    public float PageFlipTime = 1;
    public float TimeBetweenPages = 1;
    public float DelayBeforeStarting = 0;
    public bool AutoStartFlip = true;
    public Saving ControledBook;
    public int AnimationFramesCount = 40,CurrentpageNumber = 0;
    private int  rightpageNumber = 0, leftpageNumber = 0;
    bool isFlipping = false;
    public Text f1text;
    public Text f2text;
    public Text f3text;
    public Text f4text;
    public Text f5text;
    public Text f6text;
    public Text f7text;
    public Text f8text;

    // Use this for initialization
    void Start()
    {
        if (!ControledBook)
            ControledBook = GetComponent<Saving>();
        if (AutoStartFlip)
            StartFlipping();
        ControledBook.OnFlip.AddListener(new UnityEngine.Events.UnityAction(PageFlipped));

    }
    void PageFlipped()
    {
        isFlipping = false;
    }
    public void StartFlipping()
    {
        StartCoroutine(FlipToEnd());
    }
    public void FlipRightPage()
    {
        if (isFlipping) return;
        if (ControledBook.currentPage >= ControledBook.TotalPageCount) return;
        GameObject.Find("Save 5").GetComponentInChildren<Text>().text = "Save " + ((ControledBook.currentPage + 2) * 4 + 5) + "\n\tplace\n\t<size=30>2020/11/11</size>";
        GameObject.Find("Save 6").GetComponentInChildren<Text>().text = "Save " + ((ControledBook.currentPage + 2) * 4 + 6) + "\n\tplace\n\t<size=30>2020/11/11</size>";
        GameObject.Find("Save 7").GetComponentInChildren<Text>().text = "Save " + ((ControledBook.currentPage + 2) * 4 + 7) + "\n\tplace\n\t<size=30>2020/11/11</size>";
        GameObject.Find("Save 8").GetComponentInChildren<Text>().text = "Save " + ((ControledBook.currentPage + 2) * 4 + 8) + "\n\tplace\n\t<size=30>2020/11/11</size>";
        f1text.text = "Save " + ((ControledBook.currentPage + 2) * 4 + 1) + "\n\tplace\n\t<size=30>2020/11/11</size>";
        f2text.text = "Save " + ((ControledBook.currentPage + 2) * 4 + 2) + "\n\tplace\n\t<size=30>2020/11/11</size>";
        f3text.text = "Save " + ((ControledBook.currentPage + 2) * 4 + 3) + "\n\tplace\n\t<size=30>2020/11/11</size>";
        f4text.text = "Save " + ((ControledBook.currentPage + 2) * 4 + 4) + "\n\tplace\n\t<size=30>2020/11/11</size>";
        f5text.text = "Save " + (ControledBook.currentPage * 4 + 5) + "\n\tplace\n\t<size=30>2020/11/11</size>";
        f6text.text = "Save " + (ControledBook.currentPage * 4 + 6) + "\n\tplace\n\t<size=30>2020/11/11</size>";
        f7text.text = "Save " + (ControledBook.currentPage * 4 + 7) + "\n\tplace\n\t<size=30>2020/11/11</size>";
        f8text.text = "Save " + (ControledBook.currentPage * 4 + 8) + "\n\tplace\n\t<size=30>2020/11/11</size>";
        isFlipping = true;
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
        GameObject.Find("Save 1").GetComponentInChildren<Text>().text = "Save " + ((ControledBook.currentPage - 2) * 4 + 1) + "\n place 2020/11/11";
        GameObject.Find("Save 2").GetComponentInChildren<Text>().text = "Save " + ((ControledBook.currentPage - 2) * 4 + 2) + "\n place 2020/11/11";
        GameObject.Find("Save 3").GetComponentInChildren<Text>().text = "Save " + ((ControledBook.currentPage - 2) * 4 + 3) + "\n place 2020/11/11";
        GameObject.Find("Save 4").GetComponentInChildren<Text>().text = "Save " + ((ControledBook.currentPage - 2) * 4 + 4) + "\n place 2020/11/11";
        f1text.text = "Save " + (ControledBook.currentPage * 4 + 1) + "\n place 2020/11/11";
        f2text.text = "Save " + (ControledBook.currentPage * 4 + 2) + "\n place 2020/11/11";
        f3text.text = "Save " + (ControledBook.currentPage * 4 + 3) + "\n place 2020/11/11";
        f4text.text = "Save " + (ControledBook.currentPage * 4 + 4) + "\n place 2020/11/11";
        f5text.text = "Save " + ((ControledBook.currentPage - 2) * 4 + 5) + "\n place 2020/11/11";
        f6text.text = "Save " + ((ControledBook.currentPage - 2) * 4 + 6) + "\n place 2020/11/11";
        f7text.text = "Save " + ((ControledBook.currentPage - 2) * 4 + 7) + "\n place 2020/11/11";
        f8text.text = "Save " + ((ControledBook.currentPage - 2) * 4 + 8) + "\n place 2020/11/11";
        isFlipping = true;
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
}

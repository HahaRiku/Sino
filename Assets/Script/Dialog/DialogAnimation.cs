﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogAnimation : MonoBehaviour
{

    public AnimationCurve animationCurve;
    public CanvasGroup 對話框;
    public CanvasGroup 人物;
    // Use this for initialization

    public void StartOpenPanelAnim(bool isMoveCamera = false)
    {
        StartCoroutine(OpenAnim(isMoveCamera));
    }

    public void StartClosePanelAnim(bool isMoveCamera = false)
    {
        StartCoroutine(CloseAnim(isMoveCamera));
    }

    public void StartMovieOpeningAnim()
    {
        StartCoroutine(MovieOpeningAnim());
    }

    public void StartMovieEndingAnim()
    {
        StartCoroutine(MovieEndingAnim());
    }

    bool muxLock;
    IEnumerator anim1()
    {
        float timer = 0, orginY = 0.0f, finalY = -0.5f, totalTime = 0.5f, interval = 0.2f, y, alpha;
        float waitBackTime = 1.0f;
        while (timer < totalTime)
        {
            timer += Time.deltaTime;
            y = animationCurve.Evaluate(timer / totalTime) * (finalY - orginY);
            Camera.main.transform.position = new Vector3(0, orginY + y, -10);
            if (timer > totalTime - interval)
            {
                alpha = animationCurve.Evaluate((timer - totalTime + interval) / interval);
                對話框.alpha = alpha;
                人物.alpha = alpha;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Camera.main.transform.position = new Vector3(0, finalY, -10);
        對話框.alpha = 1;
        人物.alpha = 1;
        yield return new WaitForSeconds(waitBackTime);
        timer = 0;
        while (timer < totalTime)
        {
            timer += Time.deltaTime;
            y = animationCurve.Evaluate(timer / totalTime) * (finalY - orginY);
            Camera.main.transform.position = new Vector3(0, finalY - y, -10);
            if (timer < interval)
            {
                alpha = animationCurve.Evaluate(timer / interval);
                對話框.alpha = 1 - alpha;
                人物.alpha = 1 - alpha;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Camera.main.transform.position = new Vector3(0, orginY, -10);
        對話框.alpha = 0;
        人物.alpha = 0;
    }

    IEnumerator anim2()
    {
        float timer = 0, orginY = 0.0f, finalY = -0.5f, totalTime = 0.5f, interval = 0.2f, y, alpha;
        float waitBackTime = 1.0f;
        while (timer < totalTime)
        {
            timer += Time.deltaTime;
            y = animationCurve.Evaluate(timer / totalTime) * (finalY - orginY);
            Camera.main.transform.position = new Vector3(0, orginY + y, -10);
            Camera.main.orthographicSize = 5 - y;
            if (timer > totalTime - interval)
            {
                alpha = animationCurve.Evaluate((timer - totalTime + interval) / interval);
                對話框.alpha = alpha;
                人物.alpha = alpha;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Camera.main.transform.position = new Vector3(0, finalY, -10);
        對話框.alpha = 1;
        人物.alpha = 1;
        yield return new WaitForSeconds(waitBackTime);
        timer = 0;
        while (timer < totalTime)
        {
            timer += Time.deltaTime;
            y = animationCurve.Evaluate(timer / totalTime) * (finalY - orginY);
            Camera.main.transform.position = new Vector3(0, finalY - y, -10);
            Camera.main.orthographicSize = 5.5f + y;
            if (timer < interval)
            {
                alpha = animationCurve.Evaluate(timer / interval);
                對話框.alpha = 1 - alpha;
                人物.alpha = 1 - alpha;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Camera.main.transform.position = new Vector3(0, orginY, -10);
        對話框.alpha = 0;
        人物.alpha = 0;
    }

    IEnumerator OpenAnim(bool isMoveCamera)
    {
        while (muxLock)
        { yield return new WaitForSeconds(0.5f); }
        muxLock = true;
        float timer = 0, orginY = 0.0f, finalY = -0.5f, totalTime = 0.5f, interval = 0.2f, y, alpha;
        float waitBackTime = 1.0f;
        while (timer < totalTime)
        {
            timer += Time.deltaTime;
            y = animationCurve.Evaluate(timer / totalTime) * (finalY - orginY);
            if (isMoveCamera)
                Camera.main.transform.position = new Vector3(0, orginY + y, -10);
            if (timer > totalTime - interval)
            {
                alpha = animationCurve.Evaluate((timer - totalTime + interval) / interval);
                對話框.alpha = alpha;
                人物.alpha = alpha;
            }
            yield return 0;
        }
        if (isMoveCamera)
            Camera.main.transform.position = new Vector3(0, finalY, -10);
        對話框.alpha = 1;
        人物.alpha = 1;
        muxLock = false;
    }

    IEnumerator CloseAnim(bool isMoveCamera)
    {
        while (muxLock)
        { yield return new WaitForSeconds(0.5f); }
        muxLock = true;
        float timer = 0, orginY = 0.0f, finalY = -0.5f, totalTime = 0.5f, interval = 0.2f, y, alpha;
        while (timer < totalTime)
        {
            timer += Time.deltaTime;
            y = animationCurve.Evaluate(timer / totalTime) * (finalY - orginY);
            if (isMoveCamera)
                Camera.main.transform.position = new Vector3(0, finalY - y, -10);
            if (timer < interval)
            {
                alpha = animationCurve.Evaluate(timer / interval);
                對話框.alpha = 1 - alpha;
                人物.alpha = 1 - alpha;
            }
            yield return 0;
        }
        if (isMoveCamera)
            Camera.main.transform.position = new Vector3(0, orginY, -10);
        對話框.alpha = 0;
        人物.alpha = 0;
        muxLock = false;
    }

    IEnumerator MovieOpeningAnim()
    {
        while (muxLock)
        { yield return new WaitForSeconds(0.5f); }
        muxLock = true;
        float timer = 0, orginY = 0.0f, finalY = -0.5f, totalTime = 0.5f, y;
        while (timer < totalTime)
        {
            timer += Time.deltaTime;
            y = animationCurve.Evaluate(timer / totalTime) * (finalY - orginY);
            Camera.main.transform.position = new Vector3(0, orginY + y, -10);

            yield return 0;
        }
        Camera.main.transform.position = new Vector3(0, finalY, -10);
        muxLock = false;
    }

    IEnumerator MovieEndingAnim()
    {
        while (muxLock)
        { yield return new WaitForSeconds(0.5f); }
        muxLock = true;
        float timer = 0, orginY = 0.0f, finalY = -0.5f, totalTime = 0.5f, y;
        while (timer < totalTime)
        {
            timer += Time.deltaTime;
            y = animationCurve.Evaluate(timer / totalTime) * (finalY - orginY);

            Camera.main.transform.position = new Vector3(0, finalY - y, -10);
            yield return 0;
        }

        Camera.main.transform.position = new Vector3(0, orginY, -10);
        muxLock = false;
    }
}

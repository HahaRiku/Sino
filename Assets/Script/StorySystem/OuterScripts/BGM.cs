using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM: MonoBehaviour 
{
	public AudioClip[] soundaffect;
	AudioSource audioSource;
	private bool maxmusic, minmusic;
	private float curveTime = 0f,curveAmount,timer = 0.0f,inspentTime = 5, outspentTime = 5,waitTime = 0.01f,Curvetime = 1;
	public AnimationCurve Curve;

	void Start () 
	{
		audioSource = GetComponent<AudioSource>();
		minmusic=false;
		maxmusic=false;
		audioSource.loop = true;
	}

	public void SoundPlay(int getmusic)
	{
		audioSource.clip = soundaffect[getmusic] ;
		audioSource.Play();
	}

	public void musicOut(int outspendTime)
	{
		outspentTime = outspendTime;
		minmusic = true;
	}

	public void musicIn(int inspendTime)
	{
		inspentTime = inspendTime;
		maxmusic = true;
	}

	public void switching(int changemusic)
	{
		audioSource.Pause();
		audioSource.clip = soundaffect[changemusic] ;
		audioSource.Play();
	}

	public void SoundPAUSE()
	{
		audioSource.Pause();
	}

	public void StartCurve(int CurveTime){
		StartCoroutine( Animationcurve());
		Curvetime=CurveTime;
	}

	IEnumerator  Animationcurve()
	{
		curveTime = 0f;
		curveAmount = Curve.Evaluate (curveTime);

        while (curveTime< Curve[Curve.length -1].time) {
        
            curveTime +=Time.deltaTime/Curvetime;
            curveAmount = Curve.Evaluate (curveTime);

            audioSource.volume=curveAmount; 
			
            yield return null;
        }
    }
	
	void Update () 
	{
		if(minmusic)
		{
			timer += Time.deltaTime;
			if (timer > waitTime)
			{	
				audioSource.volume -= 1/outspentTime/100;
				timer = 0;
			}
			if (audioSource.volume <= 0f)
			{
				minmusic=false;
			}
		}
		else if (maxmusic)
		{
			timer += Time.deltaTime;
			if (timer > waitTime)
			{
				audioSource.volume += 1/inspentTime/100;
				timer = 0;
			}
			if (audioSource.volume >= 1f)
			{
				maxmusic=false;
			}
		}
	}
}

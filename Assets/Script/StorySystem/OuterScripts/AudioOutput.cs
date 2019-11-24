using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOutput : MonoBehaviour 
{
	public AudioClip[] soundaffect;
	AudioSource audioSource;
	public bool start,maxmusic;
	public bool minmusic,change;
	public float waitTime = 0.01f;
    public float timer = 0.0f;
	public float inspentTime = 5;
	public float outspentTime = 5;

	void Start () 
	{
		audioSource = GetComponent<AudioSource>();	
		start=false;
		minmusic=false;
		maxmusic=false;
		change=false;
	}

	public void Play1(int getmusic)
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

	public void switching(int getmusic)
	{
		audioSource.Pause();
		audioSource.clip = soundaffect[getmusic] ;
		audioSource.Play();
	}

	public void switchingEX(int getmusic)
	{
		change=true;
	}
	
	
	void Update () 
	{
		if(start)
		{
			Play1(0);
			start=false;
		}

		if(change)
		{
			minmusic = true;
			if(audioSource.volume <= 0f)
			{
				switching(1);
				change=false;
				maxmusic = true;
			}
		}

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

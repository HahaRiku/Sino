using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOutput : MonoBehaviour 
{
	public AudioClip[] soundaffect;
	AudioSource audioSource;
	public bool start,max,min;
	public float waitTime = 0.75f;
    public float timer = 0.0f;

	void Start () 
	{
		audioSource = GetComponent<AudioSource>();	
	}

	public void Play1(int getmusic)
	{
		audioSource.clip = soundaffect[getmusic] ;
		audioSource.Play();
	}
	
	
	void Update () 
	{
		if(start)
		{
			Play1(0);
			start=false;
		}

		if(min)
		{
			timer += Time.deltaTime;
			if (timer > waitTime)
			{	
				audioSource.volume -= 0.075f;
				timer = 0;
			}
			if (audioSource.volume == 0f)
			{
				min=false;
			}
		}
		else if (max)
		{
			timer += Time.deltaTime;
			if (timer > waitTime)
			{
				audioSource.volume += 0.075f;
				timer = 0;
			}
			if (audioSource.volume == 1f)
			{
				max=false;
			}
		}
	}
}

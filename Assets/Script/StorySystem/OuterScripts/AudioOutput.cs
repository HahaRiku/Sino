using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOutput : MonoBehaviour {
	public AudioClip[] soundaffect;
	AudioSource audioSource;
	public bool start;
	void Start () 
	{
		audioSource = GetComponent<AudioSource>();	
	}

	public void Play1(int getmusic)
	{
		audioSource.PlayOneShot(soundaffect[getmusic],20f);
	}
	
	
	void Update () {
		if(start)
		{
			Play1(0);
			start=false;
		}
	}
}

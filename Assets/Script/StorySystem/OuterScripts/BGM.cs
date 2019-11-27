using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM: MonoBehaviour 
{
	public AudioClip[] soundaffect;
	AudioSource audioSource;
	private float curveTime = 0f,curveAmount;
	public AnimationCurve Curve = new AnimationCurve();
	private AnimationCurve EZCurve = AnimationCurve.EaseInOut(0/*timeStart*/,1/*valueStart*/,1/*timeEnd*/,0/*valueEnd*/);  

	void Start () 
	{
		audioSource = GetComponent<AudioSource>();
		audioSource.loop = true;
	}

	public void SoundPlay(int getmusic)
	{
		audioSource.clip = soundaffect[getmusic] ;
		audioSource.Play();
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
		StartCoroutine( Animationcurve(CurveTime));
	}

	public void EasyInOut(int EZCurveTime){

		if (audioSource.volume >= 0.9f)
		{
			StartCoroutine( OutAnimationcurve(EZCurveTime));
		}
		else if (audioSource.volume <= 0.1f)
		{
			StartCoroutine(InAnimationcurve(EZCurveTime));
		}

	}

	IEnumerator  Animationcurve(float Curvetime)
	{
		curveTime = 0f;
		curveAmount = Curve.Evaluate (curveTime);

        while (curveTime< Curvetime) {
        
            curveTime +=Time.deltaTime;
            curveAmount = Curve.Evaluate (curveTime/Curvetime);

            audioSource.volume=curveAmount; 
			
            yield return null;
        }
    }

	IEnumerator InAnimationcurve(float Curvetime)
	{
		curveTime = 0f;
		curveAmount = EZCurve.Evaluate(curveTime);

		while (curveTime < Curvetime)
		{

			curveTime += Time.deltaTime;
			curveAmount = 1-EZCurve.Evaluate(curveTime / Curvetime);

			audioSource.volume = curveAmount;

			yield return null;
		}
	}

	IEnumerator OutAnimationcurve(float Curvetime)
	{
		curveTime = 0f;
		curveAmount = EZCurve.Evaluate(curveTime);

		while (curveTime < Curvetime)
		{

			curveTime += Time.deltaTime;
			curveAmount = EZCurve.Evaluate(curveTime / Curvetime);

			audioSource.volume = curveAmount;

			yield return null;
		}
	}

	void Update () 
	{

	}
}

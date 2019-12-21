using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayVideo : MonoBehaviour {
    public VideoClip video;

    private VideoPlayer videoPlayer;

	// Use this for initialization
	void Start () {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.clip = video;
        videoPlayer.playOnAwake = false;
	}

    public void Play() {
        videoPlayer.Play();
    }
}

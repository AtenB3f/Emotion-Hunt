using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System;

public class VideoPlay : MonoBehaviour
{
    VideoPlayer Video;
/*
    AudioSource Audio;
    RawImage Image;
*/
    void Start()
    {
        
        
    }
    void Awake()
    {
        Application.runInBackground = true;

        Video = gameObject.GetComponent<VideoPlayer>();
        Video.playOnAwake = false;
        Video.targetCameraAlpha = 0.6f;
        Video.aspectRatio = VideoAspectRatio.Stretch;
/*
        Audio = gameObject.GetComponent<AudioSource>();
        Audio.playOnAwake = false;
        Audio.Pause();

        Image = gameObject.GetComponent<RawImage>();
*/

        StVideoPlay();
    }

    public void StVideoPlay()
    {
        StartCoroutine(PlayVideo());
    }

    IEnumerator PlayVideo()
    {

#if UNITY_EDITOR
        // 유니티 에디터일 경우
        //string URL = "file://" + Application.streamingAssetsPath + "/particles_01.mp4";
#else
	// 런타임일 경우
	string urlAudio = Application.streamingAssetsPath + "/particles_01.mp4";
#endif
        //We want to play from url
        Video.source = VideoSource.VideoClip;
        Video.url = System.IO.Path.Combine (Application.streamingAssetsPath, "particles_01.mp4");


        //Video.audioOutputMode = VideoAudioOutputMode.AudioSource;
        Video.audioOutputMode = VideoAudioOutputMode.None;

        //Video.EnableAudioTrack(0, true);
        //Video.SetTargetAudioSource(0, Audio);
        Video.Prepare();

        //WaitForSeconds Time = new WaitForSeconds(1.0f);

        while (!Video.isPrepared)
        {
            Debug.Log("Preparing Video");
            //yield return Time;
            yield return null;
        }
        Debug.Log("Done Preparing Video");

        //Image.texture = Video.texture;

        Video.Play();
        //Audio.Play();

        Debug.Log("Playing Video");
        while (Video.isPlaying)
        {
            Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)Video.time));
            yield return null;
        }

        Debug.Log("Done Playing Video");
    }
}
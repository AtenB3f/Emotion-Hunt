using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


public class MainTitleManager : MonoBehaviour
{
    Boolean vidEn;
    public Image backgroundImg;
    public VideoPlayer Video;
    //public AudioSource Audio;
    private Color imgCol;
    public Sprite Title;
    private void Start()
    {
        Application.runInBackground = true;


        /*
                
                backgroundImg = GameObject.Find("/Title/ImageTitle").GetComponent<Image>();
                imgCol = backgroundImg.color;
                imgCol.a = 0.0f;
                backgroundImg.sprite = Resources.Load<Sprite>("/Icons/EmotionHuntTitle") as Sprite;
        */

        Video = gameObject.GetComponent<VideoPlayer>();
        Video.playOnAwake = false;
        Video.targetCameraAlpha = 0.6f;
        Video.aspectRatio = VideoAspectRatio.Stretch;

        // Audio Setting
/*
        Audio = gameObject.GetComponent<AudioSource>();
        Audio.playOnAwake = false;
        Audio.Pause();  
*/

        // Start Play Video
        StartCoroutine(PlayVideo());
        StartCoroutine(SetImage());
    }

    private IEnumerator SetImage()
    {
        while(!Video.isPrepared)
            yield return null;

        Title = gameObject.GetComponent<Sprite>();
        backgroundImg = gameObject.GetComponent<Image>();
        Debug.Log("End Loading Video");
        if (!Title)
        {
            //Debug.Log("Sprite");
            //backgroundImg.overrideSprite = Title;

        }
        /*
        if (backgroundImg != null)
        {
            backgroundImg.sprite = Resources.Load<Sprite>("/Icons/EmotionHuntTitle") as Sprite;
            imgCol.a = 1.0f;
            backgroundImg.color = imgCol;
        }
        */
    }

    private IEnumerator PlayVideo()
    {

        Video.source = VideoSource.VideoClip;
        Video.url = System.IO.Path.Combine(Application.streamingAssetsPath, "particles_01.mp4");

        
        //Video.audioOutputMode = VideoAudioOutputMode.AudioSource;
        Video.audioOutputMode = VideoAudioOutputMode.None;

        //Video.EnableAudioTrack(0, true);
        //Video.SetTargetAudioSource(0, Audio);

        Video.Prepare();

        while (!Video.isPrepared)
        {
            //Debug.Log("Preparing Video");
            //yield return Time;
            yield return null;
        }

        Video.Play();
        //Audio.Play();
        
        while (Video.isPlaying)
        {
            //Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)Video.time));
            yield return null;
        }
    }
}

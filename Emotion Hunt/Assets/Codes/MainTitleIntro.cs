using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;

public class MainTitleIntro : MonoBehaviour
{
    public GameObject TitlePanel;
    public GameObject IntroPanel;
    public Image IntroImg;
    public Image TitleImg;
    public Image BgImg;
    public GameObject Btn;
    public GameObject SubBtn;
    private VideoPlayer Video;
    //public AudioSource Audio;

    private bool SplashEn = false;

    void Start()
    {
        Application.runInBackground = true;

        // Image Color Setting
        IntroImg.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        TitleImg.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        
        /*
        // Video Setting
        Video = gameObject.AddComponent<VideoPlayer>();
        Video.playOnAwake = false;
        Video.targetCameraAlpha = 0.6f;
        Video.aspectRatio = VideoAspectRatio.Stretch;
        Video.source = VideoSource.VideoClip;
        */

        // Panel Disable
        TitlePanel.SetActive(false);
        IntroPanel.SetActive(false);

        // Splash Intro -> Main Title
        StartCoroutine(SplashIntro());
        StartCoroutine(TitleIntro());
    }

    void Update()
    {
        // Button
    }

    private IEnumerator SplashIntro()
    {
        IntroPanel.SetActive(true);

        // Fade In 2AST Team Logo  
        IntroImg.DOFade(1.0f,  0.8f);
        yield return new WaitForSeconds(2.0f);
        
        // Fade Out 2AST Team Logo
        IntroImg.DOFade(0.0f, 1.2f);
        yield return new WaitForSeconds(3.0f);

        // Splash Disable
        IntroPanel.SetActive(false);
        SplashEn = true;
        yield return null;
    }

    private IEnumerator TitleIntro()
    {
        while (!SplashEn)
            yield return null;

        // Title Enable
        TitlePanel.SetActive(true);
        new WaitForSeconds(3.0f);
        
        // Panel Fade In
        TitlePanel.GetComponent<Image>().DOFade(1, 1.8f);
        new WaitForSeconds(4.0f);

        // Title Fade In
        TitleImg.DOFade(1.0f, 1.0f);

        new WaitForSeconds(3.0f);

        TitlePanel.GetComponentInChildren<Text>().DOFade(1, 2);

        //Button Fade In
        ButtonFadeIn(Btn);
        ButtonFadeIn(SubBtn);

        yield return null;
    }

    private void ButtonFadeIn(GameObject objButton)
    {
        Button[] Arr = objButton.GetComponentsInChildren<Button>();
        int Cnt = Arr.Length;
        for (int i = 0; i < Cnt; i++)
        {
            if (Arr[i].GetComponentInChildren<Image>())
                Arr[i].GetComponent<Image>().DOFade(1, 2);

            if (Arr[i].GetComponentInChildren<Text>())
                Arr[i].GetComponentInChildren<Text>().DOFade(1, 2);
        }
    }

    private IEnumerator PlayVideo()
    {
        Debug.Log("Start Play Video");
        //TitleVideo.source = VideoSource.VideoClip;
        //TitleVideo.url = System.IO.Path.Combine(Application.streamingAssetsPath, "particles_01.mp4");
        Video.source = VideoSource.VideoClip;
        Video = gameObject.GetComponent<VideoPlayer>();

        //Video.audioOutputMode = VideoAudioOutputMode.AudioSource;
        Video.audioOutputMode = VideoAudioOutputMode.None;

        //Video.EnableAudioTrack(0, true);
        //Video.SetTargetAudioSource(0, Audio);

        Video.Prepare();

        while (!Video.isPrepared)
        {
            yield return null;
        }
        Debug.Log("Prepared Video");
        Video.Play();
        //Audio.Play();

        while (Video.isPlaying)
        {
            //Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)Video.time));
            yield return null;
        }
    }
}


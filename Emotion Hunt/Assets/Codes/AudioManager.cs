using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioInfo info = new AudioInfo();

    private AudioSource backgroundSrc;
    private AudioSource effectSrc;
    private AudioSource systemSrc;

    private float audioEffectVal;
    private float audioBGVal;

    void Start()
    {
        SetAudioVolume();

        SetAudioSource();

    }

    void SetAudioSource()
    {
        AudioSource[] audioSrc = this.GetComponents<AudioSource>();
        backgroundSrc = audioSrc[0];
        effectSrc = audioSrc[1];
        systemSrc = audioSrc[2];

        backgroundSrc.loop = true;
        effectSrc.loop = false;
        systemSrc.loop = false;
    }

    public void GetAudioVolume()
    {
        audioEffectVal = PlayerPrefs.GetFloat("AudioEffectVolume");
        audioBGVal = PlayerPrefs.GetFloat("AudioBGVolume");
    }

    public void SetAudioVolume()
    {
        GetAudioVolume();

        SetEffect(audioEffectVal);
        SetBackground(audioBGVal);
    }

    private float ChangeValueToDecibel(float value)
    {
        float Decibel;

        // Range : 0dB ~ -40dB
        Decibel = (value * 0.4f) - 40.0f;

        return (Decibel == -40.0f) ? -80.0f : Decibel;
    }

    public void SetEffect(float level)
    {
        float Decibel = ChangeValueToDecibel(level);
        audioMixer.SetFloat("effectVol", Decibel);
        audioMixer.SetFloat("systemVol", Decibel);
    }
    public void SetBackground(float level)
    {
        float Decibel = ChangeValueToDecibel(level);
        audioMixer.SetFloat("musicVol", Decibel);
    }
    private IEnumerator StartEffect()
    {
        WaitForSeconds ws = new WaitForSeconds(effectSrc.clip.length+1.0f);
        effectSrc.Play();

        if (effectSrc.isPlaying)
            yield return ws;

        info.SetToken(Token.None);
    }

    public void PlayEffect(string name)
    {
        AudioClip clip = info.GetEffect(name);

        if (clip == null)
        {
            info.SetToken(Token.None);
            return;
        }

        effectSrc.clip = clip;

        StartCoroutine("StartEffect");
    }

    public void OnEffect(string name)
    {
        AudioClip clip = info.GetEffect(name);

        if (clip == null)
        {
            info.SetToken(Token.None);
            return;
        }

        effectSrc.clip = clip;
        effectSrc.Play();
        info.SetToken(Token.None);
    }
    public void OnBGM(string name)
    {
        AudioClip clip = info.GetBGM(name);

        if (clip == null)
        {
            info.SetToken(Token.None);
            return;
        }

        backgroundSrc.clip = info.GetBGM(name);
        backgroundSrc.Play();

        info.SetToken(Token.None);
    }
}

public class AudioInfo
{
    Token token = Token.None;

    Dictionary<string, AudioClip> fileBGM = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> fileEffect = new Dictionary<string, AudioClip>();

    public void LoadBGM(string name)
    {
        string path = "Sound/BGM/" + name;
        AudioClip clip = Resources.Load<AudioClip>(path) as AudioClip;
        if (clip != null)
            fileBGM.Add(name, clip);
        else
            Debug.LogError("Load BGM Error. File no Exist.");
    }

    public AudioClip GetBGM(string key)
    {
        return fileBGM[key];
    }
    public AudioClip GetEffect(string key)
    {
        return fileEffect[key];
    }

    public void LoadEffect(string name)
    {
        string path = "Sound/Effect/" + name;
        AudioClip clip = Resources.Load<AudioClip>(path) as AudioClip;
        if (clip != null)
            fileEffect.Add(name, clip);
        else
            Debug.LogError("Load BGM Error. File no Exist."+ path);
    }

    public void SetToken(Token type)
    {
        token = type;
    }
    public Token ReadToken()
    {
        return token;
    }
}
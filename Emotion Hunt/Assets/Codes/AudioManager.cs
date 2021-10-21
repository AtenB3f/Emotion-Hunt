using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    public AudioInfo info = new AudioInfo();

    private AudioSource backgroundSrc = new AudioSource();
    private AudioSource effectSrc = new AudioSource();

    private float audioEffectVal;
    private float audioBGVal;

    void Start()
    {
        GetAudioVolume();

        backgroundSrc.loop = true;
        effectSrc.loop = false;
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

    public void OnEffect(string name)
    {
        effectSrc.clip = info.GetEffect(name);
        effectSrc.Play();
    }
    public void OnBGM(string name)
    {
        backgroundSrc.clip = info.GetBGM(name);
        backgroundSrc.Play();
    }
}

public class AudioInfo
{
    Token token = Token.None;

    Dictionary<string, AudioClip> fileBGM;
    Dictionary<string, AudioClip> fileEffect;

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
            fileBGM.Add(name, clip);
        else
            Debug.LogError("Load BGM Error. File no Exist.");
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
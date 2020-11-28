using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    private float audioEffectVal;
    private float audioBGVal;

    void Start()
    {
        GetAudioVolume();
    }

    public void GetAudioVolume()
    {
        audioEffectVal = PlayerPrefs.GetFloat("AudioEffectVolume");
        audioBGVal = PlayerPrefs.GetFloat("AudioBGVolume");
    }

    public void SetAudioVolume()
    {
        GetAudioVolume();

        SetSfxLvl(audioEffectVal);
        SetMusicLvl(audioEffectVal);
    }

    private float ChangeValueToDecibel(float value)
    {
        float Decibel;

        // Range : 0dB ~ -40dB
        Decibel = (value * 0.4f) - 40.0f;

        return (Decibel == -40.0f) ? -80.0f : Decibel;
    }

    public void SetSfxLvl(float sfxLvl)
    {
        float Decibel = ChangeValueToDecibel(sfxLvl);
        audioMixer.SetFloat("effectVol", Decibel);
        audioMixer.SetFloat("systemVol", Decibel);
    }

    public void SetMusicLvl(float musicLvl)
    {
        float Decibel = ChangeValueToDecibel(musicLvl);
        audioMixer.SetFloat("musicVol", Decibel);
    }
}

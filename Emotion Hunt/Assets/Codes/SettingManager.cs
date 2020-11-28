using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider audioEffect;
    public Slider audioBG;



    void Start()
    {
        LoadSettings();
    }

    void Update()
    {
        
    }

    public void SaveSettings()
    {
        // Audio
        PlayerPrefs.SetFloat("AudioEffectVolume", audioEffect.value);
        PlayerPrefs.SetFloat("AudioBGVolume", audioBG.value);
    }

    public void LoadSettings()
    {
        // Audio
        audioEffect.value = PlayerPrefs.GetFloat("AudioEffectVolume");
        audioBG.value = PlayerPrefs.GetFloat("AudioBGVolume");
    }


    //////////  AUDIO FUNCTIONS  //////////

    private float ChangeValueToDecibel(float value)
    {
        float Decibel;

        // Range : 0dB ~ -40dB
        Decibel = (value * 0.4f) - 40.0f;

        return (Decibel == -40.0f) ? -80.0f : Decibel;
    }

    public void SetSfxVol()
    {
        float Decibel = ChangeValueToDecibel(audioEffect.value);
        audioMixer.SetFloat("effectVol", Decibel);
        audioMixer.SetFloat("systemVol", Decibel);
    }

    public void SetMusicVol()
    {
        float Decibel = ChangeValueToDecibel(audioBG.value);
    }
}

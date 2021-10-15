using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct SaveData
{
    string playerName;
    int playDay;
    string targetNPC;
    Emotion valueEmomtion;
}

public class SaveManager
{
    public static void SavePlayDay(int day)
    {
        PlayerPrefs.SetInt("PlayDay", day);
    }

    public static int GetPlayDay()
    {
        return PlayerPrefs.GetInt("PlayDay");
    }

    public static void SavePlayerName(string name)
    {
        PlayerPrefs.SetString("PlayerName", name);
    }

    public static string GetPlayerName()
    {
        return PlayerPrefs.GetString("PlayerName");
    }

    public static void SaveValueEmotion(SelectionConfig config)
    {
        string emotion;
        switch (config.emotion)
        {
            case Emotion.Love:
                emotion = "Love";
                PlayerPrefs.SetInt(emotion, config.value);
                break;
            case Emotion.Philia:
                emotion = "Philia";
                PlayerPrefs.SetInt(emotion, config.value);
                break;
            case Emotion.Sympathy:
                emotion = "Sympathy";
                PlayerPrefs.SetInt(emotion, config.value);
                break;
            case Emotion.Contempt:
                emotion = "Contempt";
                PlayerPrefs.SetInt(emotion, config.value);
                break;
            case Emotion.Hate:
                emotion = "Hate";
                PlayerPrefs.SetInt(emotion, config.value);
                break;
            case Emotion.LoveAndHate:
                emotion = "LoveAndHate";
                PlayerPrefs.SetInt(emotion, config.value);
                break;
        }
    }

    public static int GetValueEmotion(Emotion emotion)
    {
        int value = 0;
        string key;
        switch (emotion)
        {
            case Emotion.Love:
                key = "Love";
                value = PlayerPrefs.GetInt(key);
                break;
            case Emotion.Philia:
                key = "Philia";
                value = PlayerPrefs.GetInt(key);
                break;
            case Emotion.Sympathy:
                key = "Sympathy";
                value = PlayerPrefs.GetInt(key);
                break;
            case Emotion.Contempt:
                key = "Contempt";
                value = PlayerPrefs.GetInt(key);
                break;
            case Emotion.Hate:
                key = "Hate";
                value = PlayerPrefs.GetInt(key);
                break;
            case Emotion.LoveAndHate:
                key = "LoveAndHate";
                value = PlayerPrefs.GetInt(key);
                break;
        }
        return value;
    }

}

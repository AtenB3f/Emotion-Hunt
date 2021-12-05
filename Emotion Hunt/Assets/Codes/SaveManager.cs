using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Emotions
{
    [Range(0, 100)] public int love;           // 사랑
    [Range(0, 100)] public int philia;         // 애정
    [Range(0, 100)] public int sympathy;       // 연민
    [Range(0, 100)] public int hate;           // 증오
    [Range(0, 100)] public int contempt;       // 경멸
    [Range(0, 100)] public int loveAndHate;      // 애증
    [Range(0, 100)] public int none;
}

public struct SaveData
{
    public string playerName;
    public int playDay;
    public string targetNPC;
    public Emotion targetEmotion;
    public Emotions emotions;
}


public class SaveManager : MonoBehaviour
{
    SaveData saveData = new SaveData();
    string saveString;

    void Awake()
    {
        LoadData();
    }

    public void SaveData()
    {
        saveString = SaveData(saveData);
    }

    public void LoadData()
    {
        saveData = LoadData<SaveData>(saveString);
    }

    string SaveData(object obj)
    {
        return JsonUtility.ToJson(obj);
    }

    T LoadData<T>(string jsonData)
    {
        return JsonUtility.FromJson<T>(jsonData);
    }

    public void SetPlayerName(string name)
    {
        saveData.playerName = name;
    }
    public string GetPlayerName()
    {
        return saveData.playerName;
    }

    public void SetPlayDay(int day)
    {
        saveData.playDay = day;
    }
    public int GetPlayDay()
    {
        return saveData.playDay;
    }

    public void SetTargetNpc(string name)
    {
        saveData.targetNPC = name;
    }
    public string GetTargetNpc()
    {
        return saveData.targetNPC;
    }

    public void SetTargetEmotion(Emotion type)
    {
        saveData.targetEmotion = type;
    }
    // return : String Emotion Name
    public string GetTargetEmotion()
    {
        string emotion;
        switch (saveData.targetEmotion)
        {
            case Emotion.Love:
                emotion = "Love";
                break;
            case Emotion.Philia:
                emotion = "Philia";
                break;
            case Emotion.Sympathy:
                emotion = "Sympathy";
                break;
            case Emotion.LoveAndHate:
                emotion = "LoveAndHate";
                break;
            case Emotion.Hate:
                emotion = "Hate";
                break;
            case Emotion.Contempt:
                emotion = "Contempt";
                break;
            default:
                emotion = "None";
                break;
        }
        return emotion;
    }

    public void SetEmotions(Emotion type, int value)
    {
        string emotion;
        switch (type)
        {
            case Emotion.Love:
                emotion = "Love";
                saveData.emotions.love = value;
                break;
            case Emotion.Philia:
                emotion = "Philia";
                saveData.emotions.philia = value;
                break;
            case Emotion.Sympathy:
                emotion = "Sympathy";
                saveData.emotions.sympathy = value;
                break;
            case Emotion.Contempt:
                emotion = "Contempt";
                saveData.emotions.contempt = value;
                break;
            case Emotion.Hate:
                emotion = "Hate";
                saveData.emotions.hate = value;
                break;
            case Emotion.LoveAndHate:
                emotion = "LoveAndHate";
                saveData.emotions.loveAndHate = value;
                break;
            default:
                break;
        }
        
    }

    void CheckTargetEmotion()
    {
        int[] arr = { saveData.emotions.love, saveData.emotions.philia, 
                    saveData.emotions.sympathy, saveData.emotions.hate, 
                    saveData.emotions.contempt, saveData.emotions.loveAndHate };
        Emotion[] arrEmotion = { Emotion.Love, Emotion.Philia, 
                                Emotion.Sympathy, Emotion.Hate,
                                Emotion.Contempt, Emotion.LoveAndHate};

        int greater = 0;
        bool equal = false;
        List<int> listEqual = new List<int>();

        for(int i=0; i<arr.Length-1; i++)
        {
            if (arr[i] > greater)
            {
                greater = i;
                equal = false;
                listEqual.Clear();
            } else if (arr[i] == greater)
            {
                equal = true;
                listEqual.Add(greater);
                listEqual.Add(i);
            }
        }

        if (equal)
            Debug.LogWarning("CheckTargetEmotion :: equal emotion value");

        saveData.targetEmotion = arrEmotion[greater];
    }

    public void AddValueEmotion(SelectionConfig config)
    {
        int value = GetValueEmotion(config.emotion);
        value += config.value;
        SetEmotions(config.emotion, value);

        // check target emotion
        CheckTargetEmotion();
    }

    public void ChangeEmotion(Emotion from, Emotion to)
    {
        int swap = GetValueEmotion(from);
        SetEmotions(to, swap);
        SetEmotions(from, 0);

        // check target emotion
        CheckTargetEmotion();
    }

    public int GetValueEmotion(Emotion emotion)
    {
        int value = 0;
        switch (emotion)
        {
            case Emotion.Love:
                value = saveData.emotions.love;
                break;
            case Emotion.Philia:
                value = saveData.emotions.philia;
                break;
            case Emotion.Sympathy:
                value = saveData.emotions.sympathy;
                break;
            case Emotion.Contempt:
                value = saveData.emotions.contempt;
                break;
            case Emotion.Hate:
                value = saveData.emotions.hate;
                break;
            case Emotion.LoveAndHate:
                value = saveData.emotions.loveAndHate;
                break;
            default:
                break;
        }
        return value;
    }

    
}

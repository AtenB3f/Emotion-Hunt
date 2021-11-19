﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum NPC
{
    A,
    B,
    C
}

public class CharacterManager : MonoBehaviour
{
    const int MAIN_NPC = 0;
    const int MAX_NPC = 3;
    const float DELAY_TIME = 10.0f;
    const float ON_OFF_TIME = 0.6f;
    const float DARKLIGHT_TIME = 0.4f;

    public CharacerInfo info = new CharacerInfo();
    public GameObject CharacterPanel;
    public Image NPC_A;
    public Image NPC_B;
    public Image NPC_C;

    void Start()
    {
        NPC_A.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        NPC_B.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        NPC_C.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        for(int i=0; i<MAX_NPC; i++)
            info.onCharacter[i] = false;
    }

    public void OnCharacter(int idx)
    {
        switch (idx)
        {
            case 0:
                NPC_A.DOFade(1.0f, ON_OFF_TIME);
                break;
            case 1:
                NPC_B.DOFade(1.0f, ON_OFF_TIME);
                break;
            case 2:
                NPC_C.DOFade(1.0f, ON_OFF_TIME);
                break;
        }
    }

    public void OffCharacter(int idx)
    {
        switch (idx)
        {
            case 0:
                NPC_A.DOFade(0.0f, ON_OFF_TIME);
                break;
            case 1:
                NPC_B.DOFade(0.0f, ON_OFF_TIME);
                break;
            case 2:
                NPC_C.DOFade(0.0f, ON_OFF_TIME);
                break;
        }
    }
    public void OffCharacter()
    {
        NPC_A.DOFade(0.0f, ON_OFF_TIME);
        NPC_B.DOFade(0.0f, ON_OFF_TIME);
        NPC_C.DOFade(0.0f, ON_OFF_TIME);
    }

    int AddMember(string name)
    {
        Image[] NPCs = { NPC_A, NPC_B, NPC_C };
        NPC[] indexs = { NPC.A, NPC.B, NPC.C };

        for(int i=0;i<NPCs.Length;i++)
        {
            if(info.CheckMember(name, NPCs[i]))
            {
                info.AddMember(name, indexs[i], NPCs[i]);
                return i;
            }
        }
        return 0;
    }

    public void SetCharacter(string name, string face)
    {
        if (name == "" || name == "Player")
            return;

        Sprite img = info.GetCharacter(name, face) as Sprite;

        int idx = 0;

        // No Setting Member
        if (info.CheckMember(name) >= MAX_NPC)
            idx = AddMember(name);

        switch(idx)
        {
            case 0:
                NPC_A.sprite = img;
                break;
            case 1:
                NPC_B.sprite = img;
                break;
            case 2:
                NPC_C.sprite = img;
                break;
        }

        SettingCharacter();

        if (info.onCharacter[idx] == false)
            OnCharacter(idx);

        SetImageDark(name);
    }

    public void RemoveParty(string name)
    {
        for (int i = 0; i<info.numNPC; i++)
        {
            if (info.characters[i].name == name)
            {
                info.RemoveMember(name);
                break;
            }
        }
    }

    public void SettingCharacter()
    {
        SetRect();
        SetSize();
    }

    private void SetRect()
    {
        foreach(NpcInfo npc in info.characters)
        {
            switch(npc.index)
            {
                case NPC.A:
                    NPC_A.rectTransform.anchorMin = new Vector2(0.5f, 0.0f);
                    NPC_A.rectTransform.anchorMax = new Vector2(0.5f, 0.0f);
                    break;
                case NPC.B:
                    NPC_B.rectTransform.anchorMin = new Vector2(0.8f, 0.0f);
                    NPC_B.rectTransform.anchorMax = new Vector2(0.8f, 0.0f);
                    break;
                case NPC.C:
                    NPC_C.rectTransform.anchorMin = new Vector2(1.0f, 0.0f);
                    NPC_C.rectTransform.anchorMax = new Vector2(1.0f, 0.0f);
                    break;
                default:
                    break;
            }
        }
    }
    private void SetSize()
    {
        Image[] NPC = { NPC_A, NPC_B, NPC_C };
        for(int i=0; i<info.numNPC; i++)
        {
            NPC[i].SetNativeSize();
        }
    }
    
    // nameTalking : 대화하는 사람 이름. 대사가 나오는 NPC는 밝게, 나머지는 어둡게
    public void SetImageDark(string talking)
    {
        Image[] NPC = { NPC_A, NPC_B, NPC_C };
        Color colDark = new Color(0.5f, 0.5f, 0.5f, 1.0f);
        Color colLight = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        for(int i=0; i < info.numNPC ; i++)
        {
            if (talking == info.characters[i].name)
                NPC[i].DOColor(colLight, DARKLIGHT_TIME);
            else
                NPC[i].DOColor(colDark, DARKLIGHT_TIME);
        }
    }
}

public struct NpcInfo
{
    public string name;
    public NPC index;
    public Image image;

    public NpcInfo(string npcName, NPC npcIdx, Image npcImg)
    {
        name = npcName;
        image = npcImg;
        index = npcIdx;
    }
}

public class CharacerInfo
{
    public const int MAX_NPC = 3;
    public bool[] onCharacter = new bool[MAX_NPC];
    //public string[] characters = new string[MAX_NPC];
    public List<NpcInfo> characters = new List<NpcInfo>();
    public int numNPC = 0;
    private Dictionary<string, Sprite> fileCharacter = new Dictionary<string, Sprite>();

    public int CheckMember(string name)
    {
        int i = 0;
        foreach(NpcInfo npc in characters)
        {
            if (npc.name == name)
                return i;
            i++;
        }
        return MAX_NPC;
    }

    public bool CheckMember(string name, Image img)
    {
        foreach(NpcInfo npc in characters)
        {
            if (npc.image == img)
                return false;
        }
        return true;
    }
    public void AddMember(string name, NPC idx, Image img)
    {
        //if (numNPC >= MAX_NPC)
        if(characters.Count >= MAX_NPC)
            return;

        NpcInfo info = new NpcInfo(name, idx, img);

        numNPC++;
        
        characters.Add(info);
    }

    public void RemoveMember(string name)
    {
        if (numNPC <= 0)
            return;

        numNPC--;

        foreach(NpcInfo npc in characters)
        {
            if(npc.name == name)
            {
                int index = characters.IndexOf(npc);
                characters.Remove(npc);
            }
        }
    }

    public void ResetMember()
    {
        numNPC = 0;
        characters.Clear();
    }

    public Sprite GetCharacter(string name, string face)
    {
        string key = name + "_" + face;
        return fileCharacter[key];
    }

    public void LoadCharacter(string name)
    {
        if (name == "" || name == "Player")
            return;
        string path = "Image/NPC/" + name + "/" + "Basic";
        Sprite[] sprites = Resources.LoadAll<Sprite>(path) as Sprite[];
        for(int i=0; i<sprites.Length; i++)
        {
            string key = sprites[i].name;
            fileCharacter.Add(key, sprites[i]);
        }
    }

}

public enum Emotion 
{
    None,
    Love,
    Philia,
    Sympathy,
    Hate,
    LoveAndHate,
    Contempt
}

enum EmotionInfo
{
    love,
    philia,
    sympathy,
    hate,
    contempt,
    loveNhate
}
/*
public struct EmotionInfo
{
    [Range(0, 100)] int love;           // 사랑
    [Range(0, 100)] int philia;         // 애정
    [Range(0, 100)] int sympathy;       // 연민
    [Range(0, 100)] int hate;           // 증오
    [Range(0, 100)] int contempt;       // 경멸
    [Range(0, 100)] int lovenhate;      // 애증
}
*/
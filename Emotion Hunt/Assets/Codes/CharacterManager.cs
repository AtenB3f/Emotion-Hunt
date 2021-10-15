using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterManager : MonoBehaviour
{
    const int MAIN_NPC = 0;
    const int MAX_NPC = 3;
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
    /*
    public IEnumerator OnCharacter(int num)
    {
        Image NPC;
        switch (num)
        {
            case 0:
                NPC = NPC_A;
                break;
            case 1:
                NPC = NPC_B;
                break;
            case 2:
                NPC = NPC_C;
                break;
            default:
                NPC = NPC_A;
                print("OnCharacter error");
                break;
        }

        if (NPC.color.a == 1.0f)
            info.SetToken(Token.None);
        else if(NPC.color.a == 0.0f)
        {
            NPC.DOFade(1.0f, ON_OFF_TIME);
            yield return 1;
        }
        else
            yield return 1;
    }

    public IEnumerator OffCharacter(int num)
    {
        Image NPC;
        switch (num)
        {
            case 0:
                NPC = NPC_A;
                break;
            case 1:
                NPC = NPC_B;
                break;
            case 2:
                NPC = NPC_C;
                break;
            default:
                NPC = NPC_A;
                break;
        }
        if (NPC.color.a == 0.0f)
            info.SetToken(Token.None);
        else if (NPC.color.a == 1.0f)
        {
            NPC.DOFade(0.0f, ON_OFF_TIME);
            yield return 1;
        }
        else
            yield return 1;
    }
    

    // only party member on
    public IEnumerator OnParty()
    {
        if (info.numNPC > 1)
        {
            if (NPC_B.color.a == 0.0f)
            {
                NPC_B.DOFade(1.0f, ON_OFF_TIME);
                NPC_C.DOFade(1.0f, ON_OFF_TIME);
                yield return 1;
            }
            else if (NPC_B.color.a == 1.0f)
                info.SetToken(Token.None);
            else
                yield return 1;
        }
    }

    // only party member off
    public IEnumerator OffParty()
    {
        if (info.numNPC > 1)
        {
            if(NPC_B.color.a == 1.0f)
            {
                NPC_B.DOFade(0.0f, ON_OFF_TIME);
                NPC_C.DOFade(0.0f, ON_OFF_TIME);
                yield return 1;
            }
            else if (NPC_B.color.a == 0.0f)
                info.SetToken(Token.None);
            else
                yield return 1;
        }
    }
    */

    public void SetCharacter(string name, string face)
    {
        if (name == "" || name == "Player")
            return;
        Sprite img = info.GetCharacter(name, face) as Sprite;
        int idx = 0;

        if (info.numNPC == 0)
        {
            info.AddMember(name);
        } else
        {
            for (; idx < info.numNPC; idx++)
                if (info.party[idx] == name)
                    break;
        }

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

        if (info.onCharacter[idx] == false)
            OnCharacter(idx);
    }

    public void RemoveParty(string name)
    {
        for (int i = 0; i<info.numNPC; i++)
        {
            if (info.party[i] == name)
                break;
        }
    }

    public void SettingCharacter()
    {
        SetRect();
        SetSize();
    }

    private void SetRect()
    {
        if (info.numNPC == 1)
        {
            NPC_A.rectTransform.anchorMin = new Vector2(0.5f, 0.0f);
            NPC_A.rectTransform.anchorMax = new Vector2(0.5f, 0.0f);
        }
        else if (info.numNPC == 2)
        {
            NPC_A.rectTransform.anchorMin = new Vector2(0.2f, 0.0f);
            NPC_A.rectTransform.anchorMax = new Vector2(0.2f, 0.0f);
            NPC_B.rectTransform.anchorMin = new Vector2(0.8f, 0.0f);
            NPC_B.rectTransform.anchorMax = new Vector2(0.8f, 0.0f);
        }
        else if (info.numNPC == 3)
        {
            NPC_A.rectTransform.anchorMin = new Vector2(0.5f, 0.0f);
            NPC_A.rectTransform.anchorMax = new Vector2(0.5f, 0.0f);
            NPC_B.rectTransform.anchorMin = new Vector2(0.0f, 0.0f);
            NPC_B.rectTransform.anchorMax = new Vector2(0.0f, 0.0f);
            NPC_B.rectTransform.anchorMin = new Vector2(1.0f, 0.0f);
            NPC_C.rectTransform.anchorMax = new Vector2(1.0f, 0.0f);
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
    public void SetImageDark(string nameTalking)
    {
        Image[] NPC = { NPC_A, NPC_B, NPC_C };
        Color colDark = new Color(0.5f, 0.5f, 0.5f, 1.0f);
        Color colLight = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        for(int i=0; i < info.numNPC ; i++)
        {
            if (nameTalking == info.party[i])
                NPC[i].DOColor(colLight, DARKLIGHT_TIME);
            else
                NPC[i].DOColor(colDark, DARKLIGHT_TIME);
        }
    }
}

public class CharacerInfo
{
    public const int MAX_NPC = 3;

    //public Token token = Token.None;
    public bool[] onCharacter = new bool[3];
    public int numNPC = 0;
    public string[] party = new string[MAX_NPC];
    private Dictionary<string, Sprite> fileCharacter = new Dictionary<string, Sprite>();

    /*
    public void SetToken(Token state)
    {
        token = state;
    }
    */

    public void AddMember(string name)
    {
        if (numNPC >= MAX_NPC)
            return;

        numNPC++;
        party[numNPC - 1] = name;
    }

    public void RemoveMember(string name)
    {
        if (numNPC >= 1)
            return;

        numNPC--;

        string[] tmp = new string[MAX_NPC];
        int idx = 0;
        for (int i = 0; i < numNPC; i++)
        {
            if (name == party[i])
                continue;
            else
            {
                tmp[idx] = party[idx];
                idx++;
            }
        }
        party = tmp;
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


public struct EmotionInfo
{
    [Range(0, 100)] int love;           // 사랑
    [Range(0, 100)] int philia;         // 애정
    [Range(0, 100)] int sympathy;       // 연민
    [Range(0, 100)] int hate;           // 증오
    [Range(0, 100)] int contempt;       // 경멸
    [Range(0, 100)] int lovenhate;      // 애증
}

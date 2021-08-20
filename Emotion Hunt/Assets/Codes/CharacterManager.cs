using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Character ID
/// 
/// [0] : Player 1 
/// [1] : Helen
/// [2] : Elisha
/// [3] : Lyam
/// [4] : 
/// </summary>
/// 

public class CharacterManager : MonoBehaviour
{
    // default, smile, angry, sad, special
    const int COUNT = 5;
    public Image[] npcHelen = new Image[COUNT];
    public Image[] npcElisha = new Image[COUNT];
    public Image[] npcLyam = new Image[COUNT];


    CharacterInfo charInfo;

    void Start()
    {
        InitCharacter();
    }

    void Update()
    {
        
    }

    private void InitCharacter()
    {

    }
}

public class CharacterInfo
{
    public string name;
    public int ID;
    public Image charImg;
}

public class NPCInfo : CharacterInfo
{
    const int COUNT = 5;
    [Range(0, COUNT)] int countenance;     // 표정
    public Image[] npcImg = new Image[COUNT];
    EmotionInfo emotion;        // That class only have the player and NPC. extra NPC is havn't
}

public class PlayerInfo : CharacterInfo
{
    // public string name;
    public int money;
    public string[] hunttingNPC = new string[3];
    EmotionInfo emotion;        // That class only have the player and NPC. extra NPC is havn't
}

public class EmotionInfo
{
    [Range(0, 100)] int eros;           // 사랑
    [Range(0, 100)] int philia;         // 애정
    [Range(0, 100)] int commiseration;  // 연민
    [Range(0, 100)] int hatred;         // 증오
    [Range(0, 100)] int contempt;       // 경멸
    [Range(0, 100)] int lovenhate;      // 애증
}



public class Appearence
{
    public int eye;
    public Color eyeColor;
    public int hair;
    public Color hairColor;
    public Color skin;
    public int body;

    public void Start()
    {
        LoadAppearence();
    }

    public void SelSkinColor(int num)
    {
        switch(num)
        {
            case 0:         // Pale Skin
                skin =  new Color(0, 0, 0, 1);
                break;
            case 1:
                break;      //
            case 2:
                break;
        }
    }

   private void LoadAppearence()
    {

    }
}


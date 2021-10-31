using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct StoryConfig
{
    public int index;
    public int subIndex;
    public string control;
    public string type;
    public int value;
    public string emotion;
    public string name;
    public string face;
    public string chat;
}

public enum Token
{ 
    None,
    Background,
    Object,
    BGM,
    Effect,
    Party,
    Selection,
    Dialog,
    Answer,
    Delay
}

public class StoryManager : MonoBehaviour
{
    DialogManager dialogManager;
    SelectionManager selectionManager;
    BackgroundManager backgroundManager;
    CharacterManager characterManager;
    AudioManager audioManager;

    Token token;
    private StoryConfig beforeConfigStory;
    public StoryConfig configStory;
    List<Dictionary<string, object>> storyList;
    int listCnt = 0;

    const float DELAY_TIME = 1.5f;

    void Start()
    {
        dialogManager = GameObject.Find("Dialog").GetComponent<DialogManager>();
        backgroundManager = GameObject.Find("GameObject").GetComponent<BackgroundManager>();
        selectionManager = GameObject.Find("Selection").GetComponent<SelectionManager>();
        selectionManager.OffSelection();
        characterManager = GameObject.Find("Characters").GetComponent<CharacterManager>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        // 공략 NPC 확인
        // day확인
        string targetNPC = "Helen";
        int playDay = 1;

        // CSV Read
        string path = "Script/" + targetNPC + "/Story_" + targetNPC + "_" + playDay.ToString();
        storyList = CSVReader.Read(path);

        LoadFile();
        LoadStory();
    }

    IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
        ResetToken();
    }

    void Update()
    {
        ObserveToken();

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            print("system menu");
            // System 메뉴
        }

        if ((token == Token.Selection) && (selectionManager.info.OnOff == false))
        {
            LoadStory();
        } else if(token == Token.Background || token == Token.Object || token == Token.BGM || token == Token.Effect || token == Token.Party)
        {
            LoadStory();
        } else if (token == Token.None)
        {
            //test code
            if (Input.GetKeyUp(KeyCode.Backspace))
            {
                // Skip
            }
            else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.KeypadEnter))
            {
                LoadStory();
            }
        }
    }

    private void SetToken(Token type)
    {
        token = type;

        switch (token)
        {
            case Token.Dialog:
            case Token.Answer:
            case Token.Party:
                dialogManager.info.SetToken(type);
                break;
            case Token.Background:
            case Token.Object:
                backgroundManager.info.SetToken(type);
                break;
            case Token.Selection:
                selectionManager.info.SetToken(type);
                break;
            case Token.BGM:
            case Token.Effect:
                audioManager.info.SetToken(type);
                break;
            case Token.Delay:
                break;
        }
    }

    int cntSel = 0;
    private void ObserveToken()
    {
        switch (token)
        {
            case Token.Dialog:
            case Token.Answer:
            case Token.Party:
                if (dialogManager.info.ReadToken() != token)
                {
                    token = Token.None;
                    //dialogManager.info.SetToken(token);
                }
                break;
            case Token.Selection:
                if (selectionManager.info.ReadToken() != token)
                {
                    token = Token.None;
                    int addIdx = (int)selectionManager.info.GetSelectButton();
                    int idx = (cntSel * 3) + addIdx;
                    listCnt = listAnswer[idx];
                    cntSel++;
                    LoadStory();
                }
                break;
            case Token.Background:
            case Token.Object:
                if (backgroundManager.info.ReadToken() != token)
                {
                    token = Token.None;
                    LoadStory();
                }
                break;
            case Token.BGM:
            case Token.Effect:
                if (audioManager.info.ReadToken() != token)
                {
                    token = Token.None;
                    LoadStory();
                }
                break;
            default:
                break;
        }
    }

    HashSet<string> hashNPC = new HashSet<string>();
    HashSet<string> hashBG = new HashSet<string>();
    HashSet<string> hashObj = new HashSet<string>();
    HashSet<string> hashEffect = new HashSet<string>();
    HashSet<string> hashBGM = new HashSet<string>();
    LinkedList<int> linkSelection = new LinkedList<int>();
    List<int> listAnswer = new List<int>();

    
    private void ReadStory()
    {
        HashSet<string> hashSet;
        string type = "";

        for (int i = 0; i < storyList.Count; i++)
        {
            string control = storyList[i]["Control"].ToString();

            switch (control)
            {
                case "Background":
                    hashSet = hashBG;
                    hashSet.Add(storyList[i]["Name"].ToString());
                    break;
                case "Object":
                    hashSet = hashObj;
                    hashSet.Add(storyList[i]["Name"].ToString());
                    break;
                case "BGM":
                    hashSet = hashBGM;
                    hashSet.Add(storyList[i]["Name"].ToString());
                    break;
                case "Effect":
                    hashSet = hashEffect;
                    hashSet.Add(storyList[i]["Name"].ToString());
                    break;
                case "Dialog":
                    hashSet = hashNPC;
                    hashSet.Add(storyList[i]["Name"].ToString());
                    break;
                case "Answer":
                    hashSet = hashNPC;
                    hashSet.Add(storyList[i]["Name"].ToString());
                    if(type != storyList[i]["Type"].ToString())
                    {
                        type = storyList[i]["Type"].ToString();
                        listAnswer.Add(i);
                    }
                    break;
                case "Selection":
                    if (linkSelection.Count == 0)
                        linkSelection.AddFirst(i);
                    else
                        linkSelection.AddLast(i);
                    break;
                default:
                    continue;
            }            
        }
    }
    private void LoadFile()
    {
        ReadStory();

        foreach(string str in hashNPC)
            characterManager.info.LoadCharacter(str);
        
        foreach (string str in hashBG)
            backgroundManager.info.LoadBackground(str);

        foreach (string str in hashObj)
            backgroundManager.info.LoadObject(str);
        
        foreach (string str in hashEffect)
            audioManager.info.LoadEffect(str);

        foreach (string str in hashBGM)
            audioManager.info.LoadBGM(str);
    }
    private void LoadStory ()
    {
        beforeConfigStory = configStory;

        SetConfig();

        listCnt++;
        if (listCnt >= storyList.Count)
        {
            print("End Story");
            //delay
            EndStory();
            return;
        }
        ActiveStory();
    }

    void SetConfig()
    {
        string idx = storyList[listCnt]["Index"].ToString();
        if (idx != null && idx != "")
            configStory.index = int.Parse(idx);
        string subIdx = storyList[listCnt]["Sub Index"].ToString();
        if (subIdx != null && subIdx != "")
            configStory.subIndex = int.Parse(subIdx);
        configStory.control = storyList[listCnt]["Control"].ToString();
        configStory.type = storyList[listCnt]["Type"].ToString();
        string val = storyList[listCnt]["Value"].ToString();
        if (val != null && val != "")
            configStory.value = int.Parse(val);
        configStory.emotion = storyList[listCnt]["Emotion"].ToString();
        configStory.name = storyList[listCnt]["Name"].ToString();
        configStory.face = storyList[listCnt]["Face"].ToString();
        configStory.chat = storyList[listCnt]["Chat"].ToString();
    }

    void ActiveStory ()
    {
        switch (configStory.control)
        {
            case "Background":
                CtrlBackground();
                break;
            case "Object":
                CtrlObject();
                break;
            case "BGM":
                CtrlBGM();
                break;
            case "Effect":
                CtrlEffect();
                break;
            case "Party":
                CtrlParty();
                break;
            case "Selection":
                CtrlSelection();
                break;
            case "Dialog":
                CtrlDialog();
                break;
            case "Answer":
                CtrlAnswer();
                break;
            case "Delay":
                CtrlDelay();
                break;
            default:
                print("Error csv active stroy. Control name :: " + configStory.control);
                break;
        }
        
    }

    void CtrlBackground()
    {
        SetToken(Token.Background);
        switch (configStory.type)
        {
            case "On":
                backgroundManager.SetBackground(configStory.name);
                backgroundManager.OnBackground();
                break;
            case "Off":
                backgroundManager.OffBackground();
                break;
            case "Change":
                backgroundManager.SetBackground(configStory.name);
                break;
            default:
                print("CtrlBackground. Type name :: " + configStory.type);
                break;
        }
    }

    void CtrlObject()
    {
        SetToken(Token.Object);
        switch (configStory.type)
        {
            case "On":
                backgroundManager.SetObject(configStory.name);
                backgroundManager.OnObject();
                break;
            case "Off":
                backgroundManager.OffObject();
                break;
            case "Change":
                backgroundManager.SetObject(configStory.name);
                break;
            default:
                print("CtrlObject. Type name :: " + configStory.type);
                break;
        }
    }

    void CtrlBGM()
    {
        SetToken(Token.BGM);
        switch (configStory.type)
        {
            case "On":
                audioManager.OnBGM(configStory.name);
                break;
            case "Off":
                break;
            default:
                print("CtrlBGM. Type name :: " + configStory.type);
                break;
        }
    }

    void CtrlEffect()
    {
        SetToken(Token.Effect);
        switch (configStory.type)
        {
            case "Play":
                audioManager.OnEffect(configStory.name);
                break;
            case "Fade In":
                audioManager.OnEffect(configStory.name);
                break;
            default:
                print("CtrlBGM. Type name :: " + configStory.type);
                break;
        }
    }

    void CtrlParty()
    {
        SetToken(Token.Party);
        switch (configStory.type)
        {
            case "Add":
                
                break;
            case "Remove":
                break;
            default:
                print("CtrlParty. Type name :: " + configStory.type);
                break;
        }
    }

    void CtrlSelection()
    {
        SetToken(Token.Selection);

        string emotion = configStory.emotion;
        int val = configStory.value;
        switch (configStory.type)
        {
            case "A":
                selectionManager.info.SetConfig(SelectionBtn.BUTTON_A, emotion, val);
                selectionManager.UpdateText(SelectionBtn.BUTTON_A, configStory.chat);
                break;
            case "B":
                selectionManager.info.SetConfig(SelectionBtn.BUTTON_B, emotion, val);
                selectionManager.UpdateText(SelectionBtn.BUTTON_B, configStory.chat);
                break;
            case "C":
                selectionManager.info.SetConfig(SelectionBtn.BUTTON_C, emotion, val);
                selectionManager.UpdateText(SelectionBtn.BUTTON_C, configStory.chat);
                if (selectionManager.info.OnOff == false)
                    selectionManager.OnSelection();
                break;
            default:
                print("CtrlParty. Type name :: " + configStory.type);
                break;
        }        
    }

    void CtrlDialog()
    {
        // test. 세이브 매니저에서 불러오기.
        string emotion = "None";
        if (configStory.emotion == emotion || configStory.emotion == "None")
        {
            switch (configStory.type)
            {
                case "Off":
                    SetToken(Token.Dialog);
                    dialogManager.info.onDialog = false;
                    dialogManager.OffDialog();
                    SetToken(Token.None);
                    break;
                case "Play":
                    string name = configStory.name;
                    string face = configStory.face;
                    characterManager.SetCharacter(name, face);

                    SetToken(Token.Dialog);

                    if (dialogManager.info.onDialog == false)
                    {
                        dialogManager.info.onDialog = true;
                        dialogManager.OnDialog();
                    }

                    dialogManager.SetName(name);

                    if (configStory.index == beforeConfigStory.index && configStory.subIndex > beforeConfigStory.subIndex)
                    {
                        string str = dialogManager.GetDialog() + "\n" + configStory.chat;
                        int length = dialogManager.GetDialog().Length;
                        dialogManager.SetDialog(length, str);
                    }
                    else
                    {
                        string str = configStory.chat;
                        dialogManager.SetDialog(str);
                    }
                    break;
                default:
                    SetToken(Token.None);
                    print("CtrlParty. Type name :: " + configStory.type);
                    break;
            }
        }
    }

    void CtrlAnswer()
    {
        // test. 세이브 매니저에서 불러오기
        string emotion = "None";

        SelectionBtn btn = selectionManager.info.GetSelectButton();
        SelectionConfig config = selectionManager.info.GetConfig(btn);
        string type = "A";
        switch (btn)
        {
            case SelectionBtn.BUTTON_A:
                type = "A";
                break;
            case SelectionBtn.BUTTON_B:
                type = "B";
                break;
            case SelectionBtn.BUTTON_C:
                type = "C";
                break;
        }

        if (configStory.emotion == emotion || configStory.emotion == "None")
        {
            string name = configStory.name;
            string face = configStory.face;
            characterManager.SetCharacter(name, face);

            SetToken(Token.Answer);

            if (configStory.type == type)
            {
                if (dialogManager.info.onDialog == false)
                {
                    dialogManager.info.onDialog = true;
                    dialogManager.OnDialog();
                }

                dialogManager.SetName(name);


                if (configStory.index == beforeConfigStory.index && configStory.subIndex > beforeConfigStory.subIndex)
                {
                    string str = dialogManager.GetDialog() + "\n" + configStory.chat;
                    int length = dialogManager.GetDialog().Length;
                    dialogManager.SetDialog(length, str);
                }
                else
                {
                    string str = configStory.chat;
                    dialogManager.SetDialog(str);
                }
            } else
            {
                NextDialog(configStory.index);
                SetToken(Token.None);
            }
        }
    }
    void NextDialog(int idx)
    {
        listCnt++;
        SetConfig();

        if (configStory.index == idx)
            NextDialog(idx);
        else
            return;
    }

    void ResetToken()
    {
        token = Token.None;
    }

    void CtrlDelay()
    {
        float time = (float)configStory.value;
        StartCoroutine(Delay(time));
    }

    void EndStory()
    {
        ResetToken();
        listCnt = 0;
        cntSel = 0;
        // 데이터 저장
        // scene 이동 : 메인 스토리이면 플레이어 홈으로, 캐릭터 스토리면 미니게임으로
    }
}
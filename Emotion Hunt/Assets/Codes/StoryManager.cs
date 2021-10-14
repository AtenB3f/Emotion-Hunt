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

    Token token;
    private StoryConfig beforeConfigStory;
    public StoryConfig configStory;
    List<Dictionary<string, object>> storyList;

    int listCnt = 0;

    void Start()
    {
        dialogManager = GameObject.Find("Dialog").GetComponent<DialogManager>();
        backgroundManager = GameObject.Find("GameObject").GetComponent<BackgroundManager>();
        selectionManager = GameObject.Find("Selection").GetComponent<SelectionManager>();
        selectionManager.OffSelection();
        characterManager = GameObject.Find("Characters").GetComponent<CharacterManager>();

        // 공략 NPC 확인
        // day확인
        string targetNPC = "Helen";
        int playDay = 1;

        // CSV Read
        string path = "Script/" + targetNPC + "/Story_" + targetNPC + "_" + playDay.ToString();
        storyList = CSVReader.Read(path);
        print(path);

        LoadFile();
        LoadStory();
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
                dialogManager.info.SetToken(type);
                break;
            case Token.Background:
                break;
            case Token.Object:
                break;
            case Token.Selection:
                selectionManager.info.SetToken(type);
                break;
            case Token.BGM:
                break;
            case Token.Effect:
                break;
            case Token.Party:
                break;
            case Token.Delay:
                break;
            default:
                break;
        }
    }

    private void ObserveToken()
    {
        switch (token)
        {
            case Token.Dialog:
            case Token.Answer:
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
                    LoadStory();
                }
                break;
            default:
                
                break;
        }
    }

    private void LoadFile()
    {
        // backgroundManager.LoadBackground()
        // objectManager.LoadObject()
    }

    private void LoadStory ()
    {
        beforeConfigStory = configStory;

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

        print("LoadStory  " + listCnt + "  " + configStory.control);
        print("Chat :: " + configStory.chat);
        print("Type :: " + configStory.type);
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

    void ActiveStory ()
    {
        print("Active Story :: " + configStory.control);
        switch (configStory.control)
        {
            case "Background":
                CtrlBackground();
                break;
            case "Object":
                CtrlObject();
                break;
            case "Music":
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
        token = Token.Background;

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
        ResetToken();
    }

    void CtrlObject()
    {
        token = Token.Object;

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
        ResetToken();
    }

    void CtrlBGM()
    {
        token = Token.BGM;
        switch (configStory.type)
        {
            case "On":
                break;
            case "Off":
                break;
            case "Change":
                break;
            default:
                print("CtrlBGM. Type name :: " + configStory.type);
                break;
        }
        ResetToken();
    }
    void CtrlEffect()
    {
        token = Token.Effect;
        switch (configStory.type)
        {
            case "Play":
                break;
            case "Off":
                break;
            default:
                print("CtrlBGM. Type name :: " + configStory.type);
                break;
        }
        ResetToken();
    }
    void CtrlParty()
    {
        token = Token.Party;
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
        ResetToken();
    }
    void CtrlSelection()
    {
        token = Token.Selection;
        selectionManager.info.SetToken(Token.Selection);

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
            SetToken(Token.Dialog);

            switch (configStory.type)
            {
                case "Off":
                    dialogManager.info.onDialog = false;
                    dialogManager.OffDialog();
                    break;
                case "Play":
                    if (dialogManager.info.onDialog == false)
                    {
                        dialogManager.info.onDialog = true;
                        dialogManager.OnDialog();
                    }
                    // 이름 설정
                    dialogManager.SetName(configStory.name);

                    // 대화 출력
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
            SetToken(Token.Answer);

            if (configStory.type == type)
            {
                if (dialogManager.info.onDialog == false)
                {
                    dialogManager.info.onDialog = true;
                    dialogManager.OnDialog();
                }
                // 이름 설정
                dialogManager.PrintName(configStory.name);

                // 대화 출력
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
            }
        }
    }

    void ResetToken()
    {
        token = Token.None;
    }

    void CtrlDelay()
    {
        token = Token.Delay;
        Invoke("ResetToken", configStory.value);
    }



    void EndStory()
    {
        ResetToken();
        listCnt = 0;
        // 데이터 저장
        // scene 이동 : 메인 스토리이면 플레이어 홈으로, 캐릭터 스토리면 미니게임으로
    }
}
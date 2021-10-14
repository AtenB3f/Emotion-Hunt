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

enum Token
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
    const float CHAT_SPEED_NOMAL = 0.05f;
    const float CHAT_SPEED_FAST = 0.03f;

    float chatSpeed = CHAT_SPEED_NOMAL;
    private int day = 0;
    DialogManager dialogManager;
    SelectionManager selectionManager;
    BackgroundManager backgroundManager;
    CharacterManager characterManager;
    Token token;
    private StoryConfig beforeConfigStory;
    public StoryConfig configStory;
    int listCnt = 0;
    List<Dictionary<string, object>> storyList;

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
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            // System 메뉴
        }

        if (token == Token.Dialog || token == Token.Answer)
        {
            //test code
            if (Input.GetKeyUp(KeyCode.Backspace))
            {
                // Skip
            }
            // 키 입력에 따라 현재 진행 대사 확인
            if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.KeypadEnter))
            {
                LoadStory();
            }
        }
        else if(token != Token.Selection)
        {
            LoadStory();
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
        if (selectionManager.info.OnOff == false)
            selectionManager.OnSelection();
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
                break;
            default:
                print("CtrlParty. Type name :: " + configStory.type);
                break;
        }        
    }

    void CtrlDialog()
    {
        token = Token.Dialog;
        switch (configStory.type)
        {
            case "Off":
                dialogManager.info.play = false;
                dialogManager.OffDialog();
                break;
            case "Play":
                if (dialogManager.info.play == false)
                {
                    dialogManager.info.play = true;
                    dialogManager.OnDialog();
                }

                if (configStory.index == beforeConfigStory.index)
                {
                    string str = dialogManager.GetDialog() + configStory.chat;
                    StartCoroutine(SetDialog(str));
                }
                else
                {
                    StartCoroutine(SetDialog(dialogManager.GetDialog()));
                }
                break;
            default:
                print("CtrlParty. Type name :: " + configStory.type);
                break;
        }
    }

    void CtrlAnswer()
    { 
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

    void SetName(string name)
    {
        dialogManager.PrintName(name);
    }

    bool playCoroutine = false;
    IEnumerator SetDialog(string str)
    {
        playCoroutine = true;
        WaitForSeconds ws = new WaitForSeconds(chatSpeed);
        int length = str.Length;

        for(int i=0;i< length; i++)
        {
            string data = str.Substring(0, i);
            dialogManager.PrintDialog(data);

            yield return ws;
        }
        print("End SetDialog");
        ResetToken();
        playCoroutine = false;
    }
    IEnumerator SetDialog(int stPoit, string str)
    {
        playCoroutine = true;
        WaitForSeconds ws = new WaitForSeconds(0.015f);
        int length = str.Length;

        for (int i = stPoit; i < length; i++)
        {
            string data = str.Substring(0, i);
            dialogManager.PrintDialog(str);

            yield return ws;
        }
        print("End SetDialog");
        ResetToken();
        playCoroutine = false;
    }

    void EndStory()
    {
        ResetToken();
        listCnt = 0;
        // 데이터 저장
        // scene 이동 : 메인 스토리이면 플레이어 홈으로, 캐릭터 스토리면 미니게임으로
    }
}
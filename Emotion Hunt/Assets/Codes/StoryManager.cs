using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
    Delay,
    Input
}

public class StoryManager : MonoBehaviour
{
    DialogManager dialogManager;
    SelectionManager selectionManager;
    BackgroundManager backgroundManager;
    CharacterManager characterManager;
    AudioManager audioManager;
    CSVManager csvManager;

    Token token;
    
    void Start()
    {
        csvManager = GameObject.Find("Main Camera").GetComponent<CSVManager>();
        dialogManager = GameObject.Find("Dialog").GetComponent<DialogManager>();
        backgroundManager = GameObject.Find("GameObject").GetComponent<BackgroundManager>();
        selectionManager = GameObject.Find("Selection").GetComponent<SelectionManager>();
        selectionManager.OffSelection();
        characterManager = GameObject.Find("Characters").GetComponent<CharacterManager>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

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
        } else if (token == Token.Input)
        {
            //test code
            if (Input.GetKeyUp(KeyCode.Backspace))
            {
                // Skip
            }
            else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.KeypadEnter))
            {
                print("next");
                LoadStory();
            }
        }
        else if(token == Token.None)
        {
            LoadStory();
        }
    }

    private void LoadFile()
    {
        //ReadStory();

        foreach (string str in csvManager.hashNPC)
            characterManager.info.LoadCharacter(str);

        foreach (string str in csvManager.hashBG)
            backgroundManager.info.LoadBackground(str);

        foreach (string str in csvManager.hashObj)
            backgroundManager.info.LoadObject(str);

        foreach (string str in csvManager.hashEffect)
            audioManager.info.LoadEffect(str);

        foreach (string str in csvManager.hashBGM)
            audioManager.info.LoadBGM(str);
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
            default:
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
                if (dialogManager.info.ReadToken() == Token.None || dialogManager.info.ReadToken() == Token.Input)
                {
                    token = csvManager.NextToken();
                    //dialogManager.info.SetToken(token);
                }
                break;
            case Token.Selection:
                if (selectionManager.info.ReadToken() == Token.None || selectionManager.info.ReadToken() == Token.Input)
                {
                    token = csvManager.NextToken();
                    /*
                    int addIdx = (int)selectionManager.info.GetSelectButton();
                    int idx = (cntSel * 3) + addIdx;
                    listCnt = listAnswer[idx];
                    cntSel++;
                    */
                    //LoadStory();
                    string btn = selectionManager.info.GetSelectButtonString();
                    csvManager.NextSelectionDialog(btn);
                    LoadStory();
                }
                break;
            case Token.Background:
            case Token.Object:
                if (backgroundManager.info.ReadToken() == Token.None || backgroundManager.info.ReadToken() == Token.Input)
                {
                    token = csvManager.NextToken();
                    LoadStory();
                }
                break;
            case Token.BGM:
            case Token.Effect:
                if (audioManager.info.ReadToken() == Token.None || audioManager.info.ReadToken() == Token.Input)
                {
                    token = csvManager.NextToken();
                    LoadStory();
                }
                break;
            default:
                break;
        }
    }

    private void LoadStory ()
    {
        // End Story
        if (!csvManager.NextStory())
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
        switch (csvManager.story.control)
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
                print("Error csv active stroy. Control name :: " + csvManager.story.control);
                break;
        }
        
    }

    void CtrlBackground()
    {
        SetToken(Token.Background);
        switch (csvManager.story.type)
        {
            case "On":
                backgroundManager.SetBackground(csvManager.story.name);
                backgroundManager.OnBackground();
                break;
            case "Off":
                backgroundManager.OffBackground();
                break;
            case "Change":
                backgroundManager.SetBackground(csvManager.story.name);
                break;
            default:
                print("CtrlBackground. Type name :: " + csvManager.story.type);
                break;
        }
    }

    void CtrlObject()
    {
        SetToken(Token.Object);
        switch (csvManager.story.type)
        {
            case "On":
                backgroundManager.SetObject(csvManager.story.name);
                backgroundManager.OnObject();
                break;
            case "Off":
                backgroundManager.OffObject();
                break;
            case "Change":
                backgroundManager.SetObject(csvManager.story.name);
                break;
            default:
                print("CtrlObject. Type name :: " + csvManager.story.type);
                break;
        }
    }

    void CtrlBGM()
    {
        SetToken(Token.BGM);
        switch (csvManager.story.type)
        {
            case "On":
                audioManager.OnBGM(csvManager.story.name);
                break;
            case "Off":
                break;
            default:
                print("CtrlBGM. Type name :: " + csvManager.story.type);
                break;
        }
    }

    void CtrlEffect()
    {
        SetToken(Token.Effect);
        switch (csvManager.story.type)
        {
            case "Play":
                audioManager.OnEffect(csvManager.story.name);
                break;
            case "Fade In":
                audioManager.OnEffect(csvManager.story.name);
                break;
            default:
                print("CtrlBGM. Type name :: " + csvManager.story.type);
                break;
        }
    }

    void CtrlParty()
    {
        SetToken(Token.Party);
        switch (csvManager.story.type)
        {
            case "Add":
                
                break;
            case "Remove":
                break;
            default:
                print("CtrlParty. Type name :: " + csvManager.story.type);
                break;
        }
    }

    void CtrlSelection()
    {
        SetToken(Token.Selection);

        string emotion = csvManager.story.emotion;
        int val = csvManager.story.value;
        switch (csvManager.story.type)
        {
            case "A":
                selectionManager.info.SetConfig(SelectionBtn.BUTTON_A, emotion, val);
                selectionManager.UpdateText(SelectionBtn.BUTTON_A, csvManager.story.chat);
                break;
            case "B":
                selectionManager.info.SetConfig(SelectionBtn.BUTTON_B, emotion, val);
                selectionManager.UpdateText(SelectionBtn.BUTTON_B, csvManager.story.chat);
                break;
            case "C":
                selectionManager.info.SetConfig(SelectionBtn.BUTTON_C, emotion, val);
                selectionManager.UpdateText(SelectionBtn.BUTTON_C, csvManager.story.chat);
                if (selectionManager.info.OnOff == false)
                    selectionManager.OnSelection();
                break;
            default:
                print("CtrlParty. Type name :: " + csvManager.story.type);
                break;
        }        
    }

    void SetDIalog()
    {
        string name = csvManager.story.name;
        string face = csvManager.story.face;
        
        dialogManager.SetName(name);
        characterManager.SetCharacter(name, face);

        // Dialog On
        if (dialogManager.info.onDialog == false)
        {
            dialogManager.info.onDialog = true;
            if (name == "" || name == "None")
                dialogManager.DisableName();
            else
                dialogManager.OnDialog();
        }
        /*
        if (configStory.index == beforeConfigStory.index && configStory.subIndex > beforeConfigStory.subIndex)
        {
            string str = dialogManager.GetDialog() + "\n" + configStory.chat;
            int length = dialogManager.GetDialog().Length;
            dialogManager.SetDialog(length, str);
        }
        */
        if(csvManager.story.subIndex > 1)
        {
            string str = dialogManager.GetDialog() + "\n" + csvManager.story.chat;
            int length = dialogManager.GetDialog().Length;
            dialogManager.SetDialog(length, str);
        }
        else
        {
            string str = csvManager.story.chat;
            dialogManager.SetDialog(str);
        }
    }

    void CtrlDialog()
    {
        // test. 세이브 매니저에서 불러오기.
        string emotion = "None";
        if (csvManager.story.emotion == emotion || csvManager.story.emotion == "None")
        {
            switch (csvManager.story.type)
            {
                case "On":
                    SetToken(Token.Dialog);
                    characterManager.OnCharacter(0);
                    dialogManager.info.onDialog = true;
                    dialogManager.OnDialog();
                    SetToken(Token.None);
                    break;
                case "Off":
                    SetToken(Token.Dialog);
                    characterManager.OffCharacter();
                    dialogManager.info.onDialog = false;
                    dialogManager.OffDialog();
                    SetToken(Token.None);
                    break;
                case "Play":
                    SetToken(Token.Dialog);
                    SetDIalog();
                    break;
                default:
                    ResetToken();
                    print("CtrlParty. Type name :: " + csvManager.story.type);
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
        string type = selectionManager.info.GetSelectButtonString();

        if (csvManager.story.emotion == emotion || csvManager.story.emotion == "None")
        {
            SetToken(Token.Answer);

            // A / B / C
            if (csvManager.story.type == type)
            {
                SetDIalog();
            } else
            {
                csvManager.NextDialog(csvManager.story.type);
                ResetToken();
            }
        }
    }

    void CtrlDelay()
    {
        float time = (float)csvManager.story.value;
        StartCoroutine(Delay(time));
    }

    void ResetToken()
    {
        token = csvManager.NextToken();

        SetToken(token);
    }

    void EndStory()
    {
        ResetToken();
        csvManager.ResetCSV();
        cntSel = 0;
        // 데이터 저장
        // scene 이동 : 메인 스토리이면 플레이어 홈으로, 캐릭터 스토리면 미니게임으로
    }
}
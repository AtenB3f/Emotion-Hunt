using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    SaveManager saveManager;

    Token token;

    void Start()
    {
        saveManager = GameObject.Find("Main Camera").GetComponent<SaveManager>();
        csvManager = GameObject.Find("Main Camera").GetComponent<CSVManager>();
        dialogManager = GameObject.Find("Dialog").GetComponent<DialogManager>();
        backgroundManager = GameObject.Find("GameObject").GetComponent<BackgroundManager>();
        selectionManager = GameObject.Find("Selection").GetComponent<SelectionManager>();
        selectionManager.OffSelection();
        characterManager = GameObject.Find("Characters").GetComponent<CharacterManager>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        LoadFile();
        LoadScript();
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
        
        if (token == Token.Input)
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
        else if(token == Token.None)
        {
            LoadStory();
        }
        else if (token == Token.Selection)
        {
            if (selectionManager.info.OnOff == false)
                ActiveStory();
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

    private void LoadScript()
    {
        string emotion = saveManager.GetTargetEmotion();
        switch (emotion)
        {
            case "Love":
                csvManager.currentStory = csvManager.storyLove;
                break;
            case "Philia":
                csvManager.currentStory = csvManager.storyPhilia;
                break;
            case "Sympathy":
                csvManager.currentStory = csvManager.storySympathy;
                break;
            case "LoveAndHate":
                csvManager.currentStory = csvManager.storyLoveAndHate;
                break;
            case "Hate":
                csvManager.currentStory = csvManager.storyHate;
                break;
            case "Contempt":
                csvManager.currentStory = csvManager.storyContempt;
                break;
            case "None":
                csvManager.currentStory = csvManager.storyNone;
                break;
            default:
                csvManager.currentStory = csvManager.storyNone;
                Debug.LogError("LoadScript :: "+ emotion);
                break;
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
            case Token.Party:
                if (dialogManager.info.ReadToken() == Token.None || dialogManager.info.ReadToken() == Token.Input)
                {
                    token = csvManager.NextToken();
                }
                break;
            case Token.Selection:
                if (selectionManager.info.ReadToken() == Token.None || selectionManager.info.ReadToken() == Token.Input)
                {
                    token = csvManager.NextToken();
                }
                break;
            case Token.Background:
            case Token.Object:
                if (backgroundManager.info.ReadToken() == Token.None || backgroundManager.info.ReadToken() == Token.Input)
                {
                    token = csvManager.NextToken();
                }
                break;
            case Token.BGM:
            case Token.Effect:
                if (audioManager.info.ReadToken() == Token.None || audioManager.info.ReadToken() == Token.Input)
                {
                    token = csvManager.NextToken();
                }
                break;
            default:
                break;
        }
    }

    private void LoadStory ()
    {
        // End Story
        if (csvManager.CheckLast())
        {
            print("End Story");
            //delay
            EndStory();
            return;
        }
        csvManager.SetNextStory();
        ActiveStory();
    }

    void LoadStory(int index)
    {
        // End Story
        if (csvManager.CheckLast())
        {
            print("End Story");
            //delay
            EndStory();
            return;
        }
        csvManager.SetConfig(index, ref csvManager.story);
        ActiveStory();
    }

    void ActiveStory ()
    {
        //if (!CheckVersion(csvManager.story.version))
            //return addCnt;

        switch (csvManager.story.ctrl)
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
                print("Error csv active stroy. Control name :: " + csvManager.story.ctrl);
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
                audioManager.PlayEffect(csvManager.story.name);
                break;
            case "On":
                audioManager.OnEffect(csvManager.story.name);
                break;
            default:
                print("CtrlBGM. Type name :: " + csvManager.story.type);
                break;
        }
    }

    void CtrlParty()
    {
        const int MAX_PARTY = 3;
        SetToken(Token.Party);
        switch (csvManager.story.type)
        {
            case "Add":
                characterManager.SetCharacter(csvManager.story.name, csvManager.story.face);
                break;
            case "Remove":
                int idxRmv = characterManager.RemoveParty(csvManager.story.name);
                if(idxRmv < MAX_PARTY)
                    characterManager.OffCharacter(idxRmv);
                break;
            default:
                print("CtrlParty. Type name :: " + csvManager.story.type);
                break;
        }
        SetToken(Token.None);
    }

    void CtrlSelection()
    {
        SetToken(Token.Selection);

        int count = csvManager.listCnt;
        for(int i = 0; i<3; i++)
        {
            StoryConfig? config = csvManager.GetConfig(count + i);

            if (config == null)
                continue;

            string emotion = config.Value.emotion;
            int val = config.Value.value;

            switch (config.Value.type)
            {
                case "A":
                    selectionManager.info.SetConfig(SelectionBtn.BUTTON_A, emotion, val, config.Value.nextIndex);
                    selectionManager.UpdateText(SelectionBtn.BUTTON_A, config.Value.chat);
                    break;
                case "B":
                    selectionManager.info.SetConfig(SelectionBtn.BUTTON_B, emotion, val, config.Value.nextIndex);
                    selectionManager.UpdateText(SelectionBtn.BUTTON_B, config.Value.chat);
                    break;
                case "C":
                    selectionManager.info.SetConfig(SelectionBtn.BUTTON_C, emotion, val, config.Value.nextIndex);
                    selectionManager.UpdateText(SelectionBtn.BUTTON_C, config.Value.chat);
                    if (selectionManager.info.OnOff == false)
                        selectionManager.OnSelection();
                    break;
                default:
                    print("CtrlParty. Type name :: " + config.Value.type);
                    break;
            }
        }
    }
    void SetName()
    {
        string name = csvManager.story.name;

        if (csvManager.story.subCtrl == "Unknown")
            name = "???";

        dialogManager.SetName(name);
    }
    void SetCharacter()
    {
        string name = csvManager.story.name;
        string face = csvManager.story.face;
        characterManager.SetCharacter(name, face);
    }
    void SetDIalog()
    {
        string name = csvManager.story.name;

        SetName();
        SetCharacter();
        characterManager.SetImageDark(name);

        // Dialog On
        if (name == "" || name == "None")
            dialogManager.DisableName();
        else if(dialogManager.info.onName == false)
            dialogManager.EnableName();

        if (csvManager.story.subIndex > 1)
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
    bool CheckVersion(string version)
    {
        string saveEmotion = saveManager.GetTargetEmotion();
        if (saveEmotion == version || version == "None")
            return true;
        return false;
    }
    void CtrlDialog()
    {
        switch (csvManager.story.type)
        {
            case "On":
                SetToken(Token.Dialog);
                SetName();
                SetCharacter();
                dialogManager.info.onDialog = true;
                dialogManager.OnDialog();
                break;
            case "Off":
                    SetToken(Token.Dialog);
                characterManager.OffCharacter();
                dialogManager.info.onDialog = false;
                dialogManager.OffDialog();
                characterManager.info.ResetMember();
                break;
            case "Play":
                SetToken(Token.Dialog);
                SetDIalog();
                break;
            case "Shake":
                SetToken(Token.Dialog);
                SetDIalog();
                break;
            default:
                ResetToken();
                print("CtrlParty. Type name :: " + csvManager.story.type);
                break;
        }
    }

    void CtrlAnswer()
    {
        string type = selectionManager.info.GetSelectButtonString();

        SetToken(Token.Answer);

        // A / B / C
        SetDIalog();

        if (csvManager.story.type != type)
        {
            Debug.LogWarning("CtrlAnswer:: script error. Anwer is different type.");
            Debug.LogWarning(csvManager.story.index + "   " + csvManager.story.nextIndex);
        }
    }

    void CtrlDelay()
    {
        SetToken(Token.Delay);
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

        saveManager.NextContent();
        SceneManager.LoadScene("MiniGame");
    }
}
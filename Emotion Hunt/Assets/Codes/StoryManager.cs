using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum StoryControl
{
    BackgroundInfo,
    Object,
    Music,
    Effect,
    Party,
    Selection,
    Dialog,
    Answer,
    Delay,
}

enum StoryType
{
    A,
    B,
    C,
    On,
    Off,
    Play,
    Repeat,
    Add,
    Remove
}

public struct Story
{
    int index;
    int subIndex;
    StoryControl control;
    StoryType type;
    float value;
    Emotion emotion;
    CharacterChat chat;
}


public class StoryManager : MonoBehaviour
{
    private int day = 0;
    DialogManager dialogManager;
    SelectionManager selectionManager;
    BackgroundManager backgroundManager;
    CharacterManager characterManager;
    private bool token = true;

    
    void Start()
    {
        dialogManager = GameObject.Find("Dialog").GetComponent<DialogManager>();
        backgroundManager = GameObject.Find("Background").GetComponent<BackgroundManager>();
        selectionManager = GameObject.Find("Selection").GetComponent<SelectionManager>();
        selectionManager.OffSelection();
        characterManager = GameObject.Find("Characters").GetComponent<CharacterManager>();
        // 공략 NPC 확인
        // day확인
        // 동작 파악
        // 배경, 브금 설정
        /*
        List<Dictionary<string, object>> storyList = CSVReader.Read("Script/Helen/Story_Helen_1");
        for (int i=0; i<storyList.Count; i++)
        {
            print("Index :: " + storyList[i]["Index"] );
            print("Sub Index :: " + storyList[i]["Sub Index"]);
            print("Chat :: " + storyList[i]["Chat"]);
        }
        */
    }

    int cnt = 0;
    void Update()
    {

        //test code
        if (Input.GetKeyUp(KeyCode.Backspace))
        {
            dialogManager.OnDialog();
            selectionManager.OnSelection();
            SetName("Elisha");
            //characterManager.info.AddNumNPC();
            //characterManager.OnCharacter();
            //characterManager.SettingCharacter();
            
        }
        // 키 입력에 따라 현재 진행 대사 확인
        if(Input.GetKeyUp(KeyCode.Space))
        {
            //StartCoroutine("SetDialog");
            //selectionManager.UpdateText(0, "대사 바꾸기");

            dialogManager.OffDialog();
            selectionManager.OffSelection();
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            //StartCoroutine("SetDialog");
            //selectionManager.UpdateText(1, "바꿨으");
        }
    }

    // Background
    void OnBackground(int num)
    {
        
    }

    // Dialog

    void SetName(string name)
    {
        dialogManager.PrintName(name);
    }

    /*
     *  SetDialog (미완)
     *  코루틴을 통해 하나씩 글자를 DialogManager에 전달하는 함수.
     *  
     *  아직 미완성으로 text를 시나리오 데이터에서 찾아 넣어야 함.
     */
    public string test;
    IEnumerator SetDialog()
    {
        WaitForSeconds ws = new WaitForSeconds(0.015f);
        int testCnt = test.Length;
        char[] printStr = new char[testCnt];

        for(int i=0;i<testCnt;i++)
        {
            test.CopyTo(i, printStr, i, 1);
            string setStr = new string(printStr);
            dialogManager.PrintDialog(setStr);

            yield return ws;
        }
    }

    void EndStory()
    {
        // 데이터 저장
        // scene 이동 : 메인 스토리이면 플레이어 홈으로, 캐릭터 스토리면 미니게임으로
    }
}
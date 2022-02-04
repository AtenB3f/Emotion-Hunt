using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct StoryConfig
{
    public int index;
    public int subIndex;
    public int nextIndex;
    public string version;
    public string ctrl;
    public string subCtrl;
    public string type;
    public int value;
    public string emotion;
    public string name;
    public string face;
    public string image;
    public string chat;
}

public class CSVManager : MonoBehaviour
{
    public SaveManager saveManager;

    public bool doneLoadStory = false;
    public StoryConfig prevStory;
    public StoryConfig story;
    List<Dictionary<string, object>> storyList;
    public int listCnt = 0;

    public HashSet<string> hashNPC = new HashSet<string>();
    public HashSet<string> hashBG = new HashSet<string>();
    public HashSet<string> hashObj = new HashSet<string>();
    public HashSet<string> hashEffect = new HashSet<string>();
    public HashSet<string> hashBGM = new HashSet<string>();

    public LinkedList<int> storyNone = new LinkedList<int>();
    public LinkedList<int> storyLove = new LinkedList<int>();
    public LinkedList<int> storyPhilia = new LinkedList<int>();
    public LinkedList<int> storySympathy = new LinkedList<int>();
    public LinkedList<int> storyLoveAndHate = new LinkedList<int>();
    public LinkedList<int> storyHate = new LinkedList<int>();
    public LinkedList<int> storyContempt = new LinkedList<int>();

    public Dictionary<int,string> listSelection = new Dictionary<int,string>();

    void Start()
    {
        saveManager = GameObject.Find("Main Camera").GetComponent<SaveManager>();

        string targetNPC = "Helen";
        //string targetNPC = saveManager.GetTargetNpc();
        int playDay = 2;
        //int playDay = saveManager.GetPlayDay();

        // CSV Read
        //string path = "Script/" + targetNPC + "/Story_" + targetNPC + "_" + playDay.ToString();
        
        // test csv file read
        string path = "Script/" + targetNPC + "/Story_" + targetNPC + "_" + "2";
        storyList = CSVReader.Read(path);

        ReadStory();
        SetConfig();
        doneLoadStory = true;
    }

    void Update()
    {
        
    }

    public void SetConfig()
    {
        SetConfig(listCnt, ref story);
    }

    public bool CheckLast()
    {
        if (storyList.Count <= listCnt)
            return true;
        return false;
    }

    /*
     * [SetConfig]
     * 
     * csv파일을 파싱하고 스크립트 데이터를 구조체에 데이터를 담기 위한 함수.
     * 
     * - parameters
     * index : storyList리스트의 몇 번재 데이터를 읽을 것인지
     */
    private void SetConfig(int index, ref StoryConfig config)
    {
        if (index < 0 || index >= storyList.Count)
            return;

        string idx = storyList[index]["Index"].ToString();
        if (idx != null && idx != "")
            config.index = int.Parse(idx);
        string subIdx = storyList[index]["Sub Index"].ToString();
        if (subIdx != null && subIdx != "")
            config.subIndex = int.Parse(subIdx);
        string nextIdx = storyList[index]["Next Index"].ToString();
        if (nextIdx != null && nextIdx != "")
            config.nextIndex = int.Parse(nextIdx);
        config.version = storyList[index]["Version"].ToString();
        config.ctrl = storyList[index]["Control"].ToString();
        config.subCtrl = storyList[index]["Sub Control"].ToString();
        config.type = storyList[index]["Type"].ToString();
        string val = storyList[index]["Value"].ToString();
        if (val != null && val != "")
            config.value = int.Parse(val);
        config.emotion = storyList[index]["Emotion"].ToString();
        config.name = storyList[index]["Name"].ToString();
        config.face = storyList[index]["Face"].ToString();
        config.face = storyList[index]["Image"].ToString();
        config.chat = storyList[index]["Chat"].ToString();
    }

    /* 
     * [Read Story]
     * 
     * CSV파일을 읽어 스크립트에 등장하는 파일(배경 이미지, 인물 이미지, 배경 사운드 등) 목록을 파악하기 위한 함수.
     * 
     */
    private void ReadStory()
    {
        for (int i = 0; i < storyList.Count; i++)
        {
            ReadVersion(i);
            ReadControl(i);
        }
    }

    private void ReadVersion(int index)
    {
        string version = storyList[index]["Version"].ToString();
        int nextIndex = 0;
        bool num = int.TryParse(storyList[index]["Next Index"].ToString(), out nextIndex);
        if (!num)
            return;

        switch (version)
        {
            case "None":
                storyNone.AddLast(nextIndex);
                storyLove.AddLast(nextIndex);
                storyPhilia.AddLast(nextIndex);
                storySympathy.AddLast(nextIndex);
                storyLoveAndHate.AddLast(nextIndex);
                storyHate.AddLast(nextIndex);
                storyContempt.AddLast(nextIndex);
                break;
            case "Love":
                storyLove.AddLast(nextIndex);
                break;
            case "Philia":
                storyPhilia.AddLast(nextIndex);
                break;
            case "Symphathy":
                storySympathy.AddLast(nextIndex);
                break;
            case "LoveAndHate":
                storyLoveAndHate.AddLast(nextIndex);
                break;
            case "Hate":
                storyHate.AddLast(nextIndex);
                break;
            case "Comtempt":
                storyContempt.AddLast(nextIndex);
                break;
            default:
                Debug.LogWarning("ReadVersion :: script version error");
                break;
        }
    }

    private void ReadControl(int index)
    {
        HashSet<string> hashSet;
        string type = "";
        string control = storyList[index]["Control"].ToString();

        switch (control)
        {
            case "Background":
                hashSet = hashBG;
                hashSet.Add(storyList[index]["Name"].ToString());
                break;
            case "Object":
                hashSet = hashObj;
                hashSet.Add(storyList[index]["Name"].ToString());
                break;
            case "BGM":
                hashSet = hashBGM;
                hashSet.Add(storyList[index]["Name"].ToString());
                break;
            case "Effect":
                hashSet = hashEffect;
                hashSet.Add(storyList[index]["Name"].ToString());
                break;
            case "Dialog":
                hashSet = hashNPC;
                hashSet.Add(storyList[index]["Name"].ToString());
                break;
            case "Answer":
                hashSet = hashNPC;
                hashSet.Add(storyList[index]["Name"].ToString());
                if (type != storyList[index]["Type"].ToString())
                {
                    type = storyList[index]["Type"].ToString();
                }
                break;
            case "Selection":
                string version = storyList[index]["Version"].ToString();
                int subIndex = int.Parse(storyList[index]["Sub Index"].ToString());
                if (subIndex == 1)
                    listSelection[index] = version;
                break;
            default:
                Debug.LogWarning("ReadControl:: script control name error");
                break;
        }
    }

    private void IncreaseCount()
    {
        if (listCnt >= storyList.Count)
            return;
        
        listCnt++;
        SetConfig();
    }

    private void DecreaseCount()
    {
        if (listCnt <= 0)
            return;

        listCnt--;
        SetConfig();
    }

    // Answer of seleted type (After Seleted)
    public void NextSelectionDialog(string type, int idxSelection)
    {
        if (story.type != type)
        {
            IncreaseCount();
            NextSelectionDialog(type, idxSelection);
        }
        else
        {
            SetConfig(idxSelection, ref prevStory);
            return;
        }
    }

    // Find the index after answer done
    public void NextDialog(string type)
    {
        if (story.ctrl == "Answer")
        {
            IncreaseCount();
            NextDialog(type);
        }
        else
            return;
    }

    public void CurrentStory()
    {
        IncreaseCount();
    }

    public void ResetCSV()
    {
        listCnt = 0;
    }

    public Token NextToken()
    {
        if (storyList.Count <= (listCnt))
            return Token.None;

        int idx = story.index;
        string ctrl = story.ctrl;
        int prevIdx = prevStory.index;
        string prevType = prevStory.type;
        string version = story.version;

        //세이브 매니저에서 불러오기
        string npcVersion = "None";

        if (version == "None" || version == npcVersion)
        {
            if (ctrl == "Dialog" || ctrl == "Answer")
            {
                if (prevType == "On" || prevType == "Off")
                    return Token.None;
                else if (idx != prevIdx)
                    return Token.None;
                else
                    return Token.Input;
            }
            else if (ctrl == "Party")
            {
                return Token.Input;
            }
            else
                return Token.None;
        } else
        {
            return Token.None;
        }
    }
}

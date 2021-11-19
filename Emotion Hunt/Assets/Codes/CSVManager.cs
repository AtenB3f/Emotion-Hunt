using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct StoryConfig
{
    public int index;
    public int subIndex;
    public string version;
    public string ctrl;
    public string subCtrl;
    public string type;
    public int value;
    public string emotion;
    public string name;
    public string face;
    public string chat;
}

public class CSVManager : MonoBehaviour
{
    public bool doneLoadStory = false;
    public StoryConfig prevStory;
    public StoryConfig story;
    public StoryConfig nextStory;
    List<Dictionary<string, object>> storyList;
    public int listCnt = 0;

    public HashSet<string> hashNPC = new HashSet<string>();
    public HashSet<string> hashBG = new HashSet<string>();
    public HashSet<string> hashObj = new HashSet<string>();
    public HashSet<string> hashEffect = new HashSet<string>();
    public HashSet<string> hashBGM = new HashSet<string>();
    public LinkedList<int> linkSelection = new LinkedList<int>();
    public List<int> listAnswer = new List<int>();

    void Start()
    {
        // 공략 NPC 확인
        // day확인
        string targetNPC = "Helen";
        int playDay = 2;


        // CSV Read
        string path = "Script/" + targetNPC + "/Story_" + targetNPC + "_" + playDay.ToString();
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
        SetConfig(listCnt, ref story);          // 해야할 것 
        SetConfig(listCnt - 1, ref prevStory);  // 했던 것
        SetConfig(listCnt + 1, ref nextStory);
    }

    public bool CheckLast()
    {
        if (storyList.Count <= listCnt)
            return true;
        return false;
    }

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
        config.chat = storyList[index]["Chat"].ToString();
    }

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
                    if (type != storyList[i]["Type"].ToString())
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
        string ctrl = story.ctrl;//storyList[listCnt]["Control"].ToString();
        string type = story.type;
        int prevIdx = prevStory.index;
        string prevCtrl = prevStory.ctrl;
        string prevType = prevStory.type;

        if (ctrl == "Dialog" || ctrl == "Answer")
        {
            if (prevType == "On" || prevType == "Off")
                return Token.None;
            else if (idx != prevIdx)
                return Token.None;
            else
                return Token.Input;
        }
        else
            return Token.None;
    }
}

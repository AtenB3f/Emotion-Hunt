using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class CSVManager : MonoBehaviour
{
    public StoryConfig story;
    List<Dictionary<string, object>> storyList;
    int listCnt = 0;

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
        int playDay = 1;


        // CSV Read
        string path = "Script/" + targetNPC + "/Story_" + targetNPC + "_" + playDay.ToString();
        storyList = CSVReader.Read(path);

        ReadStory();
    }

    void Update()
    {
        
    }

    public void SetConfig()
    {
        string idx = storyList[listCnt]["Index"].ToString();
        if (idx != null && idx != "")
            story.index = int.Parse(idx);
        string subIdx = storyList[listCnt]["Sub Index"].ToString();
        if (subIdx != null && subIdx != "")
            story.subIndex = int.Parse(subIdx);
        story.control = storyList[listCnt]["Control"].ToString();
        story.type = storyList[listCnt]["Type"].ToString();
        string val = storyList[listCnt]["Value"].ToString();
        if (val != null && val != "")
            story.value = int.Parse(val);
        story.emotion = storyList[listCnt]["Emotion"].ToString();
        story.name = storyList[listCnt]["Name"].ToString();
        story.face = storyList[listCnt]["Face"].ToString();
        story.chat = storyList[listCnt]["Chat"].ToString();
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

    // Answer of seleted type
    public void NextSelectionDialog(string type)
    {
        listCnt++;
        SetConfig();

        if (story.type != type)
            NextSelectionDialog(type);
        else
            return;
    }
    public void NextDialog(string type)
    {
        listCnt++;
        SetConfig();

        

        if (story.control == "Answer")
            NextDialog(type);
        else
        {
            listCnt--;
            return;
        }

    }

    // End Story(false) / if not (true)
    public bool NextStory()
    {
        SetConfig();

        listCnt++;

        if ((listCnt-1) >= storyList.Count)
        {
            return false;
        }

        return true;
    }

    public void ResetCSV()
    {
        listCnt = 0;
    }

    public Token NextToken()
    {
        if (storyList.Count <= (listCnt))
            return Token.None;

        string ctrl = storyList[listCnt]["Control"].ToString();

        if (ctrl == "Dialog" || ctrl == "Answer")
            return Token.Input;

        return Token.None;
    }
}

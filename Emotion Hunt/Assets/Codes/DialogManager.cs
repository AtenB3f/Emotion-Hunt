using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public DialogInfo info = new DialogInfo();
    
    const float ON_OFF_TIME = 0.6f;
    const float SET_ALPHA = 0.7f;
    const float CHAT_SPEED_NOMAL = 0.05f;
    const float CHAT_SPEED_FAST = 0.035f;
    float chatSpeed = CHAT_SPEED_NOMAL;

    public GameObject dialogObj;
    public GameObject nameObj;
    public GameObject chatObj;
    public GameObject skipObj;
    public GameObject triangleObj;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI chatText;
    private Image nameLineImg;
    private Image chatImg;
    private Image nameImg;
    private Image triangleImg;
    private Animation triangleAni;
    
    public void Start()
    {
        // Load object
        nameImg = nameObj.GetComponent<Image>();
        chatImg = chatObj.GetComponent<Image>();
        nameText = nameObj.GetComponentInChildren<TextMeshProUGUI>();
        nameLineImg = nameText.GetComponentInChildren<Image>();
        chatText = chatObj.GetComponentInChildren<TextMeshProUGUI>();
        triangleAni = triangleObj.GetComponent<Animation>();
        triangleImg = triangleObj.GetComponent<Image>();

        SetAlpha(0.0f);
        OffDialog();
    }

    public void Update()
    {
        // If Coroutine is done, token set None.
        if (changeStateCoroutine)
        {
            changeStateCoroutine = false;
            info.SetToken(Token.None);
        }
    }

    private void ResetData()
    {
        nameText.text = "";
        chatText.text = "";
    }

    private void SetAlpha(float value)
    {
        Color chatImgColor = chatImg.color;
        Color nameImgColor = nameImg.color;
        Color nameLineImgColor = nameLineImg.color;
        Color nameTextColor = nameText.color;
        Color chatTextColor = chatText.color;
        Color triangleColor = triangleImg.color;

        chatImgColor.a = value;
        nameImgColor.a = value;
        nameLineImgColor.a = value;
        nameTextColor.a = value;
        chatTextColor.a = value;
        triangleColor.a = value;

        chatImg.color = chatImgColor;
        nameImg.color = nameImgColor;
        nameLineImg.color = nameImgColor;
        nameText.color = nameTextColor;
        chatText.color = chatTextColor;
        triangleImg.color = triangleColor;
    }

    public string GetDialog()
    {
        return chatText.text;
    }

    public void PrintChat(string text)
    {
        chatText.text = text;
    }

    public void PrintName(string name)
    {
        nameText.text = name;
    }

    public void OnDialog()
    {
        ResetData();
        OnName();
        OnSkip();
        OnChat();
        OnTriangle();
    }

    public void OffDialog()
    {
        OffName();
        OffSkip();
        OffChat();
        OffTriangle();
        ResetData();
    }
    public void OnName()
    {
        nameImg.DOFade(SET_ALPHA, ON_OFF_TIME);
        Color col = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        nameLineImg.DOColor(col, ON_OFF_TIME);
        nameText.DOFade(1.0f, ON_OFF_TIME);
    }

    public void OnChat()
    {
        chatImg.DOFade(SET_ALPHA, ON_OFF_TIME);
        chatText.DOFade(1.0f, ON_OFF_TIME);
        triangleImg.DOFade(1.0f, ON_OFF_TIME);
    }

    public void OnTriangle()
    {
        triangleObj.SetActive(true);
        triangleImg.DOFade(1.0f, ON_OFF_TIME);
        PlayTriangelAni(true);
    }

    public void OnSkip()
    {
        skipObj.SetActive(true);
    }

    public void OffName()
    {
        nameText.DOFade(0.0f, ON_OFF_TIME);
        nameLineImg.DOFade(0.0f, ON_OFF_TIME);
        nameImg.DOFade(0.0f, ON_OFF_TIME);
    }
    public void OffChat()
    {
        chatImg.DOFade(0.0f, ON_OFF_TIME);
        chatText.DOFade(0.0f, ON_OFF_TIME);
    }
    public void OffTriangle()
    {
        PlayTriangelAni(false);
        triangleObj.SetActive(false);
    }

    public void OffSkip()
    {
        skipObj.SetActive(false);
    }

    private void PlayTriangelAni(bool play)
    {
        if (play == true)            
            triangleAni.Play();
        else
            triangleAni.Stop();
    }

    public void SetName(string name)
    {
        string str = "";
        switch (name)
        {
            case "Player":
                // 저장된 이름 불러오기
                break;
            case "Helen":
                str = "헬렌";
                break;
            case "Lyam":
                str = "리암";
                break;
            case "Elisha":
                str = "엘리샤";
                break;
            case "Edwin":
                str = "에드윈";
                break;
            case "Julieta":
                str = "줄리에타";
                break;
            case "Michael":
                str = "미카엘";
                break;
            case "Miranda":
                str = "미란다";
                break;
            case "Robert":
                str = "로베르";
                break;
            case "Vivi":
                str = "비비";
                break;
            case "Vivan":
                str = "비반";
                break;
            default:
                str = name;
                break;
        }
        PrintName(str);
    }

    public void SetDialog(string str)
    {
        StartCoroutine(PrintDialog(str));
    }

    public void SetDialog(int stPoint, string str)
    {
        StartCoroutine(PrintDialog(stPoint, str));
    }

    bool changeStateCoroutine = false;

    IEnumerator PrintDialog(string str)
    {
        WaitForSeconds ws = new WaitForSeconds(chatSpeed);
        int length = str.Length;

        for (int i = 0; i < length; i++)
        {
            string data = str.Substring(0, i+1);
            PrintChat(data);

            yield return ws;
        }
        changeStateCoroutine = true;
    }
    IEnumerator PrintDialog(int stPoit, string str)
    {
        WaitForSeconds ws = new WaitForSeconds(chatSpeed);
        int length = str.Length;

        for (int i = stPoit; i < length; i++)
        {
            string data = str.Substring(0, i+1);
            PrintChat(data);

            yield return ws;
        }
        changeStateCoroutine = true;
    }

}

public class DialogInfo
{
    Token token = Token.None;
    public bool onDialog = false;

    public void SetToken(Token type)
    {
        token = type;
    }

    public Token ReadToken()
    {
        return token;
    }
}
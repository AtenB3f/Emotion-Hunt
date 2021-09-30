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
    const float SET_ALPHA = 0.8f;

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

    public void PrintDialog(string text)
    {
        chatText.text = text;
    }

    public void PrintName(string name)
    {
        nameText.text = name;
    }

    public void OnDialog()
    {
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

}

public class DialogInfo
{
    public bool token = false;
    public string name;
    public bool skip = false;

    public void SetName(string str)
    {
        name = str;
    }
    public void ResetInfo()
    {
        name = null;
        token = false;
        skip = false;
    }

    public void ResetSkip()
    {
        token = false;
        skip = false;
    }
}
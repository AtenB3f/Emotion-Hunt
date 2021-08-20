using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class DialogManager : MonoBehaviour
{ 
    public GameObject dialogObj;
    public GameObject nameObj;
    public GameObject chatObj;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI chatText;
    private Image[] dialogImg;
    public DialogInfo info;


    public void Start()
    {
        // Load object
        dialogImg = new Image[2];
        dialogImg = dialogObj.GetComponentsInChildren<Image>();     // [0] : Chat ,[1] : Name

        nameText = nameObj.GetComponentInChildren<TextMeshProUGUI>();
        chatText = chatObj.GetComponentInChildren<TextMeshProUGUI>();

        // Alpha Setting
        Color chatImgColor = dialogImg[0].color;
        Color nameImgColor = dialogImg[1].color;
        Color nameTextColor = nameText.color;
        Color chatTextColor = chatText.color;

        chatImgColor.a = 0.0f;
        nameImgColor.a = 0.0f;
        nameTextColor.a = 0.0f;
        chatTextColor.a = 0.0f;

        dialogImg[0].color = chatImgColor;
        dialogImg[1].color = nameImgColor;
        nameText.color = nameTextColor;
        chatText.color = chatTextColor;
    }

    void Update()                 
    {
        
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
        dialogImg[0].DOFade(1.0f, 0.6f);
        dialogImg[1].DOFade(1.0f, 0.6f);
        nameText.DOFade(1.0f, 0.6f);
        chatText.DOFade(1.0f, 0.6f);
    }

    public void OffDialog()
    {
        dialogImg[0].DOFade(0.0f, 0.6f);
        dialogImg[1].DOFade(0.0f, 0.6f);
        nameText.DOFade(0.0f, 0.6f);
        chatText.DOFade(0.0f, 0.6f);
    }
    
}

public class DialogInfo
{
    bool namePos = false;       // left : false / right :  true
    bool readText = false;      // not read yet : false / read : true
}
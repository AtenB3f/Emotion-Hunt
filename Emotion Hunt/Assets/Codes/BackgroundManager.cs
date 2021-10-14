﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BackgroundManager : MonoBehaviour
{
    BackgroundInfo info = new BackgroundInfo();

    private const float ON_TIME = 0.6f;
    private const float OFF_TIME = 0.6f;

    private const int NUM_BG_IMG = 10;
    private const int NUM_OBJ_IMG = 10;

    private string[] nameBG;
    private string[] nameObj;

    public Image imgBackground;
    public Image imgObject;

    private Dictionary<string, Sprite> fileBackground;
    private Dictionary<string, Sprite> fileObject;

    void Start()
    {
        Color colorBG = imgBackground.color;
        Color colorObj = imgObject.color;
        colorBG.a = 0.0f;
        colorObj.a = 0.0f;
        imgBackground.color = colorBG;
        imgObject.color = colorObj;
    }

    // Day 시작했을 때 Stroy Manager에서 사용
    public void LoadBackground(string name, string path)
    {
        Sprite tmp = Resources.Load<Sprite>(path) as Sprite;
        fileBackground.Add(name, tmp);
        
    }
    // Day 시작했을 때 Stroy Manager에서 사용
    public void LoadObject(string name, string path)
    {
        Sprite tmp = Resources.Load<Sprite>(path) as Sprite;
        fileObject.Add(name, tmp);
    }

    public void OnBackground()
    {
        imgBackground.DOFade(1.0f, ON_TIME);
    }

    public void OffBackground()
    {
        imgBackground.DOFade(0.0f, OFF_TIME);
    }

    public void OnObject()
    {
        imgObject.DOFade(1.0f, ON_TIME);
    }

    public void OffObject()
    {
        imgObject.DOFade(0.0f, OFF_TIME);
    }

    public void SetBackground(string name)
    {
        if (fileBackground == null)
            return;

        if (fileBackground.ContainsKey(name))
        {
            imgBackground.sprite = fileBackground[name];
        }
        else
        {
            print("SetBackgrounc Error. Not Exist File");
            return;
        }
    }

    public void SetObject(string name)
    {
        if (fileObject == null)
            return;

        if (fileObject.ContainsKey(name))
        {
            imgObject.sprite = fileObject[name];
        }
        else
        {
            print("SetObject Error. Not Exist File");
            return;
        }
    }
}

public class BackgroundInfo
{
    bool token = false;
}

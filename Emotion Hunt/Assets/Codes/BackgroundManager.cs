using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BackgroundManager : MonoBehaviour
{
    public BackgroundInfo info = new BackgroundInfo();

    private const float ON_OFF_TIME = 0.6f;

    private const int NUM_BG_IMG = 10;
    private const int NUM_OBJ_IMG = 10;

    private string[] nameBG;
    private string[] nameObj;

    public Image imgBackground;
    public Image imgObject;
    
    void Start()
    {
        Color colorBG = imgBackground.color;
        Color colorObj = imgObject.color;
        colorBG.a = 0.0f;
        colorObj.a = 0.0f;
        imgBackground.color = colorBG;
        imgObject.color = colorObj;
    }

    private void ResetToken()
    {
        info.SetToken(Token.None);
    }

    public void OnBackground()
    {
        imgBackground.DOFade(1.0f, ON_OFF_TIME);
        Invoke("ResetToken", ON_OFF_TIME);
    }

    public void OffBackground()
    {
        imgBackground.DOFade(0.0f, ON_OFF_TIME);
        Invoke("ResetToken", ON_OFF_TIME);
    }

    public void OnObject()
    {
        imgObject.DOFade(1.0f, ON_OFF_TIME);
        Invoke("ResetToken", ON_OFF_TIME);
    }

    public void OffObject()
    {
        imgObject.DOFade(0.0f, ON_OFF_TIME);
        Invoke("ResetToken", ON_OFF_TIME);
    }

    public void SetBackground(string name)
    {
        if (info.fileBackground == null)
        {
            ResetToken();
            return;
        }
        if (info.fileBackground.ContainsKey(name))
        {
            imgBackground.sprite = info.fileBackground[name];
        }
        else
        {
            print("SetBackgrounc Error. Not Exist File");
            return;
        }
        ResetToken();
    }

    public void SetObject(string name)
    {
        if (info.fileObject == null)
        {
            ResetToken();
            return;
        }

        if (info.fileObject.ContainsKey(name))
        {
            imgObject.sprite = info.fileObject[name];
        }
        else
        {
            print("SetObject Error. Not Exist File");
            return;
        }
        ResetToken();
    }
}

public class BackgroundInfo
{
    Token token = Token.None;

    public Dictionary<string, Sprite> fileBackground;
    public Dictionary<string, Sprite> fileObject;

    bool onBackground = false;
    bool onObject = false;

    public void SetToken(Token type)
    {
        token = type;
    }
    public Token ReadToken()
    {
        return token;
    }

    public void LoadBackground(string name)
    {
        string path = "Image/Background/" + name;
        Sprite tmp = Resources.Load<Sprite>(path) as Sprite;
        if (tmp != null)
            fileBackground.Add(name, tmp);
        else
            Debug.LogError("Load Background Error. No Exist File");

    }
    public void LoadObject(string name)
    {
        string path = "Image/Object/" + name;
        Sprite tmp = Resources.Load<Sprite>(path) as Sprite;
        if (tmp != null)
            fileObject.Add(name, tmp);
        else
            Debug.LogError("Load Object Error. No Exist File");
    }

}

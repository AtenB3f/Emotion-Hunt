using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class SelectionManager : MonoBehaviour
{
    public SelectionInfo info = new SelectionInfo();

    public GameObject Selection;
    public GameObject A;
    public GameObject B;
    public GameObject C;

    private CanvasGroup[] groupA;
    private CanvasGroup[] groupB;
    private CanvasGroup[] groupC;

    private TextMeshProUGUI[] nomalText = new TextMeshProUGUI[3];
    private TextMeshProUGUI[] highlightText = new TextMeshProUGUI[3];

    private Image[] btnImg = new Image[3];
    private Image[] btnBG = new Image[3];

    const int BUTTON_A = 0;
    const int BUTTON_B = 1;
    const int BUTTON_C = 2;

    const int NOMAL = 0;
    const int HIGHLIGHT = 1;

    const float ON_OFF_TIME = 0.6f;

    void Start()
    {
        groupA = A.GetComponentsInChildren<CanvasGroup>();
        groupB = B.GetComponentsInChildren<CanvasGroup>();
        groupC = C.GetComponentsInChildren<CanvasGroup>();

        btnBG[BUTTON_A] = A.GetComponentInChildren<Image>();
        btnBG[BUTTON_B] = B.GetComponentInChildren<Image>();
        btnBG[BUTTON_C] = C.GetComponentInChildren<Image>();

        btnImg[BUTTON_A] = A.GetComponent<Image>();
        btnImg[BUTTON_B] = B.GetComponent<Image>();
        btnImg[BUTTON_C] = C.GetComponent<Image>();

        nomalText[BUTTON_A] = groupA[NOMAL].GetComponentInChildren<TextMeshProUGUI>(); 
        highlightText[BUTTON_A] = groupA[HIGHLIGHT].GetComponentInChildren<TextMeshProUGUI>();
        nomalText[BUTTON_B] = groupB[NOMAL].GetComponentInChildren<TextMeshProUGUI>();
        highlightText[BUTTON_B] = groupB[HIGHLIGHT].GetComponentInChildren<TextMeshProUGUI>();
        nomalText[BUTTON_C] = groupC[NOMAL].GetComponentInChildren<TextMeshProUGUI>();
        highlightText[BUTTON_C] = groupC[HIGHLIGHT].GetComponentInChildren<TextMeshProUGUI>();

        SetAlpha(0.0f);
    }

    void SetAlpha(float alpha)
    {
        groupA[NOMAL].alpha = alpha;
        groupA[HIGHLIGHT].alpha = alpha;
        groupB[NOMAL].alpha = alpha;
        groupB[HIGHLIGHT].alpha = alpha;
        groupC[NOMAL].alpha = alpha;
        groupC[HIGHLIGHT].alpha = alpha;

        for (int i = 0; i < 3; i++)
        {
            Color col = btnImg[i].color;
            col.a = alpha;
            btnImg[i].color = col;

            col = btnBG[i].color;
            col.a = alpha;
            btnBG[i].color = col;

            col = nomalText[i].color;
            col.a = alpha;
            nomalText[i].color = col;

            col = highlightText[i].color;
            col.a = alpha;
            highlightText[i].color = col;
        }
    }

    public void OnSelection()
    {
        float alpha = 1.0f;
        Selection.SetActive(true);
        SetAlpha(alpha);
        FirstHighlight();
    }

    public void OffSelection()
    {
        float alpha = 0.0f;
        Selection.SetActive(false);
        SetAlpha(alpha);
        firstHighlight = false;
    }

    // btnIdx : A(0) / B(1) / C(2)
    public void UpdateText(int btnIdx, string str)
    {

        switch (btnIdx)
        {
            case BUTTON_A:
                nomalText[btnIdx] = groupA[NOMAL].GetComponentInChildren<TextMeshProUGUI>();
                highlightText[btnIdx] = groupA[HIGHLIGHT].GetComponentInChildren<TextMeshProUGUI>();
                break;
            case BUTTON_B:
                nomalText[btnIdx] = groupB[NOMAL].GetComponentInChildren<TextMeshProUGUI>();
                highlightText[btnIdx] = groupB[HIGHLIGHT].GetComponentInChildren<TextMeshProUGUI>();
                break;
            case BUTTON_C:
                nomalText[btnIdx] = groupC[NOMAL].GetComponentInChildren<TextMeshProUGUI>();
                highlightText[btnIdx] = groupC[HIGHLIGHT].GetComponentInChildren<TextMeshProUGUI>();
                break;
            default:
                nomalText[btnIdx] = groupA[NOMAL].GetComponentInChildren<TextMeshProUGUI>();
                highlightText[btnIdx] = groupA[HIGHLIGHT].GetComponentInChildren<TextMeshProUGUI>();
                break;
        }

        nomalText[btnIdx].text = str;
        highlightText[btnIdx].text = str;
    }

    public void SelectOptionA()
    {
        print("Select A Click!");
        info.selBtn = BUTTON_A;
        info.token = false;
    }
    public void SelectOptionB()
    {
        print("Select B Click!");
        info.selBtn = BUTTON_B;
        info.token = false;
    }
    public void SelectOptionC()
    {
        print("Select C Click!");
        info.selBtn = BUTTON_C;
        info.token = false;
    }

    bool firstHighlight = false;
    public void FirstHighlight()
    {
        if (firstHighlight == false)
            A.GetComponent<Button>().enabled = true;
    }
}

public class SelectionInfo
{
    public bool token = false;
    public int selBtn = 0;
    public void ResetInfo()
    {
        token = false;
        selBtn = 0;
    }
}
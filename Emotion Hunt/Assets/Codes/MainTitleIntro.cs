using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainTitleIntro : MonoBehaviour
{
    public GameObject TitlePanel;
    public GameObject IntroPanel;
    public Image IntroImg;
    public Image TitleImg;

    private bool SplashEn = false;

    void Start()
    {
        //IntroImg = IntroPanel.gameObject.GetComponent<Image>();//IntroPanel.GetComponent<Image>();
        //TitleImg = TitlePanel.gameObject.GetComponent<Image>();

        IntroImg.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        TitleImg.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        TitlePanel.SetActive(false);
        IntroPanel.SetActive(false);

        StartCoroutine(SplashIntro());
        StartCoroutine(TitleIntro());
    }

    void Update()
    {

    }

    private IEnumerator SplashIntro()
    {
        IntroPanel.SetActive(true);

        // Fade In 2AST Team Logo
        
        IntroImg.DOFade(1.0f,  0.8f);
        yield return new WaitForSeconds(2.0f);

        // Fade Out 2AST Team Logo
        IntroImg.DOFade(0.0f, 1.2f);
        yield return new WaitForSeconds(3.0f);

        // Title Flag On
        IntroPanel.SetActive(false);
        SplashEn = true;
        yield return null;
    }

    private IEnumerator TitleIntro()
    {
        while(!SplashEn)
            yield return null;

        Debug.Log("Start title");
        
        TitlePanel.SetActive(true);

        
        //TitlePanel.GetComponent<Image>().material.color = ImgCol;
        TitleImg.DOFade(1.0f, 1.0f);
        // Video Setting
        // Audio Setting

        // Play Video

        // Show Title Image, Icons, Bottons
        yield return null;
    }
}

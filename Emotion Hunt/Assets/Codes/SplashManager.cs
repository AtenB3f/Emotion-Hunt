using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SplashManager : MonoBehaviour
{
    public float FadeInTime, FadeOutTime, DurationTime;
    public Image logoImg;
    //public GameObject imgObj;
    
    private void Start()
    {
        logoImg = GetComponent<Image>();
        StartCoroutine(Fade());
    }
    private int NextScene()
    {
        SceneManager.LoadScene(1);

        return 1;
    }

    private int FadeIn()
    {
        logoImg.material.DOFade(1.0f, FadeInTime);

        return 1;
    }

    private int FadeOut()
    {
        logoImg.material.DOFade(0.0f, FadeOutTime);

        return 1;
    }

    private IEnumerator Fade()
    {
        yield return FadeIn();
        yield return new WaitForSeconds(DurationTime); // Stay
        yield return FadeOut();
        yield return new WaitForSeconds(DurationTime); // Stay
        yield return NextScene();
    }
}

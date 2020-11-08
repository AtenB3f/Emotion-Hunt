using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    private const int EXIT_CLOSE = 0;
    private const int EXIT_YES = 1;
    private const int EXIT_NO = 2;

    public GameObject ExitMenu;
    public GameObject ExitBtnSet;
    private Button[] Btn;

    void Start()
    {
        
        Btn = ExitBtnSet.GetComponentsInChildren<Button>();
    }

    public void ExitClose()
    {
        Btn[EXIT_CLOSE].interactable = true;
        ExitMenu.SetActive(false);
    }
    public void ExitNo()
    {
        Btn[EXIT_NO].interactable = true;
        ExitMenu.SetActive(false);
    }

    public void ExitGame()
    {
        Btn[EXIT_YES].interactable = true;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

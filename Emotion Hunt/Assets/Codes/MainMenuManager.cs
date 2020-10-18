using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*      [ Button ]
 *      
 * < Button Set Hieararchy >
 * 1. Play Button
 * 2. Collect Button
 * 3. Setting Button
 * 4. Exit Button
 * 
 * <Sub Button Set Hieararchy>
 * 1. Setting Icon
 * 2. Exit Icon
 * 
 * <Play Menu Button Set Hieararchy>
 * 1. New Game Button
 * 2. Load Game Button
 * 3. Colse Button
 * 
 * <Setting Menu Button Set Hieararchy
 * 1. Develop Info Button
 * 2. Close Button
 */

/*      [Slider]
 * 
 */

public class MainMenuManager : MonoBehaviour
{
    public const int DEF_MAINMENU_PLAY = 0;
    public const int DEF_MAINMENU_COLLECT = 1;
    public const int DEF_MAINMENU_SETTING = 2;
    public const int DEF_MAINMENU_EXIT = 3;

    public GameObject MainBtn;      // Play, Collect, Setting, Exit Button (in "TitlePanel"-"ButtonSet")
    public GameObject SubBtn;       // Setting Icon, Exit Icon Button (in "TitlePanel"-"SubButtonSet")
    public GameObject PlayObj;      // Play Menu Object
    public GameObject SettingObj;   // Setting Menu Object
    public GameObject MenuPanel;
    public GameObject SystemPanel;  // System Panel
    private int MainMenuStat;
    private Button[] MainBtnArr;
    private Button[] SubBtnArr;

    void Start()
    {
        MainBtnArr = MainBtn.GetComponentsInChildren<Button>();
        SubBtnArr = SubBtn.GetComponentsInChildren<Button>();

        // Panel Disable
        EnableSystemPanel(false);

        PlayObj.SetActive(false);
        SettingObj.SetActive(false);
    }

    void Update()
    {
    }

    public void PlayMenuCliked()
    {
        MainBtnArr[DEF_MAINMENU_PLAY].interactable = false;

        MainMenuStat = DEF_MAINMENU_PLAY;

        MenuPanel.SetActive(true);

        // Enable Play Menu Panel
        PlayObj.SetActive(true);

        // System Panel Blur Effect On
        EnableSystemPanel(true);
    }

    public void SettingMenuClick()
    {
        MainBtnArr[DEF_MAINMENU_SETTING].interactable = false;

        MainMenuStat = DEF_MAINMENU_SETTING;

        MenuPanel.SetActive(true);

        // Enable Play Menu Panel
        SettingObj.SetActive(true);

        // System Panel Blur Effect On
        EnableSystemPanel(true);
    }

    public void DisableMainMenu()
    {
        switch (MainMenuStat)
        {
            case 0:     // Play Menu
                // Disable Play Menu Panel
                PlayObj.SetActive(false);

                // System Panel Blur Effect On
                EnableSystemPanel(false);
                break;
            case 1:     // Collect Menu
                break;
            case 2:     // Setting Menu
                // Disable Play Menu Panel
                SettingObj.SetActive(false);

                // System Panel Blur Effect On
                EnableSystemPanel(false);
                break;
            case 3:     // Exit Menu
                break;
        }
        MenuPanel.SetActive(false);
    }

    private void DisableMainMenu(int Num)
    {
        switch (Num)
        {
            case 0:     // Play Menu
                // Disable Play Menu Panel
                PlayObj.SetActive(false);

                // System Panel Blur Effect On
                EnableSystemPanel(false);
                break;
            case 1:     // Collect Menu
                break;
            case 2:     // Setting Menu

                // Disable Play Menu Panel
                SettingObj.SetActive(false);

                // System Panel Blur Effect On
                EnableSystemPanel(false);
                break;
            case 3:     // Exit Menu
                break;
        }
        MenuPanel.SetActive(false);
    }

    private void EnableSystemPanel(bool En)
    {
        if(En)
            SystemPanel.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.6f);
        else
            SystemPanel.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    }

    public void ResetSelected()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}
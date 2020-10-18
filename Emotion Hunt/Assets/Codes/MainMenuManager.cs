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
    public GameObject MainBtn;      // Play, Collect, Setting, Exit Button (in "TitlePanel"-"ButtonSet")
    public GameObject SubBtn;       // Setting Icon, Exit Icon Button (in "TitlePanel"-"SubButtonSet")
    public GameObject PlayObj;      // Play Menu Object
    public GameObject SettingObj;   // Setting Menu Object
    public GameObject SystemPanel;  // System Panel

    void Start()
    {
        Button[] MainBtnArr = MainBtn.GetComponentsInChildren<Button>();
        Button[] SubBtnArr = SubBtn.GetComponentsInChildren<Button>();

        // Panel Disable
        SystemPanelEnable(false);
    }

    void Update()
    {
        
    }

    private void PlayMenu(bool En)
    {
        if(En)
        {
            PlayObj.SetActive(true);

            // System Panel Blur Effect On
            SystemPanelEnable(true);

            // Enable Play Menu Panel

        }
        else
        {
            PlayObj.SetActive(false);

            // System Panel Blur Effect On
            SystemPanelEnable(false);

            // Disable Play Menu Panel
        }

    }

    private void SystemPanelEnable(bool En)
    {
        if(En)
            SystemPanel.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.4f);
        else
            SystemPanel.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    }

    private void ButtonStatus()
    {
        
    }

    private void PlayMenu()
    {

    }

    private void SettingMenu()
    {

    }

    public void ResetSelected()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}

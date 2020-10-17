using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*
 * < Button Set Hieararchy >
 * 1. Play Button
 * 2. Collect Button
 * 3. Setting Button
 * 4. Exit Button
 * 
 * <Sub Button Set Hieararchy>
 * 1. Setting Icon
 * 2. Exit Icon
 */
public class MainMenuManager : MonoBehaviour
{
    public GameObject Btn;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void ButtonStatus()
    {
        Button[] BtnArr = Btn.GetComponentsInChildren<Button>();
    }

    public void ResetSelected()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject menuPanel;

    [SerializeField]
    private GameObject blurPanel;

    [SerializeField]
    private GameObject[] menu = { };

    void Start()
    {
        menuPanel.SetActive(false);
        blurPanel.SetActive(false);

        foreach (GameObject obj in menu)
        {
            obj.SetActive(false);
        }
    }

    public void OnMenu(int index)
    {
        if (index >= menu.Length)
            return;

        menuPanel.SetActive(true);
        blurPanel.SetActive(true);

        menu[index].SetActive(true);
    }

    public void OffMenu()
    {
        menuPanel.SetActive(false);
        blurPanel.SetActive(false);

        foreach (GameObject obj in menu)
        {
            obj.SetActive(false);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameManager : MonoBehaviour
{
    SaveManager saveManager;
    void Start()
    {
        saveManager = GameObject.Find("Main Camera").GetComponent<SaveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            EndMiniGame();
        }
    }

    void EndMiniGame()
    {
        saveManager.NextContent();
        SceneManager.LoadScene("NpcStory");
    }
}

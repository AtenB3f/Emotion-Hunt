using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public string test;
    private int day = 0;
    DialogManager dialogManager;

    void Start()
    {
        dialogManager = GameObject.Find("Dialog").GetComponent<DialogManager>();

        // 공략 NPC 확인
        // day확인
        // 동작 파악
        // 배경, 브금 설정
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Backspace))
        {
            dialogManager.OnDialog();
            SetName("Elisha");
        }
        // 키 입력에 따라 현재 진행 대사 확인
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine("SetDialog");
        }
    }

    void SetName(string name)
    {
        dialogManager.PrintName(name);
    }

    /*
     *  SetDialog (미완)
     *  코루틴을 통해 하나씩 글자를 DialogManager에 전달하ㅏ는 함수.
     *  
     *  아직 미완성으로 text를 시나리오 데이터에서 찾아 넣어야 함.
     */
    IEnumerator SetDialog()
    {
        WaitForSeconds ws = new WaitForSeconds(0.015f);
        int testCnt = test.Length;
        char[] printStr = new char[testCnt];

        for(int i=0;i<testCnt;i++)
        {
            test.CopyTo(i, printStr, i, 1);
            string setStr = new string(printStr);
            dialogManager.PrintDialog(setStr);

            yield return ws;
        }
    }

    void EndStory()
    {
        // day + 1 저장
        // 데이터 저장
        // scene 이동
    }
}
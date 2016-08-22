﻿using UnityEngine;
using System.Collections;

public class Gpt_PauseMenuUI : MonoBehaviour {

    public GameObject Parent;
    public GameObject normalParent;

    private int selectNum = 0;

    public GameObject[] selectOn1;
    public GameObject[] selectOn2;

    bool isOpened = false;
    int frameMemo = 0;
    const float slowTimeScale = 0.000001f;
    const float normalTimeScale = 1.0f;

    void Start()
    {
        Parent.SetActive(false);
        isOpened = false;
    }

    void Update()
    {
        if (isOpened) Update_Pause();
        else if (!isOpened) Update_Normal();
    }

    void Update_Normal()
    {
        if (HasOptionInput())
        {
            StartPause();
        }
    }
    void Update_Pause()
    {
        if (Gpt_Input.IsMoving)
        {
            Vector2 input = Gpt_Input.Move;
            if (input.y < 0) selectNum = 1;
            if (input.y > 0) selectNum = 0;
        }

        if (HasOptionInput())
        {
            ClosePause();
        }
        else if (Gpt_Input.Attack)
        {
            if (selectNum == 0) ClosePause();
            if (selectNum == 1) EndGame();
        }

        if (selectNum == 0)
        {
            SelectObjects(selectOn1);
            UnSelectObjects(selectOn2);
        }
        if (selectNum == 1)
        {
            UnSelectObjects(selectOn1);
            SelectObjects(selectOn2);
        }
    }

    bool HasOptionInput()
    {
        if(Gpt_Input.Option && (frameMemo != Gpt_Input.OptionStartFrame))
        {
            frameMemo = Gpt_Input.OptionStartFrame;
            return true;
        }
        return false;
    }


    void StartPause()
    {
        isOpened = true;
        Parent.SetActive(true);
        normalParent.SetActive(false);
        Time.timeScale = slowTimeScale;
    }

    void ClosePause()
    {
        isOpened = false;
        Parent.SetActive(false);
        normalParent.SetActive(true);
        Time.timeScale = normalTimeScale;
    }

    void EndGame()
    {
        Application.Quit();
    }

    void SelectObjects(GameObject[] obj)
    {
        foreach (var a in obj)
        {
            a.SetActive(true);
        }
    }

    void UnSelectObjects(GameObject[] obj)
    {
        foreach(var a in obj)
        {
            a.SetActive(false);
        }
    }
}

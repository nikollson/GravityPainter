using UnityEngine;
using System.Collections;

public class Gpt_PauseMenuUI : MonoBehaviour {

    public GameObject Parent;

    private int selectNum = 0;

    public GameObject[] selector;

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

        for (int i = 0; i < selector.Length; i++)
        {
            selector[i].SetActive(i == selectNum);
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
        Debug.Log("sttt");
        isOpened = true;
        Parent.SetActive(true);
        Time.timeScale = slowTimeScale;
    }

    void ClosePause()
    {
        Debug.Log("clcls");
        isOpened = false;
        Parent.SetActive(false);
        Time.timeScale = normalTimeScale;
    }

    void EndGame()
    {
        Application.Quit();
    }
}

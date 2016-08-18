using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Gpt_ContinueUI : MonoBehaviour
{
    public GameObject parent;
    public Image SelectorYes;
    public Image SelectorNo;

    public float canSelectTime = 1.0f;

    bool isYes = false;
    bool isActive = false;

    float count = 0;

    void Start()
    {
        SelectYes(true);
        SetOff();
    }


    void Update()
    {
        Update_Slect();
    }

    void Update_Slect()
    {
        if (Gpt_Input.IsMoving)
        {
            bool isLeft = Gpt_Input.Move.x < 0;
            SelectYes(isLeft);
            Debug.Log(isLeft);
        }
    }

    public void SelectYes(bool isYes)
    {
        this.isYes = isYes;
        SelectorYes.enabled = isYes;
        SelectorNo.enabled = !isYes;
    }

    public void SetOn()
    {
        parent.SetActive(true);
        isActive = true;
    }

    public void SetOff()
    {
        parent.SetActive(false);
        isActive = false;
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gpt_ContinueUI : MonoBehaviour
{
    public GameObject parent;
    public Image SelectorYes;
    public Image SelectorNo;

    public string tileSceneName;

    public float canSelectTime = 1.0f;

    private 

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
        if (isActive)
        {
            count += Time.deltaTime;
            Update_Slect();
        }
    }

    void Update_Slect()
    {
        if (Gpt_Input.IsMoving)
        {
            bool isLeft = Gpt_Input.Move.x < 0;
            SelectYes(isLeft);

            if (count > canSelectTime)
            {

                if (Gpt_Input.AttackDown)
                {
                    if (!isYes) SceneManager.LoadScene(tileSceneName);
                    if (isYes) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
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

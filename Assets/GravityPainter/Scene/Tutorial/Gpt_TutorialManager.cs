using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Gpt_TutorialManager : MonoBehaviour
{
    public float inputStartTime = 4.0f;
    public string nextSceneName = "";

    float count = 0;


    void Update()
    {
        count += Time.deltaTime;
        if(count > inputStartTime)
        {
            if (Gpt_Input.HasAnyKey())
            {
                Gpt_SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}

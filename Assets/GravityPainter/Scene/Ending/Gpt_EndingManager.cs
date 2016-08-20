using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Gpt_EndingManager : MonoBehaviour {

    public float inputTime = 3f;
    public string NextSceneName = "";

    float count = 0;

    void Update()
    {
        count += Time.deltaTime;

        if(count > inputTime)
        {
            if (Gpt_Input.HasAnyKey())
            {
                SceneManager.LoadScene(NextSceneName);
            }
        }
    }
}

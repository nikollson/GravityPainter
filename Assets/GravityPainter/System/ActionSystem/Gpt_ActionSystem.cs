using UnityEngine;
using System.Collections;

public class Gpt_ActionSystem : MonoBehaviour {


    void Awake()
    {
        Application.targetFrameRate = 30; //30FPSに設定
    }

    void Update()
    {
        if (Gpt_Input.OptionDown)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}

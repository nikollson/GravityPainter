using UnityEngine;
using System.Collections;

public class Gpt_ActionSystem : MonoBehaviour {

    public int targetFPS = 30;

    void Awake()
    {
        Application.targetFrameRate = targetFPS; //30FPSに設定
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        /*
        if (Gpt_Input.OptionDown)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
        */
    }
}

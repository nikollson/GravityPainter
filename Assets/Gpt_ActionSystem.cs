using UnityEngine;
using System.Collections;

public class Gpt_ActionSystem : MonoBehaviour {

    public string sceneName;

    void Update()
    {
        if (Gpt_Input.OptionDown)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}

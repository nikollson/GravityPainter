using UnityEngine;
using System.Collections;

public class Gpt_ActionSystem : MonoBehaviour {
    
    void Update()
    {
        if (Gpt_Input.OptionDown)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}

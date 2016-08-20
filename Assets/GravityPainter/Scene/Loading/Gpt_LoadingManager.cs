using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Gpt_LoadingManager : MonoBehaviour {

    public float loadSceneMinTIme = 0.5f;

    float count = 0;

    void Update()
    {
        count += Time.deltaTime;
        if (count > loadSceneMinTIme)
        {
            SceneManager.LoadScene(Gpt_SceneManager.NextSceneName);
        }
    }
}

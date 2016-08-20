using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Gpt_SceneManager : MonoBehaviour {

    private const string LoadingSceneName = "Loading";
    public static string NextSceneName { get; private set; }
    public static void LoadScene(string sceneName)
    {
        NextSceneName = sceneName;
        SceneManager.LoadScene(LoadingSceneName);
    }
    

}

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Gpt_PlayerInChecker : MonoBehaviour
{
    public string nextSceneName = "Stage_Boss";
    public HitManager hitMangaer;

    void Start()
    {
    }

    void Update()
    {
        if (hitMangaer.IsHit)
        {
                Gpt_SceneManager.LoadScene(nextSceneName);
        }
    }

}

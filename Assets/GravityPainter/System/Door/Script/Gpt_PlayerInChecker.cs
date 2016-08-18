using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Gpt_PlayerInChecker : MonoBehaviour
{
    public string nextSceneName = "Stage_Boss";

    void Start()
    {
    }

    void Update()
    {
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Player")
        {
            Gpt_SceneManager.LoadScene(nextSceneName);
        }
    }
}

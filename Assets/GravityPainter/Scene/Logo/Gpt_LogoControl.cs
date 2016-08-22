using UnityEngine;
using System.Collections;

public class Gpt_LogoControl : MonoBehaviour
{
    public float changeNextTime = 2.0f;
    public string nextSceneName = "Stage_Title";
    float count = 0;
    bool loaded = false;

    void Start()
    {

    }

    void Update()
    {
        count += Time.deltaTime;

        if (!loaded && count > changeNextTime)
        {
            loaded = true;
            Gpt_FadeManager.SetFade_White(() => { Gpt_SceneManager.LoadScene(nextSceneName, false); });

        }
    }
}

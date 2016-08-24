using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Gpt_EndingManager : MonoBehaviour {

    public bool canMoveNext = false;
    public string NextSceneName = "";

    float count = 0;

    void Update()
    {
        count += Time.deltaTime;

        bool ok = false;
        ok |= canMoveNext && Gpt_Input.HasAnyKey();
        ok |= Gpt_Input.Option;
        if(ok)
        {
            if (Gpt_Input.HasAnyKey())
            {
                SceneManager.LoadScene(NextSceneName);
            }
        }
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gpt_TitleManager : MonoBehaviour
{


    public float inputRecieveTime = 4.0f;
    public string NextSceneName = "";

    float cnt;

    // 初期化関数
    void Start()
    {
        cnt = 0.0f;
    }

    // 更新関数
    void Update()
    {
        cnt += Time.deltaTime;
        if (cnt > inputRecieveTime)
        {
            if (Gpt_Input.HasAnyKey())
            {
                 Gpt_SceneManager.LoadScene(NextSceneName);
            }
        }
    }
    
}

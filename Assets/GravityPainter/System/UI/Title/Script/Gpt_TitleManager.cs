using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gpt_TitleManager : MonoBehaviour
{


    public float inputRecieveTime = 4.0f;
    public string NextSceneName = "";

    public AudioClip audioClip;
    bool selected = false;

    float cnt;
    private AudioSource audioSource;

    // 初期化関数
    void Start()
    {
        cnt = 0.0f;
        audioSource = this.GetComponent<AudioSource>();
    }

    // 更新関数
    void Update()
    {
        cnt += Time.deltaTime;
        if (cnt > inputRecieveTime)
        {
            if (!selected && Gpt_Input.HasAnyKey())
            {
                selected = true;
                audioSource.PlayOneShot(audioClip);
                Gpt_FadeManager.SetFade_Black(() => { Gpt_SceneManager.LoadScene(NextSceneName, false); });
            }
        }
    }
    
}

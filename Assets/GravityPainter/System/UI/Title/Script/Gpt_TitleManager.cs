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

    public GameObject[] upOn;
    public GameObject[] downOn;

    float cnt;
    private AudioSource audioSource;

    int selectNum = 0;

    // 初期化関数
    void Start()
    {
        cnt = 0.0f;
        audioSource = this.GetComponent<AudioSource>();
        SelectUp();
    }

    // 更新関数
    void Update()
    {
        cnt += Time.deltaTime;
        if (cnt > inputRecieveTime)
        {
            if (!selected && HasEnter())
            {
                selected = true;
                audioSource.PlayOneShot(audioClip);
                if (selectNum == 0)
                {
                    Gpt_FadeManager.SetFade_Black(() => { Gpt_SceneManager.LoadScene(NextSceneName, false); }, true);
                }
                else
                {
                    Application.Quit();
                }
            }
        }

        if (Gpt_Input.Move.y > 0)
        {
            SelectUp();
        }
        if (Gpt_Input.Move.y < 0)
        {
            SelectDown();
        }
    }

    void SelectUp()
    {
        foreach (var a in upOn) a.SetActive(true);
        foreach (var a in downOn) a.SetActive(false);
        selectNum = 0;
    }

    void SelectDown()
    {
        foreach (var a in upOn) a.SetActive(false);
        foreach (var a in downOn) a.SetActive(true);
        selectNum = 1;
    }

    bool HasEnter()
    {
        return Gpt_Input.Attack || Gpt_Input.Option;
    }
    
}

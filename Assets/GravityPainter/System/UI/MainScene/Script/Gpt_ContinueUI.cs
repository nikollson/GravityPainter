using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gpt_ContinueUI : MonoBehaviour
{
    public GameObject parent;
    public GameObject normalUI;
    public Image[] SelectorYes;
    public Image[] SelectorNo;

    public string tileSceneName;

    public float canSelectTime = 1.0f;
    public float uiActiveTime = 0.5f;

    public Gpt_UIManager uiManager;
    public AudioClip gameOver;

    bool isYes = false;
    bool isActive = false;
    Gpt_Player player;

    float count = 0;
    AudioSource audioSource;

    void Start()
    {
        player = uiManager.player.GetComponent<Gpt_Player>();
        SelectYes(true);
        SetOff();
        audioSource = this.GetComponent<AudioSource>();
    }


    void Update()
    {
        if(player.Mode == Gpt_Player.MODE.DEAD)
        {
            count += Time.deltaTime;

            if (!isActive && count > uiActiveTime)
            {
                SetOn();
            }
            if(count > canSelectTime) Update_Slect();
        }

    }

    void Update_Slect()
    {
        if (Gpt_Input.IsMoving)
        {
            bool isLeft = Gpt_Input.Move.y > 0;
            SelectYes(isLeft);
        }
        if (Gpt_Input.Attack)
        {
            if (!isYes) SceneManager.LoadScene(tileSceneName);
            if (isYes)
            {
                Debug.Log("sceeene " + SceneManager.GetActiveScene().name);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    public void SelectYes(bool isYes)
    {
        this.isYes = isYes;
        foreach (var a in SelectorYes) a.enabled = isYes;
        foreach (var a in SelectorNo) a.enabled = !isYes;
    }

    public void SetOn()
    {
        GameObject.Find("BGM_Floor").GetComponent<AudioSource>().Stop();
        audioSource.Play();
        parent.SetActive(true);
        normalUI.SetActive(false);
        isActive = true;
    }

    public void SetOff()
    {
        parent.SetActive(false);
        normalUI.SetActive(true);
        isActive = false;
    }
}

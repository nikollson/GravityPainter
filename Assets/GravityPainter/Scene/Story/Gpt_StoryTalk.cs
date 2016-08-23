using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Gpt_StoryTalk : MonoBehaviour
{

    [System.Serializable]
    public class Data
    {
        public Sprite sprite;
        public float changeTime = 5.0f;
        public float fadeInTime = 1.0f;

        public bool IsEnd = false;
    }

    public Image imagePrev;
    public Image imageNext;

    public string nextSceneName;
    public bool doChangeEffect = true;

    public Data[] data;
    public float fadeOutScale = 1.2f;

    float clickIgnoreTime = 0.35f;
    float escIgnoreTime = 0.1f;

    float time = 0.0f;
    float life = 0.0f;
    bool prevHit = false;
    int next = 0;
    bool onceEsc = false;


    void Start()
    {
        time = 0.0f;
        life = data[0].changeTime;
    }

    void Update()
    {
        bool hit = Gpt_Input.HasAnyKeyDown();
        bool isEnd = IsEnd();
        if (!isEnd)
        {
            time += Time.deltaTime;
            life -= Time.deltaTime;

            Color c = imageNext.color;
            c.a = Mathf.Min(1.0f, time * (1 / data[next].fadeInTime));
            imageNext.color = c;

            Color d = imagePrev.color;
            d.a = Mathf.Max(0.0f, 1 - c.a * fadeOutScale);
            imagePrev.color = d;

            bool escHit = Input.GetKey(KeyCode.Escape);
            onceEsc |= escHit;

            float ignoreTime = onceEsc ? escIgnoreTime : clickIgnoreTime;
            if (time >= ignoreTime)
            {
                if ((!prevHit && hit) || life < 0.0f || onceEsc)
                {
                    prevHit = true;
                    LoadNext();
                }

            }
        }
        prevHit = hit;

    }

    public bool IsEnd()
    {
        return next >= data.Length;
    }

    void LoadNext()
    {
        imagePrev.sprite = data[next].sprite;

        Color c = imagePrev.color;
        c.a = 1.0f;
        imagePrev.color = c;


        next++;
        if (next >= data.Length) return;

        if (data[next].IsEnd)
        {
            if (doChangeEffect)
            {
                Gpt_FadeManager.SetFade_Black(
                    () =>
                    {
                        Application.LoadLevel(nextSceneName);
                    }
                );
            }
            else
            {
                Application.LoadLevel(nextSceneName);
            }
        }

        if (!IsEnd())
        {
            time = 0.0f;
            life = data[next].changeTime;
            if (data[next].sprite != null)
            {
                imageNext.sprite = data[next].sprite;
            }
            Update();
        }
    }
}

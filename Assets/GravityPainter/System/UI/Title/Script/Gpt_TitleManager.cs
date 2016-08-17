using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gpt_TitleManager : MonoBehaviour
{


    public float inputRecieveTime = 4.0f;
    public string NextSceneName = "";
    
    float cnt;
    enum Scene { ROGO = 0, TITLE = 1 };
    Scene scene;

    public RawImage rogoImg;
    float rogoImg_Alpha = 0.0f;

    public RawImage titleImg;
    float titleImg_Alpha = 0.0f;

    // 初期化関数
    void Start()
    {
        cnt = 0.0f;
        scene = Scene.ROGO;
    }

    // 更新関数
    void Update()
    {
        cnt += Time.deltaTime;

        switch (scene)
        {
            case Scene.ROGO:
                Rogo();
                break;

            case Scene.TITLE:
                Title();
                break;
        }


        if (cnt > inputRecieveTime)
        {
            if (HasInput())
            {
                SceneManager.LoadScene(NextSceneName);
            }
        }
    }

    // ロゴ描画関数
    void Rogo()
    {
        if (cnt < 1.0f)
        {
            rogoImg_Alpha = cnt;
        }
        else if (cnt > 3.0f)
        {
            rogoImg_Alpha = 4.0f - cnt;
        }

        rogoImg.color = new Color(1, 1, 1, rogoImg_Alpha);
        if (rogoImg_Alpha <= 0.0f)
        {
            scene = Scene.TITLE;
            cnt = 0.0f;
        }
    }

    // タイトル画面関数
    void Title()
    {
        titleImg.color = new Color(1, 1, 1, cnt);
    }


    bool HasInput()
    {
        bool ret = false;
        ret |= Gpt_Input.Attack;
        ret |= Gpt_Input.Skill;
        ret |= Gpt_Input.Jump;
        ret |= Gpt_Input.ColorLeft;
        ret |= Gpt_Input.ColorRight;
        ret |= Gpt_Input.CameraPush;
        ret |= Gpt_Input.MovePush;
        return ret;
    }
}

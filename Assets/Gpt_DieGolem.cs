using UnityEngine;
using System.Collections;

public class Gpt_DieGolem : MonoBehaviour {


    bool flg = false;
    float cnt = 0.0f;
    public GameObject parent;

	void Start () {
        flg = false;
        cnt = 0.0f;
    }

    void Update () {

        if (!flg)
        {
            flg = true;
        }

        else
        {
            parent.transform.position += new Vector3(0, -Time.deltaTime, 0);
            cnt += Time.deltaTime;

            if (cnt >= 6.0f)
            {
                Gpt_FadeManager.SetFade_White(() => { Gpt_SceneManager.LoadScene("Story_Last", false); });
            }
            if (cnt >= 9.0f)
            {
                Application.LoadLevel("Story_Last");
            }
        }
	}
}

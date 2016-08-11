using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    /* プレイヤー関連情報 */
    public GameObject player;
    Gpt_Player playerScript;

    /* HP関連 */
    public RawImage[] hpImgs = new RawImage[12];
    Vector3[] hpVecs = new Vector3[12];     // 初期位置
    Vector3 notDrawPos = new Vector3(-10000,-10000,-10000);     // 非描画座標

    /* インクゲージ関連 */
    public RawImage inkImg;
    public RawImage inkBarImg;

    // 初期化関数
    void Start () {
        playerScript = player.GetComponent<Gpt_Player>();
        for (int i = 0; i < 12; i++) hpVecs[i] = hpImgs[i].transform.position;
    }

    // 更新関数
    void Update () {

        // HP更新関数
        HPUpdate();
        // インクゲージ更新関数
        InkUpdate();
	}

    // HP更新関数
    void HPUpdate() {
        for (int i = 0; i < 12; i++) {
            if (i < playerScript.state.HP) hpImgs[i].transform.position = hpVecs[i];
            else hpImgs[i].transform.position = notDrawPos;
        }
    }

    float aaa = 1.0f;

    // インクゲージ更新関数
    void InkUpdate()
    {
        inkImg.transform.localScale = new Vector3(playerScript.state.Ink / playerScript.state.inkMax, 1, 1);
    }
}

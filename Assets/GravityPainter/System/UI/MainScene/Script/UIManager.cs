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

    /* コンボ数関連 */
    public Text[] comboText = new Text[3];
    public RawImage[] comboTimeBar = new RawImage[3];

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
        // コンボ数更新関数
        ComboUpdate();
    }

    // HP更新関数
    void HPUpdate() {
        for (int i = 0; i < 12; i++) {
            if (i < playerScript.state.HP) hpImgs[i].transform.position = hpVecs[i];
            else hpImgs[i].transform.position = notDrawPos;
        }
    }

    // インクゲージ更新関数
    void InkUpdate()
    {
        inkImg.transform.localScale = new Vector3(playerScript.state.Ink / playerScript.state.inkMax, 1, 1);
    }

    // コンボ数更新関数
    void ComboUpdate()
    {
        // コンボ残り時間
        //comboTimeBar[0].transform.localScale = new Vector3(playerScript.state.comboTime / playerScript.state.comboTimeMax, 1, 1);
        //comboTimeBar[1].transform.localScale = new Vector3(playerScript.state.comboTime / playerScript.state.comboTimeMax, 1, 1);
        //comboTimeBar[2].transform.localScale = new Vector3(playerScript.state.comboTime / playerScript.state.comboTimeMax, 1, 1);

        // コンボ数
        comboText[0].text = "15" + " Combo!";
        comboText[1].text = "55" + " Combo!";
        comboText[2].text = "12" + " Combo!";
        //comboText[0].text = playerScript.state.BlueCombo.ToString() + "Combo!";
        //comboText[1].text = playerScript.state.YellowCombo.ToString() + "Combo!";
        //comboText[2].text = playerScript.state.RedCombo.ToString() + "Combo!";
    }
}

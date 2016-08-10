// カメラ課題
// 1.壁に埋まらないようにする
// 2.アングルが少しずつ動くようにする

using UnityEngine;
using System.Collections;

public class Gpt_Camera : MonoBehaviour
{

    public Transform lookTransform;     // 注視点
    public Transform placeTransform;    // 移動基準座標
    public GameObject player;           // プレイヤー情報
    public float slowRotTime = 1.0f;    // 回転の余韻時間
    public float distance = 5.0f;       // プレイヤーとの距離
    public float angleDown = 60.0f;
    public float rotSpeed = 30.0f;

    public float startRot = 0.0f;
    float rot;
    bool oldCameraRotFlg;               // カメラ回転制御が行われたかどうか(前のフレームにて)
    bool cameraRotFlg;                  // カメラ回転制御が行われたかどうか(このフレームにて)
    float reqEndCntAndFlg;              // 回転リクエストが終わってからの経過時間兼フラグ
    float reqMoveX;

    bool oldStickPushFlg = false;
    bool stickPushFlg = false;
    public const float MIN_ROT_Y = -5.0f;
    public const float MAX_ROT_Y = 40.0f;

    void Start()
    {
        rot = startRot;
        oldCameraRotFlg = false;
        cameraRotFlg = false;
    }

    // レンダリング前カメラ更新
    void OnPreRender()
    {
        oldCameraRotFlg = cameraRotFlg;
        oldStickPushFlg = stickPushFlg;

        Update_Rotation();
        Update_Position();
        Update_Look();
    }

    // 回転制御関数
    void Update_Rotation()
    {
        // 上下回転
        if(angleDown >= MIN_ROT_Y && angleDown <= MAX_ROT_Y)
        {
            angleDown += Gpt_Input.CamMove.y;
        }
        // 上下回転の角度限界突破を防止する
        if (angleDown < MIN_ROT_Y)
        {
            angleDown = MIN_ROT_Y;
        }
        else if (angleDown > MAX_ROT_Y)
        {
            angleDown = MAX_ROT_Y;
        }
        Debug.Log(angleDown);

        // 回転リクエストがあれば
        if (Gpt_Input.CamMove.x != 0.0f)
        {
            cameraRotFlg = true;
            rot -= Gpt_Input.CamMove.x * angToRad(rotSpeed);    // 回転させる
            reqMoveX = Gpt_Input.CamMove.x*0.01f;
        }
        else
        {
            cameraRotFlg = false;
        }

        // このフレームで回転リクエストが終わっているならば
        if(oldCameraRotFlg && !cameraRotFlg)
        {
            reqEndCntAndFlg = slowRotTime;      // 緩やかに回転する時間
        }

        // 余韻回転
        if (reqEndCntAndFlg > 0.0f)
        {
            reqEndCntAndFlg -= Time.deltaTime;
            rot -= reqMoveX * reqEndCntAndFlg;
        }
        else
        {
            reqEndCntAndFlg = 0.0f;
        }

        // 右スティック押し込みでカメラを初期位置へ
        if (Gpt_Input.CameraPush)
        {
            stickPushFlg = true;
        }
        else
        {
            stickPushFlg = false;
        }
        if (Gpt_Input.CameraPush && !oldStickPushFlg)
        {
            rot = (180.0f - player.transform.eulerAngles.y) * (Mathf.PI / 180.0f);
        }
    }

    // 移動制御関数
    void Update_Position()
    {
        float y = Mathf.Sin(angToRad(angleDown));
        float distGround = Mathf.Cos(angToRad(angleDown));
        float x = distGround * Mathf.Cos(rot);
        float z = distGround * Mathf.Sin(rot);

        this.transform.position = placeTransform.position + distance * new Vector3(x, y, z);
    }

    // 注視点設定関数
    void Update_Look()
    {
        this.transform.LookAt(lookTransform.position);
    }

    // 角度をラジアンに変換する関数
    float angToRad(float ang)
    {
        return ang / 180 * Mathf.PI;
    }
}

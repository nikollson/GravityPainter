
using UnityEngine;
using System.Collections;

public class Gpt_Camera : MonoBehaviour
{
    /* プレイヤー情報系 */
    public GameObject player;           // プレイヤー情報
    public float distance = 5.0f;       // プレイヤーとの距離

    /* 上下回転系 */
    public float rotY = 30.0f;          // 上下回転値
    public float MIN_ROT_Y = -5.0f;     // 上下回転最小値
    public float MAX_ROT_Y = 55.0f;     // 上下回転最大値

    /* 左右回転系 */
    public float rotX = 0.0f;           // 左右回転値
    public float rotSpeedX = 7.0f;      // 左右回転速度
    public float slowRotTime = 1.0f;    // 左右回転の余韻時間
    float reqEndRotXCnt_AndFlg;         // 回転リクエストが終わってからの経過時間兼フラグ(余韻に使用)
    bool oldCameraRotXFlg;              // 左右回転制御が行われたかどうか(前のフレームにて)
    bool cameraRotXFlg;                 // 左右回転制御が行われたかどうか(このフレームにて)
    float moveXVal;                     // 左右回転量
    public float MOVEX_SLOWVAL;         // 左右回転余韻の大きさ

    /* 視点反転 */
    bool turnFlg;                       // 視点反転中かどうか
    bool turnRightFlg;                  // 視点反転開始時の向きが右向きであるかどうか
    float targetRot;                    // 視点反転時の目標rot値
    public float turnXSpd = 10.0f;      // 回転速度
    bool oldStickPushFlg = false;       // スティック押し込み情報(前のフレームにて)
    bool stickPushFlg = false;          // スティック押し込み情報(このフレームにて)

    // 初期化関数
    void Start()
    {
        oldCameraRotXFlg = false;
        cameraRotXFlg = false;
    }

    // 更新関数(レンダリング前カメラ更新)
    void OnPreRender()
    {
        oldCameraRotXFlg = cameraRotXFlg;
        oldStickPushFlg = stickPushFlg;

        Update_Rotation();
        Update_Position();
        Update_Look();
    }

    // 回転制御関数
    void Update_Rotation()
    {
        // 上下回転
        if(rotY >= MIN_ROT_Y && rotY <= MAX_ROT_Y)
        {
            rotY += Gpt_Input.CamMove.y;
        }
        // 上下回転の角度限界突破を防止する
        if (rotY < MIN_ROT_Y)
        {
            rotY = MIN_ROT_Y;
        }
        else if (rotY > MAX_ROT_Y)
        {
            rotY = MAX_ROT_Y;
        }

        // 横回転
        if (Gpt_Input.CamMove.x != 0.0f)
        {
            cameraRotXFlg = true;
            rotX -= Gpt_Input.CamMove.x * angToRad(rotSpeedX);
            moveXVal = Gpt_Input.CamMove.x * MOVEX_SLOWVAL;
        }
        else
        {
            cameraRotXFlg = false;
        }
        // このフレームで回転リクエストが終わっているならば
        if(oldCameraRotXFlg && !cameraRotXFlg)
        {
            reqEndRotXCnt_AndFlg = slowRotTime;      // 緩やかに回転する時間を決定
        }

        // 余韻回転
        if (reqEndRotXCnt_AndFlg > 0.0f)
        {
            reqEndRotXCnt_AndFlg -= Time.deltaTime;
            rotX -= moveXVal * reqEndRotXCnt_AndFlg;
        }
        else
        {
            reqEndRotXCnt_AndFlg = 0.0f;
        }

        // stickPushFlg押下情報取得
        if (Gpt_Input.CameraPush)
        {
            stickPushFlg = true;
        }
        else
        {
            stickPushFlg = false;
        }
        // 押された瞬間のみ反応する
        if (stickPushFlg && !oldStickPushFlg && !turnFlg)
        {
            turnFlg = true;
            if (targetRot < rotX) turnRightFlg = true;
            else turnRightFlg = false;
            targetRot = ((180.0f - player.transform.eulerAngles.y) % 360) * (Mathf.PI / 180.0f);
        }

        // 振りかえり処理
        if (turnFlg)
        {
            if (targetRot < rotX) rotX -= Time.deltaTime * turnXSpd;
            else if (targetRot > rotX) rotX += Time.deltaTime * turnXSpd;
            if((turnRightFlg && targetRot > rotX) || (!turnRightFlg && targetRot < rotX))
            {
                turnFlg = false;
            }
        }
    }

    // 移動制御関数
    void Update_Position()
    {
        float y = Mathf.Sin(angToRad(rotY));
        float distGround = Mathf.Cos(angToRad(rotY));
        float x = distGround * Mathf.Cos(rotX);
        float z = distGround * Mathf.Sin(rotX);

        this.transform.position = player.transform.position + distance * new Vector3(x, y, z);
    }

    // 注視点設定関数
    void Update_Look()
    {
        this.transform.LookAt(player.transform.position);
    }

    // 角度をラジアンに変換する関数
    float angToRad(float ang)
    {
        return ang / 180 * Mathf.PI;
    }
}

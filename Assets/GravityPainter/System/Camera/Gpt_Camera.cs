using UnityEngine;
using System.Collections;

public class Gpt_Camera : MonoBehaviour
{
    // -------------------------------------------------- 変数 -------------------------------------------------- //

    /* スティック入力情報変数 */
    float oldCamMoveX = 0.0f;           // スティックX移動量(1フレーム前)
    float nowCamMoveX = 0.0f;           // スティックX移動量
    float oldCamMoveY = 0.0f;           // スティックY移動量(1フレーム前)
    float nowCamMoveY = 0.0f;           // スティックY移動量
    bool oldStickPush = false;          // スティック押し込み(1フレーム前)
    bool nowStickPush = false;			// スティック押し込み

    /* プレイヤー情報系 */
    public GameObject player;           // プレイヤー情報
    public float distanceXZ = 5.0f;     // プレイヤーとの距離倍率XZ
    public float distanceY = 5.0f;      // プレイヤーとの距離倍率Y

    /* 視点反転系 */
    bool turnFlg = false;               // 視点反転中かどうか
    float firstCamPosRot;               // 最初のカメラ位置
    float targetRot;                    // 目標rot値
    bool rightTurnFlg;                  // 右向きに回転するかどうか
    float moveVal;                      // 移動量
    int turnCnt;                        // ターンカウンタ

    /* 左右回転系 */
    public float rotXZ = 0.0f;          // 左右回転値
    public float ROTXZ_SPD = 3.0f;      // 左右回転速度
    // 加速関連
    public float addFirstXZ = 0.25f;    // 左右加速初期値
    public float addSpdXZ = 0.03f;      // 左右加速度の足す速さ
    float addCntXZ = 0.0f;              // 左右加速してからのカウント及び加速度(addFirstXZ～1.0f)
    // 停止関連
    float stopCntXZ = 0.0f;             // 停止した瞬間に1.0fになり0.0fまで減っていくカウンタ
    float stopMoveXZ;                   // 停止時のXZ移動方向
    public float addStopTimeXZ = 3.0f;  // 停止までの時間倍率(小さいほど長くなる)
    public float stopSpdXZ = 0.005f;    // 停止速度

    /* 上下回転系 */
    public float rotY = 30.0f;          // 上下回転値
    public float ROTY_SPD = 30.0f;      // 上下回転速度
    // 加速関連
    public float addFirstY = 0.5f;      // 上下加速初期値
    public float addSpdY = 0.05f;       // 上下加速度の足す速さ
    float addCntY = 0.0f;               // 上下加速してからのカウント及び加速度(addFirst～1.0f)
    public float MAX_ROTY = 50.0f;      // 最大回転値Y
    public float MIN_ROTY = 5.0f;       // 最小回転値Y
    // 停止関連
    float stopCntY = 0.0f;              // 停止した瞬間に1.0fになり0.0fまで減っていくカウンタ
    float stopMoveY;                    // 停止時のXZ移動方向
    public float addStopTimeY = 3.0f;   // 停止までの時間倍率(小さいほど長くなる)
    public float stopSpdY = 0.05f;      // 停止速度

    // -------------------------------------------------- 大元関数 -------------------------------------------------- //

    // 初期化関数
    void Start()
    {
    }

    //レンダリング前更新関数
    void OnPreRender()
    {
        Update_Stick();
        Update_Rotation();
        Update_Position();
        Update_Look();
    }

    // -------------------------------------------------- 機能関数 -------------------------------------------------- //

    // スティック情報更新関数
    void Update_Stick()
    {
        oldCamMoveX = nowCamMoveX;
        nowCamMoveX = Gpt_Input.CamMove.x;
        oldCamMoveY = nowCamMoveY;
        nowCamMoveY = Gpt_Input.CamMove.y;
        oldStickPush = nowStickPush;
        nowStickPush = Gpt_Input.CameraPush;
    }

    // 回転を決定する関数
    void Update_Rotation()
    {
        // 振りかえり処理
        //Turn();
        // 左右回転
        Rotation_XZ();
        // 上下回転
        Rotation_Y();
    }

    // 最終的な座標を決定する関数
    void Update_Position()
    {
        float x = Mathf.Cos(angToRad(rotY)) * distanceXZ * Mathf.Cos(rotXZ);
        float y = Mathf.Sin(angToRad(rotY)) * distanceY;
        float z = Mathf.Cos(angToRad(rotY)) * distanceXZ * Mathf.Sin(rotXZ);
        this.transform.position = player.transform.position + new Vector3(x, y, z);
    }

    // 注視点設定関数
    void Update_Look()
    {
        this.transform.LookAt(player.transform.position);
    }

    // -------------------------------------------------- 回転系関数 -------------------------------------------------- //

    // 視点反転関数
    void Turn()
    {
        // 視点反転中でない時に、スティックが押し込まれた瞬間であればフラグを立てる
        if (!turnFlg && !oldStickPush && nowStickPush)
        {
            turnFlg = true;
            turnCnt = 0;

            if (player.transform.eulerAngles.y >= 0.0f && player.transform.eulerAngles.y < 180.0f)
            {
                targetRot = player.transform.eulerAngles.y + 180.0f;
                targetRot = (Mathf.PI * 2) - (targetRot * Mathf.PI / 180.0f);
            }
            else if(player.transform.eulerAngles.y >= 180.0f && player.transform.eulerAngles.y <= 360.0f)
            {
                targetRot = player.transform.eulerAngles.y - 180.0f;
                targetRot = (Mathf.PI * 2) - (targetRot * Mathf.PI / 180.0f);
            }
            else
            {
                Debug.Log("◆◆ 視点反転中にエラーが発生しました ◆◆");
            }

            // カメラの座標を保存
            firstCamPosRot = rotXZ % (Mathf.PI * 2);
            if (firstCamPosRot < 0.0f) firstCamPosRot = (Mathf.PI * 2) + firstCamPosRot;
            // どちらの向きで回転するかを決定
            if (firstCamPosRot > targetRot)
            {
                moveVal = firstCamPosRot - targetRot;
                if(moveVal < Mathf.PI) rightTurnFlg = false;
                else rightTurnFlg = true;
                moveVal %= Mathf.PI;
            }
            else if (targetRot > firstCamPosRot)
            {
                moveVal = targetRot - firstCamPosRot;
                if (moveVal < Mathf.PI) rightTurnFlg = true;
                else rightTurnFlg = false;
                moveVal %= Mathf.PI;
            }
            else
            {
                turnFlg = false;
                return;
            }
        }

        // フラグが立っていたら視点反転する
        if (turnFlg)
        {
            if (rightTurnFlg)
            {
                rotXZ += Time.deltaTime;
            }
            else
            {
                rotXZ -= Time.deltaTime;
            }
            if(Mathf.Abs(rotXZ - targetRot) < 0.1f)
            {
                turnFlg = false;
            }
        }

        //Debug.Log("firstCamPosRot: " + firstCamPosRot);
        //Debug.Log("tgt: " + targetRot);
    }

    // 左右回転関数
    void Rotation_XZ()
    {
        // スティックが倒された瞬間であれば加速度を初期化
        if (oldCamMoveX == 0.0f && nowCamMoveX != 0.0f) addCntXZ = addFirstXZ;
        // 加速していない状態であれば加速度を上げていく
        if (addCntXZ >= addFirstXZ && addCntXZ + addSpdXZ <= 1.0f) addCntXZ += addSpdXZ;
        // スティックの傾きに応じて回転
        rotXZ -= nowCamMoveX  * addCntXZ * ROTXZ_SPD * Time.deltaTime;

        // スティックが停止した瞬間であれば1フレーム前の移動方向を保存して加速度を初期化
        if (oldCamMoveX != 0.0f && nowCamMoveX == 0.0f)
        {
            stopMoveXZ = oldCamMoveX;
            stopCntXZ = 1.0f;
        }
        // 停止していない状態であればゆっくり停止する
        if (stopCntXZ > 0.0f) stopCntXZ -= addStopTimeXZ * Time.deltaTime;
        if (stopCntXZ < 0.0f) stopCntXZ = 0.0f;
        // stopCntが0.0fよりも大きければ余韻回転
        else
        {
            rotXZ -= stopCntXZ * stopMoveXZ * stopSpdXZ;
        }
    }

    // 上下回転関数
    void Rotation_Y()
    {
        // スティックが倒された瞬間であれば加速度を初期化
        if (oldCamMoveY == 0.0f && nowCamMoveY != 0.0f) addCntY = addFirstY;
        // 加速していない状態であれば加速度を上げていく
        if (addCntY >= addFirstY && addCntY + addSpdY <= 1.0f) addCntY += addSpdY;
        // スティックの傾きに応じて回転
        rotY += nowCamMoveY * addCntY * ROTY_SPD * Time.deltaTime;

        // スティックが停止した瞬間であれば1フレーム前の移動方向を保存して加速度を初期化
        if (oldCamMoveY != 0.0f && nowCamMoveY == 0.0f)
        {
            stopMoveY = oldCamMoveY;
            stopCntY = 1.0f;
        }
        // 停止していない状態であればゆっくり停止する
        if (stopCntY > 0.0f) stopCntY -= addStopTimeY * Time.deltaTime;
        if (stopCntY < 0.0f) stopCntY = 0.0f;
        // stopCntが0.0fよりも大きければ余韻回転
        else
        {
            rotY += stopCntY * stopMoveY * stopSpdY;
        }

        // 限界回転値を超えたら戻す
        if (rotY > MAX_ROTY) rotY = MAX_ROTY;
        if (rotY < MIN_ROTY) rotY = MIN_ROTY;
    }

    // -------------------------------------------------- 補佐関数 -------------------------------------------------- //

    // 角度をラジアンに変換する関数
    float angToRad(float ang)
    {
        return ang / 180.0f * Mathf.PI;
    }
}

























































































//using UnityEngine;
//using System.Collections;

//public class Gpt_Camera : MonoBehaviour
//{
//    /* プレイヤー情報系 */
//    public GameObject player;           // プレイヤー情報
//    public float distance = 5.0f;       // プレイヤーとの距離

//    /* 上下回転系 */
//    public float rotY = 30.0f;          // 上下回転値
//    public float MIN_ROT_Y = -5.0f;     // 上下回転最小値
//    public float MAX_ROT_Y = 55.0f;     // 上下回転最大値

//    /* 左右回転系 */
//    public float rotX = 0.0f;           // 左右回転値
//    public float rotSpeedX = 7.0f;      // 左右回転速度
//    public float slowRotTime = 1.0f;    // 左右回転の余韻時間
//    float reqEndRotXCnt_AndFlg;         // 回転リクエストが終わってからの経過時間兼フラグ(余韻に使用)
//    bool oldCameraRotXFlg;              // 左右回転制御が行われたかどうか(前のフレームにて)
//    bool cameraRotXFlg;                 // 左右回転制御が行われたかどうか(このフレームにて)
//    float moveXVal;                     // 左右回転量
//    public float MOVEX_SLOWVAL;         // 左右回転余韻の大きさ

//    /* 視点反転 */
//    bool turnFlg;                       // 視点反転中かどうか
//    bool turnRightFlg;                  // 視点反転開始時の向きが右向きであるかどうか
//    float targetRot;                    // 視点反転時の目標rot値
//    public float turnXSpd = 10.0f;      // 回転速度
//    float turnVal;                      // 振りかえり量を計算
//    bool oldStickPushFlg = false;       // スティック押し込み情報(前のフレームにて)
//    bool stickPushFlg = false;          // スティック押し込み情報(このフレームにて)

//    // 初期化関数
//    void Start()
//    {
//        oldCameraRotXFlg = false;
//        cameraRotXFlg = false;
//    }

//    // 更新関数(レンダリング前カメラ更新)
//    void OnPreRender()
//    {
//        oldCameraRotXFlg = cameraRotXFlg;
//        oldStickPushFlg = stickPushFlg;

//        Update_Rotation();
//        Update_Position();
//        Update_Look();
//    }

//    // 回転制御関数
//    void Update_Rotation()
//    {
//        // 上下回転
//        if (rotY >= MIN_ROT_Y && rotY <= MAX_ROT_Y)
//        {
//            rotY += Gpt_Input.CamMove.y;
//        }
//        // 上下回転の角度限界突破を防止する
//        if (rotY < MIN_ROT_Y)
//        {
//            rotY = MIN_ROT_Y;
//        }
//        else if (rotY > MAX_ROT_Y)
//        {
//            rotY = MAX_ROT_Y;
//        }

//        // 横回転
//        if (Gpt_Input.CamMove.x != 0.0f)
//        {
//            cameraRotXFlg = true;
//            rotX -= Gpt_Input.CamMove.x * angToRad(rotSpeedX);
//            moveXVal = Gpt_Input.CamMove.x * MOVEX_SLOWVAL;
//        }
//        else
//        {
//            cameraRotXFlg = false;
//        }
//        // このフレームで回転リクエストが終わっているならば
//        if (oldCameraRotXFlg && !cameraRotXFlg)
//        {
//            reqEndRotXCnt_AndFlg = slowRotTime;      // 緩やかに回転する時間を決定
//        }

//        // 余韻回転
//        if (reqEndRotXCnt_AndFlg > 0.0f)
//        {
//            reqEndRotXCnt_AndFlg -= Time.deltaTime;
//            rotX -= moveXVal * reqEndRotXCnt_AndFlg;
//        }
//        else
//        {
//            reqEndRotXCnt_AndFlg = 0.0f;
//        }

//        // stickPushFlg押下情報取得
//        if (Gpt_Input.CameraPush)
//        {
//            stickPushFlg = true;
//        }
//        else
//        {
//            stickPushFlg = false;
//        }
//        // 押された瞬間のみ反応する
//        if (stickPushFlg && !oldStickPushFlg && !turnFlg)
//        {
//            turnFlg = true;
//            // 方向を取得しておく
//            if (targetRot < rotX) turnRightFlg = true;
//            else turnRightFlg = false;

//            // 視点反転時の目標rot値を計算
//            targetRot = ((180.0f - player.transform.eulerAngles.y) % 360) * (Mathf.PI / 180.0f);
//            // 振りかえり量を取得しておく
//            turnVal = Mathf.Abs(Mathf.Abs(targetRot % Mathf.PI) - Mathf.Abs(rotX % Mathf.PI));
//        }

//        // 振りかえり処理
//        if (turnFlg)
//        {
//            // 振りかえり
//            if (targetRot < rotX) rotX -= Time.deltaTime * turnXSpd * turnVal;
//            else if (targetRot > rotX) rotX += Time.deltaTime * turnXSpd * turnVal;
//            // 停止
//            if ((turnRightFlg && targetRot > rotX) || (!turnRightFlg && targetRot < rotX)) turnFlg = false;
//        }
//    }

//    // 移動制御関数
//    void Update_Position()
//    {
//        float y = Mathf.Sin(angToRad(rotY));
//        float distGround = Mathf.Cos(angToRad(rotY));
//        float x = distGround * Mathf.Cos(rotX);
//        float z = distGround * Mathf.Sin(rotX);

//        this.transform.position = player.transform.position + distance * new Vector3(x, y, z);
//    }

//    // 注視点設定関数
//    void Update_Look()
//    {
//        this.transform.LookAt(player.transform.position);
//    }

//    // 角度をラジアンに変換する関数
//    float angToRad(float ang)
//    {
//        return ang / 180 * Mathf.PI;
//    }
//}

using UnityEngine;
using System.Collections;

public class Gpt_Camera : MonoBehaviour
{
    // -------------------------------------------------- 変数 -------------------------------------------------- //

    /* 特殊カメラ状態 */
    public enum State {
        Normal = 0,
        Door = 1,
        BossStartMovie = 2,
        BossBattle = 3,
    }
    public int state = (int)State.Normal;
    public GameObject door;
    public GameObject doorCamPosObj;
    public GameObject camStartPos;
    Vector3 notDrawPos = new Vector3(-10000, -10000, -10000);
    bool stateStartFlg = true;
    Vector3 firstPlayerPos;
    public GameObject movieBar1;
    public GameObject movieBar2;

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
        firstPlayerPos = player.transform.position;
    }

    //レンダリング前更新関数
    void OnPreRender()
    {
        Update_Stick();

        if (state == (int)State.Normal)
        {
            Update_Rotation();
            Update_Position();
            Update_Look(player.transform.position);
        }
        else if (state == (int)State.Door)
        {
            this.transform.position = doorCamPosObj.transform.position;
            Update_Look(door.transform.position);
        }
        else if (state == (int)State.BossBattle)
        {
            // 高い位置にいればカメラ操作しない
            if (player.transform.position.y >= 15.0f)
            {
                Update_Position();
                Update_Rotation();
                Update_Look(player.transform.position);
            }
            else
            {
                this.transform.position = player.transform.position * 1.5f;
                Update_Look(new Vector3(0, 2, 0));
            }
        }
        else if (state == (int)State.BossStartMovie)
        {
            // 最初のみ場所を代入
            if (stateStartFlg)
            {
                this.transform.position = camStartPos.transform.position;
                stateStartFlg = false;
            }

            if (this.transform.position.y < 12.0f)
            {
                this.transform.position += new Vector3(0, Time.deltaTime, 0);
                Update_Look(this.transform.position + new Vector3(0, 0, -1.0f));
            }
            else if (this.transform.position.y < 12.1f)
            {
                this.transform.position += new Vector3(0, Time.deltaTime / 10.0f, 0);
                Update_Look(this.transform.position + new Vector3(0, 0, -1.0f));
            }
            else {
                Vector3 vec = new Vector3(-0.05f, 19.935f, 40.329f) - this.transform.position;
                this.transform.position += vec * Time.deltaTime;
                Update_Look(this.transform.position + new Vector3(0, 0, -1.0f));
                if(this.transform.position.z>firstPlayerPos.z) Update_Look(firstPlayerPos);

                if (this.transform.position.z >= player.transform.position.z + distanceXZ*0.85f)
                {
                    state = (int)State.BossBattle;
                    movieBar1.transform.position = notDrawPos;
                    movieBar2.transform.position = notDrawPos;
                }
            }
        }
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
            Turn();
            Rotation_XZ();
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
    void Update_Look(Vector3 pos)
    {
        this.transform.LookAt(pos);
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

            // todo
            rotXZ = targetRot;
            turnFlg = false;
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

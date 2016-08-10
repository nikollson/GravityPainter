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

    public float distance = 5.0f;       // プレイヤーとの距離
    public float angleDown = 60.0f;
    public float startRot = 0.0f;
    public float rotSpeed = 3.0f;

    float rot;

    void Start()
    {
        rot = startRot;
    }

    // レンダリング前カメラ更新
    void OnPreRender()
    {
        Update_Rotation();
        Update_Position();
        Update_Look();
    }

    // 回転制御関数
    void Update_Rotation()
    {
        rot -= Gpt_Input.CamMove.x * angToRad(rotSpeed);

        // 右スティック押し込みでカメラを初期位置へ
        if (Gpt_Input.CameraPush)
        {
            rot = (180.0f - player.transform.eulerAngles.y) * (3.14f / 180.0f);
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

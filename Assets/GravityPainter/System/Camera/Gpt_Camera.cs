using UnityEngine;
using System.Collections;

public class Gpt_Camera : MonoBehaviour
{

    public Transform lookTransform;     // 注視点
    public Transform placeTransform;    // 移動基準座標

    public float distance = 5.0f;       // プレイヤーとの距離s
    public float angleDown = 60.0f;
    public float startRot = 0.0f;
    public float rotSpeed = 3.0f;

    float rot = 180.0f;

    void Start()
    {
        rot = startRot;
        distance = 8.0f;
    }

    // カメラ更新
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

    // 角度をラジアン変換関数
    float angToRad(float ang)
    {
        return ang / 180 * Mathf.PI;
    }
}

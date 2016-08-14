using UnityEngine;
using System.Collections;

public class Gpt_InkItem : MonoBehaviour {

    GameObject player;
    Gpt_Player playerScript;
    public float upMovePower = 0.5f;
    float spd = 0.1f;       // 速度
    public float addAcceleration = 0.01f;       // 加速度変数

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Gpt_Player>();
    }
	
	void Update () {
        // プレイヤーの方向へ移動する
        MoveToPlayer();
	}

    void MoveToPlayer() {

        // 上昇力を計算(少しずつ上昇値は小さくなる)
        if (upMovePower - Time.deltaTime > 0.0f) upMovePower -= Time.deltaTime;
        else upMovePower = 0.0f;
        // 上昇する
        this.transform.position += new Vector3(0, upMovePower, 0);

        // プレイヤーへの方向ベクトル計算と正規化
        Vector3 toPlayerVec = player.transform.position - this.transform.position;
        toPlayerVec.Normalize();

        // 速度を徐々に早くする
        spd += addAcceleration;
       this.transform.position += toPlayerVec* spd;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);

            // プレイヤーインク回復処理
            //playerScript.state.Ink++;
            // エフェクト出現処理
            //Instantiate();
            // 音
            //PlaySound();
        }
    }

}

using UnityEngine;
using System.Collections;

public class Gpt_Boss : MonoBehaviour {

    enum State
    {
        Wait = 0,
        Search,
        Fall,
        Up,
        Atk1,
        Atk2,
        Atk3, 
    }
    State state = (int)State.Wait;

    const float maxHp = 10.0f;
    float hp = 10.0f;

    Vector3 toPlayerVec;        // プレイヤーまでのベクトル
    public GameObject hand1;
    public GameObject hand2;

    float fallSpd = 10.0f;      // 落下速度
    float upSpd = 25.0f;        // 上昇速度
    float fallY = -15.0f;       // マグマY位置()
    float magmaDmg = 2.5f;
    float cnt = 0.0f;
    public Vector3 firstBossPos;
    public GameObject player;

    void Start () {
	}
	
	void Update () {

        cnt += Time.deltaTime;
        toPlayerVec = player.transform.position - this.transform.position;

        // 最初の行動パターン
        if (state == State.Wait)
        {
            // 乱数生成
            float rnd = Random.Range(0.0f, 1.0f);
            // 行動をランダムに決定する
            if (rnd < 0.33f)
            {
                state = State.Atk1;
            }
            else if(rnd < 0.66f)
            {
                state = State.Atk2;
            }
            else
            {
                state = State.Atk3;
            }
        }
        // 落下ステート
        else if (state == State.Fall)
        {
            // 落下ベクトルを足す
            this.transform.position += new Vector3(0, -Time.deltaTime * fallSpd, 0);
            // 一定まで落ちると被ダメ
            if(this.transform.position.y < fallY)
            {
                state = State.Up;
                this.hp -= magmaDmg;
            }
        }
        // 上昇ステート(簡易制御)
        else if (state == State.Up)
        {
            // 上昇ベクトルを足す
            this.transform.position += new Vector3(0, Time.deltaTime * upSpd, 0);

            // 元々いた位置まで上昇すれば
            if (this.transform.position.y < firstBossPos.y)
            {
                this.transform.position = new Vector3(0, firstBossPos.y, 0);
                state = State.Wait;
            }
        }
        else if (state == State.Atk1)
        {
            // プレイヤーまでの方向ベクトルから角度を計算する
        }
        else if (state == State.Atk2)
        {
        }
        else if (state == State.Atk3)
        {
        }
    }

    public float GetMaxHp()
    {
        return maxHp;
    }

    public float GetHp()
    {
        return hp;
    }

    public void AddHP(int plus)
    {
        hp += plus;
    }
}

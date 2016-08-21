using UnityEngine;
using System.Collections;
using System;

public class Gpt_Boss : MonoBehaviour
{
    enum State
    {
        Search,
        Move,
        SelectAtk,

        Fall,
        Up,

        Atk1,
        Atk2,
        Atk3,
    }
    State state = State.Search;

    const float maxHp = 10.0f;
    float hp = 10.0f;

    public GameObject hand1;
    public GameObject hand2;

    float fallSpd = 10.0f;      // 落下速度
    float upSpd = 25.0f;        // 上昇速度
    float fallY = -15.0f;       // マグマY位置()
    float magmaDmg = 2.5f;
    float cnt = 0.0f;
    public Vector3 firstBossPos;
    public GameObject player;
    public Animator anim;
    public Transform[] AtkTargetPos = new Transform[8];
    Transform targetPos;        // プレイヤーから一番近い座標
    float readyTime = 0.0f;
    const float READY_TIME_MAX = 2.25f;

    void Start()
    {
    }

    void Update()
    {
        cnt += Time.deltaTime;
        Debug.Log("ENUM_STATE: "+state);

        // プレイヤーを探す
        if (state == State.Search)
        {
            // targetPos(プレイヤーから一番近い座標)取得
            Search_PlayerNearPos();
            // ステート変更
            state = State.Move;
        }
        // プレイヤーの方向を向く
        else if (state == State.Move)
        {
            if (readyTime < READY_TIME_MAX)
            {
                transform.rotation = Quaternion.Slerp(this.transform.rotation, targetPos.rotation, Time.deltaTime);
                readyTime += Time.deltaTime;
            }
            else
            {
                // 一定時間経過でステート変更
                state = State.SelectAtk;
            }
        }
        // 攻撃方法を選ぶ
        else if (state == State.SelectAtk)
        {
            float rnd = UnityEngine.Random.Range(0.0f,100.0f);
            if (hp >= 70.0f)
            {
                if (rnd < 50.0f)
                {
                    state = State.Atk1;
                    anim.SetBool("Atk_L_Flg", true);
                }
                else
                {
                    state = State.Atk2;
                    anim.SetBool("Atk_R_Flg", true);
                }
            }
            else
            {
                if (rnd < 33.33f)
                {
                    state = State.Atk1;
                    anim.SetBool("Atk_L_Flg", true);
                }
                else if (rnd < 66.66f)
                {
                    state = State.Atk2;
                    anim.SetBool("Atk_R_Flg", true);
                }
                else
                {
                    state = State.Atk3;
                    anim.SetBool("Atk_Nagi_Flg", true);
                }
            }
        }
        else if ( state == State.Fall)
        {
            // 落下ベクトルを足す
            this.transform.position += new Vector3(0, -Time.deltaTime * fallSpd, 0);
            // 一定まで落ちると被ダメ
            if (this.transform.position.y < fallY)
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
                state = State.Search;
            }
        }
        // 攻撃L
        else if (state == State.Atk1)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Select"))
            {
                anim.SetBool("Atk_L_Flg", false);
                state = State.Search;
            }
        }
        // 攻撃R
        else if (state == State.Atk2)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Select"))
            {
                anim.SetBool("Atk_R_Flg", false);
                state = State.Search;
            }
        }
        // なぎ払い攻撃
        else if (state == State.Atk3)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Select"))
            {
                anim.SetBool("Atk_Nagi_Flg", false);
                state = State.Search;
            }
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

    // プレイヤーと最も近い場所を探す
    void Search_PlayerNearPos()
    {
        // プレイヤーと最も近い場所を探す
        float min = 99999.9f;
        int num = 0;
        for (int i = 0; i < AtkTargetPos.Length; i++)
        {
            Vector3 vec = AtkTargetPos[i].position - player.transform.position;
            vec = new Vector3(vec.x, 0.0f, vec.z);
            if (min > vec.magnitude)
            {
                min = vec.magnitude;
                num = i;
            }
        }
        // プレイヤーと最も近い場所保存
        targetPos = AtkTargetPos[num];

        //Debug.Log("AAA"+num);
    }
}

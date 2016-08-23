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

        Die,
    }
    State state = State.Search;
    public GameObject camera;
    public GameObject se;

    const float maxHp = 10.0f;
    float hp = 10.0f;

    public GameObject hand1;
    public GameObject hand2;

    float fallSpd = 4.0f;      // 落下速度
    float upSpd = 25.0f;        // 上昇速度
    float firstUpSpd;
    float fallY = -40.0f;       // マグマY位置()
    float magmaDmg = 3.34f;
    float cnt = 0.0f;
    public Vector3 firstBossPos;
    public GameObject player;
    public Animator anim;
    public Transform[] AtkTargetPos = new Transform[8];

    /* 床関連 */
    public GameObject[] yuka = new GameObject[8];       // 床オブジェクト
    float [] yukaDieCnt = new float[8];                 // 床が死んでからのカウント(兼フラグ)
    public float YUKA_RESP_TIME = 5.0f;                 // 床復活タイム



    Transform targetPos;        // プレイヤーから一番近い座標

    float readyTime = 0.0f;
    const float READY_TIME_MAX = 2.25f;
    float attackTime = 0.0f;
    const float ATTACK_TIME_MAX = 3.5f;
    const float ATTACK_TIME_MAX_NAGI = 7.5f;

    public float screenShake = 4.0f;

    int targetYukaNum = 0;        // もくひょうゆかばんごう
    float dieCnt = 0.0f;
    float upGrav = 0.0f;        // 上昇時重力

    void Start()
    {
        for (int i = 0; i < yuka.Length; i++) yukaDieCnt[i] = 0.0f;
        firstUpSpd = upSpd;
    }

    void Update()
    {
        cnt += Time.deltaTime;

        // プレイヤーを探す
        if (state == State.Search)
        {
            // targetPos(プレイヤーから一番近い座標)取得
            Search_PlayerNearPos();
            // ステート変更(プレイヤーが下にいる時だけ)
            if(player.transform.position.y < 12.0f)state = State.Move;
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
                readyTime = 0.0f;
            }
        }
        // 攻撃方法を選ぶ
        else if (state == State.SelectAtk)
        {
            float rnd = UnityEngine.Random.Range(0.0f, 100.0f);
            if (hp >= 7.0f)
            {
                if (rnd < 50.0f)
                {
                    state = State.Atk1;
                    attackTime = 0.0f;
                    anim.SetBool("Atk_L_Flg", true);
                }
                else
                {
                    state = State.Atk2;
                    attackTime = 0.0f;
                    anim.SetBool("Atk_R_Flg", true);
                }
            }
            else
            {
                if (rnd < 25.0f)
                {
                    state = State.Atk1;
                    attackTime = 0.0f;
                    anim.SetBool("Atk_L_Flg", true);
                }
                else if (rnd < 50.0f)
                {
                    state = State.Atk2;
                    attackTime = 0.0f;
                    anim.SetBool("Atk_R_Flg", true);
                }
                else
                {
                    state = State.Atk3;
                    attackTime = 0.0f;
                    anim.SetBool("Atk_Nagi_Flg", true);
                }
            }
        }
        else if (state == State.Fall)
        {
            // 落下ベクトルを足す
            fallSpd += Time.deltaTime*20.0f;
            this.transform.position += new Vector3(0, -Time.deltaTime * fallSpd, 0);
            // 一定まで落ちると被ダメ
            if (this.transform.position.y < fallY)
            {
                state = State.Up;
                this.hp -= magmaDmg;
                fallSpd = 4.0f;

                // 死んだら
                if (this.hp <= 0.0f)
                {
                    state = State.Die;
                }
            }
        }
        // 上昇ステート(簡易制御)
        else if (state == State.Up)
        {
            // 上昇ベクトルを足す
            upSpd -= Time.deltaTime * 6.0f;
            this.transform.position += new Vector3(0, Time.deltaTime * upSpd, 0);

            // 元々いた位置まで上昇すれば
            if (this.transform.position.y < firstBossPos.y && upSpd<0.0f)
            {
                this.transform.position = new Vector3(0, firstBossPos.y, 0);
                state = State.Search;
                upSpd = firstUpSpd;
            }
        }
        // 攻撃L
        else if (state == State.Atk1)
        {
            attackTime += Time.deltaTime;

            // 途中で床は壊れる
            if (attackTime <= 10.0f)
            {
                if (attackTime >= 3.0f)
                {
                    se.GetComponent<AudioSource>().Play();
                    camera.GetComponent<Gpt_Camera>().SetScreenShake(screenShake);

                    if (yuka[(targetYukaNum) % 8].GetComponent<Gpt_YukaBox>().HP==1) {
                        yukaDieCnt[(targetYukaNum) % 8] = 0.1f;
                    }
                    SetEXP();

                    attackTime += 10.0f;
                }
            }

            // 落下判定
            if (FallCheck(true))
            {
                anim.SetBool("Atk_L_Flg", false);
                state = State.Fall;
                attackTime = 0.0f;
            }

            // 終わったらステート戻す
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Select") && attackTime >= ATTACK_TIME_MAX)
            {
                anim.SetBool("Atk_L_Flg", false);
                state = State.Search;
                attackTime=0.0f;
            }
        }
        // 攻撃R
        else if (state == State.Atk2)
        {
            attackTime += Time.deltaTime;

            // 途中で床は壊れる
                if (attackTime >= 3.0f && attackTime<=10.0f)
                {
                    se.GetComponent<AudioSource>().Play();
                    camera.GetComponent<Gpt_Camera>().SetScreenShake(screenShake);

                    if (yuka[(targetYukaNum) % 8].GetComponent<Gpt_YukaBox>().HP == 1)
                    {
                        yukaDieCnt[(targetYukaNum) % 8] = 0.1f;
                    }
                    SetEXP();

                    attackTime += 10.0f;
                }

            // 落下判定
            if (FallCheck(true))        // 本来はfalseだが今は左手でしか攻撃しないためtrue
            {
                anim.SetBool("Atk_R_Flg", false);
                state = State.Fall;
                attackTime = 0.0f;
            }

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Select") && attackTime >= ATTACK_TIME_MAX)
            {
                anim.SetBool("Atk_R_Flg", false);
                state = State.Search;
                attackTime = 0.0f;
            }
        }
        // なぎ払い攻撃
        else if (state == State.Atk3)
        {
            attackTime += Time.deltaTime;

            // 途中で床は壊れる
            // プレイヤーが下にいる
                if (attackTime >= 3.0f && attackTime <= 3.99f)
                {
                    se.GetComponent<AudioSource>().Play();
                    camera.GetComponent<Gpt_Camera>().SetScreenShake(screenShake);

                    SetEXP(1);

                    attackTime += 1.0f;
                }
                else if (attackTime >= 5.5f && attackTime <= 6.49f)
                {
                    se.GetComponent<AudioSource>().Play();
                    camera.GetComponent<Gpt_Camera>().SetScreenShake(screenShake);

                    SetEXP();

                    attackTime += 1.0f;
                }
                else if (attackTime >= 8.0f && attackTime <= 8.99f)
                {
                    se.GetComponent<AudioSource>().Play();
                    camera.GetComponent<Gpt_Camera>().SetScreenShake(screenShake);

                    SetEXP(-1);

                    attackTime += 1.0f;
                }

            // 落下判定
            if (FallCheck(false))
            {
                anim.SetBool("Atk_Nagi_Flg", false);
                state = State.Fall;
                attackTime = 0.0f;
            }

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Select") && attackTime >= ATTACK_TIME_MAX_NAGI)
            {
                anim.SetBool("Atk_Nagi_Flg", false);
                state = State.Search;
                attackTime = 0.0f;
            }
        }
        else if (state == State.Die)
        {
            dieCnt += Time.deltaTime;
            this.transform.position += new Vector3(0, -Time.deltaTime * fallSpd * 0.25f, 0);
            camera.GetComponent<Gpt_Camera>().state = 4;

            if (dieCnt >= 6.0f)
            {
                Gpt_FadeManager.SetFade_White(() => { Gpt_SceneManager.LoadScene("Ending_Staffroll", false); });
            }
            if(dieCnt >= 9.0f)
            {
                Application.LoadLevel("Ending_Staffroll");
            }
        }

        // 最後に床の更新
        //YukaUpdate();
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

    void SetEXP(int add=0)
    {
        yuka[(targetYukaNum+add) % 8].GetComponent<Gpt_YukaBox>().SetExplode(0.0f, 1.0f, 10.0f, 13.0f);
    }

    // プレイヤーと最も近い場所を探す
    void Search_PlayerNearPos()
    {
        // プレイヤーと最も近い場所を探す
        float min = 99999.9f;
        targetYukaNum = 0;
        for (int i = 0; i < AtkTargetPos.Length; i++)
        {
            Vector3 vec = AtkTargetPos[i].position - player.transform.position;
            vec = new Vector3(vec.x, 0.0f, vec.z);
            if (min > vec.magnitude)
            {
                min = vec.magnitude;
                targetYukaNum = i;
            }
        }
        // プレイヤーと最も近い場所保存
        targetPos = AtkTargetPos[(targetYukaNum + 2)%8];
    }

    // 落下判定
    bool FallCheck(bool right)
    {
        if (right)
        {
            if (yuka[(targetYukaNum + 2) % 8].GetComponent<Gpt_YukaBox>().HP <= 0)
            {
                return true;
            }
            return false;
        }
        else {
            if (yuka[(targetYukaNum - 2) % 8].GetComponent<Gpt_YukaBox>().HP <= 0)
            {
                return true;
            }
            return false;
        }
    }

    // 床更新
    //void YukaUpdate()
    //{
    //    for (int i = 0; i < yuka.Length; i++)
    //    {
    //        // 死んでいる状態であれば
    //        if (yukaDieCnt[i] > 0.0f)
    //        {
    //            // 時間経過
    //            yukaDieCnt[i] += Time.deltaTime;
    //            // 一定時間経過で復活
    //            if (yukaDieCnt[i] >= YUKA_RESP_TIME)
    //            {
    //                yuka[i].GetComponent<Gpt_YukaBox>().ReverseTile();
    //                yukaDieCnt[i] = 0.0f;
    //            }
    //        }
    //    }
    //}
}

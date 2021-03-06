﻿using UnityEngine;
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

    public GameObject dieGolemObj;

    State state = State.Search;
    public GameObject camera;
    public GameObject firstPos;
    public GameObject se;
    public GameObject se1;
    public GameObject parentObj;

    const float maxHp = 10.0f;
    float hp = 10.0f;

    public GameObject hand1;
    public GameObject hand2;
    public GameObject hpBar;
    //public GameObject hpBar1;

    bool firstFall = true;

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
    const float READY_TIME_MAX = 4.25f;
    float attackTime = -3.2f;
    const float ATTACK_TIME_MAX = 3.5f*2.0f;
    const float ATTACK_TIME_MAX_NAGI = 7.5f;

    public float screenShake = 4.0f;

    int targetYukaNum = 0;        // もくひょうゆかばんごう
    float dieCnt = 0.0f;
    float upGrav = 0.0f;        // 上昇時重力

    bool blinkFlg = false;
    bool blinks = true;     // 必ずtrueで開始する
    float blinkCnt = 0.0f;
    float moveCnt = 0.01f;
    bool yukaBlink = true;

    float fallCnt = 0.0f;
    bool fallL_Flg = true;

    bool firstDieFlg = true;
    float yukaBlinkCnt = 0.0f;

    void Start()
    {
        for (int i = 0; i < yuka.Length; i++) yukaDieCnt[i] = 0.0f;
        firstUpSpd = upSpd;
    }

    void Update()
    {
        if (player.GetComponent<Gpt_PlayerState>().HP <= 0)
        {
            hpBar.GetComponent<Gpt_BossHPBar>().notDrawFlg = true;
            //hpBar1.GetComponent<Gpt_BossHPBar>().notDrawFlg = true;
        }

        //if (cnt > 1.0f) state = State.Fall;   //debug
        //Debug.Log("STATE: "+state);
        //hp = 0.01f;

        cnt += Time.deltaTime;
        yukaBlinkCnt += Time.deltaTime;
        if (yukaBlink) yukaBlink = false;
        else yukaBlink = true;

        // プレイヤーを探す
        if (state == State.Search)
        {
            // targetPos(プレイヤーから一番近い座標)取得
            Search_PlayerNearPos();
            // ステート変更(プレイヤーが下にいる時だけ)
            //if(player.transform.position.y < 12.0f||true)
            state = State.Move;
        }
        // プレイヤーの方向を向く
        else if (state == State.Move)
        {
            if (readyTime < READY_TIME_MAX)
            {
                //transform.rotation = Quaternion.Slerp(this.transform.rotation, targetPos.rotation, Time.deltaTime);
                transform.rotation = Quaternion.Slerp(this.transform.rotation, targetPos.rotation, (moveCnt += Time.deltaTime * 0.008f));
                readyTime += Time.deltaTime;
            }
            else
            {
                // 一定時間経過でステート変更
                state = State.SelectAtk;
                readyTime = 0.0f;
                moveCnt = 0.01f;
            }
        }
        // 攻撃方法を選ぶ
        else if (state == State.SelectAtk)
        {
            //float rnd = UnityEngine.Random.Range(0.0f, 100.0f);

            state = State.Atk1;
            attackTime = 0.0f;
            anim.SetBool("Atk_L_Flg", true);

            //if (hp >= 7.0f)
            //{
            //    if (rnd < 50.0f)
            //    {
            //        state = State.Atk1;
            //        attackTime = 0.0f;
            //        anim.SetBool("Atk_L_Flg", true);
            //    }
            //    else
            //    {
            //        state = State.Atk2;
            //        attackTime = 0.0f;
            //        anim.SetBool("Atk_R_Flg", true);
            //    }
            //}
            //else
            //{
            //    if (rnd < 25.0f)
            //    {
            //        state = State.Atk1;
            //        attackTime = 0.0f;
            //        anim.SetBool("Atk_L_Flg", true);
            //    }
            //    else if (rnd < 50.0f)
            //    {
            //        state = State.Atk2;
            //        attackTime = 0.0f;
            //        anim.SetBool("Atk_R_Flg", true);
            //    }
            //    else
            //    {
            //        state = State.Atk3;
            //        attackTime = 0.0f;
            //        anim.SetBool("Atk_Nagi_Flg", true);
            //    }
            //}
        }
        else if (state == State.Fall)
        {
            fallCnt += Time.deltaTime;
            anim.SetBool("Atk_R_Flg", false);
            anim.SetBool("Atk_L_Flg", false);
            anim.SetBool("Atk_Nagi_Flg", false);
            anim.SetBool("Atk_FallR_Flg", true);
            anim.SetBool("Atk_FallL_Flg", false);

            if (fallL_Flg)
            {
                //anim.SetBool("Atk_FallR_Flg", true);
            }
            else
            {
                //anim.SetBool("Atk_FallL_Flg", true);
            }

            /* 座標調整 */
            if (firstFall)
            {
                //parentObj.transform.position += 
                //    new Vector3(yuka[(targetYukaNum - 5) % 8].transform.position.x, 0, 
                //    yuka[(targetYukaNum - 5) % 8].transform.position.z);
                firstFall = false;
            }

            // 落下ベクトルを足す
            if (fallCnt >= 0.0f)
            {
                fallSpd += Time.deltaTime * 20.0f;
                this.transform.position += new Vector3(0, -Time.deltaTime * fallSpd, 0);
            }

            if(this.transform.position.y<-20.0f && hp <= 4.0f)
            {
                anim.SetBool("Atk_FallL_Flg", true);
            }

            // 一定まで落ちると被ダメ
            if (this.transform.position.y < fallY)
            {
                fallCnt = 0.0f;
                state = State.Up;
                //parentObj.transform.position -= new Vector3(7.0f, 0, 0);
                //anim.SetBool("Atk_FallL_Flg", false);
                anim.SetBool("Atk_FallR_Flg", false);

                this.hp -= magmaDmg;
                blinkFlg = true;
                fallSpd = 4.0f;
                firstFall = true;

                //parentObj.transform.position -=
                //    new Vector3(yuka[(targetYukaNum - 5) % 8].transform.position.x, 0,
                //    yuka[(targetYukaNum - 5) % 8].transform.position.z);

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
            upSpd -= Time.deltaTime * 5.0f;
            this.transform.position += new Vector3(0, Time.deltaTime * upSpd, 0);

            // 元々いた位置まで上昇すれば
            if (this.transform.position.y < firstBossPos.y && upSpd < 0.0f)
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
            if (hp >= 9.99f)
            {
                if (yukaBlink) yuka[(targetYukaNum + 2) % 8].GetComponent<Gpt_YukaBox>().SetFlush();
                else yuka[(targetYukaNum + 2) % 8].GetComponent<Gpt_YukaBox>().UnSetFlush();
            }

            // 途中で床は壊れる
            if (attackTime <= 10.0f)
            {
                //if (attackTime >= (2.4f*2.0f))
                if (attackTime >= 4.5f)
                    {
                    se.GetComponent<AudioSource>().Play();
                    camera.GetComponent<Gpt_Camera>().SetScreenShake(screenShake);

                    if (yuka[(targetYukaNum) % 8].GetComponent<Gpt_YukaBox>().HP == 1) {
                        yukaDieCnt[(targetYukaNum) % 8] = 0.1f;
                    }
                    SetEXP();

                    attackTime += 10.0f;
                }
            }

            // 落下判定
            if (FallCheck(true) && attackTime>=0.5f)
            {
                anim.SetBool("Atk_FallR_Flg", true);
                state = State.Fall;
                attackTime = 0.0f;
                if (yukaBlink) yuka[(targetYukaNum + 2) % 8].GetComponent<Gpt_YukaBox>().UnSetFlush();
                else yukaBlink = true;
                fallL_Flg = true;
            }

            // 終わったらステート戻す
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Select") && attackTime >= ATTACK_TIME_MAX)
            {
                anim.SetBool("Atk_L_Flg", false);
                anim.SetBool("Atk_R_Flg", false);
                state = State.Search;
                attackTime = 0.0f;
                if (yukaBlink) yuka[(targetYukaNum + 2) % 8].GetComponent<Gpt_YukaBox>().UnSetFlush();
                else yukaBlink = true;
            }
        }
        // 攻撃R
        else if (state == State.Atk2)
        {
            attackTime += Time.deltaTime;
            if (hp >= 9.99f)
            {
                if (yukaBlink) yuka[(targetYukaNum + 2) % 8].GetComponent<Gpt_YukaBox>().SetFlush();
                else yuka[(targetYukaNum + 2) % 8].GetComponent<Gpt_YukaBox>().UnSetFlush();
            }

            // 途中で床は壊れる
            if (attackTime >= 2.4f && attackTime<=10.0f)
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
                if (yukaBlink) yuka[(targetYukaNum + 2) % 8].GetComponent<Gpt_YukaBox>().UnSetFlush();
                else yukaBlink = true;
                fallL_Flg = true;
            }



            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Select") && attackTime >= ATTACK_TIME_MAX)
            {
                anim.SetBool("Atk_R_Flg", false);
                state = State.Search;
                attackTime = 0.0f;
                if (yukaBlink) yuka[(targetYukaNum + 2) % 8].GetComponent<Gpt_YukaBox>().UnSetFlush();
                else yukaBlink = true;
            }
        }
        // なぎ払い攻撃
        else if (state == State.Atk3)
        {
            attackTime += Time.deltaTime;
            if (attackTime>=10.5f)
            {
                anim.SetBool("Atk_Nagi_Flg", false);
                state = State.Search;
                attackTime = 0.0f;
                if (yukaBlink) yuka[(targetYukaNum - 2) % 8].GetComponent<Gpt_YukaBox>().UnSetFlush();
                else yukaBlink = true;
            }
            //if (yukaBlink) yuka[(targetYukaNum - 2) % 8].GetComponent<Gpt_YukaBox>().SetFlush();
            //else yuka[(targetYukaNum - 2) % 8].GetComponent<Gpt_YukaBox>().UnSetFlush();

            // 途中で床は壊れる
            // プレイヤーが下にいる
            if (attackTime >= 2.0f && attackTime <= 2.99f)
            {
                se.GetComponent<AudioSource>().Play();
                camera.GetComponent<Gpt_Camera>().SetScreenShake(screenShake);

                SetEXP(1);

                attackTime += 1.0f;
            }
            else if (attackTime >= 4.5f && attackTime <= 5.49f)
            {
                se.GetComponent<AudioSource>().Play();
                camera.GetComponent<Gpt_Camera>().SetScreenShake(screenShake);

                SetEXP();

                attackTime += 1.0f;
            }
            else if (attackTime >= 6.5f && attackTime <= 7.49f)
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
                if (yukaBlink) yuka[(targetYukaNum - 2) % 8].GetComponent<Gpt_YukaBox>().UnSetFlush();
                else yukaBlink = true;
                fallL_Flg = false;
            }

            //if (anim.GetCurrentAnimatorStateInfo(0).IsName("Select"))
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Select") && attackTime >= ATTACK_TIME_MAX_NAGI)
                {
                anim.SetBool("Atk_Nagi_Flg", false);
                state = State.Search;
                attackTime = 0.0f;
                if (yukaBlink) yuka[(targetYukaNum - 2) % 8].GetComponent<Gpt_YukaBox>().UnSetFlush();
                else yukaBlink = true;
            }
        }
        else if (state == State.Die)
        {
            anim.SetBool("Atk_R_Flg", false);
            anim.SetBool("Atk_L_Flg", false);
            anim.SetBool("Atk_Nagi_Flg", false);
            anim.SetBool("Atk_FallL_Flg", true);
            anim.SetBool("Atk_FallR_Flg", false);

            Debug.Log("LLL"+anim.GetBool("Atk_FallR_Flg"));
            Debug.Log("RRR"+anim.GetBool("Atk_FallL_Flg"));

            player.GetComponent<Gpt_PlayerState>().AddHP(12);

            if (blinks) blinkFlg = false;

            if (firstDieFlg)
            {
                firstDieFlg = false;
                se1.GetComponent<AudioSource>().Play();

                dieGolemObj.SetActive(true);
                this.gameObject.SetActive(false);
            }

            if (dieCnt < 0.2f)
            {
                transform.position = new Vector3(0, -18.0f, 0);
            }



            dieCnt += Time.deltaTime;
            this.transform.position += new Vector3(0, -Time.deltaTime * fallSpd * 0.25f, 0);
            camera.GetComponent<Gpt_Camera>().state = 4;

            if (dieCnt >= 6.0f)
            {
                Gpt_FadeManager.SetFade_White(() => { Gpt_SceneManager.LoadScene("Story_Last", false); });
            }
            if(dieCnt >= 9.0f)
            {
                Application.LoadLevel("Story_Last");
            }
        }

        // 点滅
        if (blinkFlg && !(State.Die == state))
        {
            blinkCnt += Time.deltaTime;
            if(blinks)
            {
                this.transform.position += new Vector3(-10000,0,0);
                blinks = false;
            }
            else
            {
                this.transform.position += new Vector3(10000, 0, 0);
                blinks = true;
            }

            if(Mathf.Abs(this.transform.position.x)<1000.0f && blinkCnt>= 4.5f)
            {
                blinkFlg = false;
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
        yuka[(targetYukaNum + add) % 8].GetComponent<Gpt_YukaBox>().SetExplode(0.0f, 0.01f, 2.7f, 3.7f);
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

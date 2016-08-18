﻿using UnityEngine;
using System.Collections;

public class Gpt_Enemy : MonoBehaviour {

    public Gpt_EnemyMove EnemyMove;
    public Gpt_EnemyColor EnemyColor;
    public Gpt_EnemyAttack EnemyAttack;
    public Collider collider;
    private GameObject ManegerObject;
    public Rigidbody rigid;
    private Gpt_EnemyGravityManeger EnemyGravityManeger;

    private CharacterController Character;

    //雑魚パターン(0:近接 1:遠隔)
    public int enemyPattern=0;
    //重力エネルギー
    public float gravity=0.13f;

    //重力フラグ
    private bool gravityFlag;
    //接触フラグ
    private bool touchFlag;

    public int hitPoint = 1;
    float test;

    //爆発物オブジェクト
    public GameObject exploder;
    private Vector3 exploderPosition;

    //爆風の位置
    private Vector3 waveExploderPosition;
    //爆発フラグ
    private bool isExplode;

    //爆発後フラグ
    private bool isAfterExplode;

    //爆発時間
    private float explodeTime = 0f;

    private float gravityTime;

    private float motionTime1 ;
    private float motionTime2 ;

    //一時的にひきつけ時の座標とベクトルを保存
    private float preserveX;
    private float preserveZ;

    private Vector3 preserveVec;

    private GameObject getExploder;

    public Collider coll;

    //復活(爆発してから)の時間（15fで約3秒）
    public float revivalTime=15f;
    private float revivalCount;

    //復活(固まってから)の時間 (30fで約20秒)
    public float firmTime = 30f;
    private float firmCount;

    //爆風ダメージ時の点滅時間
    private float damageTime = 5f;
    private float damageCount;
    //爆風ダメージ時の点滅フラグ
    private bool damageFlag;

    //小刻み処理の際の片側だけに引力が発生するのを防ぐ
    private bool shakeFlag;

    private float enemyUpTime=5f;

    private float enemyUnderTime = 1f;

    public float waveExplodeSpeed = 3f;

    //吹っ飛ぶ時のスピード
    public float forceSpeed;
    //吹っ飛ぶ時の高さ
    public float forceHeight;
    private bool isForce;
    //力がかかる時間
    public float forceTime;
    private float forceTimeTemp;

    private bool isDeath;
    private bool isDeath2;

    // Use this for initialization
    void Start () {

        //シーン上にあるGravityManegerを自動取得する。（プレハブから一つ生成する。）
        ManegerObject = GameObject.Find("GravityManeger");
        EnemyGravityManeger = ManegerObject.GetComponent<Gpt_EnemyGravityManeger>();
        EnemyGravityManeger.AddEnemyList(this);
        //Speed(2f);
        EnemyAttack.SetEnemyPattern(enemyPattern);
        EnemyMove.SetEnemyPattern(enemyPattern);
        EnemyMove.SetVecter(this.transform.forward);
        int temp= Random.Range(0, 1); 
        EnemyColor.SetColor(temp);
        
        Character = this.GetComponent<CharacterController>();
        //Debug.Log(EnemyGravityManeger.ListIndex(this));
    }

    void OnCollisionEnter(Collision collision)
    {
        
        //重力フラグ時、敵同士の接触で爆発オブジェクト生成
        if (gravityFlag&&!touchFlag)
        {
            
            if (collision.gameObject.tag == "Enemy")
            {
                Debug.Log("Enemy");
                //自分の相手の座標からどちらかにのみ爆発オブジェクト生成
                if (this.transform.position.x * this.transform.position.z > collision.gameObject.transform.position.x * collision.gameObject.transform.position.z)
                {
                    // インスタンス生成
                    float temp = Random.Range(-0.01f, 0.01f);
                    Vector3 trans = collision.gameObject.transform.position+new Vector3(temp,temp,temp);//バグ防止
                    GameObject gameObj = Instantiate(exploder, trans, Quaternion.identity) as GameObject;
                    Gpt_Exploder targetExploder = gameObj.GetComponent<Gpt_Exploder>();
                    targetExploder.SetColor(GetColor());
                    touchFlag = true;
                }

            }

        }
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "GravityZone")
        {
            Gpt_Exploder targetExploder=collision.gameObject.GetComponent<Gpt_Exploder>();
            //Debug.Log(targetExploder.transform.position);
            if (targetExploder.GetColor() ==GetColor())
            {
                exploderPosition = collision.gameObject.transform.position;
                getExploder = collision.gameObject;
            }
            
        }
    }


    float temp;
    // Update is called once per frame
    void Update () {

        temp+=0.1f;
        if (temp > 10f) {
            //IsExplode();
        }
        if (gravityFlag)
        {
            gravityTime += 0.1f;
            rigid.AddForce(-preserveVec * gravity/100, ForceMode.VelocityChange);

            firmCount += 0.1f;
            if (firmCount > firmTime&&!isExplode)
            {
                EnemyReset();
                SetColor(0);
            }
        }
        else if (damageFlag)//爆風の点滅処理
        {
            damageCount+=0.1f;
            EnemyColor.IsDamage();
            EnemyAttack.StopAttack();
            Vector3 waveVec = (waveExploderPosition - this.transform.position).normalized;
            coll.enabled = true;
            rigid.useGravity = true;
            rigid.isKinematic = false;
            waveVec.y = -1f + motionTime2;
            //爆風でふっとぶ調整

            WaveForce(waveVec);
            
            if (damageCount > damageTime)
            {
                EnemyColor.IsDamageFalse();
                damageFlag=false;
                EnemyReset();
            }
        }

        //爆発モーション
        if (isExplode)
        {
            coll.enabled = false;
            rigid.useGravity = false;
            //エラー防止
            if (getExploder != null)
            {
                exploderPosition = getExploder.transform.position;
            }
            motionTime1 += 0.1f;
            revivalCount += 0.1f;
            
            //collider.isTrigger=true;
            Vector3 exVec = (exploderPosition - this.transform.position).normalized;
            
            if (motionTime1 < enemyUpTime)
            {
                rigid.AddForce(exVec * 10f, ForceMode.VelocityChange);
                //this.transform.position = exploderPosition;
                //gravityVec.y = 0f;
                var vec = Quaternion.Euler(0f, 90f, 0f) * exVec;
                rigid.AddForce(vec * 0.8f, ForceMode.VelocityChange);
            }
            else if(motionTime1 < enemyUpTime+ enemyUnderTime)
            {
                rigid.isKinematic = true;
                this.transform.position = Vector3.Lerp(this.transform.position, exploderPosition, 0.2f);
                if (getExploder != null)
                {
                    Gpt_Exploder explodeScript = getExploder.GetComponent<Gpt_Exploder>();
                    explodeScript.IsExplodeMotion1();
                }
            }
            else
            {
                //色を戻す
                SetColor(0);
                if (motionTime2 == 0)
                {
                    hitPoint--;
                    if (hitPoint <= 0)
                    {
                        isDeath=true;
                    }
                }
                if (getExploder != null)
                {
                    Gpt_Exploder explodeScript = getExploder.GetComponent<Gpt_Exploder>();
                    explodeScript.IsAfterExplode();
                }
                //吹っ飛ぶスピード、高さ
                WaveForce(exVec);
                motionTime2 += 0.2f;
                coll.enabled = true;
                rigid.useGravity = true;
                rigid.isKinematic = false;
                exVec.y = -1f + motionTime2;
                
                
                EnemyColor.IsDamage();

                
                if (revivalCount > revivalTime)
                {
                    //HP0で死亡
                    if (hitPoint <= 0)
                    {
                        EnemyDestroy(0f);
                    }
                    else
                    {
                        EnemyColor.IsDamageFalse();
                        EnemyReset();
                    }
                   
                }
            }
        }

        //爆風での死亡判定
        if (hitPoint<=0)
        {
            //死亡フラグ時に敵のカウントを減らす
            if (isDeath)
            {
                EnemyGravityManeger.ReduceNumCount();
                isDeath = false;
            }
            
            Speed(0);
            EnemyDestroy(2f);
            EnemyMove.IsGravity();
            this.transform.Rotate(this.transform.up, temp * 2f);
            EnemyAttack.StopAttack();
        }

        
        //爆発・爆風での力制御
    }
    

    public void Speed(float s)
    {

        EnemyMove.SetSpeed(s);
    }


    //EnemyManagerからの参照
    public void SetGravity(Vector3 gravityVec)
    {
        if (!gravityFlag)
        {
            //x,z座標保存
            preserveX = this.transform.position.x;
            preserveZ = this.transform.position.z;
            preserveVec = gravityVec;
            EnemyAttack.StopAttack();
        }
        //Debug.Log("Gravity");
        gravityFlag = true;

        if(!isExplode){
            Speed(0);
            //始めは小刻みに震える
            if (gravityTime < 2f)
            {
                shakeFlag = true;
                Character.enabled = false;

                rigid.isKinematic = false;
                rigid.useGravity = true;
                float x_temp = Random.Range(-0.2f, 0.2f);
                float z_temp = Random.Range(-0.2f, 0.2f);
                this.transform.position=new Vector3(preserveX+x_temp,this.transform.position.y,
                    preserveZ+ z_temp);
            }
            else
            {
                shakeFlag = false;
                if (EnemyAttack != null)
                {
                    EnemyAttack.StopAttack();
                }
                rigid.AddForce(-gravityVec * gravity, ForceMode.VelocityChange);
            }
            
        }
        else
        {
            //爆発処理
            //if (!isAfterExplode)
            //{
            //Debug.Log("fffss");
            //現状固定の角度とスピード（後で変えるかも）
            //coll.enabled = false;
            //rigid.useGravity = false;
            //motionTime1 += 0.1f;
            //motionTime2 += 0.1f;
            ////collider.isTrigger=true;
            //Vector3 exVec = ( exploderPosition - this.transform.position ).normalized;
            //rigid.AddForce(exVec * 20f, ForceMode.VelocityChange);
            ////this.transform.position = exploderPosition;
            //if (motionTime1 < 3f)
            //{

            //    //gravityVec.y = 0f;
            //    var vec = Quaternion.Euler(0f, 90f, 0f) * exVec;
            //    rigid.AddForce(vec * 2f, ForceMode.VelocityChange);

            //}
            //else if(motionTime2>6f)
            //{
            //    //rigid.AddForce(gravityVec * 4f, ForceMode.VelocityChange);
            //}
            

            //}
            //isAfterExplode = true;
        }
        

        EnemyMove.IsGravity();
    }

    void OnDestroy()
    {
        EnemyGravityManeger.RemoveEnemyList(this);
    }

    public void IsExplode()
    {
        isExplode = true;
    }

    public void EnemyDestroy(float delay)
    {
        Object.Destroy(this.gameObject, delay);
    }

    public void SetColor(int setColor)
    {
        EnemyColor.SetColor(setColor);
    }

    public int GetColor()
    {
        return EnemyColor.GetColor();
    }

    //
    public bool GetGravity()
    {
        //重力フラグ確認
        return gravityFlag;
    }

    public bool GetTouch()
    {
        return touchFlag;
    }

    public void EnemyReset()
    {
        isExplode = false;
        gravityFlag = false;
        coll.enabled = true;
        Character.enabled = true;
        rigid.isKinematic = true;
        rigid.useGravity = false;
        EnemyMove.IsGravityFalse();
        EnemyMove.SetPreserveSpeed();
        gravityTime = 0;
        touchFlag = false;
        shakeFlag = false;
        motionTime1 = 0;
        motionTime2 = 0;
        revivalCount = 0;
        firmCount = 0;
    }

    //爆風のダメージ
    public void ExplodeDamage(int damage)
    {
        hitPoint -= damage;
        damageFlag = true;
        if (hitPoint <= 0)
        {
            isDeath = true;
        }

    }

    public bool GetShake()
    {
        return shakeFlag;
    }

    public void SetUpTime(float time)
    {
        enemyUpTime = time;
    }

    public void SetUnderTime(float time)
    {
        enemyUnderTime = time;
    }

    public void SetWavePosition(Vector3 position)
    {
        waveExploderPosition = position;
    }

    public void WaveForce(Vector3 vec)
    {
        forceTimeTemp += 0.1f;
        vec.y = 0;
        if (forceTimeTemp < forceTime)
        {
            rigid.AddForce(vec * forceSpeed, ForceMode.VelocityChange);
        }
        else
        {
            forceTimeTemp = 0;
        }
    }
}

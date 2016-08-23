using UnityEngine;
using System.Collections;

public class Gpt_Enemy : MonoBehaviour {

    public Gpt_EnemyMove EnemyMove;
    public Gpt_EnemyColor EnemyColor;
    public Gpt_EnemyAttack EnemyAttack;
    public GameObject animationObject;
    private Gpt_EnemyAnimation EnemyAnimation;
    public Collider collider;
    private GameObject ManegerObject;
    public Rigidbody rigid;
    public NavMeshAgent navAgent;
    private Gpt_EnemyGravityManeger EnemyGravityManeger;
    private Gpt_YukaManager YukaManager;
    private CharacterController Character;
    public bool IsTop { get; private set; }
    private bool isTopExplode;

    private GameObject player;

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
    public float damageTime = 5f;
    private float damageCount;
    //爆風ダメージ時の点滅フラグ
    public bool damageFlag{get;private set;}

    //小刻み処理の際の片側だけに引力が発生するのを防ぐ
    private bool shakeFlag;

    private float enemyUpTime=5f;

    private float enemyUnderTime = 1f;

    public float waveExplodeSpeed = 3f;

    //吹っ飛ぶ時のスピード
    public float forceSpeed=700f;
    //吹っ飛ぶ時の高さ
    public float forceHeight=6f;
    private bool isForce;
    //力がかかる時間
    public float forceTime;
    private float forceTimeTemp;

    private bool isDeath;
    private bool isDeath2;

    //転倒パラメータ
    public float faliingTime=8f;

    //起点敵の位置
    private Vector3 firstVector;
    private bool firstEnemy;

    //始めの敵が空中に浮かび上がる高さ
    public float firstHeight;

    //転がっているか
    public bool CanSetColor { get; set; }

    //地面下に落ちたか
    private bool isRakka;

    //何割地面に落ちるか(全て落ちる場合は100)
    public float fallProb=30f;
    private float fallRand;

    //ポイントエフェクト
    public GameObject pointEffect_red;
    public GameObject pointEffect_blue;
    public GameObject pointEffect_yellow;
    private GameObject pointEffect;
    private Gpt_PointEffect pointScript;

    //浮く高度
    public float topSky = 1.5f;
    private float skyTemp;

    public GameObject deathExplode;

    // Use this for initialization
    void Start () {

        //シーン上にあるGravityManegerを自動取得する。（プレハブから一つ生成する。）
        ManegerObject = GameObject.Find("GravityManeger");
        EnemyGravityManeger = ManegerObject.GetComponent<Gpt_EnemyGravityManeger>();

        ManegerObject = GameObject.Find("YukaManager");
        YukaManager = ManegerObject.GetComponent<Gpt_YukaManager>();

        EnemyGravityManeger.AddEnemyList(this);
        //Speed(2f);
        EnemyAttack.SetEnemyPattern(enemyPattern);
        EnemyMove.SetEnemyPattern(enemyPattern);
        EnemyMove.SetVecter(this.transform.forward);
        int temp= Random.Range(0, 1); 
        EnemyColor.SetColor(temp);
        player = GameObject.Find("Player");
        Character = this.GetComponent<CharacterController>();
        //Debug.Log(EnemyGravityManeger.ListIndex(this));
        CanSetColor = true;

        EnemyAnimation=animationObject.GetComponent<Gpt_EnemyAnimation>();
    }

    void OnCollisionEnter(Collision collision)
    {
        
        //重力フラグ時、敵同士の接触で爆発オブジェクト生成
        if (gravityFlag&&!touchFlag)
        {
            
            if (collision.gameObject.tag == "Enemy")
            {
                //Debug.Log("Enemy");
                //自分の相手の座標からどちらかにのみ爆発オブジェクト生成
                //爆発範囲が無限ならこの処理はいらない
                //if (this.transform.position.x * this.transform.position.z > collision.gameObject.transform.position.x * collision.gameObject.transform.position.z)
                //{
                //    // インスタンス生成
                //    float temp = Random.Range(-0.01f, 0.01f);
                //    Vector3 trans = collision.gameObject.transform.position+new Vector3(temp,temp,temp);//バグ防止
                //    GameObject gameObj = Instantiate(exploder, trans, Quaternion.identity) as GameObject;
                //    Gpt_Exploder targetExploder = gameObj.GetComponent<Gpt_Exploder>();
                //    targetExploder.SetColor(GetColor());
                //    touchFlag = true;
                //}

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
                if (IsTop)
                {
                    //Debug.Log("!!!!!!!");
                }
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

        //下に床が無ければnavmeshを消す
        if (!YukaManager.HasTile(this.transform.position))
        {
            //navAgent.enabled = false;
        }
        

        if (IsTop)
        {
            if (!isExplode)
            {
                skyTemp += 0.1f;
                skyTemp = skyTemp < topSky ? skyTemp : topSky;
                this.transform.position = topSettedPosition+new Vector3(0,skyTemp,0);
                //中心に爆発オブジェクト生成
                Character.enabled = false;
                rigid.isKinematic = true;
                rigid.useGravity = false;
                navAgent.enabled = false;
                if (!isTopExplode)
                {
                    GameObject gameObj = Instantiate(exploder, this.transform.position, Quaternion.identity) as GameObject;
                    Gpt_Exploder targetExploder = gameObj.GetComponent<Gpt_Exploder>();
                    targetExploder.SetColor(GetColor());
                    touchFlag = true;

                    Vector3 yuka = new Vector3(this.transform.position.x, YukaManager.transform.position.y+0.35f,this.transform.position.z);
                    switch (GetColor())
                    {
                        case 1:
                            pointEffect = Instantiate(pointEffect_red, yuka, Quaternion.identity) as GameObject;
                            pointScript=pointEffect.GetComponent<Gpt_PointEffect>();
                            break;
                        case 2:
                            pointEffect = Instantiate(pointEffect_blue, yuka, Quaternion.identity) as GameObject;
                            pointScript = pointEffect.GetComponent<Gpt_PointEffect>();
                            break;
                        case 3:
                            pointEffect = Instantiate(pointEffect_yellow, yuka, Quaternion.identity) as GameObject;
                            pointScript = pointEffect.GetComponent<Gpt_PointEffect>();
                            break;
                    }
                    
                }
                isTopExplode = true;
                EnemyAttack.StopAttack();
            }

            //色の数が2以上で範囲をつける
            if (GetColor() != 0)
            {
                if (EnemyGravityManeger.recodeColorNum[GetColor() - 1]>=2)
                {
                    //Debug.Log("afafa");
                    pointScript.isStart = true;
                }
            }

            //床がない場合落ちる
            if (!YukaManager.HasTile(this.transform.position))
            {
                
            }
            
            
        }

        //始めの敵ならその位置固定
        if (firstEnemy)
        {
            Speed(0);
            EnemyAttack.StopAttack();
            Vector3 firstTemp=new Vector3(0,firstHeight,0);
            this.transform.position = firstVector;
            Debug.Log("first");
            this.transform.position = Vector3.Lerp(firstVector, firstVector + firstTemp, 0.5f);
        }

        if (gravityFlag)
        {
            gravityTime += 0.1f;
            if (isRakka)
            {
                Debug.Log("rakka");
                preserveVec = new Vector3(0,300f,0);
            }
            rigid.AddForce(-preserveVec * gravity / 100, ForceMode.VelocityChange);
            //coll.isTrigger = true;
            firmCount += 0.1f;
            if (firmCount > firmTime&&!isExplode)
            {
                //始めの敵ならはがれない
                if (!firstEnemy)
                {
                    EnemyReset();
                    SetColor(0);
                }
                
            }
        }
        else if (damageFlag)//爆風の点滅処理
        {
            //Debug.Log("damagegg");
            EnemyColor.IsDamage();
            EnemyAttack.StopAttack();

            //吹き飛びを派手にするために斜め上のベクトルを作る(床より下の位置からベクトルを作る)
            Vector3 underPosition = waveExploderPosition;// - new Vector3(0, YukaManager.transform.position.y-1f, 0);
            Vector3 waveVec = (underPosition - this.transform.position).normalized;
            //Character.enabled = false;
            //coll.enabled = true;
            //rigid.useGravity = true;
            //rigid.isKinematic = false;
            //navAgent.enabled = false;

            Character.enabled = false;
            coll.enabled = true;
            rigid.isKinematic = false;
            rigid.useGravity = true;
            navAgent.enabled = false;
            //waveVec.y = -1f + motionTime2;
            //爆風でふっとぶ調整
            if (damageCount == 0f)
            {

                //Debug.Log("Damage");
                //waveVec.y = 0;
                //waveVec.y=2000f;
                //waveVec = new Vector3(0,0,0);
                //rigid.AddForce(waveVec * forceSpeed/4, ForceMode.VelocityChange);
                waveVec.y = 0;
                rigid.AddForce(waveVec * forceSpeed * 10f, ForceMode.VelocityChange);
                rigid.AddForce(new Vector3(0, forceHeight / 10f, 0), ForceMode.VelocityChange);
            }
            
            //何故か↑のコードで吹っ飛ばないので暫定的に
            if (damageCount < 0.2f)
            {
                waveVec.y = 0;
                rigid.AddForce(-waveVec * forceSpeed * 6f, ForceMode.VelocityChange);
                rigid.AddForce(new Vector3(0, forceHeight / 10f, 0), ForceMode.VelocityChange);
            }

            damageCount += 0.1f;
            CanSetColor = false;
            EnemyAnimation.IsOkiAction(damageTime+4.8f);//調整
            if (damageCount > damageTime)
            {
                Debug.Log("damageCount");
                EnemyColor.IsDamageFalse();
                damageFlag=false;
                EnemyReset();
            }
        }

        //Debug.Log(Character.enabled);
        //爆発モーション
        if (isExplode)
        {
            //ポイントオブジェクトの削除
            if (pointEffect != null)
            {
                pointScript.isDelete=true;
            }
            
            if (IsTop)
            {
                IsTop = false;
                
            }
            CanSetColor = false;
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
                
                if (getExploder != null)
                {
                    Gpt_Exploder explodeScript = getExploder.GetComponent<Gpt_Exploder>();
                    explodeScript.IsAfterExplode();
                }
                //吹っ飛ぶスピード、高さ
                //WaveForce(exVec);
                
                
                
                coll.enabled = true;
                rigid.useGravity = true;
                rigid.isKinematic = false;
                //exVec.y = -1f + motionTime2;

                if (motionTime2 == 0)
                {
                    
                    hitPoint--;
                    if (hitPoint <= 0)
                    {
                        isDeath = true;
                        exVec.y=0;
                        
                    }
                    float rand = Random.Range(0, 100f);
                    fallRand = rand;
                    exVec.y = 0;
                    //Debug.Log("DD:" + rand);
                    if (fallRand < fallProb)
                    {

                        rigid.AddForce(exVec * forceSpeed, ForceMode.VelocityChange);
                        rigid.AddForce(new Vector3(0, forceHeight, 0), ForceMode.VelocityChange);
                        
                    }
                    else
                    {

                        rigid.AddForce(exVec * forceSpeed * 10f, ForceMode.VelocityChange);
                        rigid.AddForce(new Vector3(0, forceHeight / 10f, 0), ForceMode.VelocityChange);
                    }
                    
                    
                }

                if (fallRand < fallProb)
                {
                    rigid.AddForce(new Vector3(0, -0.6f, 0), ForceMode.VelocityChange);
                }

                motionTime2 += 0.2f;
                EnemyColor.IsDamage();
                EnemyAnimation.IsOkiAction(revivalTime);
                
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
            //this.transform.Rotate(this.transform.up, temp * 2f);
            EnemyAttack.StopAttack();
        }

        if (this.transform.position.y < YukaManager.transform.position.y - 0.4f)
        {
            isRakka = true;
        }


        if (isRakka)
        {
            Character.enabled = false;
            rigid.isKinematic = false;
            rigid.useGravity = false;
            navAgent.enabled = false;
            //Debug.Log("rakka");
            preserveVec = new Vector3(0, 300f, 0);
            rigid.AddForce(-preserveVec * gravity / 100, ForceMode.VelocityChange);
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

            //転ばせる
            Character.enabled = false;
            rigid.isKinematic = false;
            rigid.useGravity = false;
            navAgent.enabled = false;
            rigid.AddForce(player.transform.right * faliingTime, ForceMode.VelocityChange);
            this.gameObject.layer = LayerMask.NameToLayer("EnemyTrigger");
        }
        //Debug.Log("Gravity");
        gravityFlag = true;

        //床の位置より低かったらそのまま落下
        if (this.transform.position.y < YukaManager.transform.position.y-0.4f)
        {
            isRakka = true;
            //EnemyReset();
            //Character.enabled = false;
            //rigid.isKinematic = false;
            //navAgent.enabled = false;
            //rigid.useGravity = true;
            //Vector3 rakka = new Vector3(0, 10f, 0);
            //rigid.AddForce(rakka, ForceMode.VelocityChange);
        }

        if(!isExplode){
            Speed(0);
            //始めは小刻みに震える(8/19暫定無し)
            if (gravityTime < 2f)
            {
                shakeFlag = true;
                Character.enabled = false;

                rigid.isKinematic = false;
                rigid.useGravity = true;
                float x_temp = Random.Range(-0.2f, 0.2f);
                float z_temp = Random.Range(-0.2f, 0.2f);
                
                //this.transform.position=new Vector3(preserveX+x_temp,this.transform.position.y,
                    //preserveZ+ z_temp);
            }
            else
            {
                shakeFlag = false;
                if (EnemyAttack != null)
                {
                    EnemyAttack.StopAttack();
                }
                Vector3 exVec = (exploderPosition - this.transform.position).normalized;
                //rigid.AddForce(-gravityVec * gravity, ForceMode.VelocityChange);
                if (gravityTime < 4.5f)
                {
                    rigid.AddForce(exVec * gravity, ForceMode.VelocityChange);
                }
                else
                {
                    rigid.AddForce(exVec * gravity/1.5f, ForceMode.VelocityChange);
                }
                

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
        Instantiate(deathExplode,this.transform.position, Quaternion.identity);
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
        coll.isTrigger = false;
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
        firstEnemy = false;
        IsTop = false;
        isTopExplode = false ;
        CanSetColor = true;
        navAgent.enabled = true;
        //敵同士のあたり判定を戻す
        this.gameObject.layer = LayerMask.NameToLayer("EnemyBody");
        //起き上がるモーションを入れる。
        
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

    public void SetExploderPosition(Vector3 position)
    {
        exploderPosition = position;
    }

    public void WaveForce(Vector3 vec)
    {
        forceTimeTemp += 0.1f;
        //vec.y = 0;
        if (forceTimeTemp < forceTime)
        {
            vec.y = 0;
            rigid.AddForce(vec * forceSpeed, ForceMode.VelocityChange);
            rigid.AddForce(new Vector3(0, forceHeight, 0), ForceMode.VelocityChange);
        }
        else
        {
            forceTimeTemp = 0;
        }
    }

    public Vector3 FirstEnemyPosition()
    {
        
        firstEnemy = true;
        firstVector = this.transform.position;
        return this.transform.position;
    }

    Vector3 topSettedPosition;
    public void SetTop()
    {
        topSettedPosition = this.transform.position + new Vector3(0,0.2f,0);
        IsTop = true;
    }
    
}

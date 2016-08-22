using UnityEngine;
using System.Collections;


public class Gpt_EnemyMove : MonoBehaviour {


    public Gpt_EnemyAttack EnemyAttack;
    public GameObject Animator;
    private Gpt_EnemyAnimation EnemyAnimation;
    //走行スピード
    public float enemySpeed;
    private float preserveEnemySpeed;
    public NavMeshAgent navMesh;
    //加速
    public float enemyAccelerate;
    private float enemyTemp;

    //雑魚パターン(0:近接 1:遠隔)
    private int enemyPattern;

    //プレイヤー取得
    private GameObject player;

    //索敵範囲
    public float searchArea;

    //遠隔攻撃範囲
    public float attackArea;

    public new Rigidbody rigidbody;

    private CharacterController Character;
    //public float friction = 0.6f;

    private Vector3 enemyVector = new Vector3();
    Transform myTransform;

    float GRAVITY = 9.8f;
    private float gravity;

    private bool isGravity=false;

    //移動回転角
    private float moveAngle=-1;
    //移動時間(最小)
    public float moveTimeMin;
    //移動時間(最大)
    public float moveTimeMax;
    //停止時間(最小)
    public float stopTimeMin;
    //停止時間(最小)
    public float stopTimeMax;

    //移動時間
    private float moveTime;
    //停止時間
    private float stopTime;

    //移動係数
    private float move=0;
    //停止係数
    private float stop=0;

    //移動スイッチ
    private bool isWalked=false;
    //起動スイッチ
    private bool isMoved = false;

    private bool isAbyss = false;

    private float motionTime1;

    //攻撃時にベクトルを保存
    private Vector3 preserveVec;

    private float breakTime;

    //ナビゲーション用に回転するベクトルを保存するための変数
    private Vector3 beforePosition;
    private Vector3 afterPosition;

    // Use this for initialization
    void Start()
    {
        EnemyAnimation = Animator.GetComponent<Gpt_EnemyAnimation>();
        Character = GetComponent<CharacterController>();
        player = GameObject.Find("Player");
        myTransform = transform;
        enemyVector= new Vector3(0, 0, 0);
        preserveEnemySpeed = enemySpeed;
        beforePosition = this.transform.position;
        navMesh.enabled=false;
    }

    // Update is called once per frame
    void Update()
    {
        //始めて地面に着いたときに移動
        if (Character.isGrounded)
        {
            if(!isMoved){
                isWalked = true;
                isMoved = true;
                navMesh.enabled = true;
            }
            gravity = 0;
        }
        else
        {
            gravity -= GRAVITY * Time.deltaTime;
        }


        Vector3 enemyMove;
                
        //移動
        //重力判定時は移動処理は行わない
        if (!isGravity)
        {
            if (isWalked)
            {
                if (moveAngle == -1)
                {
                    moveAngle = Random.Range(0, 361);
                    moveTime = Random.Range(moveTimeMin, moveTimeMax);
                    stopTime = Random.Range(stopTimeMin, stopTimeMax);

                }
                move += 0.1f;
                Vector3 moveVec = AngleToVector(moveAngle);
                //Debug.Log("walked");

                Vector3 tempEnemyVec = new Vector3(0, this.transform.position.y, 0);
                Vector3 tempPlayerVec = new Vector3(0, player.transform.position.y, 0);
                //索敵処理
                //高さの判定を入れてリスポーン時は敵が引き寄せられない。
                if (Vector3.Distance(player.transform.position, this.transform.position) < searchArea&&
                    Vector3.Distance(tempPlayerVec, tempEnemyVec) < 4f)
                {
                    navMesh.enabled = true;
                    if (!EnemyAttack.GetAttack())
                    {
                        //moveVec = Vector3.Slerp(moveVec, player.transform.position - this.transform.position, 0.75f);
                        moveVec = player.transform.position - this.transform.position;
                        preserveVec=moveVec;
                        if (navMesh!=null)
                        {
                            navMesh.enabled = true;
                        }
                        
                    }
                    else
                    {
                        //攻撃モーション時は直
                        
                        moveVec = preserveVec;
                        if (navMesh != null)
                        {
                            navMesh.enabled = false;
                        }
                    }
                    //moveVec = player.transform.position - this.transform.position;
                    moveVec = moveVec.normalized;
                    move = 0;
                }
                else//範囲外の場合ナビゲーションはオフにする
                {
                    navMesh.enabled = false;

                }
                //Debug.Log("Beforenemy:" + enemyTemp);

                if (navMesh.enabled)
                {
                    //ナビゲーション用に回転
                    afterPosition = this.transform.position;
                    Vector3 rotateVector = afterPosition - beforePosition;
                    rotateVector = rotateVector.normalized;
                    float angle = Mathf.Atan2(rotateVector.z, rotateVector.x);
                    ////移動方向に回転
                    this.transform.rotation = Quaternion.Euler(new Vector3(0, radToDigree(-angle) + 90, 0));
                    beforePosition = this.transform.position;
                }
                else
                {
                    float angle = Mathf.Atan2(moveVec.z, moveVec.x);
                    ////移動方向に回転
                    this.transform.rotation = Quaternion.Euler(new Vector3(0, radToDigree(-angle) + 90, 0));
                }

                //移動処理
                if (move < moveTime)
                {
                    enemyTemp += enemyAccelerate;
                    enemyTemp = enemyTemp < enemySpeed ? enemyTemp : enemySpeed;
                    //攻撃の射程に入った時
                    if ((Vector3.Distance(player.transform.position, this.transform.position) < attackArea||EnemyAttack.GetAttack())&&EnemyAttack.CanAttack)
                    {
                        //Debug.Log("attack");
                        //motionTime1 += 2f;
                        //enemyTemp = 0.01f;
                        enemyTemp = enemySpeed;
                        EnemyAttack.IsAttack();
                        
                    }

                    enemyTemp = enemySpeed;

                    if (navMesh != null && navMesh.enabled)
                    {
                        enemyTemp = 0;
                        breakTime = 0;
                    }
                    else if (EnemyAttack.GetAttack())//攻撃時減速
                    {
                        breakTime += 0.01f;
                        breakTime= breakTime > 1f ? 1f : breakTime;
                        enemyTemp*=1f-breakTime;
                    }

                    if (isAbyss)
                    {
                        enemyTemp = 0;
                    }

                    //ダメージモーション中は動かない
                    if (EnemyAnimation.isOkiAction)
                    {
                        enemyTemp = 0;
                    }
                    //Debug.Log(enemySpeed);
                    
                    enemyMove.x = moveVec.x * enemyTemp;
                    enemyMove.y = gravity;
                    enemyMove.z = moveVec.z * enemyTemp;
                    Character.Move(enemyMove * Time.deltaTime);
                }
                //停止処理
                else
                {
                    stop += 0.1f;

                    enemyTemp -= enemyAccelerate;
                    enemyTemp = enemyTemp > 0 ? enemyTemp : 0;
                    enemyMove.x = moveVec.x * enemyTemp;
                    enemyMove.y = gravity;
                    enemyMove.z = moveVec.z * enemyTemp;
                    Character.Move(enemyMove * Time.deltaTime);


                    if (stop > stopTime)
                    {
                        move = 0;
                        stop = 0;
                        moveAngle = -1;
                        enemyTemp = 0;
                    }
                }
                //Debug.Log("enemy:"+enemyTemp);
                
            }
            else
            {
                //空中時は停止
                enemyMove.x = 0;
                enemyMove.y = gravity;
                enemyMove.z = 0;
                Character.Move(enemyMove * Time.deltaTime);
            }

        }
        
    }

    //移動方向の設定
    public void SetVecter(Vector3 setVec)
    {
        enemyVector = setVec;
    }

    //移動スピードの設定
    public void SetSpeed(float setSpeed)
    {
        enemySpeed = setSpeed;
    }

    public void SetPreserveSpeed()
    {
        enemySpeed = preserveEnemySpeed;
    }

    //淵で止まる処理
    public void IsAbyss()
    {
        isAbyss = true;
    }

    //淵で止まらない処理
    public void IsAbyssFalse()
    {
        isAbyss = false;
    }

    //移動方向の取得
    public Vector3 GetVector()
    {
        return enemyVector;
    }

    float radToDigree(float rad)
    {
        return rad / Mathf.PI * 180;
    }


    //角度からベクトル算出
    Vector3 AngleToVector(float angle)
    {
        float x = Mathf.Sin(angle);
        float z = Mathf.Cos(angle);

        return new Vector3(x, 0, z);
    }

    public void IsGravity()
    {
        isGravity =true;
    }

    public void IsGravityFalse()
    {
        isGravity = false;
    }

    public void SetEnemyPattern(int pattern)
    {
        //0:近接 1:遠隔
        enemyPattern = pattern;
    }
}

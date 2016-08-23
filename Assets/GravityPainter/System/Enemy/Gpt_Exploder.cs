using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gpt_Exploder : MonoBehaviour {

    private Gpt_EnemyGravityManeger EnemyGravityManeger;
    private GameObject ManegerObject;
    private GameObject YukaManegerObject;
    private int scale=1;
    private Gpt_Exploder targetExploder;

    private Gpt_YukaManager YukaManager;
    public float explodeArea=20f;
    //爆破エフェクト
    public GameObject Explosion_red;
    public GameObject Explosion_blue;
    public GameObject Explosion_yellow;

    //上昇エフェクト
    public GameObject Explosion_dust;

    //下降エフェクト
    public GameObject Explosion_Flare;

    private bool isDust;
    private bool isFlare;

    public Rigidbody rigid;
    //バグ防止
    private float bug_time;

    private bool isExplode;

    //爆発後フラグ
    private bool isAfterExplode;

    //爆発モーションフラグ1
    private bool isExplodeMotion1;

    //死亡フラグ
    private bool isDestroy;
    //色設定
    private int color;

    //爆発内の敵の数
    private int enemyNum;
    private int preserveEnemyNum;

    //重力フラグ
    private bool isGravity;

    //爆発起動後のエネミーの数
    private int explodeEnemyNum;

    //爆発上昇のスピード
    private float explodeUpSpeed;
    //爆発下降のスピード
    private float explodeUnderSpeed;

    //爆発箇所の高さ調整
    public float explodeY=4f;
    // Use this for initialization
    void Start() {
        ManegerObject = GameObject.Find("GravityManeger");
        YukaManegerObject = GameObject.Find("YukaManager");
        YukaManager = YukaManegerObject.GetComponent<Gpt_YukaManager>();
        EnemyGravityManeger = ManegerObject.GetComponent<Gpt_EnemyGravityManeger>();
        EnemyGravityManeger.AddExplodeList(this);
        bug_time = Random.Range(0, 4f); 
    }
	// Update is called once per frame
	void Update () {
        bug_time += 0.1f;

        //Debug.Log("before:"+enemyNum);

        

        if (isExplode)
        {
            if (!isAfterExplode)
            {
                //爆発モーション（下降）
                if (!isExplodeMotion1)
                {
                    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + explodeUpSpeed, this.transform.position.z);
                    if (!isDust)
                    {
                        if (Explosion_dust != null)
                        {
                            Instantiate(Explosion_dust, this.transform.position, Quaternion.identity);
                        }
                        
                        isDust = true;
                    }
                }else
                {
                    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - explodeUnderSpeed, this.transform.position.z);
                    if (!isFlare)
                    {
                        Instantiate(Explosion_Flare, this.transform.position-new Vector3(0,4f,0), Quaternion.identity);
                        isFlare = true;
                    }
                    
                }

            } else if (!isDestroy)
            {
                //爆発エフェクトの位置調整
                Vector3 explode = new Vector3(0, explodeY, 0);
                switch (color)
                {
                    case 1:
                        Instantiate(Explosion_red, this.transform.position + explode, Quaternion.identity);
                        YukaManager.DoExplode(color, this.transform.position + explode, explodeArea);
                        EnemyGravityManeger.IsExplodeWave();//爆風ダメージ
                        isDestroy = true;
                        break;
                    case 2:
                        Instantiate(Explosion_blue, this.transform.position + explode, Quaternion.identity);
                        YukaManager.DoExplode(color, this.transform.position + explode, explodeArea);
                        EnemyGravityManeger.IsExplodeWave();
                        isDestroy = true;
                        break;
                    case 3:
                        Instantiate(Explosion_yellow, this.transform.position+explode, Quaternion.identity);
                        YukaManager.DoExplode(color, this.transform.position + explode, explodeArea);
                        EnemyGravityManeger.IsExplodeWave();
                        isDestroy = true;
                        break;
                }
            }
        }

        if (isDestroy)
        {
            
            SetDelayDestroy(0f);
        }

        //爆発オブジェクト内に敵がいなかったら削除
        enemyNum = preserveEnemyNum;
        if (enemyNum == 0 && isGravity)
        {
            //if (!isExplode&&explodeEnemyNum==0) SetDestroy();
        }
        preserveEnemyNum = 0;

        //Debug.Log("after:" + enemyNum);

        isGravity = true;
	}

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "GravityZone")
        {
            targetExploder=collision.gameObject.GetComponent<Gpt_Exploder>();
            //Debug.Log(targetExploder.transform.position);
            //自分と相手を削除し、中間の間に新しい爆発オブジェクトを作成
            //自分の相手の座標からどちらかにのみ爆発オブジェクト生成
            if (targetExploder.GetColor()==color)
            {
                float Debug_A = this.transform.position.x * this.transform.position.y * this.transform.position.z;
                float Debug_B = collision.gameObject.transform.position.x * collision.gameObject.transform.position.y * collision.gameObject.transform.position.z;
                if (this.transform.position.x * this.transform.position.y * this.transform.position.z >
                    collision.gameObject.transform.position.x * collision.gameObject.transform.position.y * collision.gameObject.transform.position.z)
                {
                    //Vector3 median = medianPosition(this.transform.position, collision.gameObject.transform.position);
                    //SetPosition(median);

                    //Debug.Log("1ax:" + this.transform.position.x + "y:" + this.transform.position.y + "z:" + this.transform.position.z);
                    //Debug.Log("1bx:" + collision.gameObject.transform.position.x + "y:" + collision.gameObject.transform.position.y + "z:" + collision.gameObject.transform.position.z);
                    //Debug.Log("1a:" + Debug_A);
                    //Debug.Log("1b:" + Debug_B);
                    targetExploder.SetDestroy();
                }
                else
                {
                    //Debug.Log("2ax:" + this.transform.position.x + "y:" + this.transform.position.y + "z:" + this.transform.position.z);
                    //Debug.Log("2bx:" + collision.gameObject.transform.position.x + "y:" + collision.gameObject.transform.position.y + "z:" + collision.gameObject.transform.position.z);
                    //Debug.Log("2a:"+Debug_A);
                    //Debug.Log("2b:"+Debug_B);
                    SetDestroy();
                }
            }
        }
    }
    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Gpt_Enemy targetEnemy = collision.gameObject.GetComponent<Gpt_Enemy>();
            if (!isExplode&&targetEnemy.GetTouch()&&targetEnemy.GetColor()==color)
            {
                //座標が同じになるバグ回避
                float bug = Random.Range(0, 0.1f);
                Vector3 bugVec = new Vector3(bug, bug, bug);
                Vector3 VecY = new Vector3(0, 0.3f, 0);//位置調整
                SetPosition(collision.gameObject.transform.position+bugVec+VecY);
                preserveEnemyNum++;
            }
            else if (targetEnemy.GetColor() == color)//はがれる処理関係
            {
                preserveEnemyNum++;
            }

            
        }
    }

    

    //重心位置計算
    public Vector3 CalculatePosition(List<Vector3> position)
    {
        Vector3 CenterPositon=new Vector3(0,0,0);
        for (int i=0;i<position.Count;i++)
        {
            CenterPositon += position[i];
        }

        CenterPositon = CenterPositon / position.Count;

        return CenterPositon;
    }

    //二点間の中央点を求める
    public Vector3 medianPosition(Vector3 a, Vector3 b)
    {
        float temp_x = a.x + b.x;
        float temp_y = a.y + b.y;
        float temp_z = a.z + b.z;
        Vector3 temp_vec = new Vector3(temp_x/2,temp_y/2,temp_z/2);
        return temp_vec;
    }

    public void IsExplode()
    {
        //爆発前にエネミーの数を記録
        explodeEnemyNum = preserveEnemyNum;
        isExplode = true;
    }

    public Vector3 GetPosition()
    {
        return this.gameObject.transform.position;
    }

    public void SetPosition(Vector3 position)
    {
        this.gameObject.transform.position = position;
    }

    public void SetDelayDestroy(float delay)
    {
        Object.Destroy(this.gameObject, delay);
    }
    
    public void SetDestroy()
    {

        Object.Destroy(this.gameObject);
    }

    public int GetScale()
    {
        return scale; 
    }
    public void SetScale(int setScale)
    {
        scale = setScale ;
    }

    void OnDestroy()
    {
        EnemyGravityManeger.RemoveExplodeList(this);
    }

    public void SetColor(int setcolor)
    {
        color = setcolor;
    }

    public int GetColor()
    {
        return color;
    }

    public void IsAfterExplode()
    {
        isAfterExplode = true;
    }

    public void IsExplodeMotion1()
    {
        isExplodeMotion1 = true;
    }

    public int GetEnemyNum()
    {
        return enemyNum;
    }

    public void SetUpSpeed(float speed)
    {
        explodeUpSpeed = speed;
    }

    public void SetUnderSpeed(float speed)
    {
        explodeUnderSpeed = speed;
    }
}

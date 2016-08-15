using UnityEngine;
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
    //爆発フラグ
    private bool isExplode;

    //爆発後フラグ
    private bool isAfterExplode;

    //爆発時間
    private float explodeTime = 0f;

    private float gravityTime;

    private float motionTime1 ;
    private float motionTime2 ;

    private float preserveX;
    private float preserveZ;

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
        Debug.Log(EnemyGravityManeger.ListIndex(this));
    }

    void OnCollisionEnter(Collision collision)
    {
        
        //重力フラグ時、敵同士の接触で爆発オブジェクト生成
        if (gravityFlag&&!touchFlag)
        {
            
            if (collision.gameObject.tag == "Enemy")
            {
                
                for (int aIndex = 0; aIndex < collision.contacts.Length; ++aIndex)
                {
                    //自分の相手の座標からどちらかにのみ爆発オブジェクト生成
                    if (this.transform.position.x * this.transform.position.z > collision.gameObject.transform.position.x * collision.gameObject.transform.position.z)
                    {
                        Vector3 trans = collision.contacts[aIndex].point;
                        Instantiate(exploder, trans, Quaternion.identity);
                        touchFlag = true;
                    }
                }
                
            }

        }
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "GravityZone")
        {
            Debug.Log(this.transform.position);
            exploderPosition=collision.gameObject.transform.position;
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
        }
        
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
            EnemyAttack.StopAttack();
        }

        gravityFlag = true;

        if(!isExplode){
            Speed(0);
            //始めは小刻みに震える
            if (gravityTime < 3f)
            {
                float x_temp = Random.Range(-0.15f, 0.15f);
                float z_temp = Random.Range(-0.15f, 0.15f);
                this.transform.position=new Vector3(preserveX+x_temp,this.transform.position.y,
                    preserveZ+ z_temp);
            }
            else
            {
                Character.enabled = false;
                
                rigid.isKinematic = false;
                rigid.useGravity = true;
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
            motionTime1 += 0.1f;
            motionTime2 += 0.1f;
            //collider.isTrigger=true;
            Vector3 exVec = ( exploderPosition - this.transform.position ).normalized;
            rigid.AddForce(exVec * 3f, ForceMode.VelocityChange);
            //this.transform.position = exploderPosition;
            if (motionTime1 < 3f)
            {

                //gravityVec.y = 0f;
                var vec = Quaternion.Euler(0f, 90f, 0f) * exVec;
                rigid.AddForce(vec * 2f, ForceMode.VelocityChange);

            }
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
        Object.Destroy(this.gameObject, 4f);
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
}

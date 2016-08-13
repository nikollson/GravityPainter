using UnityEngine;
using System.Collections;

public class Gpt_Enemy : MonoBehaviour {

    public Gpt_EnemyMove EnemyMove;
    public Gpt_EnemyColor EnemyColor;
    public Gpt_EnemyAttack EnemyAttack;
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
    public int hitPoint = 1;
    float test;

    //爆発物オブジェクト
    public GameObject exploder;
    //爆発フラグ
    private bool isExplode;

    //爆発後フラグ
    private bool isAfterExplode;

    //爆発時間
    private float explodeTime = 0f;

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
        if (gravityFlag)
        {
            
            if (collision.gameObject.tag == "Enemy")
            {
                Debug.Log("aaaa");
                for (int aIndex = 0; aIndex < collision.contacts.Length; ++aIndex)
                {
                    Vector3 trans = collision.contacts[aIndex].point;
                    //Instantiate(exploder, trans, Quaternion.identity);

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
        
    }



    public void Speed(float s)
    {

        EnemyMove.SetSpeed(s);
    }


    //EnemyManagerからの参照
    public void SetGravity(Vector3 gravityVec)
    {
        Character.enabled=false;
        gravityFlag = true;
        Speed(0);
        rigid.isKinematic = false;
        rigid.useGravity = true;
        

        if(!isExplode){
            rigid.AddForce(-gravityVec * gravity, ForceMode.VelocityChange);
        }
        else
        {
        //爆発処理
            if (!isAfterExplode)
            {
                //現状固定の角度とスピード（後で変えるかも）
                gravityVec.y = 0.9f;
                rigid.AddForce(gravityVec * 20f, ForceMode.VelocityChange);
            }
            isAfterExplode = true;
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
        Object.Destroy(this.gameObject, 1f);
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

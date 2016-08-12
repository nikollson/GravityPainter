using UnityEngine;
using System.Collections;

public class Gpt_Enemy : MonoBehaviour {

    public Gpt_EnemyMove EnemyMove;
    public Gpt_EnemyColor EnemyColor;
    private GameObject ManegerObject;
    public Rigidbody rigid;
    private Gpt_EnemyGravityManeger EnemyGravityManeger;

    private CharacterController Character;

    //重力エネルギー
    public float gravity=0.13f;

    //重力フラグ
    private bool gravityFlag;
    public int hitPoint = 1;
    float test;

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
        EnemyMove.SetVecter(this.transform.forward);
        int temp= Random.Range(1, 3); 
        EnemyColor.SetColor(temp);
        
        Character = this.GetComponent<CharacterController>();
        Debug.Log(EnemyGravityManeger.ListIndex(this));
    }

    float temp;
    // Update is called once per frame
    void Update () {

        temp+=0.1f;
        if (temp > 10f) {
            IsExplode();
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
}

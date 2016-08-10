using UnityEngine;
using System.Collections;

public class Gpt_Enemy : MonoBehaviour {

    public Gpt_EnemyMove EnemyMove;
    public Gpt_EnemyColor EnemyColor;
    private GameObject ManegerObject;
    public Rigidbody rigid;
    private Gpt_EnemyGravityManeger EnemyGravityManeger;

    private CharacterController Character;


    private float gravity=0.13f;
    private bool gravityFlag;
    public int hitPoint = 1;
    float test;


    // Use this for initialization
    void Start () {
        //シーン上にあるGravityManegerを自動取得する。（プレハブから一つ生成する。）
        ManegerObject = GameObject.Find("GravityManeger");
        EnemyGravityManeger = ManegerObject.GetComponent<Gpt_EnemyGravityManeger>();
        EnemyGravityManeger.AddEnemyList(this);
        Speed(2f);
        EnemyMove.SetVecter(this.transform.forward);
        EnemyColor.SetColor(1);

        Character = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update () {

        test+=0.01f;
        Vector3 testvec=AngleToVector(test);
        //EnemyMove.SetVecter(testvec);
    }

    //角度からベクトル算出
    Vector3 AngleToVector(float angle)
    {
        float x=Mathf.Sin(angle);
        float z=Mathf.Cos(angle);

        return new Vector3(x,0,z);
    }


    public void Speed(float s)
    {

        EnemyMove.SetSpeed(s);
    }


    public void SetGravity(Vector3 gravityVec)
    {
        Character.enabled=false;
        gravityFlag = true;
        Speed(0);
        rigid.isKinematic = false;
        rigid.useGravity = true;
        rigid.AddForce(-gravityVec * gravity, ForceMode.VelocityChange);
    }
}

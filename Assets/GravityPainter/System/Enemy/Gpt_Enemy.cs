using UnityEngine;
using System.Collections;

public class Gpt_Enemy : MonoBehaviour {

    public Gpt_EnemyMove EnemyMove;
    public Gpt_EnemyColor EnemyColor;
    private GameObject ManegerObject;
    public Rigidbody rigid;
    private Gpt_EnemyGravityManeger EnemyGravityManeger;

    private CharacterController Character;


    public float gravity=0.13f;
    private bool gravityFlag;
    public int hitPoint = 1;
    float test;


    // Use this for initialization
    void Start () {
        //シーン上にあるGravityManegerを自動取得する。（プレハブから一つ生成する。）
        ManegerObject = GameObject.Find("GravityManeger");
        EnemyGravityManeger = ManegerObject.GetComponent<Gpt_EnemyGravityManeger>();
        EnemyGravityManeger.AddEnemyList(this);
        //Speed(2f);
        EnemyMove.SetVecter(this.transform.forward);
        int temp= Random.Range(0, 3); 
        EnemyColor.SetColor(temp);
        
        Character = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update () {

        
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

        EnemyMove.IsGravity();
    }

    public int GetColor()
    {
        return EnemyColor.GetColor();
    }
}

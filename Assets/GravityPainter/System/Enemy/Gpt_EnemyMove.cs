using UnityEngine;
using System.Collections;

public class Gpt_EnemyMove : MonoBehaviour {
    
    public float enemySpeed = 5.0f;
    public new Rigidbody rigidbody;

    private CharacterController Character;
    public float friction = 0.6f;

    private Vector3 enemyVector = new Vector3();
    Transform myTransform;

    float GRAVITY = 9.8f;
    private float gravity;


    //移動回転角
    private float moveAngle;

    // Use this for initialization
    void Start()
    {
        Character = GetComponent<CharacterController>();

        myTransform = transform;
        enemyVector= new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Character.isGrounded)
        {
        }
        else
        {
            gravity -= GRAVITY * Time.deltaTime;
        }

        moveAngle += 0.01f;
        Vector3 testvec = AngleToVector(moveAngle);
        //EnemyMove.SetVecter(testvec);


        Vector3 enemyMove;

        enemyMove.x= enemyVector.x * enemySpeed;
        enemyMove.y =gravity;
        enemyMove.z = enemyVector.z * enemySpeed;
        // 移動
        

        Character.Move(enemyMove * Time.deltaTime);
        gravity = enemyMove.y;
        //移動方向の取得
        float angle = Mathf.Atan2(enemyMove.z, enemyMove.x);
        //移動方向に回転
        this.transform.rotation = Quaternion.Euler(new Vector3(0, radToDigree(-angle), 0));
        
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
}

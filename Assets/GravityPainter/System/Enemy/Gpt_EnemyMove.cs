using UnityEngine;
using System.Collections;

public class Gpt_EnemyMove : MonoBehaviour {
    
    public float enemySpeed = 5.0f;
    public new Rigidbody rigidbody;

    public float friction = 0.6f;

    private Vector3 enemyVector = new Vector3();
    Transform myTransform;

    float GRAVITY = 9.8f;
    private float gravity;

    // Use this for initialization
    void Start()
    {
        myTransform = transform;
        enemyVector= new Vector3(0, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        gravity -= GRAVITY * Time.deltaTime;

        Vector3 enemyMove;

        enemyMove.x= enemyVector.x * enemySpeed;
        enemyMove.y =gravity;
        enemyMove.z = enemyVector.z * enemySpeed;
        // 移動

        Vector3 power = enemyMove - friction * rigidbody.velocity;
        rigidbody.AddForce(power, ForceMode.VelocityChange);

        //character.Move(enemyMove * Time.deltaTime);
        gravity = enemyMove.y;
        //移動方向の取得
        float angle = Mathf.Atan2(enemyVector.z, enemyVector.x);
        //移動方向に回転
        this.transform.rotation = Quaternion.Euler(new Vector3(0, radToDigree(-angle), 0));

        //移動量の初期化
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
}

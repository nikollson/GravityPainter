using UnityEngine;
using System.Collections;

public class Gpt_Enemy : MonoBehaviour {

    public Gpt_EnemyMove EnemyMove;
    public Gpt_EnemyColor EnemyColor;

    float test;
    
    // Use this for initialization
    void Start () {
        EnemyMove.SetSpeed(2f);
        EnemyMove.SetVecter(transform.forward);
    }

    // Update is called once per frame
    void Update () {

        test+=0.01f;
        Vector3 testvec=AngleToVector(test);

        EnemyMove.SetVecter(testvec);
    }

    //角度からベクトル算出
    Vector3 AngleToVector(float angle)
    {
        float x=Mathf.Sin(angle);
        float z=Mathf.Cos(angle);

        return new Vector3(x,0,z);
    }
}

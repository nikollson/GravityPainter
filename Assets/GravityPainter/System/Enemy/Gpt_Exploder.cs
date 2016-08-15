using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gpt_Exploder : MonoBehaviour {

    private Gpt_EnemyGravityManeger EnemyGravityManeger;
    private GameObject ManegerObject;
    private int scale=1;
    private Gpt_Exploder targetExploder;

    public Rigidbody rigid;
    //バグ防止
    private float bug_time;

    private bool isExplode;

    // Use this for initialization
    void Start() {
        ManegerObject = GameObject.Find("GravityManeger");
        EnemyGravityManeger = ManegerObject.GetComponent<Gpt_EnemyGravityManeger>();
        EnemyGravityManeger.AddExplodeList(this);
        bug_time = Random.Range(0, 4f); 
    }
	// Update is called once per frame
	void Update () {
        bug_time += 0.1f;

        if (isExplode)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.01f, this.transform.position.z);
        }
	}

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "GravityZone")
        {
            targetExploder=collision.gameObject.GetComponent<Gpt_Exploder>();
            //Debug.Log(targetExploder.transform.position);
            //自分と相手を削除し、中間の間に新しい爆発オブジェクトを作成
            //自分の相手の座標からどちらかにのみ爆発オブジェクト生成
            if (this.transform.position.x * this.transform.position.y * this.transform.position.z >
                    collision.gameObject.transform.position.x* collision.gameObject.transform.position.y * collision.gameObject.transform.position.z)
            {
                Vector3 median = medianPosition(this.transform.position, collision.gameObject.transform.position);
                SetPosition(median);
                targetExploder.SetDestroy();
            }
            else
            {
                SetDestroy();
            }
        }
    }
    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (!isExplode)
            {
                SetPosition(collision.gameObject.transform.position);
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
        isExplode = true;
    }

    public void SetPosition(Vector3 position)
    {
        this.gameObject.transform.position = position;
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
        //EnemyGravityManeger.RemoveExplodeList(this);
    }
}

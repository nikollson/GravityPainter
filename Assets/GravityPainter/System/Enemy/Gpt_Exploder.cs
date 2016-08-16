using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gpt_Exploder : MonoBehaviour {

    private Gpt_EnemyGravityManeger EnemyGravityManeger;
    private GameObject ManegerObject;
    private int scale=1;
    private Gpt_Exploder targetExploder;

    public GameObject Explosion_red;
    public GameObject Explosion_blue;
    public GameObject Explosion_yellow;

    public Rigidbody rigid;
    //バグ防止
    private float bug_time;

    private bool isExplode;

    //爆発後フラグ
    private bool isAfterExplode;

    //死亡フラグ
    private bool isDestroy;
    //色設定
    private int color;

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
            if (!isAfterExplode)
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.06f, this.transform.position.z);

            } else if (!isDestroy)
            {
                switch (color)
                {
                    case 1:
                        Instantiate(Explosion_red, this.transform.position, Quaternion.identity);
                        isDestroy = true;
                        break;
                    case 2:
                        Instantiate(Explosion_blue, this.transform.position, Quaternion.identity);
                        isDestroy = true;
                        break;
                    case 3:
                        Instantiate(Explosion_yellow, this.transform.position, Quaternion.identity);
                        isDestroy = true;
                        break;
                }
            }
        }

        if (isDestroy)
        {
            
            SetDelayDestroy();
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
            if (targetExploder.GetColor()==color)
            {
                float Debug_A = this.transform.position.x * this.transform.position.y * this.transform.position.z;
                float Debug_B = collision.gameObject.transform.position.x * collision.gameObject.transform.position.y * collision.gameObject.transform.position.z;
                if (this.transform.position.x * this.transform.position.y * this.transform.position.z >
                    collision.gameObject.transform.position.x * collision.gameObject.transform.position.y * collision.gameObject.transform.position.z)
                {
                    //Vector3 median = medianPosition(this.transform.position, collision.gameObject.transform.position);
                    //SetPosition(median);

                    Debug.Log("1ax:" + this.transform.position.x + "y:" + this.transform.position.y + "z:" + this.transform.position.z);
                    Debug.Log("1bx:" + collision.gameObject.transform.position.x + "y:" + collision.gameObject.transform.position.y + "z:" + collision.gameObject.transform.position.z);
                    Debug.Log("1a:" + Debug_A);
                    Debug.Log("1b:" + Debug_B);
                    targetExploder.SetDestroy();
                }
                else
                {
                    Debug.Log("2ax:" + this.transform.position.x + "y:" + this.transform.position.y + "z:" + this.transform.position.z);
                    Debug.Log("2bx:" + collision.gameObject.transform.position.x + "y:" + collision.gameObject.transform.position.y + "z:" + collision.gameObject.transform.position.z);
                    Debug.Log("2a:"+Debug_A);
                    Debug.Log("2b:"+Debug_B);
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
                SetPosition(collision.gameObject.transform.position+bugVec);
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

    public void SetDelayDestroy()
    {
        Object.Destroy(this.gameObject, 6f);
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
}

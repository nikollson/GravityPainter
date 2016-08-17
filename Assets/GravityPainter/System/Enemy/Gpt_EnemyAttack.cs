using UnityEngine;
using System.Collections;

public class Gpt_EnemyAttack : MonoBehaviour {

    //雑魚パターン(0:近接 1:遠隔)
    private int enemyPattern = 0;

    private bool isAttack;
    private bool continueAttack;

    public int attackSpeed=3;
    private int attack=0;

    //ビームを打つ間隔
    public float beamTime;
    private float beam;

    private float motionTime1;
    private float motionTime2;

    //ビームオブジェクト
    public GameObject beamObject;

    //近接オブジェクト
    public GameObject proxObject;
    private Collider proxCollider;

    public Quaternion firstProxRotation;
    public Vector3 firstProxPosition;

	// Use this for initialization
	void Start () {
        firstProxRotation = proxObject.transform.rotation;
        firstProxPosition = proxObject.transform.position;
        //proxCollider = proxCollider.GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
        
        //攻撃フラグが立つときにアクション
        if (isAttack)
        {
            if (enemyPattern == 0)
            {
                attack++;
                //Debug.Log("aa:"+attackSpeed);
                if (attack%attackSpeed==0)
                {
                    Debug.Log("atacck");
                    motionTime1 += 0.8f;

                    if (motionTime1 >= 8 || motionTime2 > 0)
                    {
                        motionTime2 += 0.1f;
                        motionTime1 -= 6f;
                        //proxCollider.enabled = true;
                    }

                    if (motionTime2 > 1)
                    {
                        Debug.Log("motion1:" + motionTime1);
                        StopAttack();
                        //proxCollider.enabled = false;
                        //proxObject.transform.RotateAround(this.transform.position, this.transform.right, -130);
                        proxObject.transform.position = this.transform.position + new Vector3(0, 7f*this.transform.position.y/8, 0);
                        proxObject.transform.rotation = this.transform.rotation;
                    }
                    //仮モーション

                    proxObject.transform.RotateAround(this.transform.position, this.transform.right, -motionTime1);
                }
            }
            else if (enemyPattern == 1)
            {
                beam+=0.1f;
                if (beam > beamTime)
                {
                    Instantiate(beamObject, this.transform.position, this.transform.rotation);
                    beam = 0;
                }
                
            }

        }
        
	}

    public void SetEnemyPattern(int pattern)
    {
        enemyPattern = pattern;
    }

    public bool GetAttack()
    {
        return isAttack;
    }

    public void IsAttack()
    {
        isAttack = true;
    }

    public void StopAttack()
    {
        isAttack = false;
        beam = 0;
        motionTime1 = 0;
        motionTime2 = 0;
    }
}

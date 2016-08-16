using UnityEngine;
using System.Collections;

public class Gpt_EnemyAttack : MonoBehaviour {

    //雑魚パターン(0:近接 1:遠隔)
    private int enemyPattern = 0;

    private bool isAttack;

    //ビームを打つ間隔
    public float beamTime;
    private float beam;

    private float motionTime1;
    private float motionTime2;

    //ビームオブジェクト
    public GameObject beamObject;
    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
        //攻撃フラグが立つときにアクション
        if (isAttack)
        {
            if (enemyPattern == 0)
            {
                Debug.Log("atacck");
                motionTime1 += 2f;
                
                if(motionTime1 >= 45||motionTime2>=1)
                {
                    motionTime2 += 1f;
                    motionTime1 -= 40f;
                }
                this.transform.Rotate(this.transform.up, 212);
                
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

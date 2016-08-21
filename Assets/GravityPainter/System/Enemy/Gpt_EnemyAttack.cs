using UnityEngine;
using System.Collections;

public class Gpt_EnemyAttack : MonoBehaviour {

    //雑魚パターン(0:近接 1:遠隔)
    private int enemyPattern = 0;

    private bool isAttack;
    private bool continueAttack;

    public int attackSpeed=1;
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
    private GameObject prox;
    private Collider proxCollider;
    private Renderer proxRenderer;

    public Quaternion firstProxRotation;
    public Vector3 firstProxPosition;

    private bool Damage;

    //攻撃モーションに入ってから攻撃判定に入るまでの時間
    public float startAttackTime=0.6f;
    //攻撃判定が消える時間
    public float duringAttackTime=1.5f;
    //攻撃判定後の時間
    public float endAttackTime =1f;
    //攻撃後の再攻撃するまでの時間
    public float coolTime = 2f;
    private float cool;

    public bool CanAttack{get;private set;}

    private float attackTime;
    private float jump;
    private Vector3 jumpVec;
    //起き上がっている最中の判定
    public bool isOki { get; set; }
    //攻撃中判定（アニメ用）
    public bool isAttack_ { get; set; }

	// Use this for initialization
	void Start () {
        firstProxRotation = proxObject.transform.rotation;
        firstProxPosition = proxObject.transform.position;
        //prox = GameObject.Find("/AttackArea");
        proxCollider = proxObject.GetComponent<Collider>();
        proxRenderer = proxObject.GetComponent<Renderer>();
        proxCollider.enabled = false;
        proxRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update () {

        cool+=0.1f;
        if(cool>coolTime){
            CanAttack=true;
        }

        //Debug.Log("Attack:"+isAttack);
        //攻撃フラグが立つときにアクション
        if (isAttack)
        {
            if (enemyPattern == 0)
            {
                attackTime+=0.1f;
                jump+=0.1f;
                Debug.Log(attackTime);
                if (attackTime > startAttackTime)
                {
                    //proxRenderer.enabled = true;           
                    proxCollider.enabled = true;
                    //if(jumpTime<jump)
                    jumpVec=new Vector3(0,0.1f,0);
                    this.transform.position=this.transform.position+jumpVec;
                    isAttack_ = true;
                }

                if (attackTime > startAttackTime + duringAttackTime)
                {
                    //proxRenderer.enabled = false;
                    proxCollider.enabled = false;
                    isAttack_ = false;
                    isOki = true;
                }

                if (attackTime > startAttackTime + duringAttackTime+endAttackTime)//アニメーション開始
                {
                    isOki = false;
                    StopAttack();

                }



                    
                    
                ////Debug.Log("aa:"+attackSpeed);
                //if (attack%attackSpeed==0)
                //{
                //    //Debug.Log("atacck");
                //    motionTime1 += 0.8f;

                //    if (motionTime1 >= 8 || motionTime2 > 0)
                //    {
                //        motionTime2 += 0.1f;
                //        motionTime1 -= 6f;
                //        proxCollider.enabled = true;
                //    }

                //    if (motionTime2 > 1)
                //    {
                //        Debug.Log("motion1:" + motionTime1);
                //        StopAttack();
                //        proxCollider.enabled = false;
                //        //proxObject.transform.RotateAround(this.transform.position, this.transform.right, -130);
                //        proxObject.transform.position = this.transform.position + new Vector3(0, 7f*this.transform.position.y/8, 0);
                //        proxObject.transform.rotation = this.transform.rotation;
                //    }
                //    //仮モーション

                //    //proxObject.transform.RotateAround(this.transform.position, this.transform.right, -motionTime1);
                //}
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

        }else
        {
            proxCollider.enabled = false;
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
        proxCollider.enabled = false;
        proxRenderer.enabled = false;
        beam = 0;
        motionTime1 = 0;
        motionTime2 = 0;
        attackTime = 0;
        jump = 0;
        cool=0;
        CanAttack=false;
    }

    
}

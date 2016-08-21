using UnityEngine;
using System.Collections;

public class Gpt_EnemyNavigator : MonoBehaviour {


    public Transform target;
    public NavMeshAgent navigator;
    private Gpt_Enemy EnemyClass;
    private Gpt_EnemyAttack EnemyAttack;
    private CapsuleCollider capCollider;
    private float randomX;
    private float randomZ;
    private Vector3 randomVector;
	// Use this for initialization
	void Start () {
        EnemyClass = this.GetComponent<Gpt_Enemy>();
        target = GameObject.FindWithTag("Player").transform;
        capCollider = this.GetComponent<CapsuleCollider>();
        EnemyAttack = this.GetComponent<Gpt_EnemyAttack>();
        //ランダムでプレイヤーの前後を狙うようにする。
        randomX = Random.Range(-5f, 5f);
        randomZ = Random.Range(-5f, 5f);
        randomVector = new Vector3(randomX, 0, randomZ);
	}
	
	// Update is called once per frame
	void Update () {

        ////吹っ飛ぶたびに更新
        //if (EnemyClass.GetGravity())
        //{
        //    randomX = Random.Range(-5f, 5f);
        //    randomZ = Random.Range(-5f, 5f);
        //    randomVector = new Vector3(randomX, 0, randomZ);
        //}

        if (!EnemyAttack.GetAttack())
        {

        }

        if (navigator.enabled)
        {
            navigator.SetDestination(target.transform.position);
        }
        
	}

    //void OnCollisionEnter(Collision collision)
    //{
    //    //何故かナビゲーションAgentだとあたり判定がおかしくなるので手動で補正
    //    if(!EnemyClass.GetGravity()){
    //        Debug.Log("Enemyyyy");
    //        if (collision.gameObject.tag == "Enemy")
    //        {
    //            for (int aIndex = 0; aIndex < collision.contacts.Length; ++aIndex)
    //            {
    //                Vector3 target = collision.contacts[aIndex].point - this.transform.position;
    //                target = target.normalized;
    //                this.transform.position = this.transform.position + (target * capCollider.radius);
    //                Debug.Log(collision.contacts[aIndex].point);
    //            }
    //        }
    //    }
        
    //}
}

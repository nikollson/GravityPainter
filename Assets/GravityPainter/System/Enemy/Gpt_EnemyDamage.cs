using UnityEngine;
using System.Collections;

public class Gpt_EnemyDamage : MonoBehaviour
{
    private Gpt_EnemyAttack EnemyAttack;
    public HitManager attackCollider;
    public Transform hitRoot;

    public int damage = 3;

    string playerTag = "Player";

    private bool hitAttack;

    void Start()
    {
        EnemyAttack = this.transform.parent.parent.parent.parent.parent.GetComponent<Gpt_EnemyAttack>();
    }

    void Update()
    {
        foreach (var a in attackCollider.HitColliders)
        {
            //ダブルチェック
            if (a.tag == playerTag&&hitAttack)
            {
                var playerState = Gpt_ParentTracker.Track<Gpt_PlayerState>(a.gameObject);
                if (playerState != null)
                {
                    if (EnemyAttack.GetAttack())
                    {
                        Vector3 position = this.transform.position;
                        if (hitRoot != null) position = hitRoot.transform.position;
                        playerState.AddHPDamage_Attack(damage, position);
                    }
                }
            }
            hitAttack = false;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        
        if (collision.gameObject.tag == "Player")
        {
            hitAttack = true;
            //Debug.Log("!!!!!!!");
        }
    }

    void OnTriggerExit(Collider collision)
    {
        //Debug.Log("??????");
        if (collision.gameObject.tag == "player")
        {
            hitAttack = false;
        }
    }
}

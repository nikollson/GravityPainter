using UnityEngine;
using System.Collections;

public class Gpt_EnemyDamage : MonoBehaviour
{
    private Gpt_EnemyAttack EnemyAttack;
    public HitManager attackCollider;
    public int damage = 3;

    string playerTag = "Player";

    void Start()
    {
        EnemyAttack = this.transform.parent.parent.parent.parent.parent.GetComponent<Gpt_EnemyAttack>();
    }

    void Update()
    {
        foreach (var a in attackCollider.HitColliders)
        {
            if (a.tag == playerTag)
            {
                var playerState = Gpt_ParentTracker.Track<Gpt_PlayerState>(a.gameObject);
                if (playerState != null)
                {
                    if (EnemyAttack.GetAttack())
                    {
                        playerState.AddHPDamage(damage);
                    }

                }
            }
        }
    }
}

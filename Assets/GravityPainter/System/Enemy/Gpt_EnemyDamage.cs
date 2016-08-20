using UnityEngine;
using System.Collections;

public class Gpt_EnemyDamage : MonoBehaviour
{
    private Gpt_EnemyAttack EnemyAttack;
    public HitManager attackCollider;
    public int damage = 3;

    public Transform hitRoot;

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
                    Vector3 position = this.transform.position;
                    if (hitRoot != null) position = hitRoot.transform.position;
                    playerState.AddHPDamage_Attack(damage, position);
                }
            }
        }
    }
}

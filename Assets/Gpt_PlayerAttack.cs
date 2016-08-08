using UnityEngine;
using System.Collections;

public class Gpt_PlayerAttack : MonoBehaviour
{
    public Gpt_Player player;
    public HitManager attackHitManager;
    bool isAttacking = false;

    public float attackEndTime = 0.2f;
    public float secondAttackTime = 0.15f;
    float attackCount = 0.0f;

    public void StartAttack()
    {
        isAttacking = true;
        attackCount = 0;
    }

    public void EndAttack()
    {
        isAttacking = false;
    }


    public bool IsAttackEnd()
    {
        return attackCount > attackEndTime;
    }

    public bool CanSecondAttack()
    {
        return attackCount > secondAttackTime;
    }

    void Update()
    {
        attackCount += Time.deltaTime;
    }
}

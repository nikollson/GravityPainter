using UnityEngine;
using System.Collections;

public class Gpt_PlayerAttack : MonoBehaviour
{
    public Gpt_Player player;
    public HitManager attackHitManager;

    public float attackEndTime = 0.2f;
    public float secondAttackTime = 0.15f;
    float attackCount = 0.0f;

    public void StartAttack()
    {
        attackCount = 0;
    }

    public void EndAttack()
    {

    }


    public bool IsAttackEnd()
    {
        return attackCount > attackEndTime;
    }

    public bool CanSecondAttack()
    {
        return attackCount > secondAttackTime;
    }

    public void UpdateAttack()
    {
        attackCount += Time.deltaTime;
    }
}

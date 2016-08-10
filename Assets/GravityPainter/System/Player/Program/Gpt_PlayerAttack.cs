using UnityEngine;
using System.Collections;

public class Gpt_PlayerAttack : MonoBehaviour
{
    public Gpt_Player player;
    public HitManager attackHitManager;

    public float attackEndTime = 0.2f;
    public float secondAttackTime = 0.15f;
    float attackCount = 0.0f;
    int attackInputFrame_log = -1;

    public void StartAttack(int attackInputFrame)
    {
        attackCount = 0;
        attackInputFrame_log = attackInputFrame;
    }

    public void EndAttack()
    {

    }


    public bool IsAttackEnd()
    {
        return attackCount > attackEndTime;
    }

    public bool CanFirstAttack(int attackInputFrame)
    {
        return attackInputFrame_log != attackInputFrame;
    }

    public bool CanSecondAttack(int attackInputFrame)
    {
        return attackCount > secondAttackTime && attackInputFrame_log != attackInputFrame;
    }

    public void UpdateAttack()
    {
        attackCount += Time.deltaTime;
    }
}

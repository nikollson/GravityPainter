using UnityEngine;
using System.Collections;

public class Gpt_PlayerAttack : MonoBehaviour
{
    public HitManager attackHitManager;
    public new Rigidbody rigidbody;

    public float attackEndTime = 0.2f;
    public float secondAttackTime = 0.15f;

    public float friction = 20f;

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

        Vector3 fricionPower = -1 * friction * rigidbody.velocity;
        rigidbody.AddForce(Time.deltaTime * fricionPower, ForceMode.Acceleration);
    }
}

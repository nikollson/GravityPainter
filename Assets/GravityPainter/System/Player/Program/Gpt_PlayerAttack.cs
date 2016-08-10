using UnityEngine;
using System.Collections;

public class Gpt_PlayerAttack : MonoBehaviour
{
    public Gpt_PlayerUtillity playerUtillity;
    public HitManager attackHitManager;
    public new Rigidbody rigidbody;

    public float attackEndTime = 0.2f;
    public float secondAttackTime = 0.15f;

    public float friction = 20f;

    public float dashAttackSpeed = 20;
    public float normalAttackSpeed = 10;

    float attackCount = 0.0f;
    int attackInputFrame_log = -1;
    
    public enum ATTACK_MODE { NORMAL, DASH, ROTATE }

    public void StartAttack(ATTACK_MODE attackMode, int attackInputFrame)
    {
        attackCount = 0;
        attackInputFrame_log = attackInputFrame;
        if (attackMode == ATTACK_MODE.DASH) StartDashAttack();
        if (attackMode == ATTACK_MODE.NORMAL) StartNormalAttack();
    }

    void StartDashAttack()
    {
        Vector3 force = dashAttackSpeed * playerUtillity.GetAnalogpadMove();
        rigidbody.AddForce(force, ForceMode.VelocityChange);
    }

    void StartNormalAttack()
    {
        Vector3 force = normalAttackSpeed * playerUtillity.GetAnalogpadMove();
        rigidbody.AddForce(force, ForceMode.VelocityChange);
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

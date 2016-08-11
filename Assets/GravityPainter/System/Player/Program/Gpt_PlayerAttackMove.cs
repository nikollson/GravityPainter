using UnityEngine;
using System.Collections;

public class Gpt_PlayerAttackMove : MonoBehaviour
{
    public Gpt_PlayerUtillity playerUtillity;
    public new Rigidbody rigidbody;

    public float attackEndTime = 0.4f;
    public float secondAttackTime = 0.2f;
    
    public float dashAttackFriction = 400f;
    public float normalAttackFriction = 200f;
    public float directionChangeFriction = 0.8f;

    public float dashAttackSpeed = 4;
    public float normalAttackSpeed = 1;

    float attackCount = 0.0f;
    int attackInputFrame_log = -1;
    float currentDirection = 0;
    
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
        playerUtillity.LookAnalogpadDirction();
        AttackStartAddForce(dashAttackSpeed, directionChangeFriction, dashAttackFriction);
    }

    void StartNormalAttack()
    {
        playerUtillity.LookAnalogpadDirction();
        AttackStartAddForce(normalAttackSpeed, directionChangeFriction, normalAttackFriction);
    }

    void AttackStartAddForce(float power, float stop, float friction)
    {
        Vector3 force = power * playerUtillity.GetAnalogpadMove() - stop * rigidbody.velocity;
        rigidbody.AddForce(force, ForceMode.VelocityChange);
        currentDirection = friction;
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

        Vector3 fricionPower = -1 * currentDirection * rigidbody.velocity;
        rigidbody.AddForce(Time.deltaTime * fricionPower, ForceMode.Acceleration);
    }
}

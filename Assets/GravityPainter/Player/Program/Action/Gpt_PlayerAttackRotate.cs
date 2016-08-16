using UnityEngine;
using System.Collections;

public class Gpt_PlayerAttackRotate : MonoBehaviour
{
    public Gpt_PlayerUtillity playerUtillity;
    public new Rigidbody rigidbody;

    public float directionChange1Time = 0.2f;
    public float canAttackTime = 0.4f;
    public float endTime = 0.5f;

    public float startSpeed = 5f;
    public float movePower = 500f;
    public float moveFrictionXZ = 30f;

    float rotateCount = 0;
    bool directionChange1Done = false;

    public void StartRotate()
    {
        rotateCount = 0;
        directionChange1Done = false;

        StartDush();
    }

    public bool CanStartAttack()
    {
        return rotateCount > canAttackTime;
    }

    public bool IsEnd()
    {
        return rotateCount > endTime;
    }

    public void EndRotate()
    {

    }

    bool CanDirectionChange1()
    {
        return rotateCount > directionChange1Time;
    }

    void DoDirectionChange1()
    {
        StartDush();
    }

    void StartDush()
    {
        Vector3 forward = playerUtillity.GetAnalogpadMove();
        playerUtillity.LookAnalogpadDirction();
        Vector3 force = startSpeed * forward - rigidbody.velocity;
        rigidbody.AddForce(force, ForceMode.VelocityChange);
    }

    public void UpdateRotate()
    {
        rotateCount += Time.deltaTime;

        Vector3 force = movePower * playerUtillity.GetAnalogpadMove();
        Vector3 frictionForce = moveFrictionXZ * (rigidbody.velocity - new Vector3(0, rigidbody.velocity.y, 0));
        rigidbody.AddForce(Time.deltaTime * (force - frictionForce), ForceMode.Acceleration);

        if (CanDirectionChange1()) DoDirectionChange1();
    }
}

using UnityEngine;
using System.Collections;

public class Gpt_PlayerAir : MonoBehaviour
{

    public Gpt_PlayerUtillity playerUtillity;
    public new Rigidbody rigidbody;
    public float force = 200f;
    public float friction = 10;
    public float frictionXZ = 10;
    public float downForce = 200;

    
    public float jumpContinueTime = 0.5f;
    public float jumpContinuePower = 20;
    int jumpStartFrame_log = 0;

    float airCount = 0;

    public void StartAir()
    {
        airCount = 0;
    }

    public void StartAir_FromJump(int jumpStartFame)
    {
        jumpStartFrame_log = jumpStartFame;
        StartAir();
    }

    public void EndAir()
    {

    }


    public void UpdateAir(bool jumpPushing, int jumpStartFrame)
    {
        airCount += Time.deltaTime;

        UpdateAir_JumpContinue(jumpPushing, jumpStartFrame);
        UpdateAir_MoveAndFriction();
    }

    void UpdateAir_MoveAndFriction()
    {
        Vector3 input = playerUtillity.GetAnalogpadMove();

        Vector3 power = input * force;
        Vector3 frictionXZPower = -1 * frictionXZ * (rigidbody.velocity - new Vector3(0, rigidbody.velocity.y, 0));
        Vector3 frictionPower = -1 * friction * rigidbody.velocity;
        Vector3 downPower = new Vector3(0, -downForce, 0);

        Vector3 allPower = power + frictionPower + frictionXZPower + downPower;

        rigidbody.AddForce(Time.deltaTime * allPower, ForceMode.Acceleration);

        playerUtillity.LookAnalogpadDirction();
    }

    void UpdateAir_JumpContinue(bool jumpPushing, int jumpStartFrame)
    {
        if (jumpPushing && jumpStartFrame_log == jumpStartFrame && airCount < jumpContinueTime)
        {
            Vector3 force = jumpContinuePower * new Vector3(0, 1, 0);
            rigidbody.AddForce(Time.deltaTime * force, ForceMode.Acceleration);
        }

    }
}

using UnityEngine;
using System.Collections;

public class Gpt_PlayerJump : MonoBehaviour {

    public Gpt_PlayerUtillity playerUtillity;
    public Gpt_PlayerAir playerAir;
    public new Rigidbody rigidbody;
    public Vector3 jumpForce;
    public float speedDownXZ = 0.5f;

    public Vector2 fase1Friction = new Vector2(0,0);
    public float fase1DownPower = 100;

    public float fase2StartTime = 0.15f;
    public Vector2 fase2Friction = new Vector2(0, 5);
    public float fase2DownPower = 1000;
    
    public float jumpTime = 0.3f;

    float jumpCount = 0;
    int jumpPushFrame_log = 0;

    public void StartJump(int JumpPushFrame)
    {
        jumpCount = 0;
        Vector3 speedDownXZPower = -1 * speedDownXZ * (rigidbody.velocity - new Vector3(0, rigidbody.velocity.y, 0));
        Vector3 speedDownYPower = -1 * new Vector3(0, rigidbody.velocity.y, 0);
        rigidbody.AddForce(jumpForce + speedDownXZPower + speedDownYPower, ForceMode.VelocityChange);
        jumpPushFrame_log = JumpPushFrame;
    }

    public void EndJump()
    {

    }

    public bool CanStartJump(int jumpPushFrame)
    {
        return jumpPushFrame_log != jumpPushFrame;
    }

    public bool IsJumpEnd()
    {
        return jumpCount > jumpTime;
    }

    public void UpdateJump()
    {
        jumpCount += Time.deltaTime;
        playerAir.UpdateAir_PlayerControl();

        if(jumpCount < fase2StartTime)
        {
            UpdateJump_Move(fase1Friction, fase1DownPower);
        }

        if(fase2StartTime <= jumpCount)
        {
            UpdateJump_Move(fase2Friction, fase2DownPower);
        }
    }
    
    public void UpdateJump_Move(Vector2 friction, float downPower)
    {
        Vector3 velocityY = new Vector3(0, rigidbody.velocity.y, 0);
        Vector3 velocityXZ = rigidbody.velocity - velocityY;

        Vector3 force = downPower * new Vector3(0, -1, 0) - friction.x * velocityXZ - friction.y * velocityY;
        rigidbody.AddForce(Time.deltaTime * force, ForceMode.Acceleration);
    }
  
  }

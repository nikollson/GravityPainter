using UnityEngine;
using System.Collections;

public class Gpt_PlayerJump : MonoBehaviour {

    public Gpt_PlayerUtillity playerUtillity;
    public new Rigidbody rigidbody;
    public Vector3 jumpForce;
    public float speedDownXZ = 0.5f;
    public float jumpTime = 0.0000001f;
    float jumpCount = 0;
    int jumpPushFrame_log = 0;

    public void StartJump(int JumpPushFrame)
    {
        jumpCount = 0;
        Vector3 speedDownXZPower = -1 * speedDownXZ * (rigidbody.velocity - new Vector3(0, rigidbody.velocity.y, 0));
        rigidbody.AddForce(jumpForce + speedDownXZPower, ForceMode.VelocityChange);
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
    }
  
  }

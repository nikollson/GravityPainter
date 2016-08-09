using UnityEngine;
using System.Collections;

public class Gpt_PlayerJump : MonoBehaviour {

    public Gpt_PlayerUtillity playerUtillity;
    public new Rigidbody rigidbody;
    public Vector3 jumpForce;
    public float speedDownXZ = 0.5f;

    public float jumpTime = 0.2f;
    float jumpCount = 0;

    public void StartJump()
    {
        jumpCount = 0;
        Vector3 speedDownXZPower = -1 * speedDownXZ * (rigidbody.velocity - new Vector3(0, rigidbody.velocity.y, 0));
        rigidbody.AddForce(jumpForce + speedDownXZPower, ForceMode.VelocityChange);
    }

    public void EndJump()
    {

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

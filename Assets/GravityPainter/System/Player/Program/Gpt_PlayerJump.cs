using UnityEngine;
using System.Collections;

public class Gpt_PlayerJump : MonoBehaviour {

    public Gpt_PlayerUtillity playerUtillity;
    public Rigidbody playerRigidbody;
    public Vector2 jumpForce;

    public float jumpTime = 0.2f;
    float jumpCount = 0;

    public void StartJump()
    {
        jumpCount = 0;
        playerRigidbody.AddForce(jumpForce, ForceMode.Impulse);
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

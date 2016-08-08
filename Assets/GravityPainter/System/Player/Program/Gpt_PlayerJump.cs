using UnityEngine;
using System.Collections;

public class Gpt_PlayerJump : MonoBehaviour {

    public Gpt_PlayerUtillity playerUtillity;
    public Rigidbody playerRigidbody;
    public Vector2 jumpForce;

    public float jumpTime = 0.2f;
    float jumpCount = 0;

    bool isJumping = false;

    public void StartJump()
    {
        isJumping = true;
        jumpCount = 0;

        playerRigidbody.AddForce(jumpForce, ForceMode.Impulse);
    }

    public void EndJump()
    {
        isJumping = false;
    }

    public bool IsJumpEnd()
    {
        return jumpCount > jumpTime;
    }

    void Update()
    {
        if (isJumping)
        {
            jumpCount += Time.deltaTime;
        }
    }
}

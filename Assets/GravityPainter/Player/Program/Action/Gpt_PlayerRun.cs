using UnityEngine;
using System.Collections;

public class Gpt_PlayerRun : MonoBehaviour
{
    public Gpt_PlayerUtillity playerUtillity;
    public Gpt_PlayerAnimator playerAnimator;
    public new Rigidbody rigidbody;
    public float targetPower = 2.0f;
    public float friction = 0.4f;
    public AudioClip walkClip;

    AudioSource audioSource;
    bool isRunLeft = false;
    bool isRunRight = false;

    void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    public void StartRun()
    {

    }

    public void EndRun()
    {

    }

    public void UpdateRun()
    {
        /*
        Vector3 power = movePower * playerUtillity.GetAnalogpadMove();
        rigidbody.AddForce(Time.deltaTime * rigidbody.velocity * -1 * friction, ForceMode.Acceleration);
        rigidbody.AddForce(Time.deltaTime * power, ForceMode.Acceleration);
        */

        if (playerAnimator.IsRuningAnimation())
        {
            bool isLeft = playerAnimator.IsRuningLeftFoot();
            if(!isRunLeft && isLeft)
            {
                audioSource.PlayOneShot(walkClip);
                isRunLeft = true;
                isRunRight = false;
            }

            if(!isRunRight && !isLeft)
            {
                audioSource.PlayOneShot(walkClip);
                isRunLeft = false;
                isRunRight = true;
            }
        }


        Vector3 analogPadMove = playerUtillity.GetAnalogpadMove();
        Vector3 power = targetPower * analogPadMove - friction * rigidbody.velocity;
        rigidbody.AddForce(power, ForceMode.Acceleration);

        playerUtillity.LookAnalogpadDirction();
    }



    float radToDigree(float rad)
    {
        return rad / Mathf.PI * 180;
    }


}

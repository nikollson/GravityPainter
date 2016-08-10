using UnityEngine;
using System.Collections;

public class Gpt_PlayerRun : MonoBehaviour
{
    public Gpt_PlayerUtillity playerUtillity;
    public new Rigidbody rigidbody;
    public float targetSpeed = 2.0f;
    public float friction = 0.4f;


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

        Vector3 analogPadMove = playerUtillity.GetAnalogpadMove();
        Vector3 power = targetSpeed * analogPadMove - friction * rigidbody.velocity;
        rigidbody.AddForce(power, ForceMode.VelocityChange);

        playerUtillity.LookAnalogpadDirction();
    }



    float radToDigree(float rad)
    {
        return rad / Mathf.PI * 180;
    }


}

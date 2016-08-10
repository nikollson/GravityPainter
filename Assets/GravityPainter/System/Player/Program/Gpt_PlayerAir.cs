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
    

    public void StartAir()
    {
        
    }

    public void EndAir()
    {

    }


    public void UpdateAir()
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
}

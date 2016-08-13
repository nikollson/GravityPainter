using UnityEngine;
using System.Collections;

public class Gpt_PlayerAir : MonoBehaviour
{

    public Gpt_PlayerUtillity playerUtillity;
    public new Rigidbody rigidbody;
    public float moveForce = 1000f;
    public float friction = 10;
    public float frictionXZ = 10;
    public float downForce = 200;


    float airCount = 0;

    public void StartAir()
    {
        airCount = 0;
    }
    
    public void EndAir()
    {

    }


    public void UpdateAir()
    {
        airCount += Time.deltaTime;
        
        UpdateAir_PlayerControl();
        UpdateAir_OtherPower();
    }

    public void UpdateAir_PlayerControl()
    {
        Vector3 input = playerUtillity.GetAnalogpadMove();

        Vector3 power = input * moveForce;
        Vector3 frictionXZPower = -1 * frictionXZ * (rigidbody.velocity - new Vector3(0, rigidbody.velocity.y, 0));

        Vector3 allPower = power + frictionXZPower;
        rigidbody.AddForce(Time.deltaTime * allPower, ForceMode.Acceleration);

        playerUtillity.LookAnalogpadDirction();
    }

    void UpdateAir_OtherPower()
    {
        Vector3 frictionPower = -1 * friction * rigidbody.velocity;
        Vector3 downPower = new Vector3(0, -downForce, 0);

        Vector3 allPower = frictionPower + downPower;
        rigidbody.AddForce(Time.deltaTime * allPower, ForceMode.Acceleration);
    }
    
}

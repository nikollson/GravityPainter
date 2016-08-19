using UnityEngine;
using System.Collections;

public class Gpt_PlayerDeadAction : MonoBehaviour
{
    public new Rigidbody rigidbody;

    public float frictionXZ = 20;

    public void StartDead()
    {

    }

    public void EndDead()
    {

    }

    public void UpdateDead()
    {
        Vector3 force = -1 * frictionXZ * (rigidbody.velocity - new Vector3(0, rigidbody.velocity.y, 0));
        rigidbody.AddForce(force * Time.deltaTime, ForceMode.Acceleration);
    }
}

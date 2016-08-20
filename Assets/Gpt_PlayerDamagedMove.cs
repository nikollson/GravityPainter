using UnityEngine;
using System.Collections;

public class Gpt_PlayerDamagedMove : MonoBehaviour {

    public new Rigidbody rigidbody;
    private float count = 0;

    public float frictionXZ = 100;

    public void StartDamage()
    {
        count = 0;
    }

    public void EndDamage()
    {

    }

    public void UpdateDamage()
    {
        Vector3 velocity = rigidbody.velocity - new Vector3(0, rigidbody.velocity.y, 0);
        Vector3 power = -1 * frictionXZ * velocity;
        rigidbody.AddForce(power * Time.deltaTime, ForceMode.Acceleration);
    }
}

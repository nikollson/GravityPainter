using UnityEngine;
using System.Collections;

public class Gpt_PlayerWait : MonoBehaviour
{

    public new Rigidbody rigidbody;
    public float stopFliction = 5.0f;

    public void StartWait()
    {

    }

    public void EndWait()
    {

    }

    public void UpdateWait()
    {
        rigidbody.AddForce(Time.deltaTime * stopFliction * -1 * rigidbody.velocity, ForceMode.Acceleration);
    }
}


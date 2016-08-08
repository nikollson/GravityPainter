using UnityEngine;
using System.Collections;

public class Gpt_PlayerWait : MonoBehaviour {

    public new Rigidbody rigidbody;
    public float stopFliction = 5.0f;

    bool isWaiting = false;

    public void StartWait()
    {
        isWaiting = true;
    }

    public void EndWait()
    {
        isWaiting = false;
    }
    
    void Update()
    {
        if (isWaiting)
        {
            rigidbody.AddForce(Time.deltaTime * stopFliction * -1 * rigidbody.velocity, ForceMode.Acceleration);
        }
    }
}

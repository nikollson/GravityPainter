using UnityEngine;
using System.Collections;

public class Gpt_PlayerDetonate : MonoBehaviour {

    public float detonateTime = 0.5f;
    public float friction = 400;
    public new Rigidbody rigidbody;

    int inputFrame_log = 0;
    float detonateCount = 0;

    public bool CanStartDetonate(int inputFrame)
    {
        return inputFrame_log != inputFrame;
    }


    public void StartDetonate(int inputFrame)
    {
        inputFrame_log = inputFrame;
        detonateCount = 0;
    }


    public void EndDetonate()
    {

    }

    public bool IsEndDetonate()
    {

        return detonateCount > detonateTime;
    }


    public void UpdateDetonate()
    {
        detonateCount += Time.deltaTime;

        Vector3 force = -1 * friction * (rigidbody.velocity - new Vector3(0, rigidbody.velocity.y, 0));
        rigidbody.AddForce(force, ForceMode.Acceleration);
    }



}

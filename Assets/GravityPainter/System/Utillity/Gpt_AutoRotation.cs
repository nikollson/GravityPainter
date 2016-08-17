using UnityEngine;
using System.Collections;

public class Gpt_AutoRotation : MonoBehaviour {

    public Vector3 angularVelocity;

    void Update()
    {
        Vector3 current = this.transform.localRotation.eulerAngles;
        current.x += angularVelocity.x * Time.deltaTime;
        current.y += angularVelocity.y * Time.deltaTime;
        current.z += angularVelocity.z * Time.deltaTime;
        this.transform.localRotation = Quaternion.Euler(current);
    }
}

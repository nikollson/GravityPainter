using UnityEngine;
using System.Collections;

public class Gpt_SpecialEnemyGolem : MonoBehaviour {

    public float rotateSpeed = 0.3f;
    public new Rigidbody rigidbody;

    void Update()
    {
        rigidbody.angularVelocity = new Vector3(0, rotateSpeed, 0);
    }
    
}

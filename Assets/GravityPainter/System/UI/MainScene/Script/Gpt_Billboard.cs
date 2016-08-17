using UnityEngine;
using System.Collections;

public class Gpt_Billboard : MonoBehaviour {

    public Transform target;

    void Update()
    {
        Vector3 vec = target.position - this.transform.position;
        vec.x = vec.z = 0.0f;
        this.transform.LookAt(target.position - vec);
    }
}

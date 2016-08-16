using UnityEngine;
using System.Collections;

public class Gpt_EnemyFall : MonoBehaviour {

	void Start () {
	}
	
	void Update () {
	}

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "FallCheck") {
            Object.Destroy(this.gameObject);
        }
    }
}

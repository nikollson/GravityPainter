using UnityEngine;
using System.Collections;

public class Gpt_Enemy_AvoidHole : MonoBehaviour {

    Gpt_EnemyMove moveScript;

	void Start () {
        moveScript = this.transform.parent.parent.GetComponent<Gpt_EnemyMove>();
    }

    void Update () {
        //moveScript.SetSpeed(0.0f);
    }

    void OnTriggerEnter(Collider coll) {
    }
}

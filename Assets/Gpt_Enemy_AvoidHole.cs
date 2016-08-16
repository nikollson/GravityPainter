using UnityEngine;
using System.Collections;

public class Gpt_Enemy_AvoidHole : MonoBehaviour {

    Gpt_EnemyMove moveScript;
    bool collFlg = true;        // 当たっているか

	void Start () {
        moveScript = this.transform.parent.parent.GetComponent<Gpt_EnemyMove>();
    }

    void Update () {
        //moveScript.IsAbyssFalse();

        // 当たっていない、つまり前に穴がある
        if (!collFlg) {
            moveScript.SetSpeed(0.0f);
        }

        collFlg = false;
    }

    void OnTriggerStay(Collider coll)
    {
        moveScript.SetSpeed(5.0f);
        collFlg = true;
    }
}

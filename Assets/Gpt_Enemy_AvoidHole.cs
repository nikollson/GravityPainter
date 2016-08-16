using UnityEngine;
using System.Collections;

public class Gpt_Enemy_AvoidHole : MonoBehaviour {

    Gpt_EnemyMove moveScript;
    bool collFlg = true;        // 当たっているか

	void Start () {
        moveScript = this.transform.parent.parent.GetComponent<Gpt_EnemyMove>();
    }

    void Update () {

        if (!collFlg)
        {
            // 当たっていない、つまり前に穴がある際の処理
            moveScript.IsAbyss();
        }
        else {
            // 当たっている、つまり前に穴がない際の処理
            moveScript.IsAbyssFalse();
        }

        collFlg = false;
    }

    // Update関数より先に呼ばれる
    void OnTriggerStay(Collider coll)
    {
        collFlg = true;
    }
}

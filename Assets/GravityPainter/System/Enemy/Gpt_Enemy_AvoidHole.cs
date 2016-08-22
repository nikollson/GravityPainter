using UnityEngine;
using System.Collections;

public class Gpt_Enemy_AvoidHole : MonoBehaviour
{

    Gpt_EnemyMove moveScript;
    public HitManager hitManager;
    bool collFlg = true;        // 当たっているか
    float cnt = 0.0f;

    void Start()
    {
        moveScript = this.transform.parent.parent.GetComponent<Gpt_EnemyMove>();
    }

    void Update()
    {

        if (!hitManager.IsHit)
        {
            // 当たっていない、つまり前に穴がある際の処理
            moveScript.IsAbyss();
            ////Debug.Log("!!!!!!!!!!!!");
        }
        else
        {
            // 当たっている、つまり前に穴がない際の処理
            moveScript.IsAbyssFalse();
            //Debug.Log("あたっている状態");
        }
        cnt = 0.0f;

        //collFlg = false;
    }

    /*
    // Update関数より先に呼ばれる
    void OnTriggerStay(Collider coll)
    {
        collFlg = true;
    }
    */
}

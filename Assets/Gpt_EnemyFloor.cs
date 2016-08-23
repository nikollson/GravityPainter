using UnityEngine;
using System.Collections;

public class Gpt_EnemyFloor : MonoBehaviour {


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

        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit, 30))
        {
            //Debug.Log("hit!:" + hit.transform.gameObject.name);
            moveScript.IsFloor = false;
        }
        else
        {
            moveScript.IsFloor = true;
        }

        //collFlg = false;
    }

    /*
    // Update関数より先に呼ばれる
    void OnTriggerStay(Collider coll)
    {
        collFlg = true;
    }
    */
    void OnTriggerEnter(Collider coll)
    {
        //Debug.Log("衝突はんてい"+coll.gameObject.name+" :"+this.gameObject.name);
    }
}

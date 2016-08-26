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

        if (Physics.Raycast(transform.position, -transform.up, out hit, 2f))
        {
            Debug.Log("hit!:" + hit.transform.gameObject.name+"  :"+this.transform.parent.parent.name);
            if (!moveScript.isMoved)
            {
                moveScript.IsFloor = false;
            }
            //右手の場合例外
            if(hit.transform.gameObject.name == "nurbsCircle5"|| hit.transform.gameObject.name == "FallChecker")
            {
                moveScript.IsFloor = true;
            }

            if (hit.transform.gameObject.layer==11)//EnemyBody
            {
                moveScript.IsFloor = true;
            }
        }
        else
        {
            Debug.Log("nonhit!:" + "  :" + this.transform.parent.parent.name);
            if (moveScript.isMoved)
            {
                moveScript.IsFloor = true;
            }
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

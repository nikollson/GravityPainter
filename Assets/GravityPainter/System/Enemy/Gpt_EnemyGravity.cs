using UnityEngine;
using System.Collections;

public class Gpt_EnemyGravity : MonoBehaviour {

    public Gpt_EnemyColor EnemyColor;


    private Gpt_EnemyGravity EnemyGravity;
    private int Color = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //衝突が始まったときに１度だけ呼ばれる関数（接触したオブジェのタグを表示する） 
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag =="GravityZone")
        {
            
            //if (EnemyColor.GetColor()==col.)
            //{

            //}
        }
    }


    void GravityMove()
    {


    }
}

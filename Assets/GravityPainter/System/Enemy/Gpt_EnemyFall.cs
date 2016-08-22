using UnityEngine;
using System.Collections;

public class Gpt_EnemyFall : MonoBehaviour {

    private GameObject ManegerObject;
    private Gpt_EnemyGravityManeger GravityManager;


    private bool isFall;
	void Start () {
        ManegerObject = GameObject.Find("GravityManeger");
        GravityManager = ManegerObject.GetComponent<Gpt_EnemyGravityManeger>();
	}
	
	void Update () {
	}

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "FallChecker"&&!isFall) {
            Debug.Log("fall");
            GravityManager.ReduceNumCount();
            Object.Destroy(this.gameObject);
            isFall = true;
        }
    }
}

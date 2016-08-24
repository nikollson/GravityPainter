using UnityEngine;
using System.Collections;

public class Gpt_EnemyFall : MonoBehaviour {

    private GameObject ManegerObject;
    private Gpt_EnemyGravityManeger GravityManager;

    public GameObject DeathObject;
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
            Instantiate(DeathObject, this.transform.position, Quaternion.identity);
            GravityManager.ReduceNumCount();
            Object.Destroy(this.gameObject);
            isFall = true;
        }
    }
}

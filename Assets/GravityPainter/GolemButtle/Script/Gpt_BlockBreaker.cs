using UnityEngine;
using System.Collections;

public class Gpt_BlockBreaker : MonoBehaviour {

    public GameObject[] obj = new GameObject[8];

	void Start () {
	}
	
	void Update () {
	}

    void OnTriggerEnter(Collider coll)
    {
        // 床と当たっていたらダメージを与える
        //coll;
    }
}

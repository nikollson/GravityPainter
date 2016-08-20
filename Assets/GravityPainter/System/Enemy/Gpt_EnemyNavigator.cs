using UnityEngine;
using System.Collections;

public class Gpt_EnemyNavigator : MonoBehaviour {


    public Transform target;
    public NavMeshAgent navigator;
	// Use this for initialization
	void Start () {
        target = GameObject.FindWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (navigator.enabled)
        {
            navigator.SetDestination(target.transform.position);
        }
        
	}
}

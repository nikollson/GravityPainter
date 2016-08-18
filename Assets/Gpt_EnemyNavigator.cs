using UnityEngine;
using System.Collections;

public class Gpt_EnemyNavigator : MonoBehaviour {


    public Transform target;
    public NavMeshAgent navigator;
	// Use this for initialization
	void Start () {
        navigator.SetDestination(target.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

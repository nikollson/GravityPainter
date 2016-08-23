using UnityEngine;
using System.Collections;

public class Gpt_Destroy : MonoBehaviour {


    public float destroyTime = 5f;
    private float destroy;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        destroy += 0.1f;
        if (destroy > destroyTime)
        {
            Object.Destroy(this.gameObject);
        }
	
	}
}

using UnityEngine;
using System.Collections;

public class Gpt_Beam : MonoBehaviour {

    public float speed = 2f;
    public float speedAccele = 0.1f;
    public float destroyTime =5f;


    private float speedTime;
    private float destroy;
    public Rigidbody rigid;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


        speedTime += speedAccele;
        if (speedTime > speed) speedTime = speed;
        rigid.AddForce(this.transform.forward * speedTime, ForceMode.VelocityChange);

        destroy += 0.1f;

        if (destroy > destroyTime)
        {
            Object.Destroy(this.gameObject);
        }
	}
}

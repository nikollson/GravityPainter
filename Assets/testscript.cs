using UnityEngine;
using System.Collections;

public class testscript : MonoBehaviour {

    public Rigidbody rigid;
    int te;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 temp = new Vector3(40f, 0, 0);
        Vector3 temp2 = new Vector3(0, 60f, 0);
        te++;
        if (te < 2)
        {
            rigid.AddForce(temp, ForceMode.VelocityChange);
            rigid.AddForce(temp2, ForceMode.VelocityChange);
        }
        
	}
}

using UnityEngine;
using System.Collections;

public class Gpt_PlayerBlock : MonoBehaviour {

    public GameObject cam;
    bool flg = false;
    float z = 33.0f;

	void Start () {
	}
	
	void Update () {
        if (!flg && cam.transform.position.z > z)
        {
            flg = true;
            this.transform.position += new Vector3(0,0,10000);
        }
	}
}

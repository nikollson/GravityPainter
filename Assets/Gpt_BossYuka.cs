using UnityEngine;
using System.Collections;

public class Gpt_BossYuka : MonoBehaviour {

    public GameObject player;

	void Start () {
	}
	
	void Update () {

        if (player.transform.position.y < 15.5f)
        {
            this.transform.position -= new Vector3(10000, 10000, 10000);
        }else
        {
            this.transform.position = new Vector3(0, 15, 22.41f);

        }
    }

    public void InitPos()
    {
        this.transform.position -= new Vector3(0, 15, 22.41f);
    }
}

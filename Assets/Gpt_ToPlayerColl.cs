using UnityEngine;
using System.Collections;

public class Gpt_ToPlayerColl : MonoBehaviour {

    public GameObject player;
    public GameObject boss;

    void Start () {
	}
	
	void Update () {
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player")
        {
            player.GetComponent<Gpt_PlayerState>().AddHPDamage(1);

        }
        //else if (c.tag != "Enemy")
        else if (c.tag != "Enemy")
        {
            boss.GetComponent<Gpt_Boss>().state = 1;
        }
    }
}

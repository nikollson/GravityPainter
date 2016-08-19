using UnityEngine;
using System.Collections;

public class Gpt_ToPlayerColl : MonoBehaviour {

    public GameObject player;

	void Start () {
	}
	
	void Update () {
    }

    void OnTriggerEnter()
    {
        player.GetComponent<Gpt_PlayerState>().AddHPDamage(6);
    }
}

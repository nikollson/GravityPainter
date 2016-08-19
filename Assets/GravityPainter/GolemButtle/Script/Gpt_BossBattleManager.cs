using UnityEngine;
using System.Collections;

public class Gpt_BossBattleManager : MonoBehaviour {

    public GameObject player;
    public float fallY = -20.0f;
    public Vector3 playerResPos;

    void Start () {
	}
	
	void Update () {

        if (player.transform.position.y < fallY)
        {
            player.GetComponent<Gpt_PlayerState>().AddHPDamage(4);
            player.transform.position = playerResPos;
        }
    }
}

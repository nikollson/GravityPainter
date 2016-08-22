using UnityEngine;
using System.Collections;

public class Gpt_BossBattleManager : MonoBehaviour {

    public GameObject player;
    public float fallY = -20.0f;
    public Vector3 playerResPos;

    public Transform enemyMakePos;

    public GameObject enemy;
    float cnt = 0.0f;
    const float ENEMYRES_TIME= 3.0f;
    Vector3 resPos = new Vector3(10,10,10);
    Vector3 resPos2 = new Vector3(-10,10,-10);

    void Start () {
	}
	
	void Update () {

        cnt += Time.deltaTime;

        if (cnt >= ENEMYRES_TIME)
        {
            cnt = 0.0f;
            if(Random.Range(0.0f,1.0f)>=0.5f)Instantiate(enemy, resPos,Quaternion.identity);
            else Instantiate(enemy, resPos2, Quaternion.identity);
        }

        if (player.transform.position.y < fallY)
        {
            player.GetComponent<Gpt_PlayerState>().AddHPDamage(4);
            player.transform.position = playerResPos;
        }
    }
}

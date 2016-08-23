using UnityEngine;
using System.Collections;

public class Gpt_MakeEnemy : MonoBehaviour {

    public GameObject enemyPrefab;
    public GameObject yukaObj;

    float cnt;
    public float ENEMY_MAKE_TIME = 20.0f;

	void Start () {
        cnt = Random.Range(0.0f, ENEMY_MAKE_TIME/1.5f);
    }

    void Update () {

        cnt += Time.deltaTime;

        if (yukaObj.GetComponent<Gpt_YukaBox>().HP >= 1)
        {

            if (cnt >= ENEMY_MAKE_TIME)
            {
                Instantiate(enemyPrefab, this.transform.position + new Vector3(0, 10.0f, 0), Quaternion.identity);
                cnt = Random.Range(0.0f, ENEMY_MAKE_TIME / 1.5f);
            }

        }

    }
}

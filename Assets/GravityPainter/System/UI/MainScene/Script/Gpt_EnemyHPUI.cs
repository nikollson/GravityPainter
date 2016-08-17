using UnityEngine;
using System.Collections;

public class Gpt_EnemyHPUI : MonoBehaviour {

    public GameObject player;
    const int ENEMY_HP = 2;
    public GameObject[] lifeImg = new GameObject[ENEMY_HP];
    Vector3 notDrawPos= new Vector3(-10000, -10000, -10000);

    void Start () {
    }
	
	void Update () {

        //if (hp == 1)
        //{
        //    lifeImg[0].transform.position = this.transform.position;
        //    lifeImg[1].transform.position = notDrawPos;
        //}
        //else
        //{
        //    lifeImg[1].transform.position = this.transform.position;
        //    lifeImg[0].transform.position = notDrawPos;
        //}
	}
}

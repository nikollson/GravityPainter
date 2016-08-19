﻿using UnityEngine;
using System.Collections;

public class Gpt_Boss : MonoBehaviour {

    const float maxHp = 10.0f;
    float hp = 10.0f;
    public GameObject hand1;
    public GameObject hand2;
    public int state = 0;
    float fallSpd = 10.0f;

    float cnt = 0.0f;
    const float CNTTIME = 20.0f;

    void Start () {
	}
	
	void Update () {

        cnt += Time.deltaTime;

        if (state == 0)
        {
            // 通常状態

            if (cnt >= CNTTIME)
            {
                cnt = 0.0f;
                //state = 1;
            }

        }
        else if (state == 1)
        {
            this.transform.position += new Vector3(0, -Time.deltaTime* fallSpd, 0);
            if(this.transform.position.y < -15.0f)
            {
                state = 0;
                this.hp -= 2.5f;
                this.transform.position = new Vector3(0,0.23f,0);
            }
        }
	}

    public float GetMaxHp()
    {
        return maxHp;
    }

    public float GetHp()
    {
        return hp;
    }

    public void AddHP(int plus)
    {
        hp += plus;
    }

    //void OnTriggerEnter(Collider c)
    //{
    //    if (c.tag != "Player" && c.tag != "Enemy")
    //    {
    //        Debug.Log("AA");
    //    }

    //    Debug.Log("AAaaa");

    //}
}

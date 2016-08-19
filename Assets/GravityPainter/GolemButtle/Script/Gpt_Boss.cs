using UnityEngine;
using System.Collections;

public class Gpt_Boss : MonoBehaviour {

    const float maxHp = 10.0f;
    float hp = 10.0f;
    public GameObject hand1;
    public GameObject hand2;
    public int state = 1;

    void Start () {
	}
	
	void Update () {

        if (state==0)
        {
            // 通常状態
        }
        else if (state == 1)
        {
            this.transform.position += new Vector3(0, Time.deltaTime * 100.0f, 0);
        }

        this.transform.position += new Vector3(0, Time.deltaTime * 100.0f, 0);

        Debug.Log("BOSS_STATE: "+state);
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

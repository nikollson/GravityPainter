using UnityEngine;
using System.Collections;

public class Gpt_Boss : MonoBehaviour {

    enum State
    {
        Wait = 0,
        Search,
        Fall,
        Atk1,
        Atk2,
        Atk3, 
    }
    State state = (int)State.Wait;

    const float maxHp = 10.0f;
    float hp = 10.0f;
    public GameObject hand1;
    public GameObject hand2;
    float fallSpd = 10.0f;
    float cnt = 0.0f;

    void Start () {
	}
	
	void Update () {

        cnt += Time.deltaTime;

        if (state == State.Wait)
        {
            //
        }
        else if (state == State.Fall)
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
}

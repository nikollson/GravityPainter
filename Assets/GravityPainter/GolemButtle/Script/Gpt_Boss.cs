using UnityEngine;
using System.Collections;

public class Gpt_Boss : MonoBehaviour {

    const float maxHp = 10.0f;
    float hp = 10.0f;

    void Start () {
	}
	
	void Update () {
	}

    public float GetMaxHp()
    {
        return maxHp;
    }

    public float GetHp()
    {
        return hp;
    }
}

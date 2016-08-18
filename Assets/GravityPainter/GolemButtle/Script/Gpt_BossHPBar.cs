using UnityEngine;
using System.Collections;

public class Gpt_BossHPBar : MonoBehaviour {

    public GameObject Boss;
    float maxHp;
    float firstScaleX, firstScaleY;

    void Start () {
        maxHp = Boss.GetComponent<Gpt_Boss>().GetMaxHp();
        firstScaleX = this.transform.localScale.x;
        firstScaleY = this.transform.localScale.y;
    }
	
	void Update () {

        this.transform.localScale = new Vector3(Boss.GetComponent<Gpt_Boss>().GetHp() / maxHp* firstScaleX, firstScaleY, 0.0f);

    }
}

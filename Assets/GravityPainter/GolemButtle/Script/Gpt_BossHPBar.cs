using UnityEngine;
using System.Collections;

public class Gpt_BossHPBar : MonoBehaviour {

    public GameObject Boss;
    float maxHp;
    float firstScaleX, firstScaleY;
    Vector3 vec;
    public bool notDrawFlg = false;

    void Start () {
        maxHp = Boss.GetComponent<Gpt_Boss>().GetMaxHp();
        firstScaleX = this.transform.localScale.x;
        firstScaleY = this.transform.localScale.y;
        vec = this.transform.position;
    }
	
	void Update () {

        if(Boss.GetComponent<Gpt_Boss>().GetHp()>=0) this.transform.localScale = new Vector3(Boss.GetComponent<Gpt_Boss>().GetHp() / maxHp* firstScaleX, firstScaleY, 0.0f);

        if (notDrawFlg) vec = new Vector3(-10000, -10000, -10000);
    }
}

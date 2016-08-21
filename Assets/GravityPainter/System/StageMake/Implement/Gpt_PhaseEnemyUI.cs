using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Gpt_PhaseEnemyUI : MonoBehaviour {


    public GameObject[] phaseParent;
    public Text[] enemyNum;

    Gpt_EnemyPhaseControl phaseControl;


    void Start()
    {
        phaseControl = GameObject.Find("GravityManeger").GetComponent<Gpt_EnemyPhaseControl>();
    }

    void Update()
    {

        int num = phaseControl.GetPhaseNum();

        for(int i = 0; i < phaseParent.Length; i++)
        {
            phaseParent[i].SetActive(i < num);
        }


        for (int i = 0; i < num; i++)
        {
            enemyNum[i].text = "" + phaseControl.GetPhaseEnemyNum(i);
        }
    }
}

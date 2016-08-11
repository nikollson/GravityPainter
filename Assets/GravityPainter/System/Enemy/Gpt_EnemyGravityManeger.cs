using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gpt_EnemyGravityManeger : MonoBehaviour {

    private List<Gpt_Enemy> EnemyList = new List<Gpt_Enemy>();

    public float gravityArea;

    private static int num=0;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        //距離判定
        for (int i = 0; i < num; i++)
        {
            for (int j = i; j < num; j++)
            {
                if (j == i) continue;

                if(EnemyList[i].GetColor()!=0&&EnemyList[i].GetColor()== EnemyList[j].GetColor())
                {
                    if (Vector3.Distance(EnemyList[i].transform.position, EnemyList[j].transform.position) < gravityArea)
                    {
                        //i番目へのベクトル
                        Vector3 objVec1 = EnemyList[i].transform.position - EnemyList[j].transform.position;

                        //j番目へのベクトル
                        Vector3 objVec2 = EnemyList[j].transform.position - EnemyList[i].transform.position;

                        Vector3 normVec1 = objVec1.normalized;
                        Vector3 normVec2 = objVec2.normalized;
                        EnemyList[i].SetGravity(normVec1);
                        EnemyList[j].SetGravity(normVec2);
                    }
                }
                
            }
        }

        Debug.Log(num);
    }

    public void AddEnemyList(Gpt_Enemy Enemy)
    {
        EnemyList.Add(Enemy);
        num++;
    }

    public void RemoveEnemyList(Gpt_Enemy Enemy)
    {
        EnemyList.Remove(Enemy);
        num--;

        
    }

    public int ListIndex(Gpt_Enemy Enemy)
    {
        return EnemyList.IndexOf(Enemy);
    }

}

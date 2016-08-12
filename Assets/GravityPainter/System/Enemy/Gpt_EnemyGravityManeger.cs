using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gpt_EnemyGravityManeger : MonoBehaviour {

    private List<Gpt_Enemy> EnemyList = new List<Gpt_Enemy>();

    private List<Gpt_Exploder> ExplodeList = new List<Gpt_Exploder>();
    private List<Vector3> ExplodePosition = new List<Vector3>();

    public float gravityArea;
    // Use this for initialization

    float temp;
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        
        //距離判定
        for (int i = 0; i < EnemyList.Count; i++)
        {
            for (int j = i; j < EnemyList.Count; j++)
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

            //接触判定
        }

        //Debug.Log(EnemyList.Count);
    }

    //爆発処理
    public void IsExplode()
    {
        for (int i = 0; i < EnemyList.Count; i++)
        {
            if (EnemyList[i].GetGravity())
            {
                EnemyList[i].IsExplode();
            }

        }
    }


    public void AddEnemyList(Gpt_Enemy Enemy)
    {
        EnemyList.Add(Enemy);
    }

    public void RemoveEnemyList(Gpt_Enemy Enemy)
    {
        EnemyList.Remove(Enemy);

        
    }

    public int ListIndex(Gpt_Enemy Enemy)
    {
        return EnemyList.IndexOf(Enemy);
    }

    //爆発オブジェクト追加
    public void AddExplodeList(Gpt_Exploder Exploder)
    {
        ExplodeList.Add(Exploder);
    }

    //爆発オブジェクト削除
    public void RemoveExplodeList(Gpt_Exploder Exploder)
    {
        ExplodeList.Remove(Exploder);
    }

    public void AddExplodePosition(Vector3 position)
    {
        ExplodePosition.Add(position);
    }

    public void RemoveExplodePosition(Vector3 position)
    {
        ExplodePosition.Remove(position);
    }

}

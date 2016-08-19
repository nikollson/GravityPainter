using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gpt_EnemyGravityManeger : MonoBehaviour {

    private List<Gpt_Enemy> EnemyList = new List<Gpt_Enemy>();
    private Gpt_Enemy FirstEnemy;
    private Vector3 FirstEnemyPosition;
    private List<Gpt_Exploder> ExplodeList = new List<Gpt_Exploder>();
    private List<Vector3> ExplodePosition = new List<Vector3>();

    public float gravityArea;
    // Use this for initialization
    public Gpt_DoorSystem doorSystem;

    private bool isFloor;

    float temp;

    //何体敵を倒してクリア
    public int enemyNum;


    //爆発上昇のスピード
    public float explodeUpSpeed = 0.08f;
    
    //爆発下降のスピード
    public float explodeUnderSpeed = 0.64f;
    //爆発上昇の時間
    public float enemyUpTime = 5f;
    //爆発下降の時間
    public float enemyUnderTime = 1f;
    private int enemyNumCount;

	void Start () {
        Application.targetFrameRate = 30; //30FPSに設定
	}
	
	// Update is called once per frame
	void Update () {

        
        //敵が一人以上いた時にカウントスタート
        if (EnemyList.Count > 0)
        {
            isFloor=true;
            //一番目の敵を登録

            for (int i = 0; i < EnemyList.Count; i++)
            {
                if (EnemyList[i].GetColor() != 0)
                {
                    //FirstEnemy = EnemyList[i];
                    //FirstEnemyPosition=FirstEnemy.FirstEnemyPosition();
                    break;
                }
            }
        }

        for (int i = 0; i < ExplodeList.Count; i++)
        {
            ExplodeList[i].SetUnderSpeed(explodeUnderSpeed);
            ExplodeList[i].SetUpSpeed(explodeUpSpeed);
        }

        //距離判定
        for (int i = 0; i < EnemyList.Count; i++)
        {
            EnemyList[i].SetUpTime(enemyUpTime);
            EnemyList[i].SetUnderTime(enemyUnderTime);
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
                        if(EnemyList[i].GetShake()!= EnemyList[j].GetShake())
                        {
                            if (EnemyList[i].GetShake())
                            {
                                EnemyList[i].SetGravity(normVec1);
                            }
                            else
                            {
                                EnemyList[j].SetGravity(normVec2);
                            }
                        }else
                        {
                            EnemyList[i].SetGravity(normVec1);
                            EnemyList[j].SetGravity(normVec2);
                        }
                    }
                }
                
            }

            //接触判定
        }

        //敵が一定値に満ちたらドアが開く
        if (isFloor && enemyNumCount >= enemyNum)
        {
            if (doorSystem != null)
            {
                doorSystem.OpenDoor();
            }
            
        }

        //Debug.Log(EnemyList.Count);
    }

    public int GetRestEnemy()
    {
        return enemyNum - enemyNumCount;
    }

    //爆発処理
    public void IsExplode()
    {
        for (int i = 0; i < ExplodeList.Count; i++)
        {
            ExplodeList[i].IsExplode();
        }

        for (int i = 0; i < EnemyList.Count; i++)
        {
            if (EnemyList[i].GetGravity())
            {
                EnemyList[i].IsExplode();
            }

        }
    }

    public void IsExplodeColor(int color)
    {
        for (int i = 0; i < ExplodeList.Count; i++)
        {
            //該当の色のみ爆発
            if (color == ExplodeList[i].GetColor())
            {
                ExplodeList[i].IsExplode();
            }
            
        }

        for (int i = 0; i < EnemyList.Count; i++)
        {
            //該当の色のみ爆発
            if (color == EnemyList[i].GetColor())
            {
                if (EnemyList[i].GetGravity())
                {
                    EnemyList[i].IsExplode();
                }
            }
        }
    }

    public void IsExplodeWave()
    {
        for (int i = 0; i < ExplodeList.Count; i++)
        {
            for (int j = 0; j < EnemyList.Count; j++)
            {
                //爆風に近い範囲でダメージ
                if (Vector3.Distance(ExplodeList[i].gameObject.transform.position, EnemyList[j].gameObject.transform.position)<7f)
                {
                    //引力状態にない敵のみダメージ
                    if(!EnemyList[j].GetGravity()){
                        EnemyList[j].SetWavePosition(ExplodeList[i].GetPosition());
                        EnemyList[j].ExplodeDamage(1);
                    }
                    
                }
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

    //現在のエネミー数を返す
    public int EnemyCount()
    {
        return EnemyList.Count;
    }

    public void ReduceNumCount()
    {
        //フロアで倒した敵を加算
        enemyNumCount++;
    }

}

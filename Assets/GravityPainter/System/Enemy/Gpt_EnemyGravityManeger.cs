using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gpt_EnemyGravityManeger : MonoBehaviour
{

    private List<Gpt_Enemy> EnemyList = new List<Gpt_Enemy>();
    private Gpt_Enemy FirstEnemy;
    private Vector3 FirstEnemyPosition;
    private List<Gpt_Exploder> ExplodeList = new List<Gpt_Exploder>();
    private List<Vector3> ExplodePosition = new List<Vector3>();

    private List<Gpt_PointEffect> PointList = new List<Gpt_PointEffect>();

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

    //爆風範囲
    public float explodeRange=7f;

    //各色の個数
    private List<int> colorNum=new List<int>();
    public List<int> recodeColorNum = new List<int>();

    public GameObject phaseObject;
    private Gpt_EnemyPhaseControl phaseControl;

    private bool[] hitColor=new bool[3];

    private int[] previousHitted;

    void Start()
    {
        phaseControl=phaseObject.GetComponent<Gpt_EnemyPhaseControl>();
        Application.targetFrameRate = 30; //30FPSに設定
        colorNum.Add(0);
        colorNum.Add(0);
        colorNum.Add(0);
        recodeColorNum.Add(0);
        recodeColorNum.Add(0);
        recodeColorNum.Add(0);
    }

    // Update is called once per frame
    void Update()
    {

        colorNum[0] = 0;
        colorNum[1] = 0;
        colorNum[2] = 0;
        //敵が一人以上いた時にカウントスタート
        if (EnemyList.Count > 0)
        {
            isFloor = true;
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
            for (int j = 0; j < EnemyList.Count; j++)
            {
                //色が付いた敵は、爆発オブジェクトの場所を登録
                if (ExplodeList[i].GetColor() == EnemyList[j].GetColor())
                {
                    EnemyList[j].SetExploderPosition(ExplodeList[i].transform.position);
                }
            }
        }
        int[] previousTemp =new int[EnemyList.Count];
        previousHitted = new int[EnemyList.Count];
        //距離判定
        //Debug.Log(previousHitted[1]);
        for (int i = 0; i < EnemyList.Count; i++)
        {
            EnemyList[i].SetUpTime(enemyUpTime);
            EnemyList[i].SetUnderTime(enemyUnderTime);

            int hitted = 0;
            
            previousTemp[i] = previousHitted[i];

            bool targetHit=false;

            for (int j = 0; j < EnemyList.Count; j++)
            {
                if (j == i) continue;


                if (Vector3.Distance(EnemyList[i].transform.position, EnemyList[j].transform.position) < gravityArea)
                {
                    if (EnemyList[i].GetColor() != 0 && EnemyList[i].GetColor() == EnemyList[j].GetColor())
                    {
                        hitted++;
                        //Debug.Log("Top!!");
                        //i番目へのベクトル
                        Vector3 objVec1 = EnemyList[i].transform.position - EnemyList[j].transform.position;

                        Vector3 objVec2 = EnemyList[j].transform.position - EnemyList[i].transform.position;

                        float topGravityPower = 2f;
                        float scaleA = EnemyList[j].IsTop ? topGravityPower : 0.8f;
                        Vector3 normVec1 = objVec1.normalized * scaleA;
                        Vector3 normVec2 = objVec2.normalized * scaleA;
                        if (EnemyList[i].GetShake() != EnemyList[j].GetShake())
                        {
                            if (EnemyList[i].GetShake())
                            {
                                EnemyList[i].SetGravity(normVec1);
                                EnemyList[j].SetGravity(normVec2);
                            }
                        }
                        else
                        {
                            EnemyList[i].SetGravity(normVec1);
                            EnemyList[j].SetGravity(normVec2);
                        }

                        //同時に2匹塗られた際に、Topが分からなくなるのを防ぐため
                        if (EnemyList[j].IsTop)
                        {
                            previousTemp[i]++;
                        }


                        /*---以前の処理--*/
                        //hitted++;
                        ////Debug.Log("Top!!");
                        ////i番目へのベクトル

                        //if (EnemyList[i].IsTop)
                        //{
                        //    //Vector3 objVec1 = EnemyList[i].transform.position - EnemyList[j].transform.position;
                        //    Vector3 objVec2 = EnemyList[j].transform.position - (EnemyList[i].transform.position + new Vector3(0, 1.5f, 0));

                        //    //Debug.Log("i:Top is "+EnemyList[i].name);
                        //    //float topGravityPower = 0.8f;
                        //    //float scaleA = EnemyList[j].IsTop ? topGravityPower : 0.4f;
                        //    Vector3 normVec2 = objVec2.normalized;
                        //    EnemyList[j].SetGravity(normVec2);
                        //}
                        //else if (EnemyList[j].IsTop)
                        //{
                        //    Vector3 objVec1 = EnemyList[i].transform.position - (EnemyList[j].transform.position + new Vector3(0, 1.5f, 0));
                        //    //Debug.Log("j:Top is " + EnemyList[j].name);
                        //    Vector3 normVec1 = objVec1.normalized;
                        //    EnemyList[i].SetGravity(normVec1);
                        //}
                        //else
                        //{

                        //}
                        /*---以前の処理--*/
                    }
                }

                
            }
            
            switch (EnemyList[i].GetColor())
            {
                case 1:
                    colorNum[0]++;
                    break;
                case 2:
                    colorNum[1]++;
                    break;
                case 3:
                    colorNum[2]++;
                    break;
            }

            if (previousTemp[i]==0 && EnemyList[i].GetColor() != 0 && !EnemyList[i].IsTop)
            {
                hitColor[0] = true;
                EnemyList[i].SetTop();
                break;
            }
            
            previousHitted[i] = hitted;
        }
        recodeColorNum[0] = colorNum[0];
        recodeColorNum[1] = colorNum[1];
        recodeColorNum[2] = colorNum[2];

        
        //敵が一定値に満ちたらドアが開く
        if (isFloor && enemyNumCount >= enemyNum)
        {
            if (doorSystem != null)
            {
                //doorSystem.OpenDoor();
            }

        }

        //ドアが開かれたらエフェクトを全て消す

        if (phaseControl.opended)
        {
            for (int i = 0; i < PointList.Count; i++)
            {
                PointList[i].isDelete = true;
            }
        }
        //Debug.Log(enemyNumCount);
    }


    public List<Gpt_Enemy> GetEnemyList()
    {
        return EnemyList;
    }

    public int GetRestEnemy()
    {
        return enemyNum - enemyNumCount;
    }

    public int GetEnemyNumCount()
    {
        return enemyNumCount;
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
                //爆破の範囲は地面から
                
                Vector3 exPosition = new Vector3(ExplodeList[i].gameObject.transform.position.x, EnemyList[j].gameObject.transform.position.y,
                                        ExplodeList[i].gameObject.transform.position.z);
                if (Vector3.Distance(exPosition, EnemyList[j].gameObject.transform.position) < explodeRange)
                {
                    Debug.Log("enemy: " + j);
                    //引力状態にない敵のみダメージ
                    if (!EnemyList[j].GetGravity())
                    {
                        
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

    public void AddPointList(Gpt_PointEffect point)
    {
        PointList.Add(point);
    }

    public void RemovePointList(Gpt_PointEffect point)
    {
        PointList.Remove(point);
    }
}

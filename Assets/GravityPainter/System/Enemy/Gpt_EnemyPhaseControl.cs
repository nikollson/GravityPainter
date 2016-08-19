using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Gpt_EnemyPhaseControl : MonoBehaviour {

    public Gpt_EnemyGravityManeger enemyGravityManager;
    public GameObject[] PhaseEnemyParent;
    public float loadEnemyTime = 3f;
    int currentfaseNum = -1;
    float loadPhaseCount = 0;
    bool loaded = false;

    float changeWait = 0.1f;

    void Update()
    {
        if (CanLoadPhase())
        {
            LoadPhase(currentfaseNum + 1);
        }

        if (currentfaseNum != -1)
        {
            UpdatePhase();
        }
    }

    bool CanLoadPhase()
    {
        changeWait -= Time.deltaTime;
        return changeWait < 0 && enemyGravityManager.GetEnemyList().Count <=1 && currentfaseNum + 1 < PhaseEnemyParent.Length;
    }

    void LoadPhase(int num)
    {
        changeWait = 1.0f;
        loadPhaseCount = 0;
        loaded = false;
        currentfaseNum = num;
    }

    void UpdatePhase()
    {
        loadPhaseCount += Time.deltaTime;
        if (!loaded && loadPhaseCount > loadEnemyTime)
        {
            loaded = true;
            PhaseEnemyParent[currentfaseNum].SetActive(true);
        }
    }

    bool IsEndPhase()
    {
        return changeWait < 0 && enemyGravityManager.GetEnemyList().Count <= 1;
    }

    public bool IsEndAllPhase()
    {
        return IsEndPhase() && currentfaseNum + 1 >= PhaseEnemyParent.Length;
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Gpt_EnemyPhaseControl : MonoBehaviour {

    public Gpt_EnemyGravityManeger enemyGravityManager;
    public PhaseInfo[] PhaseEnemyParent;
    public Gpt_DoorSystem doorSystem;

    bool opended = false;

    public float loadEnemyTime = 3f;
    int currentfaseNum = -1;
    float loadPhaseCount = 0;
    bool loaded = false;

    float changeWait = 0.1f;
    float changeWaitEPS = 0.2f;

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
        return changeWait < 0 && IsEndPhase() && currentfaseNum + 1 < PhaseEnemyParent.Length;
    }

    void LoadPhase(int num)
    {
        changeWait = loadEnemyTime + 0.2f;
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
            PhaseEnemyParent[currentfaseNum].parent.SetActive(true);
        }


        if (!opended && IsEndAllPhase())
        {
            doorSystem.OpenDoor();
            foreach(var a in enemyGravityManager.GetEnemyList())
            {
                Destroy(a.gameObject);
            }
            opended = true;
        }
    }

    bool IsEndPhase()
    {
        return currentfaseNum < 0 || enemyGravityManager.GetEnemyList().Count <= PhaseEnemyParent[currentfaseNum].changeNextRest;
    }

    public bool IsEndAllPhase()
    {
        return changeWait < 0 && IsEndPhase() && currentfaseNum + 1 >= PhaseEnemyParent.Length;
    }

    public int GetPhaseNum()
    {
        return PhaseEnemyParent.Length;
    }

    public int GetPhaseEnemyNum(int num)
    {
        if (num < currentfaseNum) return 0;
        if (num == currentfaseNum) return enemyGravityManager.GetEnemyList().Count;
        return PhaseEnemyParent[num].parent.transform.childCount;
    }

    public void RemoveAllEnemy()
    {
        var enemys = enemyGravityManager.GetEnemyList();
        foreach(var a in enemys)
        {
            Destroy(a.gameObject);
        }
    }

    [System.Serializable]
    public class PhaseInfo
    {
        public GameObject parent;
        public int changeNextRest;
    }
}

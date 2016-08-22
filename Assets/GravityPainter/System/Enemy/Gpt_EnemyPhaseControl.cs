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
        if (!opended && IsEndAllPhase())
        {
            DoEndPhase();
            opended = true;
        }

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
        return !IsEndAllPhase() && changeWait < 0 && IsEndPhase() && currentfaseNum + 1 < PhaseEnemyParent.Length;
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
    }

    bool IsEndPhase()
    {
        if (currentfaseNum < 0) return true;
        return enemyGravityManager.GetEnemyNumCount() >= PhaseEnemyParent[currentfaseNum].changeNextRest;
    }

    void DoEndPhase()
    {
        doorSystem.OpenDoor();
        foreach (var a in enemyGravityManager.GetEnemyList())
        {
            Destroy(a.gameObject);
        }
    }

    public bool IsEndAllPhase()
    {
        if (!(changeWait < 0)) return false;
        if (enemyGravityManager.GetEnemyNumCount() >= GetAllClearEnemyNum()) return true;
        if (currentfaseNum >= PhaseEnemyParent.Length) return true;
        return false;
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

    public int GetAllClearEnemyNum()
    {
        return PhaseEnemyParent[PhaseEnemyParent.Length-1].changeNextRest;
    }

    [System.Serializable]
    public class PhaseInfo
    {
        public GameObject parent;
        public int changeNextRest;
    }
}

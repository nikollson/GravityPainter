using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Gpt_EnemyPhaseControl : MonoBehaviour {

    public PhaseInfo[] PhaseEnemyParent;
    public Gpt_DoorSystem doorSystem;

    public Transform clearCameraPosition;
    public Transform clearCameraLook;

    public GameObject enemyNextPrefab;

    private Gpt_EnemyGravityManeger enemyGravityManager;
    private Gpt_YukaManager yukaManager;
    private Gpt_Camera camera;
    
    public bool opended { get; private set; }

    public float loadEnemyTime = 3f;
    int currentfaseNum = -1;
    float loadPhaseCount = 0;
    bool loaded = false;

    float changeWait = 0.1f;
    float changeWaitEPS = 0.2f;

    public float clearDoorTime = 3.0f;
    bool cleared = false;
    float clearCount = 0;
    bool opened = false;
    bool enemyRemoved = false;
    public float enemyRemoveTime = 2.5f;
    public float effectStartTime = 2.0f;
    bool effectStarted = false;

    public float shakeStartTime = 0.4f;
    public float shakeEndTime = 1.5f;
    public float shakePower = 0.5f;
    

    void Start()
    {
        enemyGravityManager = GameObject.FindObjectOfType<Gpt_EnemyGravityManeger>();
        yukaManager = GameObject.FindObjectOfType<Gpt_YukaManager>();
        camera = GameObject.FindObjectOfType<Gpt_Camera>();
    }

    void Update()
    {
        if (!opended && IsEndAllPhase())
        {
            DoEndAll();
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

        if (cleared)
        {
            clearCount += Time.deltaTime;
            if (!enemyRemoved && clearCount > enemyRemoveTime)
            {
                enemyRemoved = true;
                RemoveAllEnemy2();
            }
            if (clearCount > clearDoorTime)
            {
                doorSystem.OpenDoor();
            }
            if (shakeStartTime < clearCount && clearCount < shakeEndTime)
            {
                camera.SetScreenShake(shakePower);
            }
            if(!effectStarted && clearCount > effectStartTime)
            {
                RemoveAllEnemy1();
                effectStarted = true;
            }
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

    void DoEndAll()
    {
        
        yukaManager.MakeClaerFlush();
        camera.StartPositionLook(clearCameraPosition, clearCameraLook);
        cleared = true;
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

    public void RemoveAllEnemy1()
    {
        var enemys = enemyGravityManager.GetEnemyList();
        foreach (var a in enemys)
        {
            var obj = (GameObject)Instantiate(enemyNextPrefab, a.transform.position, Quaternion.identity);
            //obj.transform.parent = a.transform;
        }
    }
    public void RemoveAllEnemy2()
    {
        var enemys = enemyGravityManager.GetEnemyList();
        foreach (var a in enemys)
        {
            Destroy(a.gameObject);
        }
        foreach (var a in PhaseEnemyParent)
        {
            Destroy(a.parent);
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

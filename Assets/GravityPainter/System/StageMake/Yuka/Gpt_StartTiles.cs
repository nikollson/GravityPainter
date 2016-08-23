using UnityEngine;
using System.Collections;

public class Gpt_StartTiles : MonoBehaviour {

    public HitManager playerChecker;
    public Transform cameraPosition1;
    public Transform cameraLook1;
    public Transform cameraPosition2;
    public Transform cameraLook2;
    public Gpt_YukaParts yukaParent;
    public GameObject yukaWall;

    bool check = false;

    Gpt_YukaBox[] yukaBoxes;
    Gpt_Camera camera;
    Gpt_Player player;

    float count = 0;
    public float lookBackTime = 0.5f;
    public float lookEnemyTime = 2.0f;
    public float spawnEnemyTime = 3.0f;
    public float lookEndTime = 4.0f;
    bool lookBack = false;
    bool lookEnemy = false;
    bool spawnEnemy = false;
    bool lookEnd = false;

    Gpt_EnemyPhaseControl enemyControl;
    public Gpt_YukaBox[] ochiYukaList;
    public GameObject enterDoor;

    void Start()
    {
        yukaBoxes = yukaParent.GetComponentsInChildren<Gpt_YukaBox>();
        enemyControl = FindObjectOfType<Gpt_EnemyPhaseControl>();
        foreach(var a in yukaBoxes)
        {
            var yukaManager = FindObjectOfType<Gpt_YukaManager>();
            a.Setting(yukaManager.tileSetting, yukaParent, yukaParent.GetFirstColor());
            a.AddDamage(1);
        }
        player = FindObjectOfType<Gpt_Player>();
        camera = FindObjectOfType<Gpt_Camera>();
    }

    void Update()
    {
        check |= playerChecker.IsHit;

        if (check)
        {
            count += Time.deltaTime;
            if (!lookBack && count > lookBackTime)
            {
                lookBack = true;
                camera.StartPositionLook(cameraPosition1, cameraLook1);
                for(int i = 0; i < ochiYukaList.Length; i++)
                {
                    ochiYukaList[i].SetExplode(0.5f , 0.5f + i * 0.03f, 10000000, 100000000);
                }
                yukaWall.SetActive(true);
                player.canControl = false;
            }

            if(!lookEnemy && count > lookEnemyTime)
            {
                enemyControl.started = true;
                lookEnemy = true;
                camera.StartPositionLook(cameraPosition2, cameraLook2);
                enterDoor.SetActive(false);
            }

            if(!spawnEnemy && count > spawnEnemyTime)
            {
                spawnEnemy = true;

            }

            if(!lookEnd && count > lookEndTime)
            {
                lookEnd = true;
                camera.state = 0;
                player.canControl = true;

            }
        }
    }
}

using UnityEngine;
using System.Collections;

public class Gpt_EnemyPhaseChild : MonoBehaviour {

    public GameObject prefab;
    public GameObject effectPrefab;

    public float comeTimeSetting = 10.0f;
    public bool isRandomTimeCome = true;

    public bool bossBattleFlag = false;
    

    float count = 0;
    float comeTime = 0;
    bool made = false;
    GameObject effectObject;

    Gpt_YukaManager yukaManager;

    void Start()
    {
        comeTime = isRandomTimeCome ? Random.Range(0, comeTimeSetting) : comeTimeSetting;
        yukaManager = GameObject.FindWithTag("YukaManager").GetComponent<Gpt_YukaManager>();
        MakeGateEffect();
    }


    void Update()
    {
        count += Time.deltaTime;

        if (!bossBattleFlag)
        {
            if (!made && count > comeTime && HasTile())
            {
                made = true;
                MakeEnemy();
            }
        }else
        {
            if (!made && count > comeTime)
            {
                made = true;
                MakeEnemy();
            }
        }
    }

    bool HasTile()
    {
        return yukaManager.HasTile(this.transform.position);
    }

    void MakeEnemy()
    {
        var obj = (GameObject)Instantiate(prefab, this.transform.position, this.transform.rotation);
        obj.transform.parent = this.transform;
        Destroy(effectObject);
    }

    void MakeGateEffect()
    {
        effectObject = (GameObject)Instantiate(effectPrefab, this.transform.position, this.transform.rotation);
        effectObject.transform.parent = this.transform;
    }
}

using UnityEngine;
using System.Collections;

public class Gpt_EnemyPhaseChild : MonoBehaviour {

    public GameObject prefab;
    public GameObject effectPrefab;

    public float comeTimeSetting = 10.0f;
    public bool isRandomTimeCome = false;

    float count = 0;
    float comeTime = 0;
    bool made = false;

    void Start()
    {
        comeTime = isRandomTimeCome ? Random.Range(0, comeTimeSetting) : comeTimeSetting;
    }


    void Update()
    {
        count += Time.deltaTime;
        if(!made && count > comeTime)
        {
            made = true;
            MakeEnemy();
        }
    }

    void MakeEnemy()
    {
        var obj = (GameObject)Instantiate(prefab, this.transform.position, this.transform.rotation);
        obj.transform.parent = this.transform;
    }
}

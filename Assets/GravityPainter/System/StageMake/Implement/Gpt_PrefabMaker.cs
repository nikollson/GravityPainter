using UnityEngine;
using System.Collections;

public class Gpt_PrefabMaker : MonoBehaviour
{

    public GameObject prefab;
    public Transform[] makePosition;
    public HitManager hitManager;

    public float firstMakeTime = 2.0f;

    public bool makeMultipleObject = false;
    public float makeMultipleTime = 8.0f;

    public bool resqureMode = false;

    float count = 0;
    float nextMakeTime = 0;
    float INF = 1000000000;

    void Start()
    {
        nextMakeTime = firstMakeTime;
    }

    void Update()
    {
        if (hitManager == null || hitManager.IsHit)
        {
            count += Time.deltaTime;

            if (!resqureMode) {
                if (count > nextMakeTime)
                {
                    foreach (var a in makePosition)
                    {
                        var obj = (GameObject)Instantiate(prefab, a.position, Quaternion.identity, this.transform);
                    }

                    if (makeMultipleObject)
                    {
                        nextMakeTime += makeMultipleTime;
                    }
                    if (!makeMultipleObject)
                    {
                        nextMakeTime = INF;
                    }
                }
            }
        }
    }
}

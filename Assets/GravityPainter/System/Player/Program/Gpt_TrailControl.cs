using UnityEngine;
using System.Collections;

public class Gpt_TrailControl : MonoBehaviour {
    
    public GameObject trailPrefab;
    bool trailOn = false;

    GameObject trailObject;

    void Update()
    {

    }

    public void StartTrail()
    {
        if (trailObject == null)
        {
            trailObject = (GameObject)Instantiate(trailPrefab, this.transform.position, Quaternion.identity);
            trailObject.transform.parent = this.transform;
        }
    }

    public void EndTrail()
    {
        if(trailObject != null)
        {
            Destroy(trailObject);
        }
    }
}

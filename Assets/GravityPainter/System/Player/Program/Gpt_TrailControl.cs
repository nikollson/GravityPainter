using UnityEngine;
using System.Collections;

public class Gpt_TrailControl : MonoBehaviour {

    public GameObject trailPrefabRed;
    public GameObject trailPrefabBlue;
    public GameObject trailPrefabYellow;
    bool trailOn = false;

    GameObject trailObject;
    Gpt_InkColor currentColor = Gpt_InkColor.NONE;

    void Update()
    {

    }

    public void StartTrail(Gpt_InkColor color)
    {
        if (trailObject == null || color != currentColor)
        {
            currentColor = color;
            if (trailObject != null) EndTrail();

            MakeTrail(color);
        }
    }

    void MakeTrail(Gpt_InkColor color)
    {
        GameObject prefab = trailPrefabRed;
        if (color == Gpt_InkColor.BLUE) prefab = trailPrefabBlue;
        if (color == Gpt_InkColor.YELLOW) prefab = trailPrefabYellow;
        trailObject = (GameObject)Instantiate(prefab, this.transform.position, Quaternion.identity);
        trailObject.transform.parent = this.transform;
    }

    public void EndTrail()
    {
        if(trailObject != null)
        {
            Destroy(trailObject);
        }
    }
}

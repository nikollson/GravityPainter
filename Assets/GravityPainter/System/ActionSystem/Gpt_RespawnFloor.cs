using UnityEngine;
using System.Collections;

public class Gpt_RespawnFloor : MonoBehaviour {

    public HitManager hitManager;

    bool hitOnce = false;

    void Update()
    {
        if (hitManager.IsHit) hitOnce = true;
        if(hitOnce && !hitManager.IsHit)
        {
            Destroy(this.gameObject);
        }
    }
}

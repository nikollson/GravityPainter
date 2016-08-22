using UnityEngine;
using System.Collections;

public class Gpt_RespawnFloor : MonoBehaviour {

    public HitManager hitManager;
    public float deleteTime = 4f;
    public float deleteMinTime = 1.0f;
    public Renderer meshRenderer;

    public float flushAngle = 0.3f;
    public float alphaMin = 0.3f;
    public float alphaMax = 0.6f;

    bool hitOnce = false;
    float count = 0;
    void Update()
    {
        count += Time.deltaTime;
        

        if (hitManager.IsHit) hitOnce = true;
        if (count > deleteMinTime)
        {
            if (count > deleteTime || (hitOnce && !hitManager.IsHit))
            {
                Destroy(this.gameObject);
            }
        }

    }
}

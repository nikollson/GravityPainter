using UnityEngine;
using System.Collections;

public class Gpt_PointEffect : MonoBehaviour {
    public ParticleSystem ps;

    //止まる時間
    public float stopTime = 4f;
    private float stop;
    // Use this for initialization
    void Start()
    {
        


    }
	// Update is called once per frame
	void Update () {

        stop += 0.1f;
        if (stop > stopTime)
        {
            ps.Pause();
        }
    }
}

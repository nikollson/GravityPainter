using UnityEngine;
using System.Collections;

public class Gpt_PointEffect : MonoBehaviour {
    public ParticleSystem ps;
    public ParticleSystem ps2;

    //止まる時間
    public float stopTime = 4f;
    private float stop;

    public bool isDelete { get; set; }

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

        if (isDelete)
        {
            Debug.Log("afafas");
            ps.Play();
            ps2.Stop();
            Object.Destroy(this.gameObject, 4f);
        }
    }
}

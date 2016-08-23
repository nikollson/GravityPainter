using UnityEngine;
using System.Collections;

public class Gpt_PointEffect : MonoBehaviour {
    public ParticleSystem waveStop;
    public ParticleSystem waveStop2;

    public ParticleSystem lightLine;
    public ParticleSystem lightCircle;

    //止まる時間
    public float stopTime = 4f;
    private float stop;

    private GameObject ManegerObject;
    private Gpt_EnemyGravityManeger EnemyGravityManeger;
    public bool isStart { get; set; }
    public bool isDelete { get; set; }
    public bool isStageClear { get; set; }
    // Use this for initialization
    void Start()
    {
        ManegerObject = GameObject.Find("GravityManeger");
        EnemyGravityManeger = ManegerObject.GetComponent<Gpt_EnemyGravityManeger>();
        EnemyGravityManeger.AddPointList(this);
        //何故かパーティクルが原点に来るため
        lightLine.transform.position = this.gameObject.transform.position-new Vector3(0,1f,0);
        lightCircle.transform.position = this.gameObject.transform.position;
        waveStop.Stop();
        waveStop2.Stop();
        
    }
    bool temp;
    float temp2;
	// Update is called once per frame
	void Update () {
        lightLine.transform.position = this.gameObject.transform.position - new Vector3(0, 1f, 0); ;
        lightCircle.transform.position = this.gameObject.transform.position;
        if (isStart)
        {
            stop += 0.1f;
            waveStop.Play();
            waveStop2.Play();
            if (stop > stopTime)
            {
                waveStop.Pause();
            }

            if (isDelete)
            {
                waveStop.Play();
                waveStop2.Stop();
                lightLine.Stop();
                lightCircle.Stop();
                Object.Destroy(this.gameObject, 4f);
                temp = true;
            }

            if (temp)
            {
                temp2 += 0.1f;
                if (temp2 > 2f)
                {
                    waveStop.Pause();
                }
            }

        }

        if (!isStart&&isDelete)
        {
            lightLine.Stop();
            lightCircle.Stop();
            Object.Destroy(this.gameObject, 4f);
            //temp = true;
        }
    }

    void Ondestroy()
    {
        EnemyGravityManeger.RemovePointList(this);
    }
}

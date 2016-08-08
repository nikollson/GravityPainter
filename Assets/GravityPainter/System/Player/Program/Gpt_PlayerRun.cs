using UnityEngine;
using System.Collections;

public class Gpt_PlayerRun : MonoBehaviour
{
    public Gpt_PlayerUtillity playerUtillity;
    public float speed = 0.8f;
    bool isRunning = false;

    public void StartRun()
    {
        isRunning = true;
    }

    public void EndRun()
    {
        isRunning = false;
    }

    void Update()
    {
        if (isRunning)
        {
            Vector3 move = speed * playerUtillity.GetAnalogpadMove();

            this.transform.position += move;
            float angle = Mathf.Atan2(move.z, move.x);
            this.transform.rotation = Quaternion.Euler(new Vector3(0, radToDigree(-angle), 0));
        }
    }


    float radToDigree(float rad)
    {
        return rad / Mathf.PI * 180;
    }


}

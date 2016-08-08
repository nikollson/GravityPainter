using UnityEngine;
using System.Collections;

public class Gpt_PlayerRun : MonoBehaviour
{
    public Gpt_Player player;
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
        Debug.Log("is Running " + isRunning);
        if (isRunning)
        {
            Vector3 camForward = player.camera.transform.forward;
            camForward.y = 0;
            camForward = camForward.normalized;
            Vector3 camRight = new Vector3(camForward.z, 0, -camForward.x);

            Vector3 move = speed * (camForward * Gpt_Input.Move.y + camRight * Gpt_Input.Move.x);

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

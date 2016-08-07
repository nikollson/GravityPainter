using UnityEngine;
using System.Collections;

public class Gpt_Player : MonoBehaviour {
    
    public Gpt_Camera camera;
    public float speed = 1.0f;
    public HitManager attackHitManager;

    public Gpt_PlayerBodyColor playerBodyColor;

    public enum MODE { WAIT, RUN, ATTACK, ROT, SKILL, JUMP };
    public enum FEEVER_MODE { NONE, FEEVER};
    public enum ATTACK_MODE { RIGHT, LEFT }
    
    void Update()
    {
        Update_Move();
        Update_Attack();
    }

    void Update_Attack()
    {
        if (Gpt_Input.Attack)
        {
            IsAttacking = true;
            string log = "";
            foreach(var a in attackHitManager.HitColliders)
            {
                log += a.name;
            }
            Debug.Log(log);
        }
        else
        {
            IsAttacking = false;
        }
    }

    void Update_Move()
    {
        float EPS = 0.001f;
        bool isMoving = Gpt_Input.Move.magnitude > EPS;

        if (isMoving)
        {
            Vector3 camForward = camera.transform.forward;
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

    public bool IsRunning
    {
        get
        {
            float EPS = 0.001f;
            return Gpt_Input.Move.magnitude > EPS;
        }
    }
    public bool IsAttacking { get; private set; }
}

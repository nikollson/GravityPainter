using UnityEngine;
using System.Collections;

public class Gpt_PlayerUtillity : MonoBehaviour
{

    public Gpt_Camera camera;
    public HitManager footCollider;
    public float ignoreFootColliderTime = 0.1f;
    float ignoreFootColliderCount = 100;

    void Update()
    {
        ignoreFootColliderCount += Time.deltaTime;
    }

    public Vector3 GetAnalogpadMove()
    {
        Vector3 camForward = camera.transform.forward;
        camForward.y = 0;
        camForward = camForward.normalized;
        Vector3 camRight = new Vector3(camForward.z, 0, -camForward.x);

        return camForward * Gpt_Input.Move.y + camRight * Gpt_Input.Move.x;
    }

    public bool IsGround()
    {
        return footCollider.IsHit && ignoreFootColliderTime < ignoreFootColliderCount;
    }

    public void IgnoreFootCollider()
    {
        ignoreFootColliderCount = 0;
    }

    public void LookAnalogpadDirction()
    {
        if (HasAnalogpadMove())
        {
            Vector3 analogPadMove = GetAnalogpadMove();
            float angle = Mathf.Atan2(analogPadMove.z, analogPadMove.x);
            this.transform.rotation = Quaternion.Euler(new Vector3(0, radToDigree(-angle), 0));
        }
    }

    public bool HasAnalogpadMove()
    {
        float EPS = 0.0001f;
        return GetAnalogpadMove().magnitude > EPS;
    }

    float radToDigree(float rad)
    {
        return rad / Mathf.PI * 180;
    }
}

using UnityEngine;
using System.Collections;

public class Gpt_PlayerUtillity : MonoBehaviour {

    public Gpt_Camera camera;

    public Vector3 GetAnalogpadMove()
    {
        Vector3 camForward = camera.transform.forward;
        camForward.y = 0;
        camForward = camForward.normalized;
        Vector3 camRight = new Vector3(camForward.z, 0, -camForward.x);

        return camForward * Gpt_Input.Move.y + camRight * Gpt_Input.Move.x;
    }
}

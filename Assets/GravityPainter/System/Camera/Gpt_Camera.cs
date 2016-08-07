using UnityEngine;
using System.Collections;

public class Gpt_Camera : MonoBehaviour {

    public Transform lookTransform;
    public Transform placeTransform;

    public float distance = 5.0f;
    public float angleDown = 60;
    public float startRot = 0;
    public float rotSpeed = 3f;

    float rot = 180;

    void Start()
    {
        rot = startRot;
    }


    void OnPreRender()
    {
        Update_Rotation();
        Update_Position();
        Update_Look();
    }
    

    void Update_Rotation()
    {
        rot -= Gpt_Input.CamMove.x * digToRad(rotSpeed);
    }

    void Update_Position()
    {
        float y = Mathf.Sin(digToRad(angleDown));
        float distGround = Mathf.Cos(digToRad(angleDown));
        float x = distGround * Mathf.Cos(rot);
        float z = distGround * Mathf.Sin(rot);

        this.transform.position = placeTransform.position + distance * new Vector3(x, y, z);
    }

    void Update_Look()
    {
        this.transform.LookAt(lookTransform.position);
    }

    float digToRad(float dig)
    {
        return dig / 180 * Mathf.PI;
    }
}

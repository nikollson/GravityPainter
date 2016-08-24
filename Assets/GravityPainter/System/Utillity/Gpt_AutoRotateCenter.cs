using UnityEngine;
using System.Collections;

public class Gpt_AutoRotateCenter : MonoBehaviour {

    public Transform rotateCenter;
    public float speed = 60;

    public float currentAngle = 0;

    Vector3 startPosition;

    void Start()
    {
        startPosition = rotateCenter.transform.position;
        startPosition.y = this.transform.position.y;
        Vector3 dist = startPosition - this.transform.position;
        dist = (dist - new Vector3(0, dist.y, 0)).normalized;
        currentAngle = Mathf.Atan2(dist.z, dist.x);
    }

    void Update()
    {
        Vector3 dist = startPosition - this.transform.position;

        currentAngle += digreeToRadian(speed * Time.deltaTime);

        Vector3 ndist = new Vector3(Mathf.Cos(currentAngle), 0, Mathf.Sin(currentAngle)) * dist.magnitude;
        this.transform.position = ndist + startPosition;
    }


    float digreeToRadian(float a)
    {
        return a / 180 * Mathf.PI;
    }
}

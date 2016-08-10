using UnityEngine;
using System.Collections;

public class Gpt_JumperEnemy : MonoBehaviour {

    public HitManager footCollider;
    public new Rigidbody rigidbody;

    public float jumpPower = 20;
    public float jumpPowerFront = 10;

    public float airFrictionXZ = 20;
    public float down = 100;

    void Update()
    {
        if (IsGround())
        {
            Vector3 power = (jumpPowerFront * this.transform.forward) + (jumpPower * new Vector3(0, 1, 0));
            rigidbody.AddForce(power, ForceMode.VelocityChange);
        }

        if (!IsGround())
        {
            Vector3 frictionPower = -1 * airFrictionXZ * (rigidbody.velocity - new Vector3(0, rigidbody.velocity.y, 0));
            Vector3 downPowerVector = down * new Vector3(0, -1, 0);

            Vector3 power = frictionPower + downPowerVector;

            rigidbody.AddForce(power * Time.deltaTime, ForceMode.Acceleration);
        }

        if (footCollider.IsHit)
        {
            string aa = "";
            foreach (var a in footCollider.HitColliders)
            {
                aa += a.gameObject.name;
            }
        }
    }

    bool IsGround()
    {
        return footCollider.IsHit;
    }
}

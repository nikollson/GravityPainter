﻿using UnityEngine;
using System.Collections;

public class Gpt_PlayerAir : MonoBehaviour {

    public Gpt_PlayerUtillity playerUtillity;
    public new Rigidbody rigidbody;
    public float force = 200f;
    public float friction = 10;

    bool isAir = false;

    public void StartAir()
    {
        isAir = true;
    }

    public void EndAir()
    {
        isAir = false;
    }


    void Update()
    {
        if (isAir)
        {
            Vector3 input = playerUtillity.GetAnalogpadMove();

            Vector3 power = input * force;
            Vector3 frictionPower = -1 * friction * (rigidbody.velocity - new Vector3(0, -rigidbody.velocity.y, 0));

            rigidbody.AddForce(Time.deltaTime * (power + frictionPower), ForceMode.Acceleration);
        }
    }
}
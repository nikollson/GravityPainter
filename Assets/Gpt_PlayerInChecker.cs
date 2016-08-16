﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Gpt_PlayerInChecker : MonoBehaviour
{
    public int goSceneNum;

    void Start()
    {
    }

    void Update()
    {
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Player")
        {
            SceneManager.LoadScene(goSceneNum);
        }
    }
}
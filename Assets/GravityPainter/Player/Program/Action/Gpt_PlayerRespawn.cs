﻿using UnityEngine;
using System.Collections;

public class Gpt_PlayerRespawn : MonoBehaviour {

    public GameObject respawnFloorPrefab;
    public Transform respawnFloorPosition;
    public float cantMoveTime = 0.3f;
    public Gpt_Player player;

    Vector3 respawnPosition;
    float count = 0;

    const float INF = 1000000000;

    void Start()
    {

        respawnPosition = respawnFloorPosition.position;
        count = INF;
    }

    public void DoRespawn()
    {
        MakeFloor();
        count = 0;
        player.canControl = false;
    }

    public void MakeFloor()
    {
        Instantiate(respawnFloorPrefab, respawnPosition, Quaternion.identity);
    }

    void Update()
    {
        count += Time.deltaTime;
        if (count > cantMoveTime) player.canControl = true;
    }
}
using UnityEngine;
using System.Collections;

public class Gpt_PlayerRespawn : MonoBehaviour {
    
    public float cantMoveTime = 0.3f;
    public Gpt_Player player;

    Vector3 respawnPosition;
    float count = 0;
    bool respawned = false;

    const float INF = 1000000000;

    void Start()
    {
        count = INF;
    }

    public void DoRespawn()
    {
        count = 0;
        player.canControl = false;
        respawned = false;
    }

    void Update()
    {
        count += Time.deltaTime;
        if (!respawned && count > cantMoveTime)
        {
            player.canControl = true;
            respawned = true;
        }
    }
}

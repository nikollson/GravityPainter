using UnityEngine;
using System.Collections;

public class Gpt_PlayerRespawnArea : MonoBehaviour
{
    public GameObject respawnFloorPrefab;
    public Transform respawnFloorPosition;

    public HitManager hitManager;
    public Transform respawnPosition;


    void Update()
    {
        if (hitManager.IsHit)
        {
            var player = Gpt_ParentTracker.Track<Gpt_Player>(hitManager.HitCollider.gameObject);
            if (player != null)
            {
                MakeFloor();
                player.DoRespawn(respawnPosition.position);
            }
        }
    }

    public void MakeFloor()
    {
        Instantiate(respawnFloorPrefab, respawnFloorPosition.position, Quaternion.identity);
    }

}

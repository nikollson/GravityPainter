using UnityEngine;
using System.Collections;

public class Gpt_PlayerRespawnArea : MonoBehaviour
{
    public HitManager hitManager;
    public Transform respawnPosition;


    void Update()
    {
        if (hitManager.IsHit)
        {
            var player = Gpt_ParentTracker.Track<Gpt_Player>(hitManager.HitCollider.gameObject);
            if (player != null)
            {
                player.DoRespawn(respawnPosition.position);
            }
        }
    }

}

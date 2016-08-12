using UnityEngine;
using System.Collections;

public class Gpt_PlayerDamageCollider : MonoBehaviour
{

    public HitManager attackCollider;
    public int damage = 3;

    string playerTag = "Player";

    void Update()
    {
        foreach (var a in attackCollider.HitColliders)
        {
            if (a.tag == playerTag)
            {
                var playerState = Gpt_ParentTracker.Track<Gpt_PlayerState>(a.gameObject);
                if (playerState != null)
                {
                    playerState.AddHPDamage(damage);
                }
            }
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gpt_PlayerAttackState : MonoBehaviour
{

    public Gpt_PlayerState playerState;
    public HitManager attackCollider;
    public string enemyTag = "Enemy";

    public void UpdateAttackState()
    {
        if (attackCollider.IsHit)
        {
            foreach (var a in attackCollider.HitColliders)
            {
                if (a.gameObject.tag == enemyTag)
                {
                    var enemyColor = Gpt_ParentTracker.Track<Gpt_EnemyColor>(a.gameObject);
                    if (enemyColor != null) DrawEnemy(enemyColor);
                }
            }
        }
    }

    void DrawEnemy(Gpt_EnemyColor enemyColor)
    {
        if (enemyColor.GetColor() == 0 && enemyColor.GetColor() != (int)playerState.PlayerColor)
        {
            enemyColor.SetColor((int)playerState.PlayerColor);
            playerState.AddPlayerColorCombo();
        }
    }
}



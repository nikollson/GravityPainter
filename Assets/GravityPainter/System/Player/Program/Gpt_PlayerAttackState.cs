using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gpt_PlayerAttackState : MonoBehaviour {

    public HitManager attackCollider;
    public string enemyTag = "Enemy";

    public void UpdateAttackState()
    {
        if (attackCollider.IsHit)
        {
            List<Gpt_EnemyColor> scripts = new List<Gpt_EnemyColor>();

            foreach (var a in attackCollider.HitColliders)
            {
                if(a.gameObject.tag == enemyTag)
                {
                    var parentTracker = a.gameObject.GetComponent<Gpt_ParentTracker>();
                    if (parentTracker == null) continue;

                    var enemyColor = parentTracker.parentObject.GetComponent<Gpt_EnemyColor>();
                    if (enemyColor != null) scripts.Add(enemyColor);
                }
            }

            foreach(var a in scripts)
            {
                a.SetColor(2);
            }
        }
    }
}

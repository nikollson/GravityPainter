using UnityEngine;
using System.Collections;

public class Gpt_HitOrEnemy : HitOR
{
    string enemyTag = "Enemy";

    public override bool OR(Collider collider)
    {
        return collider.gameObject.tag == enemyTag;
    }
}

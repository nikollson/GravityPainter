using UnityEngine;
using System.Collections;

public class Gpt_HitOrPlayer : HitOR
{
    string playerTag = "Player";

    public override bool OR(Collider collider)
    {
        return collider.gameObject.tag == playerTag;
    }
}

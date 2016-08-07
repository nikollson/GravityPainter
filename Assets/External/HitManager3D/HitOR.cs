using UnityEngine;
using System.Collections;

public class HitOR : MonoBehaviour {

    virtual public bool OR(Collider collider) { return true; }
    virtual public bool IgnoreOR(Collider collider) { return OR(collider); }
}

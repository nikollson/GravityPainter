using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HitManager : MonoBehaviour
{

    public enum Mode { Normal, OnceHitRespawn, OnceHitEver}
    public Mode mode;
    public HitManager[] child;
    public Collider[] ignoreCollider;
    public bool isIgnoreTrigger = false;
    public bool isIgnoreCollider = false;

    List<Data> dict = new List<Data>();

    HitAND[] detectAnd;
    HitOR[] detectOr;

    new Collider collider;

    virtual public bool IsHit
    {
        get
        {
            RemoveEndData();
            bool res = DoDetect ? dict.Count >= 1 : false;
            if (child != null) foreach (var a in child) if (a != null) res |= a.IsHit;
            onceHit |= res;
            onceHitEver |= res;
            if (mode == Mode.OnceHitEver) return onceHitEver;
            if (mode == Mode.OnceHitRespawn) return onceHit;
            return res;
        }
    }
    public List<Data> CollisionData { get { return dict; } }
    public Collider HitCollider { get { return IsHit ? dict[0].collider : null; } }
    [HideInInspector] public bool DoDetect = true;
    public List<Collider> HitColliders
    {
        get
        {
            var ret = new List<Collider>();
            foreach (var a in dict) ret.Add(a.collider);
            return ret;
        }
    }
    public int UpdatedFrame { get; private set; }

    int timeStamp = -1;
    bool onceHit = false;
    bool onceHitEver = false;

    void Start()
    {
        detectAnd = this.GetComponents<HitAND>();
        detectOr = this.GetComponents<HitOR>();
        collider = this.GetComponent<Collider>();

        foreach(var a in ignoreCollider)
        {
            Physics.IgnoreCollision(collider, a);
        }
    }

    public void Respawn()
    {
        if (mode == Mode.OnceHitRespawn)
        {
            onceHit = false;
        }
    }

    void Stay(Data data)
    {
        UpdatedFrame = Time.frameCount;
        bool hit = false;
        foreach (var a in dict)
        {
            if (a.collider == data.collider)
            {
                a.Update();
                hit = true;
            }
        }
        if (!hit) { dict.Add(data); }
        TimeStamp();
    }
    void Exit(Data data)
    {
        for (int i = 0; i < dict.Count; i++)
        {
            if (dict[i].collider == data.collider) { 
                
                dict.RemoveAt(i); i--;
            }
        }
        TimeStamp();
    }

    void LateUpdate()
    {
        RemoveEndData(true);
    }

    void RemoveEndData(bool debug=false)
    {
        for (int i = 0; i < dict.Count; i++)
        {
            if (dict[i].IsEnd(timeStamp)) { dict.RemoveAt(i); i--; }
        }
    }

    //enter -> exitが1フレームで呼ばれるとき対策
    public void OnTriggerEnter(Collider other) { OnTriggerStay(other); }
    public void OnCollisionEnter(Collision other) { OnCollisionStay(other); }

    public void OnTriggerStay(Collider other)
    {
        if (IsIgnore(other)) { AddIgnore(other); return; }
        if (IsCurrentIgrnore(other)) { return; }
        //float t1 = Time.realtimeSinceStartup;
        Vector3 centerPoint = GetCenterPoint(other);
        //float v1 = Time.realtimeSinceStartup - t1;
        //float t2 = Time.realtimeSinceStartup;
        Data data = new Data(other, centerPoint, false);
        Stay(data);
        //float v2 = Time.realtimeSinceStartup - t2;
        //if (this.name == "水エリア") Debug.Log(this.name + " " + Time.frameCount + " " + other.name + " " + other.gameObject.name+" "+v1.ToString("0.000000")+" "+v2.ToString("0.000000"));
    }
    public void OnCollisionStay(Collision other)
    {
        if (IsIgnore(other.collider)) { AddIgnore(other.collider); return; }
        Data data = new Data(other.collider, GetContactPoint(other), true);
        Stay(data);
    }
    void OnTriggerExit(Collider other)
    {
        Exit(new Data(other, GetCenterPoint(other), false));
    }
    void OnCollisionExit(Collision other)
    {
        Exit(new Data(other.collider, GetContactPoint(other), true));
    }

    void TimeStamp()
    {
        timeStamp = Time.frameCount;
    }

    Vector3 GetVelocity(Collider collider)
    {
        var rigidbody = collider.attachedRigidbody;// .GetComponent<Rigidbody2D>();
        return (rigidbody != null) ? rigidbody.velocity : Vector3.zero;
    }

    Vector3 GetCenterPoint(Collider other)
    {
        return (GetColliderCenter(other) + GetColliderCenter(collider)) / 2;
    }

    Vector3 GetColliderCenter(Collider collider)
    {
        return (Vector3)(collider.gameObject.transform.position);// + collider.center;
    }

    Vector3 GetContactPoint(Collision other)
    {
        Vector3 ret = Vector3.zero;
        foreach (var a in other.contacts) if (a.otherCollider == this.collider) return a.point;
        return ret;
    }

    public bool IsCurrentIgrnore(Collider collider)
    {
        bool andOK = true;
        foreach (var a in detectAnd) andOK &= a.AND(collider);

        bool orOK = (detectOr.Length == 0) ? true : false;
        foreach (var a in detectOr) orOK |= a.OR(collider);

        return !andOK || !orOK;
    }

    public bool IsIgnore(Collider collider)
    {
        bool andOK = true;
        foreach (var a in detectAnd) andOK &= a.IgnoreAND(collider);
        andOK &= !(collider.isTrigger && isIgnoreTrigger);
        andOK &= !(!collider.isTrigger && isIgnoreCollider);

        bool orOK = (detectOr.Length == 0) ? true : false;
        foreach (var a in detectOr) orOK |= a.IgnoreOR(collider);


        return !andOK || !orOK;
    }

    public void AddIgnore(Collider collider)
    {
        Physics.IgnoreCollision(this.collider, collider);
    }

    // data class

    public class Data
    {
        public Collider collider;
        public Vector3 hitPosition;
        public Vector3 hitVelocity;
        public int lastStayFrame;
        public bool isCollision;
        public Data(Collider collider, Vector3 hitPosition, bool isCollision)
        {
            this.collider = collider;
            this.hitPosition = hitPosition;
            this.isCollision = isCollision;
            lastStayFrame = Time.frameCount;
        }
        public void Update() { lastStayFrame = Time.frameCount; }
        public bool IsEnd(int stampFrame) { return lastStayFrame != stampFrame || collider == null || collider.gameObject == null || collider.enabled == false || collider.gameObject.activeSelf == false; }
    }
}

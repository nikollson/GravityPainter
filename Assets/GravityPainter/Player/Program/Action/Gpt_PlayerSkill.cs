using UnityEngine;
using System.Collections;

public class Gpt_PlayerSkill : MonoBehaviour
{
    public Gpt_PlayerUtillity playerUtillity;
    public new Rigidbody rigidbody;

    public RedSkill redSkill;
    public BlueSkill blueSkill;
    public YellowSkill yellowSkill;
    public RainbowSkill rainbowSkill;

    public Gpt_InkColor Color { get; private set; }

    SkillBase currentSkill;
    float skillCount = 0;
    float skillTime = 0;

    int inputFrame_log = 0;

    public bool CanStartSkill(int inputFrame)
    {
        return inputFrame != inputFrame_log;
    }

    public void StartSkill(int inputFrame, Gpt_InkColor color)
    {
        inputFrame_log = inputFrame;

        skillCount = 0;
        Color = color;

        currentSkill = GetColorSkill(color);
        
        currentSkill.Init(rigidbody, playerUtillity);
        currentSkill.Start();
    }

    public bool IsEndSkill()
    {
        return currentSkill.IsEnd();
    }

    public bool CanEndSkill(bool hasInput, int inputFrame)
    {
        return (!hasInput || inputFrame!=inputFrame_log) && currentSkill.CanEnd();
    }

    public void EndSkill()
    {
        currentSkill.End();

        Color = Gpt_InkColor.NONE;
        currentSkill = null;
    }

    public void UpdateSkill()
    {
        skillCount += Time.deltaTime;
        currentSkill.Update();
    }

    public float GetUseInk(Gpt_InkColor color)
    {
        return GetColorSkill(color).useInk;
    }

    public float GetUseInkParsSec(Gpt_InkColor color)
    {
        return GetColorSkill(color).useInkParSec;
    }

    SkillBase GetColorSkill(Gpt_InkColor color)
    {
        if (color == Gpt_InkColor.RED) return redSkill;
        if (color == Gpt_InkColor.BLUE) return redSkill;
        if (color == Gpt_InkColor.YELLOW) return redSkill;
        if (color == Gpt_InkColor.RAINBOW) return rainbowSkill;
        return redSkill;
    }


    [System.Serializable]
    public class RedSkill : SkillBase
    {

        public float endTime = 1.0f;
        public float canEndTime = 0.2f;

        public float friction = 200;
        public float startVelocity = 5;
        public float accelPower = 500;

        float count = 0;

        public override void Start()
        {
            Vector3 force = startVelocity * playerUtillity.GetAnalogpadMove() - rigidbody.velocity;
            rigidbody.AddForce(force, ForceMode.VelocityChange);
            count = 0;
        }
        public override void Update()
        {
            count += Time.deltaTime;

            Vector3 dir = playerUtillity.HasAnalogpadMove() ? playerUtillity.GetAnalogpadMove() : rigidbody.transform.forward;
            Vector3 force = accelPower * dir - friction * (rigidbody.velocity - new Vector3(0, rigidbody.velocity.y, 0));
            rigidbody.AddForce(force, ForceMode.Acceleration);

            playerUtillity.LookAnalogpadDirction();
        }
        public override void End()
        {

        }

        public override bool CanEnd()
        {
            return count > canEndTime;
        }

        public override bool IsEnd()
        {
            return count > endTime;
        }
    }

    [System.Serializable]
    public class BlueSkill : SkillBase
    {
        public override void Start()
        {

        }
        public override void Update()
        {

        }
    }

    [System.Serializable]
    public class YellowSkill : SkillBase
    {
        public override void Start()
        {

        }
        public override void Update()
        {

        }
    }

    [System.Serializable]
    public class RainbowSkill : SkillBase
    {

    }


    [System.Serializable]
    public class SkillBase
    {
        protected Rigidbody rigidbody;
        protected Gpt_PlayerUtillity playerUtillity;
        
        public float useInk = 0.04f;
        public float useInkParSec = 0;

        public void Init(Rigidbody rigidbody, Gpt_PlayerUtillity playerUtillity)
        {
            this.rigidbody = rigidbody;
            this.playerUtillity = playerUtillity;
        }
        public virtual void Start() { }
        public virtual void Update() { }
        public virtual void End() { }
        public virtual bool CanEnd() { return true; }
        public virtual bool IsEnd() { return true; }
    }
}

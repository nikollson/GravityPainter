using UnityEngine;
using System.Collections;

public class Gpt_PlayerInkManage : MonoBehaviour
{
    public Gpt_PlayerSkill playerSkill;

    public float inkMax = 1;
    public float inkStart = 0.33f;

    public float attack = 0.03f;
    public float detonate = 0.1f;

    public float autoHealPar = 0.5f;
    public float autoHealSpeed = 0.2f;
    
    public float detonateHeal1 = 0.04f;
    public float detonateHeal2 = 0.001f;


    public float RestInk { get; private set; }

    void Start()
    {
        RestInk = inkStart;
    }
    
    public void AddInk(float value) { RestInk = Mathf.Min(inkMax, Mathf.Max(0, RestInk + value)); }
    public void ConsumeInk(float value) { AddInk(-value); }

    public bool CanUseAttack() { return RestInk >= attack; }
    public bool CanUseSkill(Gpt_InkColor color) { return RestInk >= GetInkAmount(color); }
    public bool CanUseDetonate() { return RestInk >= detonate; }

    public void UseAttak() { ConsumeInk(attack); }
    public void UseSkill(Gpt_InkColor color) { ConsumeInk(GetInkAmount(color)); }
    public void UseDetonate() { ConsumeInk(detonate); }


    public void UseSkillParSec(Gpt_InkColor color)
    {
        ConsumeInk(GetInkAmountParSec(color) * Time.deltaTime);
    }

    public bool CanContinueSkill(Gpt_InkColor color)
    {
        return RestInk > GetInkAmountParSec(color);
    }

    float GetInkAmount(Gpt_InkColor color)
    {
        return playerSkill.GetUseInk(color);
    }

    float GetInkAmountParSec(Gpt_InkColor color)
    {
        return playerSkill.GetUseInkParsSec(color);
    }

    public void DoDetonateHeal(float enemyPoint)
    {
        float heal = detonateHeal1 * enemyPoint + detonateHeal2 * enemyPoint * enemyPoint;
        AddInk(heal);
    }

    void Update()
    {
        if (RestInk < inkMax * autoHealPar)
        {
            RestInk = Mathf.Min(inkMax * autoHealPar, RestInk + Time.deltaTime * autoHealSpeed);
        }
    }

}

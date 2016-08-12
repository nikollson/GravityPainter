using UnityEngine;
using System.Collections;

public class Gpt_PlayerInkManage : MonoBehaviour
{


    public float inkMax = 1;

    public float attack = 0.03f;
    public float skill = 0.1f;
    public float detonate = 0.1f;

    public float autoHealPar = 0.5f;
    public float autoHealSpeed = 0.2f;
    
    public float detonateHeal1 = 0.04f;
    public float detonateHeal2 = 0.001f;


    public float RestInk { get; private set; }

    void Start()
    {
        MaxSet();
    }

    public void MaxSet() { RestInk = inkMax; }
    public void AddInk(float value) { RestInk = Mathf.Min(inkMax, Mathf.Max(0, RestInk + value)); }
    public void ConsumeInk(float value) { AddInk(-value); }

    public bool CanUseAttack() { return RestInk >= attack; }
    public bool CanUseSkill() { return RestInk >= skill; }
    public bool CanUseDetonate() { return RestInk >= detonate; }

    public void UseAttak() { ConsumeInk(attack); }
    public void UseSkill() { ConsumeInk(skill); }
    public void UseDetonate() { ConsumeInk(detonate); }

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

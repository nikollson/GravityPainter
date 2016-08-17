using UnityEngine;
using System.Collections;

public class Gpt_PlayerDetonate : MonoBehaviour {

    public Gpt_PlayerUtillity playerUtillity;
    public Gpt_PlayerInkManage playerInkManage;
    public Gpt_PlayerState playerState;
    public float detonateEndTime = 0.5f;
    public float detonateDoTime = 0.3f;
    public float friction = 400;
    public new Rigidbody rigidbody;

    int inputFrame_log = 0;
    float detonateCount = 0;
    bool detonateDone = false;

    public bool CanStartDetonate(int inputFrame)
    {
        return inputFrame_log != inputFrame;
    }


    public void StartDetonate(int inputFrame)
    {
        inputFrame_log = inputFrame;
        detonateCount = 0;
        detonateDone = false;
    }


    public void EndDetonate()
    {
        if (!detonateDone) DoDetonate();
    }

    public bool IsEndDetonate()
    {

        return detonateCount > detonateEndTime;
    }

    bool CanDetonate()
    {
        return detonateCount > detonateDoTime && !detonateDone;
    }

    void DoDetonate()
    {
        detonateDone = true;
        //playerUtillity.gravityManager.IsExplode();
        playerUtillity.gravityManager.IsExplodeColor((int)playerState.PlayerColor);

        float enemyPointSum = 0;
        enemyPointSum += playerState.GetDetonateEnemyPoint(Gpt_InkColor.RED);
        enemyPointSum += playerState.GetDetonateEnemyPoint(Gpt_InkColor.BLUE);
        enemyPointSum += playerState.GetDetonateEnemyPoint(Gpt_InkColor.YELLOW);

        playerInkManage.DoDetonateHeal(enemyPointSum);
        playerState.Detonate();
    }

    public void UpdateDetonate()
    {
        detonateCount += Time.deltaTime;

        Vector3 force = -1 * friction * (rigidbody.velocity - new Vector3(0, rigidbody.velocity.y, 0));
        rigidbody.AddForce(force, ForceMode.Acceleration);

        if (CanDetonate())
        {
            DoDetonate();            
        }

    }



}

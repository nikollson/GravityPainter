using UnityEngine;
using System.Collections;

public class Gpt_Player : MonoBehaviour
{

    public Gpt_PlayerUtillity playerUtillity;

    public Gpt_PlayerRun playerRun;
    public Gpt_PlayerJump playerJump;
    public Gpt_PlayerAttack playerAttack;
    public Gpt_PlayerBodyColor playerBodyColor;
    public Gpt_PlayerWait playerWait;
    public Gpt_PlayerAir playerAir;
    public Gpt_TrailControl trailControl;

    public enum MODE { WAIT, RUN, ATTACK, ROT1, ROT2, SKILL, JUMP, FEEVER, AIR };
    public enum ATTACK_DIRECTION { RIGHT, LEFT };
    public enum FEEVER_MODE { NONE, FEEVER };
    public MODE Mode { get; private set; }
    public ATTACK_DIRECTION AttackDirection { get; private set; }
    public FEEVER_MODE feeverMode { get; private set; }

    void Update()
    {
        UpdateMode();
    }

    void UpdateMode()
    {
        if (Mode == MODE.WAIT) UpdateMode_Wait();
        if (Mode == MODE.RUN) UpdateMode_Run();
        if (Mode == MODE.ATTACK) UpdateMode_Attack();
        if (Mode == MODE.JUMP) UpdateMode_Jump();
        if (Mode == MODE.AIR) UpdateMode_Air();
    }


    bool CanStartMove() { return playerUtillity.HasAnalogpadMove(); }
    bool CanStartAttack() { return Gpt_Input.Attack && playerAttack.CanFirstAttack(Gpt_Input.AttackStartFrame); }
    bool CanStartFeever() { return Gpt_Input.Feever; }
    bool CanStartSkill() { return Gpt_Input.Skill; }
    bool CanStartJump() { return Gpt_Input.Jump && playerJump.CanStartJump(Gpt_Input.JumpStartFrame); }


    void UpdateMode_StartRun()
    {
        playerRun.StartRun();
        Mode = MODE.RUN;
    }
    void UpdateMode_StartWait()
    {
        playerWait.StartWait();
        Mode = MODE.WAIT;
    }
    void UpdateMode_StartJump()
    {
        playerJump.StartJump(Gpt_Input.JumpStartFrame);
        playerUtillity.IgnoreFootCollider();
        Mode = MODE.JUMP;
    }
    void UpdateMode_StartAttack(Gpt_PlayerAttack.ATTACK_MODE mode, ATTACK_DIRECTION dir)
    {
        playerAttack.StartAttack(mode, Gpt_Input.AttackStartFrame);
        Mode = MODE.ATTACK;
        AttackDirection = dir;
        trailControl.StartTrail();
    }
    void UpdateMode_StartAir()
    {
        if (Mode == MODE.JUMP) playerAir.StartAir_FromJump(Gpt_Input.JumpStartFrame);
        else playerAir.StartAir();
        Mode = MODE.AIR;
    }


    void UpdateMode_Wait()
    {
        playerWait.UpdateWait();

        bool endWait = false;

        if (CanStartMove()) { endWait = true; UpdateMode_StartRun(); }
        if (CanStartAttack()) { endWait = true; UpdateMode_StartAttack(Gpt_PlayerAttack.ATTACK_MODE.NORMAL, ATTACK_DIRECTION.RIGHT); }
        if (CanStartJump()) { endWait = true; UpdateMode_StartJump(); }
        if (!playerUtillity.IsGround()) { endWait = true; UpdateMode_StartAir(); }

        if (endWait) playerWait.EndWait();
    }

    void UpdateMode_Run()
    {
        playerRun.UpdateRun();

        bool endRun = false;

        if (!CanStartMove()) { endRun = true; UpdateMode_StartWait(); }
        if (CanStartAttack()) { endRun = true; UpdateMode_StartAttack(Gpt_PlayerAttack.ATTACK_MODE.DASH, ATTACK_DIRECTION.RIGHT); }
        if (CanStartJump()) { endRun = true; UpdateMode_StartJump(); }
        if (!playerUtillity.IsGround()) { endRun = true; UpdateMode_StartAir(); }

        if (endRun) playerRun.EndRun();
    }

    void UpdateMode_Attack()
    {
        playerAttack.UpdateAttack();

        bool endAttack = false;

        if (playerAttack.CanSecondAttack(Gpt_Input.AttackStartFrame))
        {
            if (CanStartAttack())
            {
                ATTACK_DIRECTION next = (AttackDirection == ATTACK_DIRECTION.LEFT) ? ATTACK_DIRECTION.RIGHT : ATTACK_DIRECTION.LEFT;
                UpdateMode_StartAttack(Gpt_PlayerAttack.ATTACK_MODE.NORMAL, next);
            }
        }
        if (playerAttack.IsAttackEnd())
        {
            endAttack = true;
            if (!CanStartMove()) UpdateMode_StartWait();
            if (CanStartMove()) UpdateMode_StartRun();
            if (CanStartJump()) UpdateMode_StartJump();
         }

        if (endAttack)
        {
            playerAttack.EndAttack();
            trailControl.EndTrail();
        }
    }

    void UpdateMode_Jump()
    {
        playerJump.UpdateJump();

        bool endJump = false;

        if (playerJump.IsJumpEnd())
        {
            endJump = true;

            if (!playerUtillity.IsGround()) UpdateMode_StartAir();
            else UpdateMode_StartWait();
        }

        if (endJump) playerJump.EndJump();
    }

    void UpdateMode_Air()
    {
        playerAir.UpdateAir(Gpt_Input.Jump, Gpt_Input.JumpStartFrame);

        bool endAir = false;

        if (playerUtillity.IsGround())
        {
            endAir = true;
            UpdateMode_StartWait();
        }

        if (endAir) playerAir.EndAir();
    }
}

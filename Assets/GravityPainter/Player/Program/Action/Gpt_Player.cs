﻿using UnityEngine;
using System.Collections;

public class Gpt_Player : MonoBehaviour
{

    public Gpt_PlayerUtillity playerUtillity;

    // プレイヤーの動作に必要なクラス
    public Gpt_PlayerRun playerRun;
    public Gpt_PlayerJump playerJump;
    public Gpt_PlayerAttackMove playerAttack;
    public Gpt_PlayerAttackState playerAttackState;
    public Gpt_PlayerWait playerWait;
    public Gpt_PlayerAir playerAir;
    public Gpt_PlayerState state;
    public Gpt_PlayerColor playerColor;
    public Gpt_PlayerAnimator playerAnimator;
    public Gpt_TrailControl trailControl;
    public Gpt_PlayerSkill playerSkill;
    public Gpt_PlayerDetonate playerDetonate;
    public Gpt_PlayerInkManage playerInkManage;

    // プレイヤーの状態管理
    public enum MODE { WAIT, RUN, ATTACK, ROT1, ROT2, SKILL, JUMP, FEEVER, AIR, DETONATE };
    public enum ATTACK_DIRECTION { RIGHT, LEFT };
    public MODE Mode { get; private set; }
    public ATTACK_DIRECTION AttackDirection { get; private set; }

    void Update()
    {
        UpdateMode();
        UpdateColor();
    }

    void UpdateMode()
    {
        if (Mode == MODE.WAIT) UpdateMode_Wait();
        if (Mode == MODE.RUN) UpdateMode_Run();
        if (Mode == MODE.ATTACK) UpdateMode_Attack();
        if (Mode == MODE.JUMP) UpdateMode_Jump();
        if (Mode == MODE.AIR) UpdateMode_Air();
        if (Mode == MODE.SKILL) UpdateMode_Skill();
        if (Mode == MODE.DETONATE) UpdateMode_Detonate();
    }

    void UpdateColor()
    {
        if (Gpt_Input.ColorLeftDown) playerColor.SetPrevColor();
        if (Gpt_Input.ColorRightDown) playerColor.SetNextColor();
    }


    bool CanStartMove() { return playerUtillity.HasAnalogpadMove(); }
    bool CanStartAttack() { return Gpt_Input.Attack && playerInkManage.CanUseAttack() && playerAttack.CanFirstAttack(Gpt_Input.AttackStartFrame); }
    bool CanStartDetonate() { return Gpt_Input.Detonate && playerInkManage.CanUseDetonate() && playerDetonate.CanStartDetonate(Gpt_Input.DetonateStartFrame); }
    bool CanStartSkill() { return Gpt_Input.Skill && playerInkManage.CanUseSkill() && playerSkill.CanStartSkill(Gpt_Input.SkillStartFrame); }
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
    void UpdateMode_StartAttack(Gpt_PlayerAttackMove.ATTACK_MODE mode, ATTACK_DIRECTION dir)
    {
        Mode = MODE.ATTACK;
        playerAttack.StartAttack(mode, Gpt_Input.AttackStartFrame);
        AttackDirection = dir;
        trailControl.StartTrail(playerColor.Color);
        playerInkManage.UseAttak();
    }
    void UpdateMode_StartAir()
    {
        playerAir.StartAir();
        Mode = MODE.AIR;
    }
    void UpdateMode_StartSkill()
    {
        Mode = MODE.SKILL;
        playerSkill.StartSkill(Gpt_Input.SkillStartFrame, state.PlayerColor);
        playerInkManage.UseSkill();
    }
    void UpdateMode_StartDetonate()
    {
        Mode = MODE.DETONATE;
        playerDetonate.StartDetonate(Gpt_Input.DetonateStartFrame);
        playerInkManage.UseDetonate();
    }


    void UpdateMode_Wait()
    {
        playerWait.UpdateWait();

        bool endWait = false;

        if (CanStartMove()) { endWait = true; UpdateMode_StartRun(); }
        else if (CanStartAttack()) { endWait = true; UpdateMode_StartAttack(Gpt_PlayerAttackMove.ATTACK_MODE.NORMAL, ATTACK_DIRECTION.RIGHT); }
        else if (CanStartJump()) { endWait = true; UpdateMode_StartJump(); }
        else if (CanStartSkill()) { endWait = true; UpdateMode_StartSkill(); }
        else if (CanStartDetonate()) { endWait = true; UpdateMode_StartDetonate(); }
        else if (!playerUtillity.IsGround()) { endWait = true; UpdateMode_StartAir(); }

        if (endWait) playerWait.EndWait();
    }

    void UpdateMode_Run()
    {
        playerRun.UpdateRun();

        bool endRun = false;

        if (!CanStartMove()) { endRun = true; UpdateMode_StartWait(); }
        else if (CanStartAttack()) { endRun = true; UpdateMode_StartAttack(Gpt_PlayerAttackMove.ATTACK_MODE.DASH, ATTACK_DIRECTION.RIGHT); }
        else if (CanStartJump()) { endRun = true; UpdateMode_StartJump(); }
        else if (CanStartSkill()) { endRun = true;  UpdateMode_StartSkill(); }
        else if (CanStartDetonate()) { endRun = true; UpdateMode_StartDetonate(); }
        else if (!playerUtillity.IsGround()) { endRun = true; UpdateMode_StartAir(); }

        if (endRun) playerRun.EndRun();
    }

    void UpdateMode_Attack()
    {
        playerAttack.UpdateAttack();
        playerAttackState.UpdateAttackState();

        bool endAttack = false;

        if (playerAttack.CanSecondAttack() && CanStartAttack())
        {
            ATTACK_DIRECTION next = (AttackDirection == ATTACK_DIRECTION.LEFT) ? ATTACK_DIRECTION.RIGHT : ATTACK_DIRECTION.LEFT;
            UpdateMode_StartAttack(Gpt_PlayerAttackMove.ATTACK_MODE.NORMAL, next);
        }else if (playerAttack.CanSecondAttack() && CanStartJump())
        {
            endAttack = true;
            UpdateMode_StartJump();
        }
        else if(playerAttack.CanSecondAttack() && CanStartSkill())
        {
            endAttack = true;
            UpdateMode_StartSkill();
        }
        else if(playerAttack.CanSecondAttack() && CanStartDetonate())
        {
            endAttack = true;
            UpdateMode_StartDetonate();
        }
        else if (playerAttack.IsAttackEnd())
        {
            endAttack = true;
            if (!CanStartMove()) UpdateMode_StartWait();
            else if (CanStartMove()) UpdateMode_StartRun();
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
        playerAir.UpdateAir();

        bool endAir = false;

        if (playerUtillity.IsGround())
        {
            endAir = true;
            UpdateMode_StartWait();
        }

        if (endAir) playerAir.EndAir();
    }

    void UpdateMode_Skill()
    {
        playerSkill.UpdateSkill();

        bool endSkill = false;

        if (playerSkill.IsEndSkill())
        {
            endSkill = true;
            UpdateMode_StartWait();
        }

        if (endSkill) playerSkill.EndSkill();
    }


    void UpdateMode_Detonate()
    {
        playerDetonate.UpdateDetonate();

        bool endDetonate = false;

        if (playerDetonate.IsEndDetonate())
        {
            endDetonate = true;
            UpdateMode_StartWait();
        }

        if (endDetonate) playerDetonate.EndDetonate();
    }
}

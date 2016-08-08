using UnityEngine;
using System.Collections;

public class Gpt_Player : MonoBehaviour
{

    public Gpt_PlayerUtillity playerUtillity;

    public Gpt_PlayerRun playerRun;
    public Gpt_PlayerJump playerJump;
    public Gpt_PlayerAttack playerAttack;
    public Gpt_PlayerBodyColor playerBodyColor;

    
    public enum MODE { WAIT, RUN, ATTACK, ROT1, ROT2, SKILL, JUMP, FEEVER, AIR };
    public enum ATTACK_MODE { RIGHT, LEFT };
    public enum FEEVER_MODE { NONE, FEEVER };
    public MODE Mode { get; private set; }
    public ATTACK_MODE AttackMode { get; private set; }
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
    }


    bool HasMoveInput() { float EPS = 0.001f; return Gpt_Input.Move.magnitude > EPS; }
    bool HasAttackInput() { return Gpt_Input.Attack; }
    bool HasFeeverInput() { return Gpt_Input.Feever; }
    bool HasSkillInput() { return Gpt_Input.Skill; }
    bool HasJumpInput() { return Gpt_Input.Jump; }


    void UpdateMode_StartRun()
    {
        playerRun.StartRun();
        Mode = MODE.RUN;
    }
    void UpdateMode_StartWait()
    {
        Mode = MODE.WAIT;
    }
    void UpdateMode_StartJump()
    {
        playerJump.StartJump();
        Mode = MODE.JUMP;
    }
    void UpdateMode_StartAttack(ATTACK_MODE atmode)
    {
        playerAttack.StartAttack();
        Mode = MODE.ATTACK;
        AttackMode = atmode;
    }


    void UpdateMode_Wait()
    {
        if (HasMoveInput()) UpdateMode_StartRun();
        if (HasAttackInput()) UpdateMode_StartAttack(ATTACK_MODE.RIGHT);
        if (HasJumpInput()) UpdateMode_StartJump();
    }

    void UpdateMode_Run()
    {
        if (!HasMoveInput())
        {
            playerRun.EndRun();
            UpdateMode_StartWait();
        }
        if (HasAttackInput())
        {
            playerRun.EndRun();
            UpdateMode_StartAttack(ATTACK_MODE.RIGHT);
        }
        if (HasJumpInput())
        {
            playerRun.EndRun();
            UpdateMode_StartJump();
        }
    }

    void UpdateMode_Attack()
    {
        if (playerAttack.CanSecondAttack())
        {
            if (HasAttackInput())
            {
                playerAttack.EndAttack();
                ATTACK_MODE next = AttackMode == ATTACK_MODE.LEFT ? ATTACK_MODE.RIGHT : ATTACK_MODE.LEFT;
                UpdateMode_StartAttack(next);
            }
        }
        if (playerAttack.IsAttackEnd())
        {
            playerAttack.EndAttack();
            if (!HasMoveInput()) UpdateMode_StartWait();
            if (HasMoveInput()) UpdateMode_StartRun();
            if (HasJumpInput()) UpdateMode_StartJump();
         }
    }

    void UpdateMode_Jump()
    {
        if (playerJump.IsJumpEnd())
        {
            playerJump.EndJump();
            UpdateMode_StartWait();
        }
    }
}

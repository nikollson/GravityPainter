using UnityEngine;
using System.Collections;

public class Gpt_PlayerAnimator : MonoBehaviour {

    public Animator animator;
    public Gpt_Player player;

    const float EPS = 0.00001f;
    const int animationMoveRepeat = 6;

    void Update()
    {
        animator.SetBool("IsRunning", player.Mode == Gpt_Player.MODE.RUN);
        animator.SetBool("IsAttackingRight", player.Mode == Gpt_Player.MODE.ATTACK && player.AttackDirection == Gpt_Player.ATTACK_DIRECTION.RIGHT);
        animator.SetBool("IsAttackingLeft", player.Mode == Gpt_Player.MODE.ATTACK && player.AttackDirection == Gpt_Player.ATTACK_DIRECTION.LEFT);
        animator.SetBool("IsAttacking", player.Mode == Gpt_Player.MODE.ATTACK);
        animator.SetBool("IsJumping", player.Mode == Gpt_Player.MODE.AIR || player.Mode == Gpt_Player.MODE.JUMP);
        animator.SetBool("IsDetonating", player.Mode == Gpt_Player.MODE.DETONATE);
        animator.SetBool("IsRotating", player.Mode == Gpt_Player.MODE.ROTATE);

        for (int i = 0; i < animationMoveRepeat; i++) animator.Update(EPS);
    }

    public void UpdateDelta()
    {
        float EPS = 0.00001f;
        animator.Update(EPS);
        Update();
        animator.Update(EPS);
    }
}

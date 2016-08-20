using UnityEngine;
using System.Collections;

public class Gpt_PlayerAnimator : MonoBehaviour {

    public Animator newAnimator;
    public Gpt_Player player;

    const float EPS = 0.00001f;
    const int animationMoveRepeat = 6;

    void Update()
    {
        SetAnimatorAction(newAnimator);
    }

    void SetAnimatorAction(Animator animator)
    {
        animator.SetBool("IsRunning", player.Mode == Gpt_Player.MODE.RUN);
        animator.SetBool("IsAttackingRight", player.Mode == Gpt_Player.MODE.ATTACK && player.AttackDirection == Gpt_Player.ATTACK_DIRECTION.RIGHT);
        animator.SetBool("IsAttackingLeft", player.Mode == Gpt_Player.MODE.ATTACK && player.AttackDirection == Gpt_Player.ATTACK_DIRECTION.LEFT);
        animator.SetBool("IsAttacking", player.Mode == Gpt_Player.MODE.ATTACK);
        animator.SetBool("IsJumping", player.Mode == Gpt_Player.MODE.AIR || player.Mode == Gpt_Player.MODE.JUMP);
        animator.SetBool("IsDetonating", player.Mode == Gpt_Player.MODE.DETONATE);
        animator.SetBool("IsDamaging", player.Mode == Gpt_Player.MODE.DAMAGE);

        for (int i = 0; i < animationMoveRepeat; i++) animator.Update(EPS);
    }


    public bool IsRuningAnimation()
    {
        var stateInfo = newAnimator.GetCurrentAnimatorStateInfo(0);
        bool running = stateInfo.IsName("PlayerRunTest");
        return running;
    }
    public bool IsRuningLeftFoot()
    {
        var stateInfo = newAnimator.GetCurrentAnimatorStateInfo(0);
        int tapCount = (int)(stateInfo.normalizedTime / 0.5f);

        return tapCount % 2 == 0;
    }
}

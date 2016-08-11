using UnityEngine;
using System.Collections;

public class Gpt_PlayerAnimator : MonoBehaviour {

    public Animator animator;
    public Gpt_Player player;

    void Update()
    {
        animator.SetBool("IsRunning", player.Mode == Gpt_Player.MODE.RUN);
        animator.SetBool("IsAttackingRight", player.Mode == Gpt_Player.MODE.ATTACK && player.AttackDirection == Gpt_Player.ATTACK_DIRECTION.RIGHT);
        animator.SetBool("IsAttackingLeft", player.Mode == Gpt_Player.MODE.ATTACK && player.AttackDirection == Gpt_Player.ATTACK_DIRECTION.LEFT);
        animator.SetBool("IsAttacking", player.Mode == Gpt_Player.MODE.ATTACK);
    }

    public void UpdateDelta()
    {
        float EPS = 0.00001f;
        animator.Update(EPS);
        Update();
        animator.Update(EPS);
    }
}

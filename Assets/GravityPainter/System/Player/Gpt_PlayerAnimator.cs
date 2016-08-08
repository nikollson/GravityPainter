using UnityEngine;
using System.Collections;

public class Gpt_PlayerAnimator : MonoBehaviour {

    public Animator animator;
    public Gpt_Player player;

    void Update()
    {
        animator.SetBool("IsRunning", player.Mode == Gpt_Player.MODE.RUN);
        animator.SetBool("IsAttackingRight", player.Mode == Gpt_Player.MODE.ATTACK && player.AttackMode == Gpt_Player.ATTACK_MODE.RIGHT);
        animator.SetBool("IsAttackingLeft", player.Mode == Gpt_Player.MODE.ATTACK && player.AttackMode == Gpt_Player.ATTACK_MODE.LEFT);
    }
}

using UnityEngine;
using System.Collections;

public class Gpt_PlayerAnimator : MonoBehaviour {

    public Animator animator;
    public Gpt_Player player;

    void Update()
    {
        animator.SetBool("IsRunning", player.IsRunning);
        animator.SetBool("IsAttacking", player.IsAttacking);
    }
}

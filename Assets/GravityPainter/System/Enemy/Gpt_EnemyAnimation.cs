using UnityEngine;
using System.Collections;

public class Gpt_EnemyAnimation : MonoBehaviour {

    private Animator animator;
    private Gpt_EnemyAttack EnemyAttack;
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        EnemyAttack=this.transform.parent.GetComponent<Gpt_EnemyAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyAttack.GetAttack())
        {
            animator.SetBool("IsAttack", true);
        }else
        {
            animator.SetBool("IsAttack", false);

        }
        //animator.SetBool("IsAttack", true);

    }

}

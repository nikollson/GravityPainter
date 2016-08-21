using UnityEngine;
using System.Collections;

public class Gpt_EnemyAnimation : MonoBehaviour {

    private Animator animator;
    public RuntimeAnimatorController[] animators;
    private Gpt_EnemyAttack EnemyAttack;
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        EnemyAttack=this.transform.parent.parent.GetComponent<Gpt_EnemyAttack>();
        float randomAnimation = Random.Range(0,2f);
        if (randomAnimation > 1f)
        {
            animator.runtimeAnimatorController = animators[0];
        }
        else
        {
            animator.runtimeAnimatorController = animators[1];
        }
    }

    // Update is called once per frame
    void Update(){
    
        //Debug.Log("Anime:"+EnemyAttack.GetAttack());
        if (EnemyAttack.isAttack_)
        {
            animator.SetBool("IsAttack", true);
        }else
        {
            animator.SetBool("IsAttack", false);
            //Debug.Log("SetAnime:" + EnemyAttack.GetAttack());

        }
        //animator.SetBool("IsAttack", true);

        if (EnemyAttack.isOki)
        {
            animator.SetTrigger("IsOk");
        }

    }

}

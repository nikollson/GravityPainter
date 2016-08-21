using UnityEngine;
using System.Collections;

public class Gpt_EnemyAnimation : MonoBehaviour {

    public GameObject okiObject;
    public GameObject walkObject;
    private Animator okiAnimator;
    private Animator walkAnimator;
    public RuntimeAnimatorController[] animators;
    private Gpt_EnemyAttack EnemyAttack;
    public Transform okiJoint;
    public Transform walkJoint;

    //モーションの向きを補正する変数
    private Vector3 walkVec;
    private Vector3 okiVec;

    //攻撃中に引力に載った場合の対処
    public float okiTime=3f;
    public bool isOkiAction{get;private set;}
    private float okiActionTemp;
    private float explodeTime;

    // Use this for initialization
    void Start()
    {
        okiAnimator = okiObject.GetComponent<Animator>();
        walkAnimator = walkObject.GetComponent<Animator>();
        EnemyAttack=this.transform.parent.parent.GetComponent<Gpt_EnemyAttack>();
        float randomAnimation = Random.Range(0,2f);
        if (randomAnimation > 1f)
        {
            okiAnimator.runtimeAnimatorController = animators[0];
            walkAnimator.runtimeAnimatorController = animators[0];
        }
        else
        {
            okiAnimator.runtimeAnimatorController = animators[1];
            walkAnimator.runtimeAnimatorController = animators[1];
        }
        okiJoint.gameObject.SetActive(false);

        //walkVec = new Vector3(1.099f, 0.814f, 114.416f);
        //okiVec = new Vector3(1.099f, 0.814f, 61.169f);
        //joint.transform.rotation = Quaternion.Euler(okiVec);

    }


    // Update is called once per frame
    void Update(){
        
        //Debug.Log("Anime:"+EnemyAttack.GetAttack());
        if (EnemyAttack.isAttack_)
        {
            Debug.Log(this.transform.parent.parent.name);
            walkAnimator.SetBool("IsAttack", true);
            okiAnimator.SetBool("IsAttack", true);
        }else
        {
            walkAnimator.SetBool("IsAttack", false);
            okiAnimator.SetBool("IsAttack", false);
            //Debug.Log("SetAnime:" + EnemyAttack.GetAttack());

        }
        //animator.SetBool("IsAttack", true);

        if (EnemyAttack.isOki)
        {
            walkAnimator.SetBool("IsOki", true);
            okiAnimator.SetBool("IsOki", true);
            //joint.transform.rotation = Quaternion.Euler(okiVec);
            //animator.SetTrigger("IsOk");
            okiJoint.gameObject.SetActive(true);
            walkJoint.gameObject.SetActive(false);
        }
        else
        {
            walkAnimator.SetBool("IsOki", false);
            okiAnimator.SetBool("IsOki", false);
            //joint.transform.rotation = Quaternion.Euler(okiVec);
            walkJoint.gameObject.SetActive(true);
            okiJoint.gameObject.SetActive(false);
        }

        if (isOkiAction)
        {
            okiActionTemp += 0.1f;
            //Debug.Log(okiActionTemp);
            walkAnimator.SetBool("IsDamage", true);
            okiAnimator.SetBool("IsDamage", true);
            
            if (okiActionTemp > explodeTime-(okiTime*1.95f))
            {
                
                walkAnimator.SetBool("IsOki", true);
                okiAnimator.SetBool("IsOki", true);
                walkAnimator.SetBool("IsDamage", false);
                okiAnimator.SetBool("IsDamage", false);
                okiJoint.gameObject.SetActive(true);
                walkJoint.gameObject.SetActive(false);
                
                if (okiActionTemp > explodeTime+0.2f)
                {
                    Debug.Log("anime");   
                    isOkiAction = false;
                    walkAnimator.SetBool("IsAttack", false);
                    okiAnimator.SetBool("IsAttack", false);
                    walkJoint.gameObject.SetActive(true);
                    okiJoint.gameObject.SetActive(false);
                    okiActionTemp = 0;
                }
            }
        }

    }

    public void IsOkiAction(float time)
    {
        explodeTime = time;
        isOkiAction=true;
    }
}

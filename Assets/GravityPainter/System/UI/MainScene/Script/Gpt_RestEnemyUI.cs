using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Gpt_RestEnemyUI : MonoBehaviour
{
    public Text enemyNumText;

    private Gpt_EnemyGravityManeger gravityManager;
    private Gpt_EnemyPhaseControl phaseControl;

    void Start()
    {
        gravityManager = FindObjectOfType<Gpt_EnemyGravityManeger>();
        phaseControl = FindObjectOfType<Gpt_EnemyPhaseControl>();

        if(phaseControl == null)
        {
            this.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (phaseControl != null)
        {
            enemyNumText.text = "" + Mathf.Max(0, phaseControl.GetAllClearEnemyNum() - gravityManager.GetEnemyNumCount());
        }
    }

}

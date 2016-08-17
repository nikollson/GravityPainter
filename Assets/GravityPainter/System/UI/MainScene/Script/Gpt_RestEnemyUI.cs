using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Gpt_RestEnemyUI : MonoBehaviour
{
    public Text enemyNumText;

    private Gpt_EnemyGravityManeger gravityManager;

    void Start()
    {
        gravityManager = FindObjectOfType<Gpt_EnemyGravityManeger>();
    }

    void Update()
    {
        enemyNumText.text = "" + Mathf.Max(0, gravityManager.GetRestEnemy());
    }

}

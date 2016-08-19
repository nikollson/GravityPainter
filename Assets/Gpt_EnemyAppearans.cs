using UnityEngine;
using System.Collections;

public class Gpt_EnemyAppearans : MonoBehaviour
{
    private Gpt_YukaManager yukaManager;

    void Start()
    {
        yukaManager = GameObject.Find("YukaManager").GetComponent<Gpt_YukaManager>();

        foreach (Transform a in this.transform)
        {
            Debug.Log(this.gameObject.name + " " + yukaManager.GetTileCordinate(a.transform.position));
            if (!yukaManager.HasTile(a.position))
            {
                a.gameObject.SetActive(false);
            }
        }
    }
}

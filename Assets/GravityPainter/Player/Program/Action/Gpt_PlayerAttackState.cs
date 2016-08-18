using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gpt_PlayerAttackState : MonoBehaviour
{

    public Gpt_PlayerState playerState;
    //public HitManager attackCollider;
    public GameObject bulletPrefab;
    public string enemyTag = "Enemy";

    public float attackStartTime = 0.1f;
    public float attackEndTime = 0.2f;
    public float speed = 5;
    public float startAngle = -60;
    public float startAngleDiff = 15;
    public int wayNum = 5;

    List<HitManager> bullets = new List<HitManager>();
    float count = 0;

    public void StartBullet(Vector3 forward)
    {
        ResetBullets();
        Vector3 eularAngle = this.transform.rotation.eulerAngles;
        for (int i = 0; i < wayNum; i++)
        {
            var obj = (GameObject)Instantiate(bulletPrefab, this.transform.position, Quaternion.identity);
            Rigidbody objrig = obj.GetComponent<Rigidbody>();

            float rot = (startAngle + startAngleDiff * i) / 180 * Mathf.PI;
            float x = Mathf.Cos(rot) * forward.x - Mathf.Sin(rot) * forward.z;
            float z = Mathf.Sin(rot) * forward.x + Mathf.Cos(rot) * forward.z;
            objrig.velocity = new Vector3(x, forward.y, z) * speed;
            bullets.Add(obj.GetComponent<HitManager>());
        }
        count = 0;
    }

    void ResetBullets()
    {
        if (bullets == null) bullets = new List<HitManager>();

        for (int i = bullets.Count - 1; i >= 0; i--)
        {
            if (bullets[i] == null) continue;
            if (bullets[i].gameObject == null) continue;
            Destroy(bullets[i].gameObject);
            bullets.RemoveAt(i);
        }
    }

    public void UpdateAttackState()
    {
        count += Time.deltaTime;
        if (count > attackStartTime)
        {
            for (int i=bullets.Count-1; i>=0; i--)
            {
                if (bullets[i] == null) continue;
                HitManager attackCollider = bullets[i];

                if (attackCollider.IsHit)
                {
                    bool drawed = false;
                    foreach (var a in attackCollider.HitColliders)
                    {
                        if (a.gameObject.tag == enemyTag)
                        {
                            var enemyColor = Gpt_ParentTracker.Track<Gpt_EnemyColor>(a.gameObject);
                            if (enemyColor != null)
                            {
                                if (CanDrawEnemy(enemyColor))
                                {
                                    DrawEnemy(enemyColor);
                                    drawed = true;
                                }
                            }
                        }
                    }
                    if (drawed)
                    {
                        Destroy(bullets[i].gameObject);
                        bullets.RemoveAt(i);
                    }
                }
            }
        }
        if(count > attackEndTime)
        {
            ResetBullets();
        }
    }

    bool CanDrawEnemy(Gpt_EnemyColor enemyColorScript)
    {
        Gpt_InkColor enemyColor = GetEnemyColor(enemyColorScript);
        return enemyColor == Gpt_InkColor.NONE;
    }
    void DrawEnemy(Gpt_EnemyColor enemyColorScript)
    {
        Gpt_InkColor playerColor = playerState.PlayerColor;
        Gpt_InkColor enemyColor = GetEnemyColor(enemyColorScript);

        Gpt_InkColor nextColor = Gpt_InkColor.NONE;

        if (enemyColor == Gpt_InkColor.NONE) nextColor = playerColor;
        if (nextColor != Gpt_InkColor.NONE)
        {
            enemyColorScript.SetColor((int)nextColor);
            playerState.AddPlayerColorCombo();
        }
    }

    Gpt_InkColor GetEnemyColor(Gpt_EnemyColor enemyColor)
    {
        int color = enemyColor.GetColor();
        if (color == 0) return Gpt_InkColor.NONE;
        if (color == 1) return Gpt_InkColor.RED;
        if (color == 2) return Gpt_InkColor.BLUE;
        if (color == 3) return Gpt_InkColor.YELLOW;
        if (color == 4) return Gpt_InkColor.PURPLE;
        if (color == 5) return Gpt_InkColor.ORANGE;
        if (color == 6) return Gpt_InkColor.GREEN;
        if (color == 7) return Gpt_InkColor.RAINBOW;
        return Gpt_InkColor.NONE;
    }
}



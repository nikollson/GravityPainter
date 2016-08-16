using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gpt_PlayerAttackState : MonoBehaviour
{

    public Gpt_PlayerState playerState;
    public HitManager attackCollider;
    public string enemyTag = "Enemy";

    public void UpdateAttackState()
    {
        if (attackCollider.IsHit)
        {
            foreach (var a in attackCollider.HitColliders)
            {
                if (a.gameObject.tag == enemyTag)
                {
                    var enemyColor = Gpt_ParentTracker.Track<Gpt_EnemyColor>(a.gameObject);
                    if (enemyColor != null) DrawEnemy(enemyColor);
                }
            }
        }
    }

    void DrawEnemy(Gpt_EnemyColor enemyColorScript)
    {
        Gpt_InkColor playerColor = playerState.PlayerColor;
        Gpt_InkColor enemyColor = GetEnemyColor(enemyColorScript);

        Gpt_InkColor nextColor = Gpt_InkColor.NONE;

        if (enemyColor == Gpt_InkColor.NONE) nextColor = playerColor;

        if (playerColor == Gpt_InkColor.RED) {
            if (enemyColor == Gpt_InkColor.BLUE) nextColor = Gpt_InkColor.PURPLE;
            if (enemyColor == Gpt_InkColor.YELLOW) nextColor = Gpt_InkColor.ORANGE;
        }
        if (playerColor == Gpt_InkColor.BLUE) {
            if (enemyColor == Gpt_InkColor.RED) nextColor = Gpt_InkColor.PURPLE;
            if (enemyColor == Gpt_InkColor.YELLOW) nextColor = Gpt_InkColor.GREEN;
        }

        if (playerColor == Gpt_InkColor.YELLOW)
        {
            if (enemyColor == Gpt_InkColor.BLUE) nextColor = Gpt_InkColor.GREEN;
            if (enemyColor == Gpt_InkColor.RED) nextColor = Gpt_InkColor.ORANGE;
        }

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



using UnityEngine;
using System.Collections;

public class Gpt_PlayerColor : MonoBehaviour
{
    public Material redMaterial;
    public Material blueMaterial;
    public Material yellowMaterial;

    public Renderer[] renderers;

    public Gpt_InkColor startColor = Gpt_InkColor.RED;
    public Gpt_InkColor Color { get; private set; }

    void Start()
    {
        SetColor(startColor);
    }

    public void SetColor(Gpt_InkColor color)
    {
        Color = color;

        for (int i = 0; i < renderers.Length; i++)
        {
            Renderer renderer = renderers[i];
            if (color == Gpt_InkColor.RED) renderer.sharedMaterial = redMaterial;
            if (color == Gpt_InkColor.BLUE) renderer.sharedMaterial = blueMaterial;
            if (color == Gpt_InkColor.YELLOW) renderer.sharedMaterial = yellowMaterial;
        }

    }

    public void SetNextColor()
    {
        Gpt_InkColor tmpColor = Color;
        if (tmpColor == Gpt_InkColor.RED) SetColor(Gpt_InkColor.YELLOW);
        if (tmpColor == Gpt_InkColor.YELLOW) SetColor(Gpt_InkColor.BLUE);
        if (tmpColor == Gpt_InkColor.BLUE) SetColor(Gpt_InkColor.RED);
    }

    public void SetPrevColor()
    {
        Gpt_InkColor tmpColor = Color;
        if (tmpColor == Gpt_InkColor.RED) SetColor(Gpt_InkColor.BLUE);
        if (tmpColor == Gpt_InkColor.BLUE) SetColor(Gpt_InkColor.YELLOW);
        if (tmpColor == Gpt_InkColor.YELLOW) SetColor(Gpt_InkColor.RED);
    }
}

using UnityEngine;
using System.Collections;

public class Gpt_PlayerColor : MonoBehaviour
{
    public Material redHairMaterial;
    public Material blueHairMaterial;
    public Material yellowHairMaterial;

    public Material redBodyMaterial;
    public Material blueBodyMaterial;
    public Material yellowBodyMaterial;

    public Renderer[] hairRenders;
    public Renderer[] bodyRenderes;

    public Gpt_InkColor startColor = Gpt_InkColor.RED;
    public Gpt_InkColor Color { get; private set; }

    void Start()
    {
        SetColor(startColor);
    }

    public void SetColor(Gpt_InkColor color)
    {
        Color = color;
        Material hairMaterial = redHairMaterial;
        Material bodyMaterial = redBodyMaterial;

        if(color == Gpt_InkColor.BLUE)
        {
            hairMaterial = blueHairMaterial;
            bodyMaterial = blueBodyMaterial;
        }

        if(color == Gpt_InkColor.YELLOW)
        {
            hairMaterial = yellowHairMaterial;
            bodyMaterial = yellowBodyMaterial;
        }

        for (int i = 0; i < hairRenders.Length; i++) hairRenders[i].sharedMaterial = hairMaterial;
        for (int i = 0; i < bodyRenderes.Length; i++) bodyRenderes[i].sharedMaterial = bodyMaterial;
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

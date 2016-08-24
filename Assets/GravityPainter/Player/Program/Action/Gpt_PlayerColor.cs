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

    public Material redFudeMaterial;
    public Material blueFudeMaterial;
    public Material yellowFudeMaterial;

    public Material mutekiMaterial;
    public float mutekiFlushOnTime = 0.2f;
    public float mutekiFlushOffTime = 0.2f;

    public Renderer[] hairRenders;
    public Renderer[] bodyRenderes;
    public Renderer[] fudeRenderes;

    public Gpt_InkColor startColor = Gpt_InkColor.RED;
    public Gpt_InkColor Color { get; private set; }

    private float flushLength = 0;
    private float flushTime = 0;
    private bool flushing = false;

    void Start()
    {
        SetColor(startColor);
    }

    void Update()
    {
        if (flushing)
        {
            flushTime += Time.deltaTime;

            float allTime = mutekiFlushOnTime + mutekiFlushOffTime;
            float amari = flushTime - (int)(flushTime / allTime) * allTime;

            if (amari < mutekiFlushOnTime) SetFlushColor();
            else SetColor(Color);

            if (flushTime > flushLength) flushing = false;
        }
        else
        {
            SetColor(Color);
        }
    }

    void SetFlushColor()
    {
        SetColor(Gpt_InkColor.NONE);
    }

    public void SetColor(Gpt_InkColor color)
    {
        if (color != Gpt_InkColor.NONE) Color = color;
        Material hairMaterial = redHairMaterial;
        Material bodyMaterial = redBodyMaterial;
        Material fudeMaterial = redFudeMaterial;

        if(color == Gpt_InkColor.BLUE)
        {
            hairMaterial = blueHairMaterial;
            bodyMaterial = blueBodyMaterial;
            fudeMaterial = blueFudeMaterial;
        }

        if(color == Gpt_InkColor.YELLOW)
        {
            hairMaterial = yellowHairMaterial;
            bodyMaterial = yellowBodyMaterial;
            fudeMaterial = yellowFudeMaterial;
        }

        if(color == Gpt_InkColor.NONE)
        {
            hairMaterial = mutekiMaterial;
            bodyMaterial = mutekiMaterial;
            fudeMaterial = mutekiMaterial;
        }

        for (int i = 0; i < hairRenders.Length; i++) hairRenders[i].sharedMaterial = hairMaterial;
        for (int i = 0; i < bodyRenderes.Length; i++) bodyRenderes[i].sharedMaterial = bodyMaterial;
        for (int i = 0; i < fudeRenderes.Length; i++) fudeRenderes[i].sharedMaterial = fudeMaterial;
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

    public void StartMutekiFlush(float flushTime)
    {
        this.flushing = true;
        this.flushLength = flushTime;
        this.flushTime = 0.0f;
    }

    public void MeshRendererOn()
    {
        for (int i = 0; i < hairRenders.Length; i++) hairRenders[i].enabled = true;
        for (int i = 0; i < bodyRenderes.Length; i++) bodyRenderes[i].enabled = true;
        for (int i = 0; i < fudeRenderes.Length; i++) fudeRenderes[i].enabled = true;
    }

    public void MeshRendererOff()
    {
        for (int i = 0; i < hairRenders.Length; i++) hairRenders[i].enabled = false;
        for (int i = 0; i < bodyRenderes.Length; i++) bodyRenderes[i].enabled = false;
        for (int i = 0; i < fudeRenderes.Length; i++) fudeRenderes[i].enabled = false;
    }
}

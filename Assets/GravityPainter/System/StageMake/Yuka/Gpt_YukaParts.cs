using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Gpt_YukaParts : MonoBehaviour {

    public enum YukaColor { WHITE, RED, BLUE, YELLOW };
    public enum Mode { CONST, RANDOM, LOOP }

    public YukaColor firstColor;
    public Mode changeMode;
    public YukaColor[] colorLoop;

    public MaterialSetting materialSetting;

    private Gpt_YukaBox[] yukaBoxes;

    Gpt_InkColor currentColor;

    void Start()
    {
        LoadYukaBoxes();
        SetFirstColor();
    }

    void SetFirstColor()
    {
        SetColor(firstColor);
    }

    void LoadYukaBoxes()
    {
        yukaBoxes = transform.GetComponentsInChildren<Gpt_YukaBox>();
    }

    public void SetColor(YukaColor yukaColor)
    {
        SetColor(yukaColorToInkColor(yukaColor));
    }

    public void SetColor(Gpt_InkColor color)
    {
        if (yukaBoxes == null) LoadYukaBoxes();
        currentColor = color;

        foreach(var a in yukaBoxes)
        {
            if (color == Gpt_InkColor.RED) a.SetColor(materialSetting.red);
            else if (color == Gpt_InkColor.BLUE) a.SetColor(materialSetting.blue);
            else if (color == Gpt_InkColor.YELLOW) a.SetColor(materialSetting.yellow);
            else a.SetColor(materialSetting.white);
        }
    }

    Gpt_InkColor yukaColorToInkColor(YukaColor yukaColor)
    {
        if (yukaColor == YukaColor.RED) return Gpt_InkColor.RED;
        if (yukaColor == YukaColor.BLUE) return Gpt_InkColor.BLUE;
        if (yukaColor == YukaColor.YELLOW) return Gpt_InkColor.YELLOW;
        return Gpt_InkColor.NONE;
    }

    public void DoExplode(Gpt_InkColor color, Vector3 point, float radius)
    {
        bool hit = false;
        foreach(var a in yukaBoxes)
        {
            if ((a.transform.position - point).magnitude < radius) hit = true;
        }

        if(hit && color == currentColor)
        {
            this.transform.position -= new Vector3(0, 1, 0);
        }
    }


    [System.Serializable]
    public class MaterialSetting
    {
        public Material white, red, blue, yellow;
    }
}

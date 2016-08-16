using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Gpt_YukaParts : MonoBehaviour {

    public enum YukaColor { WHITE, RED, BLUE, YELLOW };
    public enum Mode { CONST, RANDOM, LOOP }

    public YukaColor firstColor;
    public Mode changeMode;
    public YukaColor[] colorLoop;
    
    private Gpt_YukaBox[] yukaBoxes;

    Gpt_InkColor currentColor;

    void LoadYukaBoxes()
    {
        yukaBoxes = transform.GetComponentsInChildren<Gpt_YukaBox>();
    }

    Gpt_InkColor yukaColorToInkColor(YukaColor yukaColor)
    {
        if (yukaColor == YukaColor.RED) return Gpt_InkColor.RED;
        if (yukaColor == YukaColor.BLUE) return Gpt_InkColor.BLUE;
        if (yukaColor == YukaColor.YELLOW) return Gpt_InkColor.YELLOW;
        return Gpt_InkColor.NONE;
    }

    public Gpt_YukaBox[] GetYukaBoxes()
    {
        if (yukaBoxes == null || yukaBoxes.Length == 0) LoadYukaBoxes();
        return yukaBoxes;
    }

    public Gpt_InkColor GetFirstColor()
    {
        return yukaColorToInkColor(firstColor);
    }
}

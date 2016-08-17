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

    private Gpt_InkColor[] colorSeq;

    Gpt_InkColor currentColor;
    int changeFrame_log = 0;
    int colorSeqID = 0;

    void Start()
    {
        SetColorSequence();
    }

    void SetColorSequence()
    {
        if(changeMode == Mode.CONST)
        {
            colorSeq = new Gpt_InkColor[] { yukaColorToInkColor(firstColor) };
        }
        if(changeMode == Mode.LOOP)
        {
            colorSeq = new Gpt_InkColor[colorLoop.Length];
            for (int i = 0; i < colorSeq.Length; i++) colorSeq[i] = yukaColorToInkColor(colorLoop[i]);
        }
        if(changeMode == Mode.RANDOM)
        {
            colorSeq = new Gpt_InkColor[20];
            for(int i = 0; i < colorSeq.Length; i++)
            {
                int rand = Random.Range(0, 3);
                Gpt_InkColor color = Gpt_InkColor.NONE;
                if (rand == 0) color = Gpt_InkColor.RED;
                if (rand == 1) color = Gpt_InkColor.BLUE;
                if (rand == 2) color = Gpt_InkColor.YELLOW;

                colorSeq[i] = color;
            }
        }
    }

    public void UpdateColor(int changeFrame)
    {
        if(changeFrame_log != changeFrame)
        {
            changeFrame_log = changeFrame;
            colorSeqID = (colorSeqID + 1) % colorSeq.Length;
        }
    }

    public Gpt_InkColor GetCurrentColor()
    {
        return Gpt_InkColor.RED;
        //return color colorSeq[colorSeqID % colorSeq.Length];
    }

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

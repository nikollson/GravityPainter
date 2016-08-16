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

    void Start()
    {
        LoadYukaBoxes();
    }

    void LoadYukaBoxes()
    {
        yukaBoxes = transform.GetComponentsInChildren<Gpt_YukaBox>();
    }

    public void SetColor(Gpt_InkColor color)
    {
        if (yukaBoxes == null) LoadYukaBoxes();
        foreach(var a in yukaBoxes)
        {
            if (color == Gpt_InkColor.RED) a.SetColor(materialSetting.red);
            else if (color == Gpt_InkColor.BLUE) a.SetColor(materialSetting.blue);
            else if (color == Gpt_InkColor.YELLOW) a.SetColor(materialSetting.yellow);
            else a.SetColor(materialSetting.white);
        }
    }


    [System.Serializable]
    public class MaterialSetting
    {
        public Material white, red, blue, yellow;
    }
}

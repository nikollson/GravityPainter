﻿using UnityEngine;
using System.Collections;

public class Gpt_PlayerState : MonoBehaviour
{

    public int HPMax = 12;
    public float inkMax = 1;
    public float feeverMax = 1;
    public int firstPlayerColor = 1;

    public ComboControlSetting comboSetting;
    ComboControl redCombo, blueCombo, yellowCombo;


    // プロパティ
    public int HP { get; private set; }
    public float Feever { get; private set; }
    public float Ink { get; private set; }
    
    public int RedCombo { get { return redCombo.Combo; } }
    public int BlueCombo { get { return blueCombo.Combo; } }
    public int YellowCombo { get { return yellowCombo.Combo; } }

    public bool IsFeever { get; private set; }
    public int PlayerColor { get; private set; }


    // プロパティをいじる関数
    public void AddHP(int value) { HP = intValueLimit(0, HPMax, HP + value); }
    public void MinusHP(int value) { AddHP(-value); }
    public void AddInk(float value) { Ink = floatValueLimit(0f, inkMax, Ink + value); }
    public void MinusInk(float value) { AddInk(-value); }
    public void AddFeever(float value) { Feever = floatValueLimit(0f, feeverMax, Feever + value); }
    public void MinusFeever(float value) { AddFeever(-value); }

    public void StartFeever() { IsFeever = true; }
    public void EndFeever() { IsFeever = false; }

    int intValueLimit(int min, int max, int value) { return Mathf.Max(min, Mathf.Min(max, value)); }
    float floatValueLimit(float min, float max, float value) { return Mathf.Max(min, Mathf.Min(max, value)); }

    public void AddPlayerColorCombo()
    {
        if (PlayerColor == Gpt_InkColor.RED) AddRedCombo();
        if (PlayerColor == Gpt_InkColor.BLUE) AddBlueCombo();
        if (PlayerColor == Gpt_InkColor.YELLOW) AddYellowCombo();
        if (PlayerColor == Gpt_InkColor.RAINBOW) AddRainbowCombo();
    }
    public void AddRedCombo() { redCombo.AddCombo(); }
    public void AddBlueCombo() { blueCombo.AddCombo(); }
    public void AddYellowCombo() { yellowCombo.AddCombo(); }
    public void AddRainbowCombo() { Debug.Log("RaibowColorComboとは?"); }


    void Start()
    {
        PlayerColor = firstPlayerColor;
        redCombo = new ComboControl(comboSetting);
        blueCombo = new ComboControl(comboSetting);
        yellowCombo = new ComboControl(comboSetting);
    }

    void Update()
    {
        if (redCombo.IsComboCutted()) MakeSpecialEnemy(redCombo);
        if (blueCombo.IsComboCutted()) MakeSpecialEnemy(blueCombo);
        if (yellowCombo.IsComboCutted()) MakeSpecialEnemy(yellowCombo);

        redCombo.Update();
        blueCombo.Update();
        yellowCombo.Update();
    }

    void MakeSpecialEnemy(ComboControl comboControl)
    {
        comboControl.ResetCombo();
    }

    
    // implement class
    
    [System.Serializable]
    public class ComboControlSetting
    {
        public float comboCutTime = 0.4f;
    }
    class ComboControl
    {
        ComboControlSetting setting;
        public int Combo { get; private set; }
        float comboTimeCount = 0;

        public ComboControl(ComboControlSetting setting)
        {
            this.setting = setting;
            Combo = 0;
        }
        public void AddCombo()
        {
            Combo++;
            comboTimeCount = 0;
        }
        public void Update()
        {
            if (Combo != 0) comboTimeCount += Time.deltaTime;
        }
        public bool IsComboCutted()
        {
            return Combo != 0 && comboTimeCount > setting.comboCutTime;
        }
        public void ResetCombo()
        {
            Combo = 0;
            comboTimeCount = 0;
        }
    }
}

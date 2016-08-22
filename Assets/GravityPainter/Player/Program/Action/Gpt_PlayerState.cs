using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Gpt_PlayerState : MonoBehaviour
{
    public Gpt_PlayerUtillity playerUtillity;
    public Gpt_PlayerColor playerColor;
    public Gpt_PlayerInkManage playerInkManage;
    public GameObject damagedEffectPrefab;

    public int HPMax = 12;
    public float feeverMax = 1;
    public float mutekiTime = 1.0f;
    public float damageTime = 0.1f;
    public int respawnDamage = 4;
    
    public ComboControlSetting comboSetting;
    ComboControl redCombo, blueCombo, yellowCombo;
    float mutekiCount = 0;

    public GameObject specialEnemy;

    // プロパティ
    public int HP { get; private set; }
    public float Feever { get; private set; }
    public float Ink { get { return playerInkManage.RestInk; } }
    public float inkMax { get { return playerInkManage.inkMax; } }
    
    public int RedCombo { get { return redCombo.Combo; } }
    public int BlueCombo { get { return blueCombo.Combo; } }
    public int YellowCombo { get { return yellowCombo.Combo; } }
    public int Combo { get { return redCombo.Combo + blueCombo.Combo + yellowCombo.Combo; } }

    public float RedComboRestTimePer { get { return redCombo.RestTime; } }
    public float BlueComboRestTimePer { get { return blueCombo.RestTime; } }
    public float YellowComboRestTimePer { get { return yellowCombo.RestTime; } }

    public bool IsFeever { get; private set; }

    public bool IsMuteki { get { return mutekiCount < mutekiTime || HP == 0; } }
    public Gpt_InkColor PlayerColor { get { return playerColor.Color; } }


    public void DoRespawn(bool addHpDamage)
    {
        if (addHpDamage) AddHPDamage(respawnDamage, false);
    }

    // プロパティをいじる関数
    public void AddHP(int value) { HP = intValueLimit(0, HPMax, HP + value); }
    public void AddHPDamage(int value, bool setMuteki = true)
    {
        if (!IsMuteki)
        {
            AddHP(-value);
            if(setMuteki) mutekiCount = 0;
            MakeDamageEffect();
        }
    }
    public void AddHPDamage_Attack(int value, Vector3 hitPosition)
    {
        if (!IsMuteki)
        {
            AddHPDamage(value);
            Vector3 look = hitPosition;
            look.y = this.transform.position.y;
            playerUtillity.LookAt(look);
            playerColor.StartMutekiFlush(mutekiTime);
        }
    }

    void MakeDamageEffect()
    {
        var obj = (GameObject)Instantiate(damagedEffectPrefab, this.transform.position, Quaternion.identity);
    }

    public void AddFeever(float value) { Feever = floatValueLimit(0f, feeverMax, Feever + value); }

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

    public bool IsDead() { return HP <= 0; }
    public bool IsDamaging() { return mutekiCount < damageTime; }

    public void MaxStatusSet()
    {
        float inf = 10000000;
        HP = HPMax;
        mutekiCount = inf;
    }
    
    void Start()
    {
        redCombo = new ComboControl(comboSetting);
        blueCombo = new ComboControl(comboSetting);
        yellowCombo = new ComboControl(comboSetting);
        MaxStatusSet();
    }

    void Update()
    {
        if (redCombo.IsComboCutted()) MakeSpecialEnemy(redCombo);
        if (blueCombo.IsComboCutted()) MakeSpecialEnemy(blueCombo);
        if (yellowCombo.IsComboCutted()) MakeSpecialEnemy(yellowCombo);

        redCombo.Update();
        blueCombo.Update();
        yellowCombo.Update();

        mutekiCount += Time.deltaTime;
    }

    void MakeSpecialEnemy(ComboControl comboControl)
    {
        comboControl.ResetCombo();
    }

    public float GetDetonateEnemyPoint(Gpt_InkColor color)
    {
        if (color == Gpt_InkColor.RED) return redCombo.Combo;
        if (color == Gpt_InkColor.BLUE) return blueCombo.Combo;
        if (color == Gpt_InkColor.YELLOW) return yellowCombo.Combo;
        return 0.0f;
    }

    public void Detonate()
    {
        redCombo.ResetCombo();
        blueCombo.ResetCombo();
        yellowCombo.ResetCombo();
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
        public float RestTime { get { return Combo == 0 ? 0 : 1 - Mathf.Min(1.0f, Mathf.Max(0.0f, comboTimeCount / setting.comboCutTime)); } }
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

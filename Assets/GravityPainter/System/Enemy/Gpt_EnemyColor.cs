using UnityEngine;
using System.Collections;

public class Gpt_EnemyColor : MonoBehaviour {

    //Color 0=無色,1=赤,2=青,3=黄,4=紫,5=オレンジ,6=緑,
    private int Color = 0;
    public Renderer renderer;
    public Renderer rendererFootR;
    public Renderer rendererFootL;

    public Material[] _material;

    private bool isDamage;

    //点滅間隔
    private int damageTime = 4;
    private int damageCount;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        damageCount++;
        if (isDamage)
        {
            if (damageCount % damageTime == 0)
            {
                renderer.enabled = false;
                rendererFootR.enabled = false;
                rendererFootL.enabled = false;
            }
            else
            {
                renderer.enabled = true;
                rendererFootR.enabled = true;
                rendererFootL.enabled = true;
            }

        }
        else
        {
            renderer.enabled = true;
            rendererFootR.enabled = true;
            rendererFootL.enabled = true;

        }
	}

    public void SetColor(int setColor)
    {
        Color = setColor;
        if (renderer == null) renderer = this.GetComponent<Renderer>();
        renderer.sharedMaterial = _material[setColor];
        rendererFootR.sharedMaterial = _material[setColor];
        rendererFootL.sharedMaterial = _material[setColor];
    }

    public int GetColor()
    {
        return Color;

    }
    //点滅処理
    public void IsDamage()
    {
        isDamage = true;
    }

    public void IsDamageFalse()
    {
        isDamage = false;
    }
}

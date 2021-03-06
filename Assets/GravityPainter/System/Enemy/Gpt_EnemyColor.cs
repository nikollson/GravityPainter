﻿using UnityEngine;
using System.Collections;

public class Gpt_EnemyColor : MonoBehaviour {

    //Color 0=無色,1=赤,2=青,3=黄,4=紫,5=オレンジ,6=緑,
    private int Color = 0;
    public Renderer renderer;
    public Renderer rendererFootR;
    public Renderer rendererFootL;
    public Gpt_Enemy EnemyClass;
    public Material[] _material;
    public Material[] _materialIkemen;
    public Material ikemen_damage;

    private bool isDamage;
    private bool ikemen;
    //点滅間隔
    private int damageTime = 4;
    private int damageCount;

    private float textureOffset;
    // Use this for initialization
    void Start () {
        int texture=Random.Range(0,101);

        if (texture < 50)
        {
            renderer.sharedMaterial = _material[0];
            //textureOffset = 0f;//ノーマル顔
        }
        else
        {
            renderer.sharedMaterial = _materialIkemen[0];
            ikemen = true;
        }

    }
	
	// Update is called once per frame
	void Update () {

        //if (EnemyClass.GetGravity()||EnemyClass.IsTop)
        //{
        //    if (ikemen)
        //    {
        //        renderer.sharedMaterial.SetTextureOffset("_MainTex", new Vector2(0, 0.39f));
        //    }else
        //    {
        //        renderer.sharedMaterial.SetTextureOffset("_MainTex", new Vector2(0, 0.2f));
        //    }


        //}
        //else if(EnemyClass.isFall|| EnemyClass.GetIsDamage())
        //{
        //    if (ikemen)
        //    {
        //        renderer.sharedMaterial.SetTextureOffset("_MainTex", new Vector2(0, 0.39f));
        //    }
        //    else
        //    {
        //        renderer.sharedMaterial.SetTextureOffset("_MainTex", new Vector2(0, 0.2f));
        //    }
        //}
        //else
        //{
        //    if (ikemen)
        //    {
        //        renderer.sharedMaterial = ikemen_material;
        //        renderer.sharedMaterial.SetTextureOffset("_MainTex", new Vector2(0, textureOffset));
        //    }
        //    else
        //    {
        //        renderer.sharedMaterial = _material[0];
        //        renderer.sharedMaterial.SetTextureOffset("_MainTex", new Vector2(0, textureOffset));
        //    }

        //}

        if (EnemyClass.isFall || EnemyClass.GetIsDamage())
        {
            if (!ikemen)
            {
                renderer.sharedMaterial = ikemen_damage;
            }
                
        }
        else
        {
            if (!ikemen&&Color==0)
            {
                renderer.sharedMaterial = _material[0];
            }
        }

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
        if (setColor == 0)
        {
            if (ikemen)
            {
                renderer.sharedMaterial = _materialIkemen[0];
            }
            else
            {
                renderer.sharedMaterial = _material[setColor];
            }
        }
        else
        {
            if (ikemen)
            {
                renderer.sharedMaterial = _materialIkemen[setColor];
            }
            else
            {
                renderer.sharedMaterial = _material[setColor];
            }
            
        }
        
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

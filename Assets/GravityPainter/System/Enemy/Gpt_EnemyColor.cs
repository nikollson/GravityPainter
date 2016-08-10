using UnityEngine;
using System.Collections;

public class Gpt_EnemyColor : MonoBehaviour {

    //Color 0=無色,1=赤,2=青,3=黄,4=紫,5=オレンジ,6=緑,
    private int Color = 0;
    private int previusColor;

    public Material[] _material;

    // Use this for initialization
    void Start () {
        previusColor = Color;
	}
	
	// Update is called once per frame
	void Update () {

        //Colorが変更されたらマテリアル変更
        if (previusColor != Color)
        {
            this.GetComponent<Renderer>().material = _material[Color];
            previusColor = Color;
        }
	}

    public void SetColor(int setColor)
    {
        Color = setColor;

    }

    public int GetColor()
    {
        return Color;

    }
}

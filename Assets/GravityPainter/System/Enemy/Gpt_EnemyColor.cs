using UnityEngine;
using System.Collections;

public class Gpt_EnemyColor : MonoBehaviour {

    //Color 0=無色,1=赤,2=青,3=黄,4=紫,5=オレンジ,6=緑,
    private int Color = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void SetColor(int setColor)
    {
        Color = setColor;

    }

    int GetColor()
    {
        return Color;

    }
}

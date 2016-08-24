using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Gpt_ImageColorFlush : MonoBehaviour {


    public float rot = 0;
    public float rotSpeed = 90;

    public float mini = 0.2f;
    public float maxi = 0.8f;

    Image image;

    void Start()
    {
        image = this.GetComponent<Image>();
    }

    void Update()
    {
        rot += rotSpeed * Time.deltaTime;
        Color col = image.color;
        float par = (Mathf.Sin(rot / Mathf.PI * 180) + 1) / 2;
        col.a = par * (maxi - mini) + mini;
        image.color = col;
    }
}

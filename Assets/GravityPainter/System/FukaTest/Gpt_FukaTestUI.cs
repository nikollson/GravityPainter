using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Gpt_FukaTestUI : MonoBehaviour
{
    
    public InputField inputX;
    public InputField inputY;
    public Text fpsText;
    public Text nowText;
    public Text countText;

    public Gpt_FukaTestMesh[] fukaTestMesh;

    float[] times = new float[10];

    void Start()
    {
        fukaTestMesh = FindObjectsOfType<Gpt_FukaTestMesh>();
    }

    public void RemakeMesh()
    {
        for (int i = 0; i < fukaTestMesh.Length; i++)
        {
            var a = fukaTestMesh[i];
            a.MakeMesh(int.Parse(inputX.text), int.Parse(inputY.text));
        }
    }

    void Update()
    {
        for (int i = 0; i < times.Length - 1; i++)
        {
            times[i] = times[i + 1];
        }

        float time = Time.timeSinceLevelLoad;

        times[times.Length - 1] = Time.realtimeSinceStartup;
        nowText.text = "" + time;

        int sum = 0;
        for (int i = 0; i < fukaTestMesh.Length; i++)
        {
            var a = fukaTestMesh[i];
            sum += a.GetMeshCount();
        }


        countText.text = "" + sum;

        fpsText.text = "" + (1 / ((times[times.Length - 1] - times[0]) / (times.Length - 1)));
    }
}

using UnityEngine;
using System.Collections;

public class Gpt_YukaBox : MonoBehaviour {

    public Renderer renderer;
    
    public void SetColor(Material material)
    {
        renderer.sharedMaterial = material;
    }

}

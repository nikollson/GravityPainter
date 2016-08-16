using UnityEngine;
using System.Collections;

public class Gpt_MeshRendererOff : MonoBehaviour
{

    public Renderer meshRenderer;

    void Awake()
    {
        if (meshRenderer == null) meshRenderer = this.GetComponent<MeshRenderer>();

        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class SwapTextures : MonoBehaviour
{
    private Renderer myRenderer;

    private void Awake()
    {
        myRenderer = GetComponent<Renderer>();
        if (Random.value > 0.5f)
        {
            myRenderer.material.SetVector("_TexSlot", new Vector4(0.5f, -0.21f, 0, 0));
        }
    }
}

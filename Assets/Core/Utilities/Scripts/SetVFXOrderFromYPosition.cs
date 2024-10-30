using System.Collections.Generic;
using UnityEngine;

public class SetVFXOrderFromYPosition : MonoBehaviour
{
    Renderer[] renderers;
    [SerializeField] int additionalOrder;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }

    void Update()
    {
        foreach (var vfx in renderers)
        {
            vfx.sortingOrder = -Mathf.RoundToInt(transform.position.y) * 10 + additionalOrder;
        }        
    }
}

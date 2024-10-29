using System.Collections.Generic;
using UnityEngine;

public class SetSpriteOrderFromYPosition : MonoBehaviour
{
    SpriteRenderer[] spriteRenderers;
    [SerializeField] int additionalOrder;

    private void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    void Update()
    {
        foreach (var sprite in spriteRenderers)
        {
            sprite.sortingOrder = -Mathf.RoundToInt(transform.position.y) + additionalOrder;
        }        
    }
}

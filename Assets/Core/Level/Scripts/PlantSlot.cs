using System;
using UnityEngine;
using System.Collections;
[SelectionBase]
public class PlantSlot : MonoBehaviour
{
    [SerializeField] int sellingTime = 10;
     
    public SlotState slotState = SlotState.empty;
    public bool IsSelling = false;
    public void Plant()
    {
        Debug.Log("Slot was planted");
        slotState = SlotState.planted;
    }

    public void Sell()
    {
        StartCoroutine(SellDelay());
    }

    //SellDelay
    IEnumerator SellDelay ()
    {
        IsSelling = true;
        float startTime = Time.time;
        float endTime = Time.time + sellingTime;
        while (Time.time < endTime)
        {
            yield return null;

        }
        IsSelling = false;
        Debug.Log("Slot was sold");
        slotState = SlotState.empty;
    }

}

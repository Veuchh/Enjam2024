using System;
using UnityEngine;
using System.Collections;
[SelectionBase]
public class PlantSlot : MonoBehaviour
{
    [SerializeField] int sellingTime = 10;
    public GameObject pumpkinDisplay;

    public SlotState slotState = SlotState.empty;
    public bool IsSelling = false;
    public void Plant()
    {
        Debug.Log("Slot was planted");
        Debug.Log($"Remaining seeds : {PlayerData.seed}");
        slotState = SlotState.planted;
        pumpkinDisplay.SetActive(true);

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
        pumpkinDisplay.SetActive(false);
    }

}

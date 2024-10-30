using UnityEngine;

public class PlayerGardening : MonoBehaviour
{
    [SerializeField]
    int plantingCost = 10;
    [SerializeField]
    int defaultSeedAmount = 100;
    PlantSlot currentSlot;

    private void Start()
    {
        PlayerData.seed = defaultSeedAmount;
        GameManager.Instance.UpdateSeedUI();
    }

    public void OnInteract()
    {
        if (currentSlot == null)
            return;
        if (currentSlot.slotState == SlotState.empty)
        {
            Debug.Log("Slot empty");
            if (PlayerData.seed >= plantingCost)
            {
                PlayerData.seed -= plantingCost;
                currentSlot.Plant();
            }
        }
        else if (currentSlot.slotState == SlotState.planted)
        {
            if (!currentSlot.IsSelling && !currentSlot.isDismantling)
            {
                Debug.Log("Slot already planted");
                currentSlot.Sell();
            }
            else { Debug.Log("Plant in queue"); }

        }
    }

    public void OnDismantlePumpkin()
    {
        if (currentSlot == null || currentSlot.slotState != SlotState.planted)
            return;

        if (!currentSlot.IsSelling && !currentSlot.isDismantling)
        {
            Debug.Log("Slot already planted");
            currentSlot.Dismantle();
        }
        else
        {
            Debug.Log("Plant in queue");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlantSlot otherSlot = collision.GetComponent<PlantSlot>();
        if (otherSlot == null)
        {
            return;
        }
        currentSlot = otherSlot;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlantSlot otherSlot = collision.GetComponent<PlantSlot>();
        if (otherSlot != currentSlot)
        {
            return;
        }
        currentSlot = null;
    }



}

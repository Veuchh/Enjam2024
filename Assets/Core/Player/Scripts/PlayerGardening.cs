using System.Collections.Generic;
using UnityEngine;

public class PlayerGardening : MonoBehaviour
{
    [SerializeField]
    int plantingCost = 10;
    [SerializeField]
    int defaultSeedAmount = 100;
    List<PlantSlot> nearbySlots = new List<PlantSlot>();
    PlantSlot highlightedSlot;

    private void Start()
    {
        PlayerData.seed = defaultSeedAmount;
        GameManager.Instance.UpdateSeedUI();
    }

    private void Update()
    {
        PlantSlot closest = ClosestSlot();
        if (highlightedSlot != closest)
        {
            if (highlightedSlot != null)
            {
                highlightedSlot.Highlight(false);
                highlightedSlot = null;
            }

            if (closest != null)
            {
                closest.Highlight(true);
                highlightedSlot = closest;
            }
        }
    }

    public void OnInteract()
    {
        if (nearbySlots.Count == 0)
            return;

        PlantSlot closest = ClosestSlot();

        if (closest.slotState == SlotState.empty)
        {
            if (PlayerData.seed >= plantingCost)
            {
                PlayerData.seed -= plantingCost;
                closest.Plant();
            }
        }
        else if (closest.slotState == SlotState.planted)
        {
            if (!closest.IsSelling && !closest.isDismantling)
            {
                closest.Sell();
            }
        }
    }

    public void OnDismantlePumpkin()
    {
        PlantSlot closest = ClosestSlot();

        if (closest == null || closest.slotState != SlotState.planted)
            return;

        if (!closest.IsSelling && !closest.isDismantling)
        {
            closest.Dismantle();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        PlantSlot otherSlot = collision.GetComponent<PlantSlot>();
        if (otherSlot == null || nearbySlots.Contains(otherSlot))
        {
            return;
        }
        nearbySlots.Add(otherSlot);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlantSlot otherSlot = collision.GetComponent<PlantSlot>();
        if (otherSlot == null || !nearbySlots.Contains(otherSlot))
        {
            return;
        }
        nearbySlots.Remove(otherSlot);
    }

    PlantSlot ClosestSlot()
    {
        PlantSlot bestCandidate = null;
        float closestDistance = Mathf.Infinity;

        foreach (PlantSlot slot in nearbySlots)
        {
            float distance = Vector3.Distance(transform.position, slot.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                bestCandidate = slot;
            }
        }

        return bestCandidate;
    }

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;

[SelectionBase]
public class PlantSlot : MonoBehaviour
{
    public static List<PlantSlot> slots = new List<PlantSlot>();

    public static event Action onSlotPlanted;
    public static event Action<float> onPumpkinSold;
    public event Action onPumpkinDestroyed;

    [SerializeField] int sellingTime = 10;
    [SerializeField] float currentPumpkinSize = 1f;
    [SerializeField] float growthDuration = 2;
    [SerializeField] float MaxSize;
    [SerializeField] float minValue;
    [SerializeField] float maxValue;
    [SerializeField] GameObject pumpkinDisplay;
    [SerializeField] SpriteRenderer spr;
    [SerializeField] GameObject enemyLayout;
    [SerializeField] TextMeshPro enemyDisplayAmount;
    [SerializeField] SpriteRenderer sellUISpriteRenderer;
    [SerializeField] float pumpkinMaxHP = 100;
    [SerializeField] float ratDPS = 1;
    float pumpkinCurrentHP = 100;

    public SlotState slotState = SlotState.empty;
    public bool IsSelling = false;
    int currentAttackerAmount;
    float startGrowthTime;
    float endGrowthTime;

    Coroutine sellRoutine;

    float PumpkinValue => Mathf.Lerp(minValue, maxValue, Mathf.InverseLerp(startGrowthTime, endGrowthTime, Time.time));

    private void Awake()
    {
        slots.Add(this);
        ResetSlot();
    }

    public void Plant()
    {
        Debug.Log("Slot was planted");
        SetNewSlotState(SlotState.planted);
        onSlotPlanted?.Invoke();
        startGrowthTime = Time.time;
        endGrowthTime = Time.time + growthDuration;
    }

    public void Sell()
    {
        sellRoutine = StartCoroutine(SellDelay());
    }

    private void Update()
    {
        if (slotState == SlotState.planted)
        {
            if (pumpkinDisplay.transform.localScale.x < MaxSize)
            {
                pumpkinDisplay.transform.localScale = Vector3.one * Mathf.Lerp(1, MaxSize, Mathf.InverseLerp(startGrowthTime, endGrowthTime, Time.time)); ;
            }

            pumpkinCurrentHP -= ratDPS * Time.deltaTime * currentAttackerAmount;

            if (pumpkinCurrentHP <= 0)
            {
                DestroyPumpkin();
            }
        }
    }

    void DestroyPumpkin()
    {
        onPumpkinDestroyed?.Invoke();
        ResetSlot();
    }

    private void ResetSlot()
    {
        pumpkinCurrentHP = pumpkinMaxHP;
        sellUISpriteRenderer.material.SetFloat("_Arc2", 360);
        IsSelling = false;
        SetNewSlotState(SlotState.empty);
        RemoveAllAttackers();

        if (sellRoutine != null)
        {
            StopCoroutine(sellRoutine);
            sellRoutine = null;
        }
    }

    IEnumerator SellDelay()
    {
        IsSelling = true;
        float startTime = Time.time;
        float endTime = Time.time + sellingTime;

        while (Time.time < endTime)
        {
            yield return null;
            sellUISpriteRenderer.material.SetFloat("_Arc2", Mathf.Lerp(360, 0, Mathf.InverseLerp(startTime, endTime, Time.time)));
        }

        ResetSlot();
        onPumpkinSold?.Invoke(PumpkinValue);
        Debug.Log("Pumpkin was sold");
    }

    void SetNewSlotState(SlotState newState)
    {
        pumpkinDisplay.SetActive(newState == SlotState.planted);
        slotState = newState;

        switch (newState)
        {
            case SlotState.empty:
                spr.color = Color.black;
                break;
            case SlotState.planted:
                spr.color = Color.green;
                break;
        }
    }

    void RemoveAllAttackers()
    {
        currentAttackerAmount = Mathf.Max(0, currentAttackerAmount - 1);
        enemyLayout.SetActive(false);
    }

    public void AddAttacker()
    {
        currentAttackerAmount++;
        enemyLayout.SetActive(true);
        enemyDisplayAmount.text = currentAttackerAmount.ToString();
    }

    public void RemoveAttacker()
    {
        currentAttackerAmount = Mathf.Max(0, currentAttackerAmount - 1);
        enemyDisplayAmount.text = currentAttackerAmount.ToString();

        if (currentAttackerAmount <= 0)
            enemyLayout.SetActive(false);
    }
}

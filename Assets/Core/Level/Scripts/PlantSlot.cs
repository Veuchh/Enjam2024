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
    
    [SerializeField] int sellingTime = 10;
    [SerializeField] float grossFactorSize ;
    [SerializeField] float currentPumpkinSize = 1f;
    [SerializeField] float MaxSize;
    [SerializeField] float pumpkinGainPerSec;
    [SerializeField] float pumpkinBenefits;
    public GameObject pumpkinDisplay;
    [SerializeField] SpriteRenderer spr;
    [SerializeField] GameObject enemyLayout;
    [SerializeField] TextMeshPro enemyDisplayAmount;
    [SerializeField] SpriteRenderer sellUISpriteRenderer;

    public SlotState slotState = SlotState.empty;
    public bool IsSelling = false;
    int currentAttackerAmount;

    private void Awake()
    {
        slots.Add(this);
    }

    public void Plant()
    {
        Debug.Log("Slot was planted");
        SetNewSlotState(SlotState.planted);
        StartCoroutine(IncreasePumpkinSize());
        onSlotPlanted?.Invoke();
    }

    public void Sell()
    {
        StartCoroutine(SellDelay());
    }

    private void Update()
    {
        if(slotState == SlotState.planted)
        {
            pumpkinBenefits += pumpkinGainPerSec;
        }
    }

    IEnumerator IncreasePumpkinSize()
    {
        float currentPumpkinSize =1f;
        while(currentPumpkinSize<MaxSize)
        {
            pumpkinDisplay.transform.localScale += new Vector3(grossFactorSize, grossFactorSize, grossFactorSize);
            currentPumpkinSize += grossFactorSize;
            yield return new WaitForSeconds(1f);
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

        sellUISpriteRenderer.material.SetFloat("_Arc2", 360);
        IsSelling = false;
        SetNewSlotState(SlotState.empty);
        RemoveAllAttackers();
        Debug.Log("Slot was sold");
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

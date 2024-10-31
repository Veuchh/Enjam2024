using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine.UI;

[SelectionBase]
public class PlantSlot : MonoBehaviour
{
    public static List<PlantSlot> slots = new List<PlantSlot>();

    public static event Action onSlotPlanted;
    public static event Action<float> onPumpkinSold;
    public static event Action<int> onPumpkinDismantled;
    public event Action onPumpkinDestroyed;

    [SerializeField] int sellingTime = 10;
    [SerializeField] float currentPumpkinSize = 1f;
    [SerializeField] float growthDuration = 2;
    [SerializeField] float MaxSize;
    [SerializeField] float minValue;
    [SerializeField] float maxValue;
    [SerializeField] int minSeedOnDismantle = 1;
    [SerializeField] int maxSeedOnDismantle = 10;
    [SerializeField] GameObject pumpkinDisplay;
    [SerializeField] SpriteRenderer clodSpr;
    [SerializeField] SpriteRenderer pumpkinSpr;
    [SerializeField] GameObject enemyLayout;
    [SerializeField] TextMeshPro enemyDisplayAmount;
    [SerializeField] SpriteRenderer sellUISpriteRenderer;
    [SerializeField] float pumpkinMaxHP = 100;
    [SerializeField] float ratDPS = 1;
    [SerializeField] Slider hpBar;
    [SerializeField] SoundEffectData plantSFX;
    [SerializeField] SoundEffectData pumpkinDestroyedSFX;
    [SerializeField] SoundEffectData pumpkinDismantledSFX;
    [SerializeField] SoundEffectData pumpkinSoldSFX;
    [SerializeField] SoundEffectData pumpkinAttackedSFX;
    [SerializeField] float attackSFXCooldown = .5f;
    [SerializeField] Color highlightedColor;
    [SerializeField] Color neutralColor;

    float NextAllowedAttackSFX;
    float pumpkinCurrentHP = 100;

    public SlotState slotState = SlotState.empty;
    public bool IsSelling = false;
    public bool isDismantling = false;

    // VFX
    [SerializeField] GameObject dismantledVFX;
    [SerializeField] GameObject soldVFX;

    int currentAttackerAmount;
    float startGrowthTime;
    float endGrowthTime;

    Coroutine removeRoutine;

    float PumpkinValue => Mathf.Lerp(minValue, maxValue, Mathf.InverseLerp(startGrowthTime, endGrowthTime, Time.time));
    int PumpkinSeedAmount => Mathf.RoundToInt(Mathf.Lerp(minSeedOnDismantle, maxSeedOnDismantle, Mathf.InverseLerp(startGrowthTime, endGrowthTime, Time.time)));

    private void Awake()
    {
        slots.Add(this);
        ResetSlot();
        Highlight(false);
    }

    public void Plant()
    {
        Debug.Log("Slot was planted");
        SetNewSlotState(SlotState.planted);
        onSlotPlanted?.Invoke();
        startGrowthTime = Time.time;
        endGrowthTime = Time.time + growthDuration;
        AudioPlayer.Instance.PlayAudio(plantSFX);
    }

    public void Sell()
    {
        removeRoutine = StartCoroutine(RemovePumpkinDelay(false));
    }

    public void Dismantle()
    {
        removeRoutine = StartCoroutine(RemovePumpkinDelay(true));
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
            hpBar.value = Mathf.InverseLerp(0, pumpkinMaxHP, pumpkinCurrentHP);

            if (pumpkinCurrentHP <= 0)
            {
                DestroyPumpkin();
            }

            if (NextAllowedAttackSFX < Time.time && currentAttackerAmount > 0)
            {
                AudioPlayer.Instance.PlayAudio(pumpkinAttackedSFX);
                NextAllowedAttackSFX = Time.time + currentAttackerAmount;
            }
        }
    }

    void DestroyPumpkin()
    {
        AudioPlayer.Instance.PlayAudio(pumpkinDestroyedSFX);
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
        pumpkinDisplay.transform.localScale = Vector3.one;
        hpBar.value = 1;

        if (removeRoutine != null)
        {
            StopCoroutine(removeRoutine);
            removeRoutine = null;
        }
    }

    IEnumerator RemovePumpkinDelay(bool isDismantling)
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
        if (isDismantling)
        {
            onPumpkinDismantled?.Invoke(PumpkinSeedAmount);
            AudioPlayer.Instance.PlayAudio(pumpkinDismantledSFX);
        }
        else
        {
            onPumpkinSold?.Invoke(PumpkinValue);
            AudioPlayer.Instance.PlayAudio(pumpkinSoldSFX);
        }

        Instantiate(isDismantling ? dismantledVFX : soldVFX, transform.position, Quaternion.identity);
    }

    void SetNewSlotState(SlotState newState)
    {
        hpBar.gameObject.SetActive(newState == SlotState.planted);
        pumpkinDisplay.SetActive(newState == SlotState.planted);
        slotState = newState;
    }

    void RemoveAllAttackers()
    {
        currentAttackerAmount = 0;
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

    internal void Highlight(bool isHighlighted)
    {
        clodSpr.color = isHighlighted ? highlightedColor : neutralColor;
        pumpkinSpr.color = isHighlighted ? highlightedColor : neutralColor;
    }
}

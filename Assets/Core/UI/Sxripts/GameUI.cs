using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    [SerializeField] Image remainingTimeBar;
    [SerializeField] TextMeshProUGUI seedDisplay;


    private void Awake()
    {
        Instance = this;
    }

    public void UpdateTimeRatio(float ratio)
    {
        remainingTimeBar.fillAmount = ratio;
    }

    public void UpdateSeedAmount(int seed)
    {
        seedDisplay.text = seed.ToString();
    }
}

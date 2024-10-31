using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    [SerializeField] Image remainingTimeBar;
    [SerializeField] TextMeshProUGUI seedDisplay;
    [SerializeField] GameObject seedUI;


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
        PlayAnim();
    }
    public void PlayAnim()
    {
        seedUI.transform.DOScale(1, .7f)
            .From(2.4f)
            .SetEase(Ease.InOutElastic)
            .Play();
    }
}

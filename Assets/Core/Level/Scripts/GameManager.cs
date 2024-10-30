using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] float maxTime = 90;

    bool isGameOngoing = false;
    float startTime;
    float endTime;

        private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        PlantSlot.onSlotPlanted += StartGame;
        PlantSlot.onSlotPlanted += UpdateSeedUI;
        PlantSlot.onPumpkinDismantled += AddSeedToPlayerData;
        PlantSlot.onPumpkinSold += AddTime;
    }

    private void AddSeedToPlayerData(int seedToAdd)
    {
        PlayerData.seed += seedToAdd;
        UpdateSeedUI();
    }

    public void UpdateSeedUI()
    {
        GameUI.Instance.UpdateSeedAmount(PlayerData.seed);
    }

    private void AddTime(float addedTime)
    {
        endTime = Mathf.Min(endTime + addedTime, Time.time + maxTime);
    }

    private void Update()
    {
        if (!isGameOngoing) 
            return;

        float timeRatio = Mathf.InverseLerp(0, maxTime, endTime - Time.time);

        GameUI.Instance.UpdateTimeRatio(timeRatio);

        if (timeRatio <= 0)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        Debug.LogWarning("TODO : End game method => go to game over scene");
        PlantSlot.onSlotPlanted += StartGame;
    }

    void StartGame()
    {
        Debug.Log("Game starting");
        startTime = Time.time;
        endTime = Time.time + maxTime;
        isGameOngoing = true;
        PlantSlot.onSlotPlanted -= StartGame;
        EnemySpawner.Instance.StartEnemyWaves();
    }
}

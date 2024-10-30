using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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
    }

    void StartGame()
    {
        Debug.Log("Game starting");
        PlantSlot.onSlotPlanted -= StartGame;
        EnemySpawner.Instance.StartEnemyWaves();
    }
}

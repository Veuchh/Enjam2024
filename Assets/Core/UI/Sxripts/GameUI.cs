using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    [SerializeField] Image remainingTimeBar;


    private void Awake()
    {
        Instance = this;
    }

    public void UpdateTimeRatio(float ratio)
    {
        remainingTimeBar.fillAmount = ratio;
    }
}

using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button tutorialButton;
    [SerializeField] GameObject tutorialScreen;
    [SerializeField] Button quitButton;
    [SerializeField] string gameSceneName = "Game";

    private void Awake()
    {
        startButton.onClick.AddListener(OnStartButtonPressed);
        tutorialButton.onClick.AddListener(OnTutorialButtonPressed);
        quitButton.onClick.AddListener(OnQuitButtonPressed);
    }

    private void OnDestroy()
    {
        startButton.onClick.RemoveAllListeners();
        tutorialButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
    }

    void OnStartButtonPressed()
    {
        LevelLoader.Instance.LoadScene(gameSceneName);
    }

    void OnTutorialButtonPressed()
    {
        tutorialScreen.SetActive(true); 
        startButton.interactable = false;
        tutorialButton.interactable = false;
        quitButton.interactable = false;
    }

    public void OnTutorialClosed()
    {
        startButton.interactable = true;
        tutorialButton.interactable = true;
        quitButton.interactable = true;
    }

    void OnQuitButtonPressed()
    {
        Application.Quit();
    }
}

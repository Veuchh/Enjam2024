using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] string firstSceneToLoad;
    public static LevelLoader Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadScene(firstSceneToLoad);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game State")]
    public int initialLives = 3;
    public static int currentLives { get; private set; } = -1;
    public int lastLevelBuildIndex = 3;

    public bool IsGameStarted { get; private set; }
    public bool IsGameOver { get; private set; }
    public bool isPaused { get; private set; }

    [Header("Scene References")]
    private PlayerManager playerManager;
    private PathFollower pathFollower;
    private LivesUIManager livesUIManager;
    private GameObject tapToStartUI_Parent;
    private GameObject gameOverUI;
    private GameObject pauseMenuUI;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (currentLives == -1)
            {
                currentLives = initialLives;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1 && currentLives <= 0)
        {
            Debug.Log("Kembali ke Level 1 setelah kalah total. Mereset nyawa.");
            currentLives = initialLives;
        }

        InitializeLevel();
    }

    void InitializeLevel()
    {
        playerManager = FindAnyObjectByType<PlayerManager>();
        pathFollower = FindAnyObjectByType<PathFollower>();
        livesUIManager = FindAnyObjectByType<LivesUIManager>();

        tapToStartUI_Parent = GameObject.Find("Tap_To_Start");
        gameOverUI = GameObject.Find("Game_Over");
        pauseMenuUI = GameObject.Find("PauseMenu_Panel");

        Time.timeScale = 1f;
        IsGameStarted = false;
        IsGameOver = false;
        isPaused = false;

        if (tapToStartUI_Parent != null) tapToStartUI_Parent.SetActive(true);
        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);

        if (livesUIManager != null)
        {
            livesUIManager.UpdateLives(currentLives);
        }
    }

    void Update()
    {
        if (!IsGameStarted && !IsGameOver && Input.GetMouseButtonDown(0))
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        if (IsGameStarted) return;
        AudioManager.instance.PlayStartSound();
        AudioManager.instance.PlayMusic();
        IsGameStarted = true;
        if (playerManager != null)
        {
            playerManager.SetRunnerAnimations(true);
        }
        if (tapToStartUI_Parent != null)
        {
            tapToStartUI_Parent.SetActive(false);
        }
    }

    public void GameOver()
    {
        if (IsGameOver) return;
        IsGameOver = true;

        currentLives--;
        if (livesUIManager != null) livesUIManager.UpdateLives(currentLives);

        AudioManager.instance.PlayGameOverSound();
        Debug.Log("GAME OVER! Lives remaining: " + currentLives);

        if (pathFollower != null) pathFollower.speed = 0;

        Invoke(nameof(ShowGameOverUI), 1.5f);
    }

    void ShowGameOverUI()
    {
        if (gameOverUI != null) gameOverUI.SetActive(true);
    }

    public void HandleRestartClick()
    {
        Time.timeScale = 1f;
        if (currentLives > 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            Debug.Log("Nyawa habis! Pindah ke Lose Screen.");
            SceneManager.LoadScene("LoseSplashScreen");
        }
    }

    public void TogglePause()
    {
        if (!IsGameStarted || IsGameOver) return;
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0f;
            if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        }
    }

    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex >= lastLevelBuildIndex)
        {
            Debug.Log("Game Selesai! Pindah ke Win Screen.");
            SceneManager.LoadScene("WinSplashScreen");
        }
        else
        {
            int nextSceneIndex = currentSceneIndex + 1;
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    public void ResetGame()
    {
        currentLives = initialLives;
        SceneManager.LoadScene(0);
    }

    public void ResetLives()
    {
        currentLives = initialLives;
    }
}

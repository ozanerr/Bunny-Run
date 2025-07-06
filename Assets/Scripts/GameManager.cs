using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game State")]
    public int initialLives = 3;
    public static int currentLives { get; private set; } = -1;
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
        InitializeLevel();
    }

    void InitializeLevel()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        pathFollower = FindObjectOfType<PathFollower>();
        livesUIManager = FindObjectOfType<LivesUIManager>();

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
            currentLives = initialLives;
            SceneManager.LoadScene(0);
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
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            currentLives = initialLives;
            Debug.Log("Selamat! Anda telah menyelesaikan semua level! Kembali ke Level 1.");
            SceneManager.LoadScene(0);
        }
    }
}

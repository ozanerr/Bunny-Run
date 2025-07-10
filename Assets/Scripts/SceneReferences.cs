using UnityEngine;

// Script ini hanya bertugas sebagai "papan nama" atau direktori
// untuk semua objek penting di dalam satu scene.
public class SceneReferences : MonoBehaviour
{
    [Header("Player & Movement")]
    public PlayerManager playerManager;
    public PathFollower pathFollower;

    [Header("UI Panels & Managers")]
    public GameObject tapToStartPanel;
    public GameObject gameOverPanel;
    public GameObject pauseMenuPanel;
    public LivesUIManager livesUIManager;
}
